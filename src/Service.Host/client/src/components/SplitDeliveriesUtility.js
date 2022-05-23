import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    Page,
    SaveBackCancelButtons,
    collectionSelectorHelpers,
    Typeahead,
    InputField,
    SnackbarMessage,
    itemSelectorHelpers,
    Loading
} from '@linn-it/linn-form-components-library';
import history from '../history';
import config from '../config';
import purchaseOrderDeliveriesActions from '../actions/purchaseOrderDeliveriesActions';

function SplitDeliveriesUtility({ orderNumber, orderLine, inDialogBox, deliveries, cancelClick, backClick }) {
    const dispatch = useDispatch();

    // useEffect(() => {
    //     if (orderNumber && orderLine && !deliveries) {
    //         dispatch(purchaseOrderDeliveriesActions.fetch(`${orderNumber}/${orderLine}`));
    //     }
    // }, [orderNumber, orderLine, dispatch]);
    // const deliveries = useSelector(state =>
    //     itemSelectorHelpers.getItem(state.purchaseOrderDeliveries)
    // );
    // const loading = useSelector(state =>
    //     itemSelectorHelpers.getItemLoading(state.purchaseOrderDeliveries)
    // );

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'deliverySeq', headerName: 'Delivery', width: 100 },
        { field: 'ourDeliveryQty', headerName: 'Qty', width: 100 },
        { field: 'dateRequested', headerName: 'Request Date', width: 100 },
        { field: 'dateAdvised', headerName: 'Advised Date', width: 100 },
        { field: 'availableAtSupplier', headerName: 'Available at Supplier?', width: 100 }
    ];

    const content = () => (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                <DataGrid
                    rows={deliveries}
                    columns={columns}
                    rowHeight={34}
                    autoHeight
                    disableSelectionOnClick
                    loading={false}
                    hideFooter
                    // checkboxSelection
                    // onSelectionModelChange={handleSelectRow}
                />
            </Grid>
            <Grid item xs={12}>
                <SaveBackCancelButtons
                    cancelClick={cancelClick}
                    backClick={backClick}
                    saveDisabled={false}
                    saveClick={() =>
                        dispatch(
                            purchaseOrderDeliveriesActions.update(
                                `${orderNumber}/${orderLine}`,
                                deliveries
                            )
                        )
                    }
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
    deliveries: PropTypes.arrayOf(PropTypes.shape({}))
};

SplitDeliveriesUtility.defaultProps = {
    inDialogBox: false,
    deliveries: null
};

export default SplitDeliveriesUtility;
