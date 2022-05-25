import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import {
    Page,
    SaveBackCancelButtons,
    SnackbarMessage,
    itemSelectorHelpers
} from '@linn-it/linn-form-components-library';
import history from '../history';
import config from '../config';
import purchaseOrderDeliveriesActions from '../actions/purchaseOrderDeliveriesActions';
import { purchaseOrderDeliveries } from '../itemTypes';

function SplitDeliveriesUtility({ orderNumber, orderLine, inDialogBox, deliveries, backClick }) {
    const dispatch = useDispatch();

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'deliverySeq', headerName: 'Delivery', width: 100 },
        { field: 'ourDeliveryQty', headerName: 'Qty', width: 100, editable: true },
        { field: 'dateRequested', headerName: 'Request Date', width: 100 },
        { field: 'dateAdvised', headerName: 'Advised Date', width: 100 },
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
    const [changesMade, setChangesMade] = useState(false);
    const [rows, setRows] = useState(deliveries);

    const handleEditRowsModelChange = model => {
        setEditRowsModel(model);
        setChangesMade(true);
        if (model && Object.keys(model)[0]) {
            const id = Object.keys(model)[0];
            //console.log(model);
            const propertyName = Object.keys(model[id])[0];
            if (model && model[id] && model[id][propertyName] && model[id][propertyName].value) {
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

    const content = () => (
        <Grid container spacing={3}>
            <SnackbarMessage
                visible={purchaseOrderDeliveriesSnackbarVisible}
                onClose={() => dispatch(purchaseOrderDeliveriesActions.setSnackbarVisible(false))}
                message="Split Successful"
            />
            <Typography variant="h5">Split Deliveries</Typography>
            <Grid item xs={12}>
                <DataGrid
                    rows={rows}
                    columns={columns}
                    rowHeight={34}
                    autoHeight
                    disableSelectionOnClick
                    loading={false}
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
            <Grid item xs={12}>
                <SaveBackCancelButtons
                    cancelClick={() => setChangesMade(false)}
                    backClick={backClick}
                    saveDisabled={!changesMade}
                    saveClick={() => {
                        setChangesMade(false);
                        dispatch(
                            purchaseOrderDeliveriesActions.postByHref(
                                `${config.appRoot}/purchasing/purchase-orders/deliveries/${orderNumber}/${orderLine}`,
                                rows
                            )
                        );
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

SplitDeliveriesUtility.propTypes = {
    orderNumber: PropTypes.number.isRequired,
    orderLine: PropTypes.number.isRequired,
    inDialogBox: PropTypes.bool,
    deliveries: PropTypes.arrayOf(PropTypes.shape({})),
    backClick: PropTypes.func.isRequired
};

SplitDeliveriesUtility.defaultProps = {
    inDialogBox: false,
    deliveries: null
};

export default SplitDeliveriesUtility;
