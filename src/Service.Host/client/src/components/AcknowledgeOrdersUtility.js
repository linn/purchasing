import React, { useState, useEffect, useCallback } from 'react';
import { DataGrid } from '@mui/x-data-grid';
import { useSelector, useDispatch } from 'react-redux';
import {
    SnackbarMessage,
    InputField,
    Page,
    collectionSelectorHelpers,
    CheckboxWithLabel,
    DatePicker,
    Dropdown,
    processSelectorHelpers,
    FileUploader,
    getItemError,
    ErrorCard,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import moment from 'moment';
import Dialog from '@mui/material/Dialog';
import { makeStyles } from '@mui/styles';
import Accordion from '@mui/material/Accordion';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import queryString from 'query-string';
import { useLocation } from 'react-router';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import Typography from '@mui/material/Typography';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import purchaseOrderDeliveryActions from '../actions/purchaseOrderDeliveryActions';
import purchaseOrderDeliveriesActions from '../actions/purchaseOrderDeliveriesActions';
import { purchaseOrderDeliveries, batchPurchaseOrderDeliveriesUpload } from '../itemTypes';
import history from '../history';
import config from '../config';
import batchPurchaseOrderDeliveriesUploadActions from '../actions/batchPurchaseOrderDeliveriesUploadActions';
import batchPurchaseOrderDeliveriesUpdateActions from '../actions/batchPurchaseOrderDeliveriesUpdateActions';

import PurchaseOrderDeliveriesUtility from './PurchaseOrderDeliveriesUtility';

function AcknowledgeOrdersUtility() {
    const dispatch = useDispatch();
    const { search } = useLocation();
    const [lookUpExpanded, setLookUpExpanded] = useState(true);
    const [deliveriesDialogOpen, setDeliveriesDialogOpen] = useState(false);
    const [applyChangesDialogOpen, setApplyChangesDialogOpen] = useState(false);

    const [deliveriesToUpdate, setDeliveriesToUpdate] = useState();
    const orderNumberSearchTerm = queryString.parse(search)?.orderNumber;

    useEffect(() => {
        if (orderNumberSearchTerm) {
            dispatch(
                purchaseOrderDeliveriesActions.fetchByHref(
                    `${purchaseOrderDeliveries.uri}?${queryString.stringify({
                        orderNumberSearchTerm,
                        includeAcknowledged: true
                    })}`
                )
            );
            setLookUpExpanded(true);
        }
    }, [orderNumberSearchTerm, dispatch]);
    const useStyles = makeStyles(theme => ({
        gap: {
            marginTop: theme.spacing(4)
        },
        dialog: {
            margin: theme.spacing(6),
            minWidth: theme.spacing(62)
        }
    }));
    const classes = useStyles();

    const [rows, setRows] = useState([]);

    const items = useSelector(state =>
        collectionSelectorHelpers.getItems(state[purchaseOrderDeliveries.item])
    );

    useEffect(() => {
        if (items && items.length) {
            setRows(
                items.map(i => ({
                    ...i,
                    dateRequested: i.dateRequested,
                    dateAdvised: i.dateAdvised,
                    id: `${i.orderNumber}/${i.orderLine}/${i.deliverySeq}`
                }))
            );
        }
    }, [items]);

    const itemsLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state[purchaseOrderDeliveries.item])
    );

    const getDateString = isoString =>
        isoString ? new Date(isoString).toLocaleDateString('en-GB') : null;

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'orderNumber', headerName: 'Order', width: 100 },
        {
            field: 'orderLine',
            headerName: 'Line',
            width: 100,
            renderCell: params => (
                <>
                    {params.row.orderLine}
                    <Button
                        onClick={() => {
                            setDeliveriesToUpdate(
                                rows
                                    .filter(
                                        d =>
                                            d.orderNumber === params.row.orderNumber &&
                                            d.orderLine === params.row.orderLine
                                    )
                                    .map(d => ({
                                        ...d,
                                        dateRequested: getDateString(d.dateRequested),
                                        dateAdvised: getDateString(d.dateAdvised)
                                    }))
                            );
                            setDeliveriesDialogOpen(true);
                        }}
                    >
                        DELIVS
                    </Button>
                </>
            )
        },
        { field: 'deliverySeq', headerName: 'Delivery', width: 100 },
        { field: 'ourUnitPriceCurrency', headerName: 'Unit Price', width: 100 },
        { field: 'partNumber', headerName: 'Part', width: 100 },
        { field: 'ourDeliveryQty', headerName: 'Qty', width: 100 },
        {
            field: 'dateRequested',
            headerName: 'Request Date',
            width: 150,
            renderCell: params => (
                <>
                    {getDateString(params.row.dateRequested)}
                    <IconButton
                        onClick={() => {
                            const delivery = rows.find(
                                d =>
                                    d.orderNumber === params.row.orderNumber &&
                                    d.orderLine === params.row.orderLine &&
                                    d.deliverySeq === params.row.deliverySeq
                            );
                            dispatch(
                                batchPurchaseOrderDeliveriesUpdateActions.requestProcessStart([
                                    {
                                        orderNumber: params.row.orderNumber,
                                        orderLine: params.row.orderLine,
                                        deliverySequence: delivery.deliverySeq,
                                        dateAdvised: delivery.dateRequested
                                            ? moment(delivery.dateRequested).format(
                                                  'YYYY-MM-DDTHH:mm:ss'
                                              )
                                            : null,
                                        dateRequested: delivery.dateRequested,
                                        qty: delivery.ourDeliveryQty,
                                        reason: 'ADVISED',
                                        comment: delivery.supplierConfirmationComment,
                                        availableAtSupplier: delivery.availableAtSupplier,
                                        unitPrice: delivery.orderUnitPriceCurrency
                                    }
                                ])
                            );
                        }}
                    >
                        <ArrowForwardIcon />
                    </IconButton>
                </>
            )
        },
        {
            field: 'dateAdvised',
            headerName: 'Advised Date',
            width: 100,
            renderCell: params => getDateString(params.row.dateAdvised)
        },
        { field: 'rescheduleReason', headerName: 'Reason', width: 100 },
        { field: 'supplierConfirmationComment', headerName: 'Comment', width: 100 },
        { field: 'availableAtSupplier', headerName: 'Available at Supplier?', width: 100 }
    ];

    const [searchOptions, setSearchOptions] = useState({ includeAcknowledged: true });
    const [newValues, setNewValues] = useState({
        rescheduleReason: 'ADVISED',
        dateAdvised: new Date()
    });

    const onKeyDown = data => {
        if (
            (searchOptions.orderNumberSearchTerm || searchOptions.supplierSearchTerm) &&
            data.keyCode === 13
        ) {
            setRows([]);
            dispatch(
                purchaseOrderDeliveriesActions.fetchByHref(
                    `${purchaseOrderDeliveries.uri}?${queryString.stringify(searchOptions)}`
                )
            );
        }
    };
    const handleSelectRow = selected => {
        setRows(
            rows.map(r =>
                selected.includes(r.id) ? { ...r, selected: true } : { ...r, selected: false }
            )
        );
    };

    const uploadLoading = useSelector(state =>
        processSelectorHelpers.getWorking(state[batchPurchaseOrderDeliveriesUpload.item])
    );

    const uploadResult = useSelector(state =>
        processSelectorHelpers.getData(state[batchPurchaseOrderDeliveriesUpload.item])
    );

    const uploadError = useSelector(state =>
        getItemError(state, batchPurchaseOrderDeliveriesUpload.item)
    );

    const uploadMessage = useSelector(state =>
        processSelectorHelpers.getMessageText(state[batchPurchaseOrderDeliveriesUpload.item])
    );

    const uploadSnackbarVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[batchPurchaseOrderDeliveriesUpload.item])
    );
    const setUploadSnackbarVisible = () =>
        dispatch(batchPurchaseOrderDeliveriesUploadActions.setMessageVisible(false));

    const refreshResults = useCallback(() => {
        setRows([]);
        if (searchOptions.orderNumberSearchTerm || searchOptions.supplierSearchTerm) {
            dispatch(
                purchaseOrderDeliveriesActions.fetchByHref(
                    `${purchaseOrderDeliveries.uri}?${queryString.stringify(searchOptions)}`
                )
            );
        } else if (orderNumberSearchTerm) {
            dispatch(
                purchaseOrderDeliveriesActions.fetchByHref(
                    `${purchaseOrderDeliveries.uri}?${queryString.stringify({
                        orderNumberSearchTerm,
                        includeAcknowledged: true
                    })}`
                )
            );
        }
    }, [dispatch, searchOptions, orderNumberSearchTerm]);

    useEffect(() => {
        if (uploadResult) {
            refreshResults();
        }
    }, [uploadResult, refreshResults]);

    const handleSaveClick = () => {
        setApplyChangesDialogOpen(false);
        const selectedRows = rows.filter(r => r.selected);
        setRows([]);
        dispatch(
            batchPurchaseOrderDeliveriesUpdateActions.requestProcessStart(
                selectedRows.map(r => ({
                    orderNumber: r.orderNumber,
                    orderLine: r.orderLine,
                    deliverySequence: r.deliverySeq,
                    dateAdvised: newValues.dateAdvised
                        ? moment(newValues.dateAdvised).format('YYYY-MM-DDTHH:mm:ss')
                        : null,
                    dateRequested: r.dateRequested,
                    qty: r.ourDeliveryQty,
                    reason: newValues.rescheduleReason,
                    comment: newValues.supplierConfirmationComment,
                    availableAtSupplier: newValues.availableAtSupplier,
                    unitPrice: r.orderUnitPriceCurrency
                }))
            )
        );
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <SnackbarMessage
                visible={uploadSnackbarVisible}
                onClose={() =>
                    dispatch(batchPurchaseOrderDeliveriesUpdateActions.setSnackbarVisible(false))
                }
                message="Save Successful"
            />

            <Grid container spacing={3}>
                <Dialog open={deliveriesDialogOpen} fullWidth maxWidth="lg">
                    <div className={classes.dialog}>
                        <PurchaseOrderDeliveriesUtility
                            orderNumber={deliveriesToUpdate?.[0]?.orderNumber}
                            orderLine={1} // todo
                            inDialogBox
                            cancelClick={() => setDeliveriesDialogOpen(false)}
                            backClick={() => {
                                setDeliveriesDialogOpen(false);
                                refreshResults();
                            }}
                            deliveries={deliveriesToUpdate}
                        />
                    </div>
                </Dialog>
                <Dialog open={applyChangesDialogOpen} fullWidth maxWidth="lg">
                    <div className={classes.dialog}>
                        <Grid container spacing={3}>
                            <Grid item xs={3}>
                                <DatePicker
                                    label="Advised Date"
                                    value={newValues.dateAdvised}
                                    propertyName="dateAdvised"
                                    minDate="01/01/2000"
                                    maxDate="01/01/2100"
                                    onChange={newVal =>
                                        setNewValues(o => ({ ...o, dateAdvised: newVal }))
                                    }
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <Dropdown
                                    label="Reason"
                                    propertyName="rescheduleReason"
                                    allowNoValue
                                    value={newValues.rescheduleReason}
                                    onChange={(_, newVal) =>
                                        setNewValues(o => ({
                                            ...o,
                                            rescheduleReason: newVal
                                        }))
                                    }
                                    items={[
                                        'ADVISED',
                                        'AUTO FAIL',
                                        'AUTO PASS',
                                        'BROUGHT IN',
                                        'DECOMMIT',
                                        'IGNORE',
                                        'REQUESTED',
                                        'RESCHEDULE OUT'
                                    ]}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <InputField
                                    value={newValues.supplierConfirmationComment}
                                    propertyName="supplierConfirmationComment"
                                    fullWidth
                                    label="Comment"
                                    onChange={(_, newVal) =>
                                        setNewValues(o => ({
                                            ...o,
                                            supplierConfirmationComment: newVal
                                        }))
                                    }
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <Dropdown
                                    label="Available at Supplier"
                                    propertyName="availableAtSupplier"
                                    value={newValues.availableAtSupplier}
                                    allowNoValue
                                    onChange={(_, newVal) =>
                                        setNewValues(o => ({
                                            ...o,
                                            availableAtSupplier: newVal
                                        }))
                                    }
                                    items={['Y', 'N']}
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <SaveBackCancelButtons
                                    saveClick={handleSaveClick}
                                    cancelClick={() => setApplyChangesDialogOpen(false)}
                                    backClick={() => setApplyChangesDialogOpen(false)}
                                    saveDisabled={false}
                                />
                            </Grid>
                        </Grid>
                    </div>
                </Dialog>

                {uploadError && (
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={uploadError.details} />
                    </Grid>
                )}
                <Grid item xs={12}>
                    <Typography variant="h4">Choose an option: </Typography>
                </Grid>
                <Grid item xs={12}>
                    <Accordion
                        expanded={lookUpExpanded}
                        onChange={() => setLookUpExpanded(!lookUpExpanded)}
                    >
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel1a-content"
                            id="panel1a-header"
                        >
                            <Typography variant="h5">Look Up Some Orders</Typography>
                        </AccordionSummary>
                        <AccordionDetails>
                            <Grid container spacing={3}>
                                <Grid item xs={3}>
                                    <InputField
                                        value={searchOptions.orderNumberSearchTerm}
                                        propertyName="orderNumberSearchTerm"
                                        fullWidth
                                        label="Order Number"
                                        onChange={(_, newVal) => {
                                            dispatch(purchaseOrderDeliveryActions.clearItem());

                                            setSearchOptions(o => ({
                                                ...o,
                                                orderNumberSearchTerm: newVal
                                            }));
                                        }}
                                        textFieldProps={{
                                            onKeyDown
                                        }}
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <InputField
                                        value={searchOptions.supplierSearchTerm}
                                        propertyName="supplierSearchTerm"
                                        fullWidth
                                        label="Supplier"
                                        onChange={(_, newVal) => {
                                            dispatch(purchaseOrderDeliveryActions.clearItem());
                                            setSearchOptions(o => ({
                                                ...o,
                                                supplierSearchTerm: newVal
                                            }));
                                        }}
                                        helperText="press enter to search"
                                        textFieldProps={{
                                            onKeyDown
                                        }}
                                    />
                                </Grid>
                                <Grid item xs={6} />

                                <Grid item xs={3}>
                                    <CheckboxWithLabel
                                        label="includeAcknowledged"
                                        checked={searchOptions.includeAcknowledged}
                                        onChange={() => {
                                            dispatch(purchaseOrderDeliveryActions.clearItem());

                                            setSearchOptions(o => ({
                                                ...o,
                                                includeAcknowledged: !o.includeAcknowledged
                                            }));
                                        }}
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <DataGrid
                                        className={classes.gap}
                                        rows={rows}
                                        columns={columns}
                                        rowHeight={34}
                                        columnBuffer={12}
                                        autoHeight
                                        disableSelectionOnClick
                                        loading={itemsLoading || uploadLoading}
                                        hideFooter
                                        checkboxSelection
                                        onSelectionModelChange={handleSelectRow}
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <Button
                                        variant="outlined"
                                        disabled={!rows.some(r => r.selected)}
                                        onClick={() => {
                                            setNewValues({
                                                rescheduleReason: 'ADVISED'
                                            });
                                            setApplyChangesDialogOpen(true);
                                        }}
                                    >
                                        Apply Changes To Selected
                                    </Button>
                                </Grid>
                            </Grid>
                        </AccordionDetails>
                    </Accordion>

                    <FileUploader
                        doUpload={data => {
                            dispatch(
                                batchPurchaseOrderDeliveriesUploadActions.clearErrorsForItem()
                            );
                            dispatch(batchPurchaseOrderDeliveriesUploadActions.clearProcessData());
                            dispatch(
                                batchPurchaseOrderDeliveriesUploadActions.requestProcessStart(data)
                            );
                        }}
                        loading={uploadLoading}
                        result={uploadResult}
                        error={uploadError}
                        snackbarVisible={uploadSnackbarVisible}
                        setSnackbarVisible={setUploadSnackbarVisible}
                        message={uploadMessage}
                        initiallyExpanded={false}
                        helperText="Upload a csv file with the following columns: Order Number, New Advised Date, Qty, Unit Price and (optionally) New Reason. Date must be in a format matching either 31/01/2022, 31-jan-2022 or 2022-01-31. New Reason must be one of the following: ADVISED, AUTO FAIL, AUTO PASS, BROUGHT IN, DECOMMIT, IGNORE, REQUESTED, RESCHEDULE OUT and will default to ADVISED if no value is supplied."
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default AcknowledgeOrdersUtility;
