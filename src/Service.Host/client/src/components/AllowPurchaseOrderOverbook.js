import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    OnOffSwitch,
    InputField,
    itemSelectorHelpers,
    Loading,
    Page,
    SaveBackCancelButtons,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import { useParams } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import purchaseOrderActions from '../actions/purchaseOrderActions';
import history from '../history';
import config from '../config';

function AllowPurchaseOrderOverbook() {
    const reduxDispatch = useDispatch();
    const { orderNumber } = useParams();
    const [state, setState] = useState({});
    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.purchaseOrder));
    const overbookLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.purchaseOrder)
    );

    const clearErrors = () => reduxDispatch(purchaseOrderActions.clearErrorsForItem());
    const updatePurchaseOrder = () =>
        reduxDispatch(purchaseOrderActions.update(state.orderNumber, state));
    const snackbarVisible = useSelector(reduxState =>
        itemSelectorHelpers.getSnackbarVisible(reduxState.purchaseOrder)
    );

    useEffect(() => {
        if (orderNumber) {
            reduxDispatch(purchaseOrderActions.fetch(orderNumber));
        }
    }, [orderNumber, reduxDispatch]);

    useEffect(() => {
        if (item?.orderNumber) {
            setState(item);
        }
    }, [item]);

    const [saveDisabled, setSaveDisabled] = useState(true);
    const handleFieldChange = (propertyName, newValue) => {
        setState(d => ({ ...d, [propertyName]: newValue }));
        setSaveDisabled(false);
    };

    const handleOverbookFieldChange = () => {
        if (state.overbook !== 'Y') {
            setState(d => ({ ...d, overbook: 'Y' }));
        } else {
            setState(d => ({ ...d, overbook: 'N' }));
        }
        setSaveDisabled(false);
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            {overbookLoading ? (
                <Loading />
            ) : (
                <Grid container spacing={3}>
                    <SnackbarMessage
                        visible={snackbarVisible}
                        onClose={() =>
                            reduxDispatch(purchaseOrderActions.setSnackbarVisible(false))
                        }
                        message="Save Successful"
                    />
                    <Grid item xs={12}>
                        <Typography variant="h3">Allow Overbook UT</Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <OnOffSwitch
                            label="Overbook"
                            value={state?.overbook === 'Y'}
                            onChange={() => {
                                handleOverbookFieldChange();
                            }}
                            propertyName="overbook"
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <InputField
                            value={state?.orderNumber}
                            label="Order Number"
                            propertyName="orderNumber"
                            onChange={() => {}}
                            disabled
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <InputField
                            value={state?.overbookQty}
                            label="Overbook Qty"
                            type="number"
                            propertyName="overbookQty"
                            onChange={handleFieldChange}
                            disabled={state.overbook !== 'Y'}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <SaveBackCancelButtons
                            saveDisabled={saveDisabled}
                            saveClick={() => {
                                setSaveDisabled(true);
                                clearErrors();
                                updatePurchaseOrder();
                            }}
                            backClick={() =>
                                history.push('/purchasing/purchase-orders/allow-over-book/')
                            }
                        />
                    </Grid>
                </Grid>
            )}
        </Page>
    );
}
export default AllowPurchaseOrderOverbook;
