import React, { Fragment, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link as RouterLink } from 'react-router-dom';
import {
    collectionSelectorHelpers,
    InputField,
    Page,
    utilities
} from '@linn-it/linn-form-components-library';
import { makeStyles } from '@mui/styles';
import Button from '@mui/material/Button';
import Divider from '@mui/material/Divider';
import Grid from '@mui/material/Grid';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import moment from 'moment';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import history from '../../history';
import config from '../../config';

function PurchaseOrdersSearch() {
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
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(purchaseOrdersActions.fetchState());
        dispatch(purchaseOrdersActions.search(''));
    }, [dispatch]);
    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state.purchaseOrders,
            100,
            'orderNumber',
            'orderNumber',
            'orderNumber'
        )
    );
    const [orderNumber, setOrderNumber] = useState('');
    const search = () => history.push(`/purchasing/purchase-orders/${orderNumber}/`);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={11}>
                    <Typography variant="h3">Search Purchase Orders</Typography>
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        fullWidth
                        placeholder="Go to Order Number"
                        value={orderNumber}
                        label="Order Number"
                        propertyName="orderNumber"
                        onChange={(_, newValue) => setOrderNumber(newValue)}
                        textFieldProps={{
                            onKeyDown: data => {
                                if (data.keyCode === 13 || data.keyCode === 9) {
                                    search();
                                }
                            }
                        }}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        color="primary"
                        className={classes.button}
                        onClick={search}
                    >
                        Go
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    <List dense>
                        {searchResults.map(x => (
                            <Fragment key={x.orderNumber}>
                                <Link
                                    className={classes.a}
                                    component={RouterLink}
                                    to={utilities.getSelfHref(x)}
                                >
                                    <ListItem spacing={15}>
                                        <Grid item xs={1}>
                                            <Typography variant="subtitle1">
                                                {x.orderNumber}
                                            </Typography>
                                        </Grid>
                                        <Grid item xs={9}>
                                            <Typography>
                                                {`${moment(x.orderDate).format('DD MMM YYYY')} - ${
                                                    x.supplier.name
                                                } (${x.supplier.id})`}
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

export default PurchaseOrdersSearch;
