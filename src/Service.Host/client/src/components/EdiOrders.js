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
    utilities
} from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
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
import ediSuppliersActions from '../actions/ediSuppliersActions';
import sendEdiEmailActions from '../actions/sendEdiEmailActions';
import history from '../history';
import config from '../config';

function EdiOrder() {
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(suppliersActions.fetchState());
    }, [dispatch]);

    useEffect(() => {
        dispatch(ediSuppliersActions.fetch());
    }, [dispatch]);

    const applicationState = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.suppliers)
    );

    const [rows, setRows] = useState([]);
    const suppliers = useSelector(state => collectionSelectorHelpers.getItems(state.ediSuppliers));
    const [selectedSuppliers, setSelectedSuppliers] = useState(null);

    const suppliersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.ediSuppliers)
    );

    const columns = [
        { field: 'supplierId', headerName: 'Supplier', width: 90 },
        { field: 'supplierName', headerName: 'Name', width: 260 },
        { field: 'vendorManangerName', headerName: 'Vendor Manager', width: 170 },
        { field: 'ediEmailAddress', headerName: 'Email', width: 170, editable: true },
        {
            field: 'numOrders',
            headerName: 'Orders',
            width: 70
        },
        {
            field: 'getOrders',
            headerName: '',
            width: 120,
            renderCell: params => (
                <>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={() =>
                            dispatch(
                                ediOrdersActions.searchWithOptions(
                                    null,
                                    `&supplierId=${params.row.supplierId}`
                                )
                            )
                        }
                    >
                        Get Orders
                    </Button>
                </>
            )
        }
    ];

    useEffect(() => {
        setRows(!suppliers ? [] : suppliers.map(s => ({ ...s, id: s.supplierId })));
    }, [suppliers]);

    const processRowUpdate = newRow => {
        setRows(r =>
            r.map(x =>
                x.id === newRow.id
                    ? {
                          ...newRow,
                          ediEmailAddress: newRow,
                          alternativeEmail: true
                      }
                    : x
            )
        );
        return newRow;
    };

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

    const [additionalText, setAdditionalText] = useState('');

    const sendEdiUrl = utilities.getHref(applicationState, 'edi');

    const handleSelectRow = selected => {
        const newRows = rows.map(r => ({ ...r, selected: selected.includes(r.id) }));
        setRows(newRows);
        setSelectedSuppliers(selected);
    };

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'additionalText') {
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
        const sendEmails = rows
            .filter(r => r.selected)
            .map(s => ({
                supplierId: s.id,
                altEmail: s.alternativeEmail ? s.ediEmailAddress : null,
                additionalText
            }));

        sendEmails.forEach(s => {
            dispatch(sendEdiEmailActions.add(s));
        });
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
                    <DataGrid
                        rows={rows}
                        columns={columns}
                        checkboxSelection
                        onSelectionModelChange={handleSelectRow}
                        processRowUpdate={processRowUpdate}
                        density="compact"
                        rowHeight={34}
                        autoHeight
                        loading={suppliersLoading}
                        hideFooter
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
                <Grid item xs={9}>
                    {emailSending ? (
                        <Loading />
                    ) : (
                        <Button
                            variant="contained"
                            disabled={!sendEdiUrl || !selectedSuppliers || emailSentResult}
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
