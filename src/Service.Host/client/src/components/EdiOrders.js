import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import {
    collectionSelectorHelpers,
    itemSelectorHelpers,
    InputField,
    Loading,
    Page,
    SnackbarMessage,
    Typeahead,
    utilities
} from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import suppliersActions from '../actions/suppliersActions';
import ediOrdersActions from '../actions/ediOrdersActions';
import sendEdiEmailActions from '../actions/sendEdiEmailActions';
import history from '../history';
import config from '../config';

function EdiOrder() {
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(suppliersActions.fetchState());
    }, [dispatch]);

    const applicationState = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.suppliers)
    );
    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers)
    )?.map(c => ({
        id: c.id,
        name: c.id.toString(),
        description: c.name,
        links: c.links
    }));

    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );

    const ordersLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.ediOrders)
    );

    const ediOrders = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.ediOrders)
    );

    const emailSending = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.sendEdiEmail)
    );

    const emailSentResult = useSelector(state => itemSelectorHelpers.getItem(state.sendEdiEmail));

    const [supplier, setSupplier] = useState({ id: '', name: 'click to set supplier', links: [] });
    const [altEmail, setAltEmail] = useState('');
    const [additionalEmail, setAdditionalEmail] = useState('');
    const [additionalText, setAdditionalText] = useState('');

    const sendEdiUrl = utilities.getHref(applicationState, 'edi');

    const handleSupplierChange = selectedsupplier => {
        setSupplier(selectedsupplier);
    };

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'altEmail') {
            setAltEmail(newValue);
        } else if (propertyName === 'additionalEmail') {
            setAdditionalEmail(newValue);
        } else if (propertyName === 'additionalText') {
            setAdditionalText(newValue);
        }
    };

    const showOrders = orders => {
        if (!orders || orders.length === 0) {
            return '';
        }

        return (
            <List>
                <ListItem>Order Number</ListItem>
                {orders.map(order => (
                    <ListItem
                        component={Link}
                        to={`/purchasing/purchase-orders/${order.orderNumber}`}
                    >
                        {order.orderNumber}
                    </ListItem>
                ))}
            </List>
        );
    };

    const handleSendEdiEmail = () => {
        const sendEmailOptions = {
            supplierId: supplier.id,
            altEmail,
            additionalEmail,
            additionalText
        };
        dispatch(sendEdiEmailActions.add(sendEmailOptions));
    };

    const snackbarMessage = () => {
        if (emailSentResult) {
            if (emailSentResult.success) {
                return 'Email will be sent by server';
            }

            return emailSentResult.message;
        }
        return '';
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <SnackbarMessage visible={emailSentResult} message={snackbarMessage()} />
            <Grid container spacing={3}>
                <Grid item xs={11}>
                    <Typography variant="h3">PL EDI</Typography>
                </Grid>
                <Grid item xs={1}>
                    {sendEdiUrl ? (
                        <Tooltip title="You have access to send Edi emails">
                            <ModeEditIcon fontSize="large" color="primary" />
                        </Tooltip>
                    ) : (
                        <Tooltip title="You do not have access to send Edi emails">
                            <EditOffIcon fontSize="large" color="secondary" />
                        </Tooltip>
                    )}
                </Grid>
                <Grid item xs={12}>
                    <Typeahead
                        label="Supplier"
                        title="Search for a supplier"
                        onSelect={handleSupplierChange}
                        items={suppliersSearchResults}
                        loading={suppliersSearchLoading}
                        fetchItems={searchTerm => dispatch(suppliersActions.search(searchTerm))}
                        clearSearch={() => dispatch(suppliersActions.clearSearch)}
                        value={`${supplier?.id} - ${supplier?.description}`}
                        modal
                        links={false}
                        debounce={1000}
                        minimumSearchTermLength={2}
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={altEmail}
                        label="Alternative Email"
                        propertyName="altEmail"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={additionalEmail}
                        label="Additional Email"
                        propertyName="additionalEmail"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={additionalText}
                        label="Additional Text"
                        propertyName="additionalText"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={3}>
                    <Button
                        variant="contained"
                        color="primary"
                        disabled={!sendEdiUrl || !supplier.id}
                        onClick={() =>
                            dispatch(
                                ediOrdersActions.searchWithOptions(
                                    null,
                                    `&supplierId=${supplier.id}`
                                )
                            )
                        }
                    >
                        Get orders
                    </Button>
                </Grid>
                <Grid item xs={9}>
                    {emailSending ? (
                        <Loading />
                    ) : (
                        <Button
                            variant="contained"
                            disabled={!ediOrders || !ediOrders.length || emailSentResult}
                            onClick={() => handleSendEdiEmail()}
                        >
                            Send Emails
                        </Button>
                    )}
                </Grid>
                <Grid item xs={12}>
                    {ordersLoading ? (
                        <Grid item xs={12}>
                            <Loading />
                        </Grid>
                    ) : (
                        showOrders(ediOrders)
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default EdiOrder;