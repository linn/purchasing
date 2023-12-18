import React, { useEffect, useReducer, useState, useMemo } from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';
import {
    itemSelectorHelpers,
    Page,
    Loading,
    SaveBackCancelButtons,
    utilities,
    InputField,
    ErrorCard,
    getItemError,
    SnackbarMessage,
    userSelectors
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
import ContactTab from './tabs/ContactTab';

function Supplier({ creating }) {
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
    const itemError = useSelector(reduxState => getItemError(reduxState, 'supplier'));

    const { id } = useParams();
    const supplier = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.supplier));
    const supplierLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.supplier)
    );

    const holdChangeLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.putSupplierOnHold)
    );
    const currentUserNumber = useSelector(reduxState => userSelectors.getUserNumber(reduxState));
    const currentUserName = useSelector(reduxState => userSelectors.getName(reduxState));

    const clearErrors = () => reduxDispatch(supplierActions.clearErrorsForItem());
    const updateSupplier = body => reduxDispatch(supplierActions.update(id, body));
    const addSupplier = body => reduxDispatch(supplierActions.add(body));
    const snackbarVisible = useSelector(reduxState =>
        itemSelectorHelpers.getSnackbarVisible(reduxState.supplier)
    );
    const bulkUpdateLeadTimesUrl = utilities.getHref(state.supplier, 'bulk-update-lead-times');
    const defaults = useMemo(
        () => ({
            accountingCompany: 'LINN',
            expenseAccount: 'N',
            currencyCode: 'GBP',
            paysInFc: 'N',
            approvedCarrier: 'N',
            paymentMethod: 'CHEQUE',
            invoiceContactMethod: 'EMAIL',
            orderContactMethod: 'EMAIL',
            paymentDays: 30,
            orderHold: 'N',
            openedById: Number(currentUserNumber),
            openedByName: currentUserName,
            vendorManagerId: 'A',
            supplierContacts: []
        }),
        [currentUserNumber, currentUserName]
    );
    useEffect(() => {
        if (creating) {
            dispatch({
                type: 'initialise',
                payload: defaults
            });
        } else if (supplier) {
            dispatch({ type: 'initialise', payload: supplier });
        }
    }, [supplier, creating, currentUserName, currentUserNumber, defaults]);

    const canEdit = () => creating || supplier?.links.some(l => l.rel === 'edit');
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
        if (propertyName === 'plannerId' || propertyName === 'groupId') {
            formatted = Number(newValue);
        }

        dispatch({ type: 'fieldChange', fieldName: propertyName, payload: formatted });
        if (propertyName === 'currencyCode') {
            if (newValue === 'GBP') {
                if (state.supplier.paymentMethod === 'FORPAY') {
                    dispatch({ type: 'fieldChange', fieldName: 'paysInFc', payload: 'A' });
                } else {
                    dispatch({ type: 'fieldChange', fieldName: 'paysInFc', payload: 'N' });
                }
            } else {
                dispatch({ type: 'fieldChange', fieldName: 'paysInFc', payload: 'A' });
            }
        }
        if (propertyName === 'paymentMethod') {
            if (state.supplier.currencyCode === 'GBP' && newValue === 'FORPAY') {
                dispatch({ type: 'fieldChange', fieldName: 'paysInFc', payload: 'A' });
            }
        }
    };

    const [holdReason, setHoldReason] = useState('');

    const editStatus = useSelector(reduxState =>
        itemSelectorHelpers.getItemEditStatus(reduxState.supplier)
    );

    const userNumber = useSelector(reduxState => getUserNumber(reduxState));

    const [holdChangeDialogOpen, setHoldChangeDialogOpen] = useState(false);
    const [newCounter, setNewCounter] = useState(1);
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
    const addContact = () => {
        setEditStatus('edit');
        dispatch({
            type: 'addContact',
            payload: {
                supplierId: state.supplier.supplierId,
                id: 0 - newCounter,
                personId: 0 - newCounter
            }
        });
        setNewCounter(c => c + 1);
    };
    const deleteContacts = ids => {
        setEditStatus('edit');
        dispatch({
            type: 'deleteContacts',
            payload: ids
        });
    };
    const updateContact = newRow => {
        setEditStatus('edit');
        dispatch({ type: 'updateContact', payload: newRow });
    };
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <SnackbarMessage
                    visible={snackbarVisible}
                    onClose={() => reduxDispatch(supplierActions.setSnackbarVisible(false))}
                    message="Save Successful"
                />
                {supplierLoading || holdChangeLoading ? (
                    <>
                        <Grid item xs={12}>
                            <Loading />
                        </Grid>
                    </>
                ) : (
                    <>
                        {state.supplier && (
                            <>
                                {itemError && (
                                    <Grid item xs={12}>
                                        <ErrorCard errorMessage={itemError.details} />
                                    </Grid>
                                )}
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
                                                    onChange={(_, newValue) =>
                                                        setHoldReason(newValue)
                                                    }
                                                />
                                            </Grid>
                                            <Grid item xs={12}>
                                                <SaveBackCancelButtons
                                                    backClick={() => setHoldChangeDialogOpen(false)}
                                                    cancelClick={() =>
                                                        setHoldChangeDialogOpen(false)
                                                    }
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
                                                <Tab label="Contacts" />
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
                                                    accountingCompany={
                                                        state.supplier.accountingCompany
                                                    }
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
                                                    invoiceGoesToName={
                                                        state.supplier.invoiceGoesToName
                                                    }
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
                                                    openHoldDialog={() =>
                                                        setHoldChangeDialogOpen(true)
                                                    }
                                                    bulkUpdateLeadTimesUrl={bulkUpdateLeadTimesUrl}
                                                    groupId={state.supplier.groupId}
                                                    receivesPurchaseOrderReminders={
                                                        state.supplier
                                                            .receivesPurchaseOrderReminders
                                                    }
                                                    printTerms={state.supplier.printTerms}
                                                />
                                            </Box>
                                        )}
                                        {tab === 3 && (
                                            <Box sx={{ paddingTop: 3 }}>
                                                <WhereTab
                                                    orderAddressId={state.supplier.orderAddressId}
                                                    orderFullAddress={
                                                        state.supplier.orderFullAddress
                                                    }
                                                    invoiceAddressId={
                                                        state.supplier.invoiceAddressId
                                                    }
                                                    invoiceFullAddress={
                                                        state.supplier.invoiceFullAddress
                                                    }
                                                    country={state.supplier.country}
                                                    handleFieldChange={handleFieldChange}
                                                    supplierName={state.supplier.name}
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
                                                    closedById={state.supplier.closedById}
                                                    closedByName={state.supplier.closedByName}
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
                                        {tab === 7 && (
                                            <Box sx={{ paddingTop: 3 }}>
                                                <ContactTab
                                                    contacts={state.supplier.supplierContacts}
                                                    updateContact={updateContact}
                                                    addContact={addContact}
                                                    deleteContacts={deleteContacts}
                                                />
                                            </Box>
                                        )}
                                    </Box>
                                </Grid>
                                <Grid item xs={12}>
                                    <SaveBackCancelButtons
                                        saveDisabled={!canEdit() || editStatus === 'view'}
                                        saveClick={() => {
                                            if (creating) {
                                                clearErrors();
                                                addSupplier(state.supplier);
                                            } else {
                                                clearErrors();
                                                updateSupplier({
                                                    ...state.supplier,
                                                    closedById: state.supplier.reasonClosed
                                                        ? Number(currentUserNumber)
                                                        : null
                                                });
                                            }
                                        }}
                                        cancelClick={() => {
                                            if (creating) {
                                                dispatch({ type: 'initialise', payload: defaults });
                                            } else {
                                                dispatch({ type: 'initialise', payload: supplier });
                                            }
                                            setEditStatus('view');
                                        }}
                                        backClick={() => history.push('/purchasing/suppliers')}
                                    />
                                </Grid>
                            </>
                        )}
                    </>
                )}
            </Grid>
        </Page>
    );
}

Supplier.propTypes = { creating: PropTypes.bool };
Supplier.defaultProps = { creating: false };

export default Supplier;
