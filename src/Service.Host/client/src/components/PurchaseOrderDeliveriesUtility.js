import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import {
    Page,
    SaveBackCancelButtons,
    SnackbarMessage,
    itemSelectorHelpers,
    getItemError,
    ErrorCard,
    InputField
} from '@linn-it/linn-form-components-library';
import { DatePicker } from '@mui/x-date-pickers';
import history from '../history';
import config from '../config';
import purchaseOrderDeliveriesActions from '../actions/purchaseOrderDeliveriesActions';
import purchaseOrderActions from '../actions/purchaseOrderActions';
import { purchaseOrderDeliveries, purchaseOrder } from '../itemTypes';

function PurchaseOrderDeliveriesUtility({
    orderNumber,
    orderLine,
    inDialogBox,
    deliveries,
    backClick,
    closeOnSave
}) {
    const dispatch = useDispatch();
    const error = useSelector(state => getItemError(state, purchaseOrderDeliveries.item));
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[purchaseOrderDeliveries.item])
    );

    const order = useSelector(state => itemSelectorHelpers.getItem(state[purchaseOrder.item]));
    const orderLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[purchaseOrder.item])
    );
    const [rows, setRows] = useState(deliveries);
    const [changesMade, setChangesMade] = useState(false);

    const getDateString = isoString => {
        if (!isoString) {
            return '__/__/____';
        }
        const date = new Date(isoString);
        return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
    };

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'deliverySeq', headerName: 'Delivery', width: 100 },
        { field: 'ourDeliveryQty', headerName: 'Qty', width: 100, editable: true },
        {
            field: 'dateRequested',
            headerName: 'Request Date',
            width: 200,
            renderCell: params => (
                <DatePicker
                    label=""
                    value={params.row.dateRequested}
                    onChange={newValue => {
                        setChangesMade(true);
                        setRows(r =>
                            r.map(x => (x.id === params.id ? { ...x, dateRequested: newValue } : x))
                        );
                    }}
                    renderInput={({ inputRef, InputProps }) => (
                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                            <span>{getDateString(params.row.dateRequested)}</span>
                            <span ref={inputRef}> {InputProps?.endAdornment}</span>
                        </Box>
                    )}
                />
            )
        },
        {
            field: 'dateAdvised',
            headerName: 'Advised Date',
            width: 200,
            renderCell: params => (
                <DatePicker
                    label=""
                    value={params.row.dateAdvised}
                    onChange={newValue => {
                        setChangesMade(true);
                        setRows(r =>
                            r.map(x => (x.id === params.id ? { ...x, dateAdvised: newValue } : x))
                        );
                    }}
                    renderInput={({ inputRef, InputProps }) => (
                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                            <span>{getDateString(params.row.dateAdvised)}</span>
                            <span ref={inputRef}> {InputProps?.endAdornment}</span>
                        </Box>
                    )}
                />
            )
        },
        {
            field: 'availableAtSupplier',
            headerName: 'Available at Supplier?',
            width: 100,
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true
        }
    ];
    const [editRowsModel, setEditRowsModel] = useState({});

    useEffect(() => {
        if (deliveries && order?.orderNumber !== orderNumber) {
            dispatch(purchaseOrderActions.fetch(deliveries[0].orderNumber));
        }
    }, [deliveries, dispatch, order, orderNumber]);

    const handleEditRowsModelChange = model => {
        setEditRowsModel(model);
        setChangesMade(true);
        if (model && Object.keys(model)[0]) {
            const id = Object.keys(model)[0];
            const propertyName = Object.keys(model[id])[0];
            if (model && model[id] && model[id][propertyName]) {
                const newValue = model[id][propertyName].value;
                setRows(r => r.map(x => (x.id === id ? { ...x, [propertyName]: newValue } : x)));
            }
        }
    };
    const handleSelectRow = selected => {
        setRows(
            rows.map(r =>
                selected.includes(r.id) ? { ...r, selected: true } : { ...r, selected: false }
            )
        );
    };

    const deleteSelected = () => {
        setRows(r => r.filter(x => !x.selected));
    };

    const purchaseOrderDeliveriesSnackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state[purchaseOrderDeliveries.item])
    );

    const addDelivery = () => {
        const deliverySeq =
            !rows || rows.length === 0 ? 1 : Math.max(...rows.map(o => o.deliverySeq)) + 1;

        setRows(r => [
            ...r,
            {
                deliverySeq,
                orderNumber,
                orderLine,
                id: `${orderNumber}/${orderLine}/${deliverySeq}`
            }
        ]);
    };
    const orderQty = () => order?.details?.find(x => x.line === deliveries[0].orderLine)?.ourQty;
    const add = (a, b) => {
        const x = Number.isNaN(a) ? 0 : a;
        const y = Number.isNaN(b) ? 0 : b;
        return x + y;
    };
    const total = () =>
        rows.length > 0
            ? rows.map(r => Number(r.ourDeliveryQty)).reduce((a, b) => add(a, b), 0)
            : 0;

    const content = () => (
        <Grid container spacing={3}>
            <SnackbarMessage
                visible={purchaseOrderDeliveriesSnackbarVisible}
                onClose={() => dispatch(purchaseOrderDeliveriesActions.setSnackbarVisible(false))}
                message="Update Successful"
            />
            <Grid item xs={12}>
                <Typography variant="h5">Deliveries</Typography>
            </Grid>
            <Grid item xs={3}>
                <InputField
                    propertyName="orderQty"
                    label="Qty On Order"
                    value={orderQty()}
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={3}>
                <InputField
                    propertyName="orderQty"
                    label="Total"
                    type="number"
                    error={!orderLoading && total() !== orderQty()}
                    value={total()}
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={6} />
            {error && (
                <Grid item xs={12}>
                    <ErrorCard errorMessage={error.details} />
                </Grid>
            )}
            <Grid item xs={12}>
                <DataGrid
                    rows={rows}
                    columns={columns}
                    rowHeight={50}
                    autoHeight
                    columnBuffer={6}
                    disableSelectionOnClick
                    loading={loading}
                    hideFooter
                    checkboxSelection
                    onSelectionModelChange={handleSelectRow}
                    editRowsModel={editRowsModel}
                    onEditRowsModelChange={handleEditRowsModelChange}
                />
            </Grid>
            <Grid item xs={1}>
                <Button variant="outlined" onClick={addDelivery}>
                    +
                </Button>
            </Grid>
            <Grid item xs={1}>
                <Button
                    variant="outlined"
                    onClick={deleteSelected}
                    disabled={
                        !rows.some(r => r.selected) ||
                        rows.length - rows.filter(r => r.selected).length === 0 ||
                        rows.length < 2
                    }
                >
                    -
                </Button>
            </Grid>
            <Grid item xs={12}>
                <SaveBackCancelButtons
                    cancelClick={() => {
                        setChangesMade(false);
                        setRows(deliveries);
                    }}
                    backClick={backClick}
                    saveDisabled={!changesMade}
                    saveClick={() => {
                        dispatch(purchaseOrderDeliveriesActions.clearErrorsForItem());
                        dispatch(
                            purchaseOrderDeliveriesActions.postByHref(
                                `/purchasing/purchase-orders/deliveries/${orderNumber}/${orderLine}`,
                                rows
                            )
                        );
                        setChangesMade(false);
                        if (closeOnSave) {
                            backClick();
                        }
                    }}
                />
            </Grid>
        </Grid>
    );

    return (
        <>
            {inDialogBox ? (
                content()
            ) : (
                <Page history={history} homeUrl={config.appRoot}>
                    {content()}
                </Page>
            )}
        </>
    );
}

PurchaseOrderDeliveriesUtility.propTypes = {
    orderNumber: PropTypes.number.isRequired,
    orderLine: PropTypes.number.isRequired,
    inDialogBox: PropTypes.bool,
    deliveries: PropTypes.arrayOf(
        PropTypes.shape({ orderNumber: PropTypes.number, orderLine: PropTypes.number })
    ),
    backClick: PropTypes.func.isRequired,
    closeOnSave: PropTypes.bool
};

PurchaseOrderDeliveriesUtility.defaultProps = {
    inDialogBox: false,
    deliveries: null,
    closeOnSave: false
};

export default PurchaseOrderDeliveriesUtility;
