import React, { useEffect, useReducer, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    itemSelectorHelpers,
    Page,
    Loading,
    SaveBackCancelButtons,
    utilities,
    InputField
} from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { useParams } from 'react-router-dom';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Dialog from '@mui/material/Dialog';
import { makeStyles } from '@mui/styles';
import IconButton from '@mui/material/IconButton';
import Close from '@mui/icons-material/Close';
import Box from '@mui/material/Box';
import supplierActions from '../../actions/supplierActions';
import putSupplierOnHoldActions from '../../actions/putSupplierOnHoldActions';
import history from '../../history';
import config from '../../config';
import supplierReducer from './supplierReducer';
import GeneralTab from './tabs/GeneralTab';
import FinanceTab from './tabs/FinanceTab';
import PurchTab from './tabs/PurchTab';
import { getUserNumber } from '../../selectors/oidcSelectors';
import WhereTab from './tabs/WhereTab';
import WhoseTab from './tabs/WhoseTab';
import LifecycleTab from './tabs/LifecycleTab';
import NotesTab from './tabs/NotesTab';

function Supplier() {
    const useStyles = makeStyles(theme => ({
        dialog: {
            margin: theme.spacing(6),
            minWidth: theme.spacing(62)
        },
        total: {
            float: 'right'
        }
    }));
    const classes = useStyles();

    const reduxDispatch = useDispatch();

    const [state, dispatch] = useReducer(supplierReducer, {
        supplier: {},
        prevPart: {}
    });

    const { id } = useParams();
    const supplier = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.supplier));
    const supplierLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.supplier)
    );

    const holdChangeLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.putSupplierOnHold)
    );

    const clearErrors = () => reduxDispatch(supplierActions.clearErrorsForItem());
    const updateSupplier = body => reduxDispatch(supplierActions.update(id, body));

    useEffect(() => {
        if (supplier) {
            dispatch({ type: 'initialise', payload: supplier });
        }
    }, [supplier]);

    const canEdit = () => supplier?.links.some(l => l.rel === 'edit');
    const holdLink = () => utilities.getHref(supplier, 'hold');

    useEffect(() => {
        if (id) {
            reduxDispatch(supplierActions.fetch(id));
        }
    }, [id, reduxDispatch]);
    const [tab, setTab] = useState(0);
    const setEditStatus = status => reduxDispatch(supplierActions.setEditStatus(status));

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        let formatted = newValue;
        if (propertyName === 'plannerId') {
            formatted = Number(newValue);
        }
        dispatch({ type: 'fieldChange', fieldName: propertyName, payload: formatted });
    };

    const [holdReason, setHoldReason] = useState('');

    const editStatus = useSelector(reduxState =>
        itemSelectorHelpers.getItemEditStatus(reduxState.supplier)
    );

    const userNumber = useSelector(reduxState => getUserNumber(reduxState));

    const [holdChangeDialogOpen, setHoldChangeDialogOpen] = useState(false);

    const changeSupplierHoldStatus = () => {
        if (state.supplier.orderHold === 'Y') {
            reduxDispatch(
                putSupplierOnHoldActions.add({
                    reasonOffHold: holdReason,
                    takenOffHoldBy: userNumber,
                    supplierId: state.supplier.id
                })
            );
        } else {
            reduxDispatch(
                putSupplierOnHoldActions.add({
                    reasonOnHold: holdReason,
                    putOnHoldBy: userNumber,
                    supplierId: state.supplier.id
                })
            );
        }
        setHoldChangeDialogOpen(false);
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                {supplierLoading || holdChangeLoading ? (
                    <>
                        <Grid item xs={12}>
                            <Loading />
                        </Grid>
                    </>
                ) : (
                    state.supplier && (
                        <>
                            <Dialog open={holdChangeDialogOpen} fullWidth maxWidth="md">
                                <div>
                                    <IconButton
                                        className={classes.pullRight}
                                        aria-label="Close"
                                        onClick={() => setHoldChangeDialogOpen(false)}
                                    >
                                        <Close />
                                    </IconButton>
                                    <div className={classes.dialog}>
                                        <Grid item xs={12}>
                                            <InputField
                                                fullWidth
                                                value={holdReason}
                                                label="Must give a reason:"
                                                propertyName="holdReason"
                                                onChange={(_, newValue) => setHoldReason(newValue)}
                                            />
                                        </Grid>
                                        <Grid item xs={12}>
                                            <SaveBackCancelButtons
                                                backClick={() => setHoldChangeDialogOpen(false)}
                                                cancelClick={() => setHoldChangeDialogOpen(false)}
                                                saveClick={changeSupplierHoldStatus}
                                            />
                                        </Grid>
                                    </div>
                                </div>
                            </Dialog>
                            <Grid item xs={3}>
                                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                    <Typography variant="h6">{state.supplier.id}</Typography>
                                </Box>
                            </Grid>
                            <Grid item xs={7}>
                                <Box
                                    sx={{
                                        borderBottom: 1,
                                        borderColor: 'divider'
                                    }}
                                >
                                    <Typography variant="h6">{state.supplier.name}</Typography>
                                </Box>
                            </Grid>
                            <Grid item xs={1} />
                            <Grid item xs={1}>
                                {canEdit() ? (
                                    <Tooltip title="You have write access to Suppliers">
                                        <ModeEditIcon fontSize="large" color="primary" />
                                    </Tooltip>
                                ) : (
                                    <Tooltip title="You do not have write access to Suppliers">
                                        <EditOffIcon fontSize="large" color="secondary" />
                                    </Tooltip>
                                )}
                            </Grid>
                            <Grid item xs={12}>
                                <Box sx={{ width: '100%' }}>
                                    <Box sx={{ borderBottom: 0, borderColor: 'divider' }}>
                                        <Tabs
                                            value={tab}
                                            onChange={(event, newValue) => {
                                                setTab(newValue);
                                            }}
                                        >
                                            <Tab label="General" />
                                            <Tab label="Finance" />
                                            <Tab label="Purch" />
                                            <Tab label="Where" />
                                            <Tab label="Whose" />
                                            <Tab label="Lifecycle" />
                                            <Tab label="Notes" />
                                        </Tabs>
                                    </Box>
                                    {tab === 0 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <GeneralTab
                                                name={state.supplier.name}
                                                phoneNumber={state.supplier.phoneNumber}
                                                webAddress={state.supplier.webAddress}
                                                orderContactMethod={
                                                    state.supplier.orderContactMethod
                                                }
                                                invoiceContactMethod={
                                                    state.supplier.invoiceContactMethod
                                                }
                                                suppliersReference={
                                                    state.supplier.suppliersReference
                                                }
                                                liveOnOracle={state.supplier.liveOnOracle}
                                                accountingCompany={state.supplier.accountingCompany}
                                                handleFieldChange={handleFieldChange}
                                            />
                                        </Box>
                                    )}
                                    {tab === 1 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <FinanceTab
                                                handleFieldChange={handleFieldChange}
                                                vatNumber={state.supplier.vatNumber}
                                                invoiceGoesToId={state.supplier.invoiceGoesToId}
                                                invoiceGoesToName={state.supplier.invoiceGoesToName}
                                                expenseAccount={state.supplier.expenseAccount}
                                                paymentDays={state.supplier.paymentDays}
                                                paymentMethod={state.supplier.paymentMethod}
                                                currencyCode={state.supplier.currencyCode}
                                                paysInFc={state.supplier.paysInFc}
                                                approvedCarrier={state.supplier.approvedCarrier}
                                            />
                                        </Box>
                                    )}
                                    {tab === 2 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <PurchTab
                                                handleFieldChange={handleFieldChange}
                                                partCategory={state.supplier.partCategory}
                                                partCategoryDescription={
                                                    state.supplier.partCategoryDescription
                                                }
                                                orderHold={state.supplier.orderHold}
                                                notesForBuyer={state.supplier.notesForBuyer}
                                                deliveryDay={state.supplier.deliveryDay}
                                                refersToFcId={state.supplier.refersToFcId}
                                                refersToFcName={state.supplier.refersToFcName}
                                                pmDeliveryDaysGrace={
                                                    state.supplier.pmDeliveryDaysGrace
                                                }
                                                holdLink={holdLink()}
                                                openHoldDialog={() => setHoldChangeDialogOpen(true)}
                                            />
                                        </Box>
                                    )}
                                    {tab === 3 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <WhereTab
                                                orderAddressId={state.supplier.orderAddressId}
                                                orderFullAddress={state.supplier.orderFullAddress}
                                                invoiceAddressId={state.supplier.invoiceAddressId}
                                                invoiceFullAddress={
                                                    state.supplier.invoiceFullAddress
                                                }
                                                handleFieldChange={handleFieldChange}
                                            />
                                        </Box>
                                    )}
                                    {tab === 4 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <WhoseTab
                                                handleFieldChange={handleFieldChange}
                                                accountControllerId={
                                                    state.supplier.accountControllerId
                                                }
                                                accountControllerName={
                                                    state.supplier.accountControllerName
                                                }
                                                vendorManagerId={state.supplier.vendorManagerId}
                                                plannerId={state.supplier.plannerId}
                                            />
                                        </Box>
                                    )}
                                    {tab === 5 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <LifecycleTab
                                                handleFieldChange={handleFieldChange}
                                                openedById={state.supplier.openedById}
                                                openedByName={state.supplier.openedByName}
                                                dateOpened={state.supplier.dateOpened}
                                                closedById={state.supplier.plannerId}
                                                dateClosed={state.supplier.dateClosed}
                                                reasonClosed={state.supplier.reasonClosed}
                                            />
                                        </Box>
                                    )}
                                    {tab === 6 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <NotesTab
                                                handleFieldChange={handleFieldChange}
                                                notes={state.supplier.notes}
                                                organisationId={state.supplier.organisationId}
                                            />
                                        </Box>
                                    )}
                                </Box>
                            </Grid>
                            <Grid item xs={12}>
                                <SaveBackCancelButtons
                                    saveDisabled={!canEdit() || editStatus === 'view'}
                                    saveClick={() => {
                                        clearErrors();
                                        updateSupplier(state.supplier);
                                    }}
                                    cancelClick={() => {
                                        dispatch({ type: 'initialise', payload: supplier });
                                        setEditStatus('view');
                                    }}
                                    backClick={() => history.push('/purchasing/part-suppliers')}
                                />
                            </Grid>
                        </>
                    )
                )}
            </Grid>
        </Page>
    );
}

export default Supplier;
