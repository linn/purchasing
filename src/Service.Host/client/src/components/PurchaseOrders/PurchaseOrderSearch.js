import React, { Fragment, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link as RouterLink } from 'react-router-dom';
import {
    collectionSelectorHelpers,
    InputField,
    Page,
    utilities,
    CreateButton,
    itemSelectorHelpers,
    Search
} from '@linn-it/linn-form-components-library';
import { makeStyles } from '@mui/styles';
import Button from '@mui/material/Button';
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import Typography from '@mui/material/Typography';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import Divider from '@mui/material/Divider';
import Grid from '@mui/material/Grid';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import Link from '@mui/material/Link';
import moment from 'moment';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import history from '../../history';
import config from '../../config';

function PurchaseOrderSearch() {
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

    const [searchTerm, setSearchTerm] = useState();

    useEffect(() => {
        dispatch(purchaseOrdersActions.fetchState());
        dispatch(purchaseOrdersActions.search(''));
    }, [dispatch]);
    const purchaseOrdersStoreItem = useSelector(state => state.purchaseOrders);
    const searchResults = collectionSelectorHelpers.getSearchItems(
        purchaseOrdersStoreItem,
        100,
        'orderNumber',
        'orderNumber',
        'orderNumber'
    );

    const [recentOrders, setRecentOrders] = useState();

    useEffect(() => {
        if (!recentOrders && searchResults && searchResults.length > 0) {
            setRecentOrders(searchResults);
        }
    }, [searchResults, recentOrders]);

    const searchLoading = collectionSelectorHelpers.getSearchLoading(purchaseOrdersStoreItem);

    const [orderNumber, setOrderNumber] = useState('');
    const applicationState = itemSelectorHelpers.getApplicationState(purchaseOrdersStoreItem);

    return (
        <Page history={history} homeUrl={config.appRoot} title="PO Search">
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h3">Search Purchase Orders</Typography>
                </Grid>

                <Grid item xs={10} />

                <Grid item xs={2}>
                    <CreateButton
                        disabled={!utilities.getHref(applicationState, 'quick-create')}
                        createUrl={utilities.getHref(applicationState, 'quick-create')}
                    />
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
                                    history.push(`/purchasing/purchase-orders/${orderNumber}/`);
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
                        onClick={() => history.push(`/purchasing/purchase-orders/${orderNumber}/`)}
                    >
                        Go
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    <Accordion>
                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <Typography>Advanced Search</Typography>
                        </AccordionSummary>
                        <AccordionDetails>
                            <Search
                                propertyName="searchTerm"
                                label="Search by terms contained in the suppliers designation or internal comments fields"
                                value={searchTerm}
                                handleValueChange={(_, newVal) => setSearchTerm(newVal)}
                                search={s => dispatch(purchaseOrdersActions.search(s))}
                                searchResults={searchResults.map(s => ({
                                    ...s,
                                    name: s.orderNumber.toString(),
                                    description: `${s.details[0].partNumber} - ${
                                        s.supplier.name
                                    } - ${moment(s.orderDate).format('DD MMM YYYY')}`
                                }))}
                                resultsInModal
                                loading={searchLoading}
                                // prioritise exact matches
                                priorityFunction={(item, s) => {
                                    if (
                                        item.details.some(
                                            d =>
                                                d.suppliersDesignation?.toUpperCase() ===
                                                s?.toUpperCase()
                                        )
                                    ) {
                                        return 3;
                                    }
                                    if (
                                        item.details.some(
                                            d =>
                                                d.internalComments?.toUpperCase() ===
                                                s?.toUpperCase()
                                        )
                                    ) {
                                        return 2;
                                    }
                                    return 1;
                                }}
                                onResultSelect={res => history.push(utilities.getSelfHref(res))}
                                clearSearch={() => dispatch(purchaseOrdersActions.clearSearch())}
                            />
                        </AccordionDetails>
                    </Accordion>
                </Grid>
                <Grid item xs={12}>
                    <Accordion>
                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <Typography>Show Recent Orders</Typography>
                        </AccordionSummary>
                        <AccordionDetails>
                            <List dense>
                                {recentOrders?.map(x => (
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
                                                        {`${moment(x.orderDate).format(
                                                            'DD MMM YYYY'
                                                        )} - ${x.supplier.name} (${x.supplier.id})`}
                                                    </Typography>
                                                </Grid>
                                            </ListItem>
                                        </Link>
                                        <Divider component="li" />
                                    </Fragment>
                                ))}
                            </List>
                        </AccordionDetails>
                    </Accordion>
                </Grid>
            </Grid>
        </Page>
    );
}

export default PurchaseOrderSearch;
