import React, { useEffect, useState } from 'react';
import { useMediaQuery } from 'react-responsive';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import { Decimal } from 'decimal.js';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { useParams } from 'react-router-dom';
import Dialog from '@mui/material/Dialog';
import IconButton from '@mui/material/IconButton';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import Close from '@mui/icons-material/Close';
import PrintIcon from '@mui/icons-material/Print';
import Email from '@mui/icons-material/Email';
import Send from '@mui/icons-material/Send';
import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';
import Collapse from '@mui/material/Collapse';
import PageviewIcon from '@mui/icons-material/Pageview';
import { makeStyles } from '@mui/styles';
import {
    Page,
    SaveBackCancelButtons,
    collectionSelectorHelpers,
    Typeahead,
    InputField,
    SnackbarMessage,
    itemSelectorHelpers,
    Loading,
    Dropdown,
    TypeaheadTable,
    userSelectors,
    getItemError,
    ErrorCard,
    processSelectorHelpers,
    getPreviousPaths,
    utilities,
    OnOffSwitch
} from '@linn-it/linn-form-components-library';
import currenciesActions from '../../actions/currenciesActions';
import employeesActions from '../../actions/employeesActions';
import nominalsActions from '../../actions/nominalsActions';
import countriesActions from '../../actions/countriesActions';
import suppliersActions from '../../actions/suppliersActions';
import partsActions from '../../actions/partsActions';
import poReqActions from '../../actions/purchaseOrderReqActions';
import poReqApplicationStateActions from '../../actions/purchaseOrderReqApplicationStateActions';
import purchaseOrderReqStatesActions from '../../actions/purchaseOrderReqStatesActions';
import sendReqAuthEmailActions from '../../actions/sendPurchaseOrderReqAuthEmailActions';
import sendReqFinanceEmailActions from '../../actions/sendPurchaseOrderReqFinanceEmailActions';
import history from '../../history';
import config from '../../config';
import {
    sendPurchaseOrderReqAuthEmail,
    sendPurchaseOrderReqFinanceEmail,
    pOReqCheckIfCanAuthOrder,
    sendPurchaseOrderPdfEmail
} from '../../itemTypes';
import checkIfCanAuthorisePurchaseOrderActions from '../../actions/checkIfCanAuthorisePurchaseOrderActions';
import handleBackClick from '../../helpers/handleBackClick';
import sendPurchaseOrderPdfEmailActionTypes from '../../actions/sendPurchaseOrderPdfEmailActions';

function POReqUtility({ creating }) {
    const dispatch = useDispatch();
    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'id', 'name')
    );
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );
    const searchSuppliers = searchTerm => dispatch(suppliersActions.search(searchTerm));

    const partsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.parts)
    ).map?.(c => ({
        id: c.partNumber,
        name: c.partNumber,
        partNumber: c.partNumber,
        description: c.description
    }));

    const partsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.parts)
    );

    const countriesSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state.countries,
            100,
            'countryCode',
            'countryCode',
            'countryName'
        )
    );
    const countriesSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.countries)
    );
    const searchCountries = searchTerm => dispatch(countriesActions.search(searchTerm));

    const currencies = useSelector(state => collectionSelectorHelpers.getItems(state.currencies));
    const employees = useSelector(state => collectionSelectorHelpers.getItems(state.employees));

    const reqStates = useSelector(state =>
        collectionSelectorHelpers.getItems(state.purchaseOrderReqStates)
    );

    const nominalsSearchItems = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.nominals)
    );
    const nominalsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.nominals)
    );

    const currentUserId = useSelector(state => userSelectors.getUserNumber(state));
    const currentUserName = useSelector(state => userSelectors.getName(state));

    const previousPaths = useSelector(state => getPreviousPaths(state));

    const defaultCreatingReq = {
        requestedBy: {
            id: parseInt(currentUserId, 10),
            fullName: currentUserName
        },
        reqNumber: 'creating',
        state: 'AUTHORISE WAIT',
        reqDate: new Date().toDateString(),
        partNumber: 'SUNDRY',
        currency: { code: 'GBP', name: 'UK Sterling' }
    };

    const [req, setReq] = useState(defaultCreatingReq);
    const snackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state.purchaseOrderReq)
    );
    const [snackbarMessage, setSnackbarMessage] = useState('Save successful');
    const authEmailMessageVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[sendPurchaseOrderReqAuthEmail.item])
    );

    const authEmailMessage = useSelector(state =>
        processSelectorHelpers.getMessageText(state[sendPurchaseOrderReqAuthEmail.item])
    );

    const financeEmailMessageVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[sendPurchaseOrderReqFinanceEmail.item])
    );

    const financeEmailMessage = useSelector(state =>
        processSelectorHelpers.getMessageText(state[sendPurchaseOrderReqFinanceEmail.item])
    );

    const canAuthOrderMessage = useSelector(state =>
        processSelectorHelpers.getMessageText(state[pOReqCheckIfCanAuthOrder.item])
    );

    const canAuthOrderMessageVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[pOReqCheckIfCanAuthOrder.item])
    );

    const orderPdfEmailMessageVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[sendPurchaseOrderPdfEmail.item])
    );

    const orderPdfEmailMessage = useSelector(state =>
        processSelectorHelpers.getMessageText(state[sendPurchaseOrderPdfEmail.item])
    );

    const loading = useSelector(state =>
        creating
            ? itemSelectorHelpers.getApplicationStateLoading(state.purchaseOrderReqApplicationState)
            : itemSelectorHelpers.getItemLoading(state.purchaseOrderReq)
    );
    const item = useSelector(state => itemSelectorHelpers.getItem(state.purchaseOrderReq));
    const clearErrors = () => dispatch(poReqActions.clearErrorsForItem());
    const itemError = useSelector(state => getItemError(state, 'purchaseOrderReq'));

    const [editStatus, setEditStatus] = useState('view');
    const [explainDialogOpen, setExplainDialogOpen] = useState(false);
    const [authEmailDialogOpen, setAuthEmailDialogOpen] = useState(false);
    const [financeEmailDialogOpen, setFinanceEmailDialogOpen] = useState(false);
    const [employeeToEmail, setEmployeeToEmail] = useState();
    const [signingLimitDialogOpen, setSigningLimitDialogOpen] = useState(false);
    const [orderPdfEmailDialogOpen, setOrderPdfEmailDialogOpen] = useState(false);
    const [purchaseOrderEmailState, setPurchaseOrderEmailState] = useState({
        email: '',
        bcc: false
    });

    useEffect(() => {
        if (!creating && item?.reqNumber) {
            setReq(item);
            setPurchaseOrderEmailState({ bcc: false, email: item.email });
        } else {
            dispatch(poReqActions.clearErrorsForItem());
        }
    }, [item, creating, dispatch]);

    useEffect(() => dispatch(currenciesActions.fetch()), [dispatch]);
    useEffect(() => dispatch(employeesActions.fetch()), [dispatch]);
    useEffect(() => dispatch(purchaseOrderReqStatesActions.fetch()), [dispatch]);

    const { id } = useParams();
    useEffect(() => {
        if (id) {
            dispatch(poReqActions.fetch(id));
        } else if (creating) {
            dispatch(poReqApplicationStateActions.fetchState());
        }
    }, [id, dispatch, creating]);

    const handleSupplierChange = newSupplier => {
        setEditStatus('edit');
        let supplierContact = newSupplier.supplierContacts?.find(x => x.isMainOrderContact === 'Y');
        if (!supplierContact && newSupplier.supplierContacts?.length) {
            [supplierContact] = newSupplier.supplierContacts;
        }
        setReq(a => ({
            ...a,
            supplier: { id: newSupplier.id, name: newSupplier.description },
            supplierContact: supplierContact?.contactName,
            email: supplierContact?.emailAddress,
            phoneNumber: supplierContact?.phoneNumber,
            currency: {
                code: newSupplier.currency?.code ?? req.currency?.code,
                name: newSupplier.currency?.name ?? req.currency?.name
            },
            country: {
                countryCode: newSupplier.country ?? req.country.countryCode
            },
            addressLine1: newSupplier.orderAddress?.line1,
            addressLine2: newSupplier.orderAddress?.line2,
            addressLine3: newSupplier.orderAddress?.line3,
            addressLine4: newSupplier.orderAddress?.line4,
            postCode: newSupplier.orderAddress?.postCode
        }));
    };

    const nominalAccountsTable = {
        totalItemCount: nominalsSearchItems.length,
        rows: nominalsSearchItems?.map(nom => ({
            id: nom.nominalAccountId,
            values: [
                { id: 'nominalCode', value: `${nom.nominalCode}` },
                { id: 'description', value: `${nom.description || ''}` },
                { id: 'departmentCode', value: `${nom.departmentCode || ''}` },
                { id: 'departmentDescription', value: `${nom.departmentDescription || ''}` }
            ],
            links: nom.links
        }))
    };

    const allowedToCancel = () => !creating && req.links?.some(l => l.rel === 'cancel');
    const allowedToAuthorise = () => !creating && req.state === 'AUTHORISE WAIT';
    const allowedTo2ndAuthorise = () => !creating && req.state === 'AUTHORISE 2ND WAIT';
    const userHasFinancePower = () => req.links?.some(l => l.rel === 'finance-check');
    const allowedToFinanceCheck = () =>
        !creating && userHasFinancePower && req.state === 'FINANCE WAIT';
    const allowedToCreateOrder = () =>
        !creating &&
        req.links?.some(l => l.rel === 'turn-req-into-purchase-order') &&
        req.state === 'ORDER WAIT';

    const hasOrderNumber = () => req.links?.some(l => l.rel === 'view-purchase-order');

    const editingAllowed = req?.state !== 'CANCELLED';

    const inputIsInvalid = () =>
        !`${req.supplier?.id}`?.length ||
        !req.supplier?.name?.length ||
        !req.state.length ||
        !req.reqDate.length ||
        !`${req.qty}`.length ||
        !`${req.unitPrice}`.length ||
        !req.currency?.code?.length ||
        !req.country?.countryCode?.length ||
        !req.nominal?.nominalCode.length ||
        !req.department?.departmentCode.length;

    const canSave = () =>
        editStatus !== 'view' && editingAllowed && !inputIsInvalid() && req !== item;

    const handleAuthorise = () => {
        setEditStatus('edit');
        if (allowedToAuthorise) {
            setSnackbarMessage('Authorisation saved');
            clearErrors();
            dispatch(poReqActions.postByHref(utilities.getHref(req, 'authorise')));
        }
    };

    const handleSecondAuth = () => {
        setEditStatus('edit');
        if (allowedTo2ndAuthorise) {
            setSnackbarMessage('Second auth saved');
            clearErrors();
            dispatch(poReqActions.postByHref(utilities.getHref(req, 'authorise')));
        }
    };

    const handleFinanceCheck = () => {
        setEditStatus('edit');
        if (allowedToFinanceCheck) {
            clearErrors();
            setSnackbarMessage('Finance auth saved');
            dispatch(poReqActions.postByHref(utilities.getHref(req, 'finance-check')));
        }
    };

    const handleCreateOrderButton = () => {
        dispatch(checkIfCanAuthorisePurchaseOrderActions.clearErrorsForItem());
        dispatch(checkIfCanAuthorisePurchaseOrderActions.clearProcessData());
        dispatch(
            checkIfCanAuthorisePurchaseOrderActions.requestProcessStart('', {
                reqNumber: id
            })
        );
        setSigningLimitDialogOpen(true);
    };

    const handleActuallyCreateOrder = () => {
        setEditStatus('edit');
        if (allowedToCreateOrder) {
            clearErrors();
            setSnackbarMessage('Order created. See order number field and view order button');
            dispatch(
                poReqActions.postByHref(utilities.getHref(req, 'turn-req-into-purchase-order'))
            );
            setSigningLimitDialogOpen(false);
            setOrderPdfEmailDialogOpen(true);
        }
    };

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        setReq(a => ({ ...a, [propertyName]: newValue }));
    };

    const handleCancelClick = () => {
        if (allowedToCancel) {
            clearErrors();
            dispatch(poReqActions.postByHref(utilities.getHref(req, 'cancel')));
        }
    };

    const handleSendAuthoriseEmailClick = () => {
        setAuthEmailDialogOpen(false);
        dispatch(sendReqAuthEmailActions.clearProcessData);
        dispatch(
            sendReqAuthEmailActions.requestProcessStart('', {
                reqNumber: id,
                toEmployeeId: employeeToEmail
            })
        );
    };

    const handleSendFinanceEmailClick = () => {
        setFinanceEmailDialogOpen(false);
        dispatch(sendReqFinanceEmailActions.clearProcessData);
        dispatch(
            sendReqFinanceEmailActions.requestProcessStart('', {
                reqNumber: id,
                toEmployeeId: employeeToEmail
            })
        );
    };

    const handleOrderPdfEmailClick = () => {
        setOrderPdfEmailDialogOpen(false);
        dispatch(sendPurchaseOrderPdfEmailActionTypes.clearProcessData);
        dispatch(
            sendPurchaseOrderPdfEmailActionTypes.requestProcessStart('', {
                orderNumber: req.orderNumber,
                emailAddress: purchaseOrderEmailState.email,
                bcc: purchaseOrderEmailState.bcc
            })
        );
    };

    const handleNominalUpdate = newNominal => {
        setEditStatus('edit');

        setReq(r => ({
            ...r,
            nominal: {
                nominalCode: newNominal.values.find(x => x.id === 'nominalCode')?.value,
                description: newNominal.values.find(x => x.id === 'description')?.value
            },
            department: {
                departmentCode: newNominal.values.find(x => x.id === 'departmentCode')?.value,
                description: newNominal.values.find(x => x.id === 'departmentDescription')?.value
            }
        }));
    };

    const [showCostWarning, setShowCostWarning] = useState(false);
    const [alreadyShownCostWarning, setAlreadyShownCostWarning] = useState(false);

    useEffect(() => {
        if (req.unitPrice && req.qty) {
            let total = Decimal.mul(req.unitPrice, req.qty);
            if (req.carriage) {
                total = Decimal.add(total, req.carriage);
            }
            if (!alreadyShownCostWarning && total > 250) {
                setShowCostWarning(true);
                setAlreadyShownCostWarning(true);
            }
            setReq(r => ({
                ...r,
                totalReqPrice: parseFloat(total).toFixed(2)
            }));
        }
    }, [req.qty, req.carriage, req.unitPrice, alreadyShownCostWarning, creating]);

    const useStyles = makeStyles(theme => ({
        buttonMarginTop: {
            marginTop: '28px',
            height: '40px'
        },
        centerTextInDialog: {
            textAlign: 'center',
            margin: theme.spacing(2)
        },
        cursorPointer: {
            '&:hover': {
                cursor: 'pointer'
            },
            marginLeft: theme.spacing(2)
        },
        centeredIcon: {
            textAlign: 'center'
        },
        pullRight: {
            float: 'right'
        }
    }));

    const classes = useStyles();
    const screenIsSmall = useMediaQuery({ query: `(max-width: 1024px)` });

    return (
        <>
            <Page history={history} homeUrl={config.appRoot} width={screenIsSmall ? 'xl' : 'm'}>
                {loading ? (
                    <Loading />
                ) : (
                    <Grid container spacing={2} justifyContent="center">
                        <SnackbarMessage
                            visible={snackbarVisible}
                            onClose={() => dispatch(poReqActions.setSnackbarVisible(false))}
                            message={snackbarMessage}
                        />
                        <SnackbarMessage
                            visible={authEmailMessageVisible}
                            onClose={() =>
                                dispatch(sendReqAuthEmailActions.setMessageVisible(false))
                            }
                            message={authEmailMessage}
                        />
                        <SnackbarMessage
                            visible={financeEmailMessageVisible}
                            onClose={() =>
                                dispatch(sendReqFinanceEmailActions.setMessageVisible(false))
                            }
                            message={financeEmailMessage}
                        />
                        <SnackbarMessage
                            visible={orderPdfEmailMessageVisible}
                            onClose={() =>
                                dispatch(
                                    sendPurchaseOrderPdfEmailActionTypes.setMessageVisible(false)
                                )
                            }
                            message={orderPdfEmailMessage}
                        />
                        {itemError && (
                            <Grid item xs={12}>
                                <ErrorCard
                                    errorMessage={itemError?.details ?? itemError.statusText}
                                />
                            </Grid>
                        )}
                        <Collapse in={showCostWarning}>
                            <Alert
                                severity="warning"
                                action={
                                    <IconButton
                                        aria-label="close"
                                        color="inherit"
                                        size="small"
                                        onClick={() => {
                                            setShowCostWarning(false);
                                        }}
                                    >
                                        <Close fontSize="inherit" />
                                    </IconButton>
                                }
                                sx={{ mb: 2 }}
                            >
                                <AlertTitle>Warning</AlertTitle>
                                This req is over £250. <br /> You may want to approach Purchasing
                                directly to see if they can make cost savings on this purchase
                            </Alert>
                        </Collapse>
                        <Dialog open={explainDialogOpen} fullWidth maxWidth="md">
                            <div className={classes.centerTextInDialog}>
                                <IconButton
                                    className={classes.pullRight}
                                    aria-label="Close"
                                    onClick={() => setExplainDialogOpen(false)}
                                >
                                    <Close />
                                </IconButton>
                                <Typography variant="body1" gutterBottom>
                                    <p>
                                        <b>
                                            Order is: Draft --{'>'} Authorise Wait --{'>'} Finance
                                            Wait --{'>'} Order Wait --{'>'} Order
                                        </b>
                                    </p>
                                    <p>
                                        <b>DRAFT</b> - Req is not ready to be authorised. It will
                                        not show up on authorising screens.
                                    </p>
                                    <p>
                                        <b>AUTHORISE WAIT</b> - Req is ready to be authorised.
                                        Waiting for team leader/reporting head to check it.
                                    </p>
                                    <p>
                                        <b>FINANCE WAIT</b> - Req had been authorised but now must
                                        be checked by the finance department.
                                    </p>
                                    <p>
                                        <b>ORDER WAIT</b> - Req has been double checked. Now waiting
                                        for purchasing to create an order from it.
                                    </p>
                                    <p>
                                        <b>ORDER</b> - An order has been created from the req. Order
                                        number is on req screen.
                                    </p>
                                    <p>
                                        <b>CANCELLED</b> - Requistion has been cancelled without
                                        ever turning into an order.
                                    </p>
                                </Typography>
                            </div>
                        </Dialog>

                        <Dialog open={signingLimitDialogOpen} fullWidth maxWidth="md">
                            <div className={classes.centerTextInDialog}>
                                <IconButton
                                    className={classes.pullRight}
                                    aria-label="Close"
                                    onClick={() => setSigningLimitDialogOpen(false)}
                                >
                                    <Close />
                                </IconButton>
                                {!canAuthOrderMessageVisible ? (
                                    <Loading />
                                ) : (
                                    <>
                                        <Typography variant="h6">{canAuthOrderMessage}</Typography>
                                        <Typography variant="body1" gutterBottom>
                                            <Grid container spacing={1}>
                                                <Grid item xs={4}>
                                                    <Button
                                                        className={classes.buttonMarginTop}
                                                        color="primary"
                                                        variant="contained"
                                                        onClick={() =>
                                                            setSigningLimitDialogOpen(false)
                                                        }
                                                    >
                                                        Cancel
                                                    </Button>
                                                </Grid>
                                                <Grid item xs={4} />
                                                <Grid item xs={4}>
                                                    <Button
                                                        className={classes.buttonMarginTop}
                                                        color="primary"
                                                        variant="contained"
                                                        disabled={!allowedToCreateOrder()}
                                                        onClick={handleActuallyCreateOrder}
                                                    >
                                                        Create Order
                                                    </Button>
                                                </Grid>
                                            </Grid>
                                        </Typography>
                                    </>
                                )}
                            </div>
                        </Dialog>

                        <Dialog open={authEmailDialogOpen} fullWidth maxWidth="md">
                            <div className={classes.centerTextInDialog}>
                                <IconButton
                                    className={classes.pullRight}
                                    aria-label="Close"
                                    onClick={() => setAuthEmailDialogOpen(false)}
                                >
                                    <Close />
                                </IconButton>
                                <Typography variant="h6">
                                    Send authorisation request email
                                </Typography>
                                <Typography variant="body1" gutterBottom>
                                    <Grid container spacing={1}>
                                        <Grid item xs={8}>
                                            <Dropdown
                                                fullWidth
                                                value={employeeToEmail}
                                                label="Send Authorisation Email To"
                                                items={employees.map(e => ({
                                                    displayText: `${e.fullName} (${e.id})`,
                                                    id: parseInt(e.id, 10)
                                                }))}
                                                propertyName="sendTo"
                                                onChange={(propertyName, selected) => {
                                                    setEmployeeToEmail(selected);
                                                }}
                                            />
                                        </Grid>
                                        <Grid item xs={4}>
                                            <Tooltip
                                                title="Send"
                                                placement="top"
                                                className={classes.cursorPointer}
                                            >
                                                <Send
                                                    className={classes.buttonMarginTop}
                                                    onClick={() => handleSendAuthoriseEmailClick()}
                                                />
                                            </Tooltip>
                                        </Grid>
                                    </Grid>
                                </Typography>
                            </div>
                        </Dialog>

                        <Dialog open={financeEmailDialogOpen} fullWidth maxWidth="md">
                            <div className={classes.centerTextInDialog}>
                                <IconButton
                                    className={classes.pullRight}
                                    aria-label="Close"
                                    onClick={() => setFinanceEmailDialogOpen(false)}
                                >
                                    <Close />
                                </IconButton>
                                <Typography variant="h6">
                                    Send finance &quot;sign off request&quot; email
                                </Typography>
                                <Typography variant="body1" gutterBottom>
                                    <Grid container spacing={1}>
                                        <Grid item xs={12}>
                                            <i>
                                                Note: Are you sure you need to email Finance? The
                                                team monitor the list regularly and check all reqs
                                                but you can email if it is particularly urgent.
                                            </i>
                                        </Grid>

                                        <Grid item xs={8}>
                                            <Dropdown
                                                fullWidth
                                                value={employeeToEmail}
                                                label="Send Finance Email To"
                                                items={employees.map(e => ({
                                                    displayText: `${e.fullName} (${e.id})`,
                                                    id: parseInt(e.id, 10)
                                                }))}
                                                propertyName="sendTo"
                                                onChange={(propertyName, selected) => {
                                                    setEmployeeToEmail(selected);
                                                }}
                                            />
                                        </Grid>
                                        <Grid item xs={4}>
                                            <Tooltip
                                                title="Send"
                                                placement="top"
                                                className={classes.cursorPointer}
                                            >
                                                <Send
                                                    className={classes.buttonMarginTop}
                                                    onClick={() => handleSendFinanceEmailClick()}
                                                />
                                            </Tooltip>
                                        </Grid>
                                    </Grid>
                                </Typography>
                            </div>
                        </Dialog>

                        <Dialog open={orderPdfEmailDialogOpen} fullWidth maxWidth="md">
                            <div className={classes.centerTextInDialog}>
                                <IconButton
                                    className={classes.pullRight}
                                    aria-label="Close"
                                    onClick={() => setOrderPdfEmailDialogOpen(false)}
                                >
                                    <Close />
                                </IconButton>
                                <Typography variant="h6">
                                    Email purchase order pdf to supplier
                                </Typography>
                                <Typography variant="body1" gutterBottom>
                                    <Grid container spacing={1}>
                                        <Grid item xs={6}>
                                            <InputField
                                                fullWidth
                                                value={purchaseOrderEmailState?.email}
                                                label="Send PO Email To"
                                                number
                                                propertyName="emailTo"
                                                onChange={(name, newEmail) => {
                                                    setPurchaseOrderEmailState({
                                                        ...purchaseOrderEmailState,
                                                        email: newEmail
                                                    });
                                                }}
                                            />
                                        </Grid>
                                        <Grid item xs={3}>
                                            <OnOffSwitch
                                                label="Also send to self? (bcc)"
                                                value={purchaseOrderEmailState.bcc}
                                                onChange={() => {
                                                    setPurchaseOrderEmailState({
                                                        ...purchaseOrderEmailState,
                                                        bcc: !purchaseOrderEmailState.bcc
                                                    });
                                                }}
                                                propertyName="bcc"
                                            />
                                        </Grid>
                                        <Grid item xs={3}>
                                            <Tooltip
                                                title="Send"
                                                placement="top"
                                                className={classes.cursorPointer}
                                            >
                                                <Send
                                                    className={classes.buttonMarginTop}
                                                    onClick={() => handleOrderPdfEmailClick()}
                                                />
                                            </Tooltip>
                                        </Grid>
                                    </Grid>
                                </Typography>
                            </div>
                        </Dialog>

                        <Grid item xs={12}>
                            <Typography variant="h6">Purchase Order Req Utility </Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                fullWidth
                                value={req.reqNumber}
                                label="PO Req Number"
                                propertyName="reqNumber"
                                onChange={handleFieldChange}
                                disabled
                            />
                        </Grid>
                        <Grid item xs={5}>
                            <Dropdown
                                items={reqStates
                                    ?.sort((a, b) => a.displayOrder - b.displayOrder)
                                    .map(e => ({
                                        displayText: e.state,
                                        id: e.State
                                    }))}
                                propertyName="state"
                                label="State"
                                value={req.state}
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                                fullwidth
                                allowNoValue={false}
                                required
                            />
                            <Typography
                                variant="button"
                                onClick={() => setExplainDialogOpen(true)}
                                className={classes.cursorPointer}
                            >
                                <u>Explain states</u>
                            </Typography>
                        </Grid>
                        <Grid item xs={3}>
                            <InputField
                                fullWidth
                                value={req.reqDate}
                                label="Req Date"
                                propertyName="reqDate"
                                onChange={handleFieldChange}
                                type="date"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={1}>
                            <Tooltip title="Print req screen">
                                <IconButton
                                    className={classes.pullRight}
                                    aria-label="Print"
                                    onClick={() => history.push(utilities.getHref(req, 'print'))}
                                    disabled={creating}
                                >
                                    <PrintIcon />
                                </IconButton>
                            </Tooltip>
                        </Grid>
                        <Grid item xs={1}>
                            <div className={classes.centeredIcon}>
                                {editingAllowed ? (
                                    <Tooltip
                                        title={`You can ${
                                            creating ? 'create' : 'edit'
                                        } purchase order reqs`}
                                    >
                                        <ModeEditIcon color="primary" />
                                    </Tooltip>
                                ) : (
                                    <Tooltip title="cannot edit cancelled req">
                                        <EditOffIcon color="secondary" />
                                    </Tooltip>
                                )}
                            </div>
                        </Grid>
                        <Grid item xs={5} container spacing={1}>
                            <Grid item xs={8}>
                                <Typeahead
                                    label="Part"
                                    title="Search for a part"
                                    onSelect={newPart => {
                                        handleFieldChange('partNumber', newPart.id);
                                        handleFieldChange('description', newPart.description);
                                    }}
                                    items={partsSearchResults}
                                    loading={partsSearchLoading}
                                    fetchItems={searchTerm =>
                                        dispatch(partsActions.search(searchTerm))
                                    }
                                    clearSearch={() => dispatch(partsActions.clearSearch)}
                                    value={req.partNumber ? `${req.partNumber}` : null}
                                    modal
                                    links={false}
                                    debounce={1000}
                                    minimumSearchTermLength={2}
                                    disabled={!editingAllowed}
                                    placeholder="click to set part"
                                />
                            </Grid>
                            <Grid item xs={4}>
                                <InputField
                                    fullWidth
                                    value={req.qty}
                                    label="Quantity"
                                    propertyName="qty"
                                    onChange={handleFieldChange}
                                    disabled={
                                        (!editingAllowed || !creating) && !userHasFinancePower()
                                    }
                                    type="number"
                                    required
                                />
                            </Grid>
                            <Grid item xs={4}>
                                <Dropdown
                                    fullWidth
                                    value={req.currency?.code}
                                    label="Currency"
                                    propertyName="currency"
                                    items={currencies.map(c => ({
                                        displayText: c.code,
                                        id: c.code
                                    }))}
                                    allowNoValue
                                    onChange={(propertyName, newValue) => {
                                        setReq(a => ({
                                            ...a,
                                            currency: {
                                                code: newValue,
                                                name: currencies.find(x => x.code === newValue)
                                                    ?.name
                                            }
                                        }));
                                    }}
                                    disabled={!editingAllowed || !creating}
                                    required
                                />
                            </Grid>
                            <Grid item xs={8}>
                                <InputField
                                    fullWidth
                                    value={req.currency?.name}
                                    label="Name"
                                    propertyName="currencyName"
                                    disabled
                                />
                            </Grid>

                            <Grid item xs={4}>
                                <InputField
                                    fullWidth
                                    value={req.unitPrice}
                                    label="Unit Price"
                                    number
                                    propertyName="unitPrice"
                                    onChange={handleFieldChange}
                                    disabled={
                                        (!editingAllowed || !creating) && !userHasFinancePower()
                                    }
                                    decimalPlaces={2}
                                    type="number"
                                    required
                                />
                            </Grid>
                            <Grid item xs={4}>
                                <InputField
                                    fullWidth
                                    value={req.carriage}
                                    label="Carriage"
                                    number
                                    propertyName="carriage"
                                    onChange={handleFieldChange}
                                    disabled={!editingAllowed}
                                    type="number"
                                    decimalPlaces={2}
                                />
                            </Grid>
                            <Grid item xs={4}>
                                <InputField
                                    fullWidth
                                    value={parseFloat(req.totalReqPrice).toFixed(2)}
                                    label="Total Req Price"
                                    number
                                    propertyName="totalReqPrice"
                                    onChange={handleFieldChange}
                                    disabled
                                    type="number"
                                    decimalPlaces={2}
                                />
                            </Grid>
                        </Grid>
                        <Grid item xs={7}>
                            <InputField
                                fullWidth
                                value={req.description}
                                label="Description"
                                propertyName="description"
                                onChange={handleFieldChange}
                                rows={8}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={12} />
                        <Grid item xs={3}>
                            <Typeahead
                                onSelect={newValue => {
                                    handleSupplierChange(newValue);
                                }}
                                label="Supplier"
                                modal
                                propertyName="supplierId"
                                items={suppliersSearchResults}
                                value={req.supplier ? req.supplier.id : null}
                                loading={suppliersSearchLoading}
                                fetchItems={searchSuppliers}
                                links={false}
                                text
                                clearSearch={() => {}}
                                placeholder="Search Suppliers"
                                minimumSearchTermLength={3}
                                fullWidth
                                disabled={!editingAllowed || !creating}
                                required
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req.supplier?.name}
                                label="Supplier Name"
                                number
                                propertyName="supplierContact"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={3} />
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req.supplierContact}
                                label="Supplier Contact"
                                number
                                propertyName="supplierContact"
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req.phoneNumber}
                                label="Phone Number"
                                propertyName="phoneNumber"
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req.email}
                                label="Email Address"
                                propertyName="email"
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req.addressLine1}
                                label="Line 1"
                                propertyName="addressLine1"
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req.addressLine2}
                                label="Line 2"
                                propertyName="addressLine2"
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req.addressLine3}
                                label="Line 3"
                                propertyName="addressLine3"
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req.addressLine4}
                                label="Line 4"
                                propertyName="addressLine4"
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <InputField
                                fullWidth
                                value={req.postCode}
                                label="Postcode"
                                propertyName="postCode"
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={5} />
                        <Grid item xs={4}>
                            <Typeahead
                                onSelect={newValue => {
                                    setReq(a => ({
                                        ...a,
                                        country: {
                                            countryCode: newValue.countryCode,
                                            countryName: newValue.countryName
                                        }
                                    }));
                                }}
                                label="Country Lookup"
                                modal
                                propertyName="countryCode"
                                items={countriesSearchResults}
                                value={req.country?.countryCode}
                                loading={countriesSearchLoading}
                                fetchItems={searchCountries}
                                links={false}
                                priorityFunction={(i, searchTerm) => {
                                    if (i.countryCode === searchTerm?.toUpperCase()) {
                                        return 1;
                                    }
                                    return 0;
                                }}
                                text
                                placeholder="Search by Name or Code"
                                minimumSearchTermLength={2}
                                disabled={!editingAllowed}
                                required
                            />
                        </Grid>
                        <Grid item xs={5}>
                            <InputField
                                fullWidth
                                value={req.country?.countryName}
                                label="Name"
                                propertyName="countryName"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={3} />
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={req.quoteRef}
                                label="Quote Ref"
                                propertyName="quoteRef"
                                onChange={handleFieldChange}
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req.dateRequired}
                                label="Date Required"
                                propertyName="dateRequired"
                                onChange={handleFieldChange}
                                type="date"
                                disabled={!editingAllowed}
                            />
                        </Grid>
                        <Grid item xs={12} />
                        <Grid item xs={4}>
                            <TypeaheadTable
                                table={nominalAccountsTable}
                                columnNames={['Nominal', 'Description', 'Dept', 'Name']}
                                fetchItems={searchTerm =>
                                    dispatch(nominalsActions.search(searchTerm))
                                }
                                modal
                                placeholder="Search Dept/Nominal"
                                links={false}
                                clearSearch={() => dispatch(nominalsActions.clearSearch)}
                                loading={nominalsSearchLoading}
                                label="Department"
                                title="Search on Department or Nominal"
                                value={req.department?.departmentCode}
                                onSelect={newValue => handleNominalUpdate(newValue)}
                                debounce={1000}
                                minimumSearchTermLength={2}
                                disabled={!editingAllowed}
                                required
                            />
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={req.department?.description}
                                label="Description"
                                disabled
                                propertyName="nominalDescription"
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req.nominal?.nominalCode}
                                label="Nominal"
                                onChange={() => {}}
                                propertyName="nominalCode"
                                required
                                disabled
                            />
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={req.nominal?.description}
                                label="Description"
                                propertyName="nominalDescription"
                                onChange={() => {}}
                                disabled
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                fullWidth
                                value={`${req.requestedBy?.fullName} (${req.requestedBy?.id})`}
                                label="Raised By"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <Button
                                className={classes.buttonMarginTop}
                                color="primary"
                                variant="contained"
                                disabled={!allowedToAuthorise()}
                                onClick={handleAuthorise}
                            >
                                Authorise
                            </Button>
                        </Grid>
                        <Grid item xs={1}>
                            <Tooltip title="Email to request authorisation">
                                <IconButton
                                    className={classes.buttonMarginTop}
                                    aria-label="Email"
                                    onClick={() => setAuthEmailDialogOpen(true)}
                                    disabled={creating}
                                >
                                    <Email />
                                </IconButton>
                            </Tooltip>
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={
                                    req.authorisedBy
                                        ? `${req.authorisedBy?.fullName} (${req.authorisedBy?.id})`
                                        : ''
                                }
                                label="Authorised by"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <Button
                                className={classes.buttonMarginTop}
                                color="primary"
                                variant="contained"
                                disabled={!allowedTo2ndAuthorise()}
                                onClick={handleSecondAuth}
                            >
                                Authorise (secondary)
                            </Button>
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={
                                    req.secondAuthBy
                                        ? `${req.secondAuthBy?.fullName} (${req.secondAuthBy?.id})`
                                        : ''
                                }
                                label="Second auth by"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <Button
                                className={classes.buttonMarginTop}
                                color="primary"
                                variant="contained"
                                disabled={!allowedToFinanceCheck()}
                                onClick={handleFinanceCheck}
                            >
                                Sign off (finance)
                            </Button>
                        </Grid>
                        <Grid item xs={1}>
                            <Tooltip title="Email to request finance sign off">
                                <IconButton
                                    className={classes.buttonMarginTop}
                                    aria-label="Email"
                                    onClick={() => setFinanceEmailDialogOpen(true)}
                                    disabled={creating}
                                >
                                    <Email />
                                </IconButton>
                            </Tooltip>
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={
                                    req.financeCheckBy
                                        ? `${req.financeCheckBy?.fullName} (${req.financeCheckBy?.id})`
                                        : ''
                                }
                                label="Finance check by"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <Button
                                className={classes.buttonMarginTop}
                                color="primary"
                                variant="contained"
                                disabled={!allowedToCreateOrder()}
                                onClick={handleCreateOrderButton}
                            >
                                Create Order
                            </Button>
                        </Grid>
                        <Grid item xs={1}>
                            <Tooltip title="Email pdf to supplier">
                                <IconButton
                                    className={classes.buttonMarginTop}
                                    aria-label="Email"
                                    onClick={() => setOrderPdfEmailDialogOpen(true)}
                                    disabled={creating}
                                >
                                    <Email />
                                </IconButton>
                            </Tooltip>
                        </Grid>
                        <Grid item xs={1}>
                            <Tooltip title="View purchase order">
                                <IconButton
                                    className={classes.buttonMarginTop}
                                    aria-label="View"
                                    onClick={() =>
                                        history.push(utilities.getHref(req, 'view-purchase-order'))
                                    }
                                    disabled={!hasOrderNumber}
                                >
                                    <PageviewIcon />
                                </IconButton>
                            </Tooltip>
                        </Grid>
                        <Grid item xs={5}>
                            <InputField
                                fullWidth
                                value={
                                    req.turnedIntoOrderBy
                                        ? `${req.turnedIntoOrderBy?.fullName} (${req.turnedIntoOrderBy?.id})`
                                        : ''
                                }
                                label="Turned into order by"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <InputField
                                fullWidth
                                value={req.orderNumber}
                                label="Order Number"
                                propertyName="orderNumber"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req.remarksForOrder}
                                label="Remarks to print on order"
                                propertyName="remarksForOrder"
                                onChange={handleFieldChange}
                                rows={4}
                                disabled={!editingAllowed}
                                maxLength={200}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req.internalNotes}
                                label="Internal order remarks"
                                propertyName="internalNotes"
                                onChange={handleFieldChange}
                                rows={4}
                                disabled={!editingAllowed}
                                maxLength={300}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            {!creating && (
                                <Button
                                    color="secondary"
                                    variant="contained"
                                    disabled={!editingAllowed}
                                    onClick={handleCancelClick}
                                >
                                    Cancel Req
                                </Button>
                            )}
                        </Grid>
                        <Grid item xs={6}>
                            <SaveBackCancelButtons
                                saveDisabled={!canSave()}
                                backClick={() => handleBackClick(previousPaths, history.goBack)}
                                saveClick={() => {
                                    setEditStatus('view');
                                    clearErrors();
                                    setSnackbarMessage('Save successful');
                                    if (creating) {
                                        dispatch(poReqActions.add({ ...req, reqNumber: -1 }));
                                    } else {
                                        dispatch(poReqActions.update(req.reqNumber, req));
                                    }
                                }}
                                cancelClick={() => {
                                    setEditStatus('view');
                                    if (creating) {
                                        setReq(defaultCreatingReq);
                                    } else {
                                        setReq(item);
                                    }
                                }}
                            />
                        </Grid>
                    </Grid>
                )}
            </Page>
        </>
    );
}

POReqUtility.propTypes = { creating: PropTypes.bool };
POReqUtility.defaultProps = { creating: false };
export default POReqUtility;
