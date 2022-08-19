import React, { useState, Fragment, useEffect } from 'react';
import PropTypes from 'prop-types';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { makeStyles } from '@mui/styles';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import Link from '@mui/material/Link';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    InputField,
    Loading,
    utilities,
    CreateButton,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Divider from '@mui/material/Divider';
import { Link as RouterLink } from 'react-router-dom';
import moment from 'moment';
import purchaseOrderReqsActions from '../../actions/purchaseOrderReqsActions';
import history from '../../history';
import config from '../../config';

function POReqSearch({ print }) {
    const useStyles = makeStyles(theme => ({
        button: {
            marginLeft: theme.spacing(1),
            marginTop: theme.spacing(4)
        },
        a: {
            textDecoration: 'none'
        }
    }));

    const classes = useStyles();

    const loading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.purchaseOrderReqs)
    );

    const results = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.purchaseOrderReqs)
    );

    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(purchaseOrderReqsActions.fetchState());
        dispatch(purchaseOrderReqsActions.searchWithOptions('', `&reqNumber=&part=&supplier=`));
    }, [dispatch]);

    const [options, setOptions] = useState({
        reqNumber: '',
        part: '',
        supplier: '',
        purchaseOrderNumber: null
    });

    const handleOptionsChange = (propertyName, newValue) =>
        setOptions({ ...options, [propertyName]: newValue });

    useEffect(() => {
        if (
            options.reqNumber.length > 2 ||
            options.part.length > 2 ||
            options.supplier.length > 2 ||
            options.purchaseOrderNumber?.length > 2
        ) {
            let queryString = `&reqNumber=${options.reqNumber}&part=${options.part}&supplier=${options.supplier}`;
            if (options.purchaseOrderNumber) {
                queryString += `&purchaseOrderNumber=${options.purchaseOrderNumber}`;
            }

            dispatch(purchaseOrderReqsActions.searchWithOptions('', queryString));
        }
    }, [dispatch, options.reqNumber, options.part, options.supplier, options.purchaseOrderNumber]);

    return (
        <Page history={history} homeUrl={config.appRoot} width="m">
            <Grid container spacing={3}>
                <Grid item xs={10}>
                    <Typography variant="h4">
                        {print ? 'Search For a Req To Print' : 'Purchase Order Reqs Search'}
                    </Typography>
                </Grid>
                <Grid item xs={2}>
                    <CreateButton createUrl="/purchasing/purchase-orders/reqs/create" />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        fullWidth
                        value={options.reqNumber}
                        label="Req Number"
                        propertyName="reqNumber"
                        onChange={handleOptionsChange}
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        fullWidth
                        value={options.part}
                        label="Part"
                        propertyName="part"
                        onChange={handleOptionsChange}
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        fullWidth
                        value={options.supplier}
                        label="Supplier name or Id"
                        propertyName="supplier"
                        onChange={handleOptionsChange}
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        fullWidth
                        value={options.purchaseOrderNumber}
                        label="PO Number"
                        propertyName="purchaseOrderNumber"
                        onChange={handleOptionsChange}
                    />
                </Grid>
                {loading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
                <Grid item xs={12}>
                    <List dense>
                        {results.map(req => (
                            <Fragment key={req.reqNumber}>
                                <Link
                                    className={classes.a}
                                    component={RouterLink}
                                    to={
                                        print
                                            ? utilities.getHref(req, 'print')
                                            : utilities.getSelfHref(req)
                                    }
                                >
                                    <ListItem spacing={15}>
                                        <Grid item xs={1}>
                                            <Typography variant="subtitle1">
                                                {req.reqNumber}
                                            </Typography>
                                        </Grid>
                                        <Grid item xs={1} sx={{ mr: 2 }}>
                                            <Typography>
                                                {moment(req.reqDate).format('DD MMM YYYY')}
                                            </Typography>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <Typography>{req.state}</Typography>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <Typography>{req.partNumber}</Typography>
                                        </Grid>
                                        <Grid item xs={4}>
                                            <Typography>{req.description}</Typography>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <Typography>
                                                {req.supplier?.name} ({req.supplier?.id})
                                            </Typography>
                                        </Grid>
                                    </ListItem>
                                </Link>

                                <Divider component="li" />
                            </Fragment>
                        ))}
                    </List>
                </Grid>
            </Grid>
        </Page>
    );
}

POReqSearch.propTypes = {
    print: PropTypes.bool
};

POReqSearch.defaultProps = {
    print: false
};

export default POReqSearch;
