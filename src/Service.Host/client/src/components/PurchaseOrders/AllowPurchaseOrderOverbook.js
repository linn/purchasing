import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    OnOffSwitch,
    InputField,
    itemSelectorHelpers,
    Loading,
    Page,
    SaveBackCancelButtons,
    SnackbarMessage,
    utilities
} from '@linn-it/linn-form-components-library';
import { useParams } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import Typography from '@mui/material/Typography';
import purchaseOrderActions from '../../actions/purchaseOrderActions';
import history from '../../history';
import config from '../../config';
import { purchaseOrder } from '../../itemTypes';

function AllowPurchaseOrderOverbook() {
    const reduxDispatch = useDispatch();
    const { orderNumber } = useParams();
    const [state, setState] = useState({});
    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.purchaseOrder));
    const overbookLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.purchaseOrder)
    );
    const canEdit = utilities.getHref(item, 'overbook');
    const clearErrors = () => reduxDispatch(purchaseOrderActions.clearErrorsForItem());
    const updatePurchaseOrder = () =>
        reduxDispatch(
            purchaseOrderActions.patch(item.orderNumber, {
                from: {
                    orderNumber: item.orderNumber,
                    overbook: item.overbook,
                    overbookQty: item.overbookQty
                },
                to: {
                    orderNumber: item.orderNumber,
                    overbook: state.overbook,
                    overbookQty: state.overbookQty
                }
            })
        );
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

    const partDetails = () => {
        if (state.details?.[0]) {
            return `${state.details[0].partNumber} ${state.details[0].partDescription}`;
        }
        return null;
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
                    <Grid item xs={11}>
                        <Typography variant="h3">Allow Overbook UT</Typography>
                    </Grid>
                    <Grid item xs={1}>
                        {canEdit ? (
                            <Tooltip title="You have write access to allow overbooking">
                                <ModeEditIcon fontSize="large" color="primary" />
                            </Tooltip>
                        ) : (
                            <Tooltip title="You do not have write access to allow overbooking">
                                <EditOffIcon fontSize="large" color="secondary" />
                            </Tooltip>
                        )}
                    </Grid>
                    <Grid item xs={12}>
                        <OnOffSwitch
                            label="Overbook"
                            value={state.overbook === 'Y'}
                            onChange={() => {
                                handleOverbookFieldChange();
                            }}
                            propertyName="overbook"
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            value={state.orderNumber}
                            label="Order Number"
                            propertyName="orderNumber"
                            onChange={() => {}}
                            disabled
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <Typography display="inline">
                            Part:
                            <br />
                            {partDetails()}
                        </Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <InputField
                            value={state.overbookQty}
                            label="Overbook Qty"
                            type="number"
                            propertyName="overbookQty"
                            onChange={handleFieldChange}
                            disabled={state.overbook !== 'Y'}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <SaveBackCancelButtons
                            saveDisabled={!canEdit || saveDisabled}
                            saveClick={() => {
                                setSaveDisabled(true);
                                clearErrors();
                                updatePurchaseOrder();
                            }}
                            backClick={() =>
                                history.push('/purchasing/purchase-orders/allow-over-book/')
                            }
                            cancelClick={() => setState(purchaseOrder)}
                        />
                    </Grid>
                </Grid>
            )}
        </Page>
    );
}
export default AllowPurchaseOrderOverbook;
