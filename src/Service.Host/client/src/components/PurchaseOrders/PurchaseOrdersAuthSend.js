import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    collectionSelectorHelpers,
    Page,
    DatePicker,
    utilities,
    userSelectors,
    processSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Tooltip from '@mui/material/Tooltip';
import Radio from '@mui/material/Radio';
import RadioGroup from '@mui/material/RadioGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import FormControl from '@mui/material/FormControl';
import FormLabel from '@mui/material/FormLabel';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import DialogContentText from '@mui/material/DialogContentText';
import { Link } from 'react-router-dom';
import moment from 'moment';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import emailMultiplePurchaseOrdersActions from '../../actions/emailMultiplePurchaseOrdersActions';
import authoriseMultiplePurchaseOrdersActions from '../../actions/authoriseMultiplePurchaseOrdersActions';
import history from '../../history';
import config from '../../config';

function PurchaseOrdersAuthSend() {
    const dispatch = useDispatch();

    const defaultStartDate = new Date();
    defaultStartDate.setMonth(defaultStartDate.getMonth() - 1);

    const [options, setOptions] = useState({
        startDate: defaultStartDate,
        endDate: new Date(),
        enteredBy: 'all',
        sent: 'all',
        auth: 'all'
    });
    const [selectedRows, setSelectedRows] = useState([]);
    const [showDialog, setShowDialog] = useState(false);
    const [dialogText, setDialogText] = useState(null);

    useEffect(() => {
        dispatch(purchaseOrdersActions.fetchState());
        dispatch(
            purchaseOrdersActions.searchWithOptions(
                '',
                `&startDate=${options.startDate.toISOString()}&endDate=${options.endDate.toISOString()}`
            )
        );
    }, [dispatch, options.endDate, options.startDate]);

    const userNumber = useSelector(state => userSelectors.getUserNumber(state));
    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.purchaseOrders, 50, 'orderNumber')
    );
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.purchaseOrders)
    );

    const emailProcessResult = useSelector(state =>
        processSelectorHelpers.getData(state.emailMultiplePurchaseOrders)
    );

    const authoriseProcessResult = useSelector(state =>
        processSelectorHelpers.getData(state.authoriseMultiplePurchaseOrders)
    );

    useEffect(() => {
        if (authoriseProcessResult) {
            setDialogText(authoriseProcessResult.message);
            setShowDialog(true);
        }

        if (emailProcessResult) {
            setDialogText(emailProcessResult.message);
            setShowDialog(true);
        }
    }, [emailProcessResult, authoriseProcessResult]);

    const orderColumns = [
        {
            field: 'orderNumber',
            headerName: 'Order',
            width: 100,
            renderCell: params => (
                <Link to={utilities.getSelfHref(searchResults?.find(a => a.id === params.value))}>
                    {params.value}
                </Link>
            )
        },
        {
            field: 'orderDate',
            headerName: 'Date',
            minWidth: 150,
            valueGetter: ({ value }) => value && moment(value).format('DD MMM YYYY')
        },
        {
            field: 'supplierName',
            headerName: 'Supplier',
            width: 240,
            renderCell: params => (
                <Tooltip title={`Supplier id ${params.row.supplierId}`}>
                    <div>{params.value}</div>
                </Tooltip>
            )
        },
        {
            field: 'enteredBy',
            headerName: 'Entered By',
            width: 100,
            renderCell: params => (
                <Tooltip title={params.row.enteredByName}>
                    <div>{params.value}</div>
                </Tooltip>
            )
        },
        { field: 'authorisedBy', headerName: 'Auth By', width: 240 },
        { field: 'sentByMethod', headerName: 'Sent By', width: 100 },
        { field: 'value', headerName: 'GBP Val', width: 120 },
        {
            field: 'lines',
            headerName: 'Parts Ordered',
            width: 300,
            renderCell: params => (
                <Tooltip
                    title={<div style={{ whiteSpace: 'pre-line' }}>{params.row.linesDetails}</div>}
                >
                    <div>{params.value}</div>
                </Tooltip>
            )
        }
    ];

    const getRows = () => {
        if (!searchResults) {
            return [];
        }

        let results = searchResults;
        if (options.enteredBy === 'auto') {
            results = searchResults.filter(a => a.enteredBy.id === 100);
        } else if (options.enteredBy === 'self') {
            results = searchResults.filter(a => a.enteredBy.id === userNumber);
        }

        if (options.auth === 'auth') {
            results = searchResults.filter(a => a.authorisedBy);
        } else if (options.enteredBy === 'unauth') {
            results = searchResults.filter(a => !a.authorisedBy);
        }

        if (options.sent === 'sent') {
            results = searchResults.filter(a => a.sentByMethod);
        } else if (options.sent === 'unsent') {
            results = searchResults.filter(a => !a.sentByMethod);
        }

        return results.map(r => ({
            id: r.id,
            orderNumber: r.orderNumber,
            orderDate: r.orderDate,
            supplierId: r.supplier.id,
            supplierName: r.supplier.name,
            sentByMethod: r.sentByMethod,
            authorisedBy: r.authorisedBy?.fullName,
            enteredByName: r.enteredBy?.fullName,
            enteredBy: r.enteredBy.id,
            value: 123.23,
            lines: r.details?.map(d => `${d.partNumber}x${d.ourQty}`).join(', '),
            linesDetails: r.details
                ?.map(d => `Line ${d.line} ${d.ourQty} of ${d.partNumber}`)
                .join('\n')
        }));
    };

    const handleEnteredChange = event => {
        setOptions({ ...options, enteredBy: event.target.value });
    };

    const handleAuthChange = event => {
        setOptions({ ...options, auth: event.target.value });
    };

    const handleSentChange = event => {
        setOptions({ ...options, sent: event.target.value });
    };

    const emailOrders = () => {
        dispatch(emailMultiplePurchaseOrdersActions.clearProcessData());
        dispatch(
            emailMultiplePurchaseOrdersActions.requestProcessStart({
                orders: selectedRows,
                copySelf: 'false'
            })
        );
    };
    const emailOrdersAndCopy = () => {
        dispatch(emailMultiplePurchaseOrdersActions.clearProcessData());
        dispatch(
            emailMultiplePurchaseOrdersActions.requestProcessStart({
                orders: selectedRows,
                copySelf: 'true'
            })
        );
    };
    const authoriseOrders = () => {
        dispatch(authoriseMultiplePurchaseOrdersActions.clearProcessData());
        dispatch(
            authoriseMultiplePurchaseOrdersActions.requestProcessStart({
                orders: selectedRows
            })
        );
    };
    const handleSelectionModelChange = model => {
        setSelectedRows(model);
    };

    const handleClose = () => {
        setShowDialog(false);
        setDialogText(null);
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={11}>
                    <Typography variant="h6">Authorise Or Send Purchase Orders</Typography>
                </Grid>
                <Grid item xs={3}>
                    <DatePicker
                        label="Start Date"
                        value={options.startDate}
                        propertyName="startDate"
                        minDate={(new Date().getMonth() - 12).toString()}
                        maxDate={new Date().toString()}
                        onChange={newVal => setOptions(o => ({ ...o, startDate: newVal }))}
                    />
                </Grid>
                <Grid item xs={3}>
                    <DatePicker
                        label="End Date"
                        propertyName="endDate"
                        value={options.endDate}
                        minDate={(new Date().getMonth() - 12).toString()}
                        maxDate={new Date().toString()}
                        onChange={newVal => setOptions(o => ({ ...o, endDate: newVal }))}
                    />
                </Grid>
                <Grid item xs={6} />
                <Grid item xs={4}>
                    <FormControl>
                        <FormLabel id="enteredByOptionLabel">Orders Entered By</FormLabel>
                        <RadioGroup
                            name="enteredByOption"
                            row
                            value={options.enteredBy}
                            onChange={handleEnteredChange}
                        >
                            <FormControlLabel
                                value="all"
                                control={<Radio size="small" />}
                                label="All"
                            />
                            <FormControlLabel
                                value="self"
                                control={<Radio size="small" />}
                                label="My Orders"
                            />
                            <FormControlLabel
                                value="auto"
                                control={<Radio size="small" />}
                                label="Auto Orders"
                            />
                        </RadioGroup>
                    </FormControl>
                </Grid>
                <Grid item xs={4}>
                    <FormControl>
                        <FormLabel id="authOptionLabel">Authorised State</FormLabel>
                        <RadioGroup
                            name="authOption"
                            row
                            value={options.auth}
                            onChange={handleAuthChange}
                        >
                            <FormControlLabel
                                value="all"
                                control={<Radio size="small" />}
                                label="All"
                            />
                            <FormControlLabel
                                value="auth"
                                control={<Radio size="small" />}
                                label="Auth Orders"
                            />
                            <FormControlLabel
                                value="unauth"
                                control={<Radio size="small" />}
                                label="Unauth Orders"
                            />
                        </RadioGroup>
                    </FormControl>
                </Grid>
                <Grid item xs={4}>
                    <FormControl>
                        <FormLabel id="sentOptionLabel">Sent/Unsent</FormLabel>
                        <RadioGroup
                            name="sentOption"
                            row
                            value={options.sent}
                            onChange={handleSentChange}
                        >
                            <FormControlLabel
                                value="all"
                                control={<Radio size="small" />}
                                label="All"
                            />
                            <FormControlLabel
                                value="sent"
                                control={<Radio size="small" />}
                                label="Sent"
                            />
                            <FormControlLabel
                                value="unsent"
                                control={<Radio size="small" />}
                                label="Unsent"
                            />
                        </RadioGroup>
                    </FormControl>
                </Grid>
                <Grid item xs={12}>
                    <Tooltip title="Email selected orders to suppliers and copy to self">
                        <Button
                            style={{ marginRight: '30px' }}
                            size="small"
                            variant="outlined"
                            onClick={emailOrdersAndCopy}
                            disabled={!selectedRows || selectedRows.length === 0}
                        >
                            Email Orders + Copy To Self
                        </Button>
                    </Tooltip>
                    <Tooltip title="Email selected orders to supplier only">
                        <Button
                            style={{ marginRight: '30px' }}
                            size="small"
                            variant="outlined"
                            onClick={emailOrders}
                            disabled={!selectedRows || selectedRows.length === 0}
                        >
                            Email Orders
                        </Button>
                    </Tooltip>
                    <Tooltip title="Authorise selected orders">
                        <Button
                            style={{ marginRight: '30px' }}
                            size="small"
                            variant="outlined"
                            onClick={authoriseOrders}
                            disabled={!selectedRows || selectedRows.length === 0}
                        >
                            Authorise Orders
                        </Button>
                    </Tooltip>
                </Grid>
                <Grid item xs={12}>
                    <DataGrid
                        rows={getRows()}
                        columns={orderColumns}
                        density="compact"
                        rowHeight={34}
                        autoHeight
                        checkboxSelection
                        onSelectionModelChange={handleSelectionModelChange}
                        loading={searchLoading}
                    />
                </Grid>
            </Grid>
            <Dialog
                id="alert-dialog"
                open={showDialog}
                onClose={handleClose}
                maxWidth="md"
                fullWidth
            >
                <DialogTitle id="alert-dialog-title">
                    <IconButton
                        aria-label="close"
                        onClick={handleClose}
                        sx={{
                            position: 'absolute',
                            right: 8,
                            top: 8
                        }}
                    >
                        <CloseIcon />
                    </IconButton>
                </DialogTitle>
                <DialogContent>
                    <DialogContentText id="alert-content">{dialogText}</DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Ok</Button>
                </DialogActions>
            </Dialog>
        </Page>
    );
}

export default PurchaseOrdersAuthSend;
