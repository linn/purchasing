/* eslint-disable react/jsx-props-no-spreading */
import React, { useState, useEffect, useCallback } from 'react';
import { DataGrid } from '@mui/x-data-grid';
import { useDropzone } from 'react-dropzone';
import { useSelector, useDispatch } from 'react-redux';
import {
    SnackbarMessage,
    InputField,
    Page,
    collectionSelectorHelpers,
    itemSelectorHelpers,
    CheckboxWithLabel,
    DatePicker,
    Dropdown,
    processSelectorHelpers,
    getItemError,
    ErrorCard,
    SaveBackCancelButtons,
    Loading
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { makeStyles } from '@mui/styles';
import Accordion from '@mui/material/Accordion';
import Button from '@mui/material/Button';
import queryString from 'query-string';
import Box from '@mui/material/Box';
import Chip from '@mui/material/Chip';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import Typography from '@mui/material/Typography';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import purchaseOrderDeliveryActions from '../actions/purchaseOrderDeliveryActions';
import purchaseOrderDeliveriesActions from '../actions/purchaseOrderDeliveriesActions';
import {
    purchaseOrderDelivery,
    purchaseOrderDeliveries,
    batchPurchaseOrderDeliveriesUpload
} from '../itemTypes';
import history from '../history';
import config from '../config';
import batchPurchaseOrderDeliveriesUploadActions from '../actions/batchPurchaseOrderDeliveriesUploadActions';

const FileUpload = () => {
    const [file, setFile] = useState(null);
    const onDrop = useCallback(acceptedFile => {
        setFile(acceptedFile[0]);
    }, []);
    const { getRootProps, getInputProps } = useDropzone({ onDrop });

    const dispatch = useDispatch();

    const loading = useSelector(state =>
        processSelectorHelpers.getWorking(state[batchPurchaseOrderDeliveriesUpload.item])
    );

    const result = useSelector(state =>
        processSelectorHelpers.getData(state[batchPurchaseOrderDeliveriesUpload.item])
    );

    const error = useSelector(state =>
        getItemError(state, batchPurchaseOrderDeliveriesUpload.item)
    );

    const message = useSelector(state =>
        processSelectorHelpers.getMessageText(state[batchPurchaseOrderDeliveriesUpload.item])
    );

    const snackbarVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[batchPurchaseOrderDeliveriesUpload.item])
    );
    const setSnackbarVisible = () =>
        dispatch(batchPurchaseOrderDeliveriesUpload.setMessageVisible(false));

    const handleUploadClick = () => {
        dispatch(batchPurchaseOrderDeliveriesUploadActions.clearErrorsForItem());
        dispatch(batchPurchaseOrderDeliveriesUploadActions.clearProcessData());
        const reader = new FileReader();
        reader.onload = () => {
            const binaryStr = reader.result;
            dispatch(batchPurchaseOrderDeliveriesUploadActions.requestProcessStart(binaryStr));
        };
        reader.readAsArrayBuffer(file);
        setFile(null);
    };
    return (
        <>
            {error && (
                <Grid item xs={12}>
                    <ErrorCard errorMessage={error.details} />
                </Grid>
            )}
            <SnackbarMessage
                visible={snackbarVisible && result?.success}
                onClose={() => setSnackbarVisible(false)}
                message={message}
            />
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h3">Bulk Lead Time Changes Uploader</Typography>
                </Grid>
                <Grid item xs={12}>
                    <Typography variant="subtitle1">
                        Upload a CSV file with two columns: Part Number and Lead Time Weeks value
                    </Typography>
                </Grid>
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={12}>
                            <Box
                                sx={{ border: '1px dashed grey', margin: '10px' }}
                                style={{ cursor: 'pointer' }}
                                {...getRootProps()}
                            >
                                <Typography
                                    style={{ paddingTop: '10px', paddingBottom: '20px' }}
                                    variant="subtitle2"
                                >
                                    Drop the file here or click to browse...
                                </Typography>
                                <input {...getInputProps()} />

                                {file && (
                                    <Chip
                                        label={file.name}
                                        color="primary"
                                        onDelete={() => setFile(null)}
                                        variant="outlined"
                                    />
                                )}
                            </Box>
                        </Grid>
                        <Grid item xs={12}>
                            <SaveBackCancelButtons
                                backClick={() => history.push('/purchasing/part-suppliers')}
                                cancelClick={() => setFile(null)}
                                saveDisabled={!file}
                                saveClick={handleUploadClick}
                            />
                        </Grid>
                        {result && !result.success && (
                            <Grid item xs={12}>
                                <ErrorCard errorMessage={result.message} />
                            </Grid>
                        )}
                    </>
                )}
            </Grid>
        </>
    );
};

function AcknowledgeOrdersUtility() {
    const dispatch = useDispatch();

    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
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
                    id: `${i.orderNumber}/${i.orderLine}/${i.deliverySeq}`
                }))
            );
        }
    }, [items]);

    const itemsLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state[purchaseOrderDeliveries.item])
    );

    const snackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state[purchaseOrderDelivery.item])
    );

    const updateLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[purchaseOrderDelivery.item])
    );

    const updatedItem = useSelector(state =>
        itemSelectorHelpers.getItem(state[purchaseOrderDelivery.item])
    );

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'orderNumber', headerName: 'Order', width: 100 },
        { field: 'orderLine', headerName: 'Line', width: 100 },
        { field: 'deliverySeq', headerName: 'Delivery', width: 100 },
        { field: 'baseOrderUnitPrice', headerName: 'Unit Price', width: 100 },
        { field: 'partNumber', headerName: 'Part', width: 100 },
        { field: 'ourDeliveryQty', headerName: 'Qty', width: 100 },
        { field: 'dateRequested', headerName: 'Request Date', width: 100 },
        { field: 'dateAdvised', headerName: 'Advised Date', width: 100 },
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

    useEffect(() => {
        if (updatedItem) {
            dispatch(
                purchaseOrderDeliveriesActions.fetchByHref(
                    `${purchaseOrderDeliveries.uri}?${queryString.stringify(searchOptions)}`
                )
            );
        }
    }, [updatedItem, dispatch, searchOptions]);

    const handleSaveClick = () => {
        const selectedRows = rows.filter(r => r.selected);
        selectedRows.forEach(s => {
            const from = items.find(
                i => `${i.orderNumber}/${i.orderLine}/${i.deliverySeq}` === s.id
            );
            const to = {
                ...from,
                supplierConfirmationComment: newValues.supplierConfirmationComment,
                dateAdvised: newValues.dateAdvised,
                rescheduleReason: newValues.rescheduleReason,
                availableAtSupplier: newValues.availableAtSupplier
            };
            dispatch(purchaseOrderDeliveryActions.patch(s.id, { from, to }));
        });
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <SnackbarMessage
                visible={snackbarVisible}
                onClose={() => dispatch(purchaseOrderDeliveryActions.setSnackbarVisible(false))}
                message="Save Successful"
            />
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h4">Choose an option: </Typography>
                </Grid>
                <Grid item xs={12}>
                    <Accordion>
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
                                        autoHeight
                                        loading={itemsLoading || updateLoading}
                                        hideFooter
                                        checkboxSelection
                                        onSelectionModelChange={handleSelectRow}
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <Typography variant="h5">Apply Changes To Selected</Typography>
                                </Grid>
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
                                <Grid item xs={3}>
                                    <Button
                                        variant="contained"
                                        color="primary"
                                        onClick={handleSaveClick}
                                    >
                                        Save
                                    </Button>
                                </Grid>
                            </Grid>
                        </AccordionDetails>
                    </Accordion>
                    <Accordion>
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel2a-content"
                            id="panel2a-header"
                        >
                            <Typography variant="h5">Upload a File</Typography>
                        </AccordionSummary>
                        <AccordionDetails>
                            <FileUpload />
                        </AccordionDetails>
                    </Accordion>
                </Grid>
            </Grid>
        </Page>
    );
}

export default AcknowledgeOrdersUtility;
