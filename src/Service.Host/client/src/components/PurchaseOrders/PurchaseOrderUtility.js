import React, { useEffect, useState, useReducer } from 'react';
import { useMediaQuery } from 'react-responsive';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { useParams } from 'react-router-dom';
import Dialog from '@mui/material/Dialog';
import IconButton from '@mui/material/IconButton';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import Close from '@mui/icons-material/Close';
import Email from '@mui/icons-material/Email';
import Send from '@mui/icons-material/Send';
import { makeStyles } from '@mui/styles';
import {
    Page,
    collectionSelectorHelpers,
    Typeahead,
    InputField,
    SnackbarMessage,
    itemSelectorHelpers,
    Loading,
    Dropdown,
    TypeaheadTable,
    //userSelectors,
    getItemError,
    ErrorCard,
    utilities,
    getPreviousPaths,
    SaveBackCancelButtons,
    OnOffSwitch,
    processSelectorHelpers
} from '@linn-it/linn-form-components-library';
import currenciesActions from '../../actions/currenciesActions';
import employeesActions from '../../actions/employeesActions';
import nominalsActions from '../../actions/nominalsActions';
//import countriesActions from '../../actions/countriesActions';
import suppliersActions from '../../actions/suppliersActions';
// import partsActions from '../../actions/partsActions';
import history from '../../history';
import config from '../../config';
import purchaseOrderActions from '../../actions/purchaseOrderActions';
import handleBackClick from '../../helpers/handleBackClick';
import reducer from './purchaseOrderReducer';
import sendPurchaseOrderPdfEmailActionTypes from '../../actions/sendPurchaseOrderPdfEmailActions';
import { sendPurchaseOrderPdfEmail } from '../../itemTypes';

function PurchaseOrderUtility({ creating }) {
    const reduxDispatch = useDispatch();
    const clearErrors = () => reduxDispatch(purchaseOrderActions.clearErrorsForItem());

    const { orderNumber } = useParams();

    useEffect(() => {
        if (orderNumber) {
            reduxDispatch(purchaseOrderActions.fetch(orderNumber));
        } else if (creating) {
            reduxDispatch(purchaseOrderActions.fetchState());
        }
    }, [orderNumber, reduxDispatch, creating]);

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.purchaseOrder));
    const loading = useSelector(state =>
        creating
            ? itemSelectorHelpers.getApplicationStateLoading(state.purchaseOrderApplicationState)
            : itemSelectorHelpers.getItemLoading(state.purchaseOrder)
    );

    const itemError = useSelector(state => getItemError(state, 'purchaseOrder'));

    const [order, dispatch] = useReducer(reducer, {});
    const [purchaseOrderEmailState, setPurchaseOrderEmailState] = useState({
        email: '',
        bcc: false
    });

    useEffect(() => {
        if (item?.orderNumber) {
            dispatch({ type: 'initialise', payload: item });
            setPurchaseOrderEmailState({ bcc: false, email: item.supplierContactEmail });
        } else {
            reduxDispatch(purchaseOrderActions.clearErrorsForItem());
        }
    }, [item, reduxDispatch]);

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'id', 'name')
    );
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );
    const searchSuppliers = searchTerm => reduxDispatch(suppliersActions.search(searchTerm));

    const currencies = useSelector(state => collectionSelectorHelpers.getItems(state.currencies));
    const employees = useSelector(state => collectionSelectorHelpers.getItems(state.employees));

    const nominalsSearchItems = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.nominals)
    );
    const nominalsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.nominals)
    );

    const snackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state.purchaseOrder)
    );

    const [editStatus, setEditStatus] = useState('view');
    const [authEmailDialogOpen, setAuthEmailDialogOpen] = useState(false);
    const [employeeToEmail, setEmployeeToEmail] = useState();

    useEffect(() => reduxDispatch(currenciesActions.fetch()), [reduxDispatch]);
    useEffect(() => reduxDispatch(employeesActions.fetch()), [reduxDispatch]);

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

    const previousPaths = useSelector(state => getPreviousPaths(state));

    const allowedToAuthorise = () => !creating && order.links?.some(l => l.rel === 'authorise');

    const allowedToUpdate = () =>
        !creating && order.links?.some(l => l.rel === 'edit') && order.cancelled !== 'Y';

    const inputIsInvalid = () => false;

    const canSave = () =>
        editStatus !== 'view' && allowedToUpdate && !inputIsInvalid() && order !== item;

    const handleAuthorise = () => {
        setEditStatus('edit');
        if (allowedToAuthorise) {
            clearErrors();
            reduxDispatch(purchaseOrderActions.postByHref(utilities.getHref(item, 'authorise')));
        }
    };

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        dispatch({ payload: newValue, propertyName, type: 'orderFieldChange' });
    };

    const handleDetailFieldChange = (propertyName, newValue, detail) => {
        setEditStatus('edit');

        dispatch({ payload: { ...detail, [propertyName]: newValue }, type: 'detailFieldChange' });
    };

    const handleDeliveryFieldChange = (propertyName, newValue, delivery) => {
        setEditStatus('edit');

        dispatch({
            payload: { ...delivery, [propertyName]: newValue },
            type: 'deliveryFieldChange'
        });
    };

    const handleSendAuthoriseEmailClick = () => {
        setAuthEmailDialogOpen(false);
        // dispatch(sendOrderAuthEmailActions.clearProcessData);
        // dispatch(
        //     sendOrderAuthEmailActions.requestProcessStart('', {
        //         orderNumber,
        //         toEmployeeId: employeeToEmail
        //     })
        // );
    };

    const handleNominalUpdate = (newNominal, lineNumber) => {
        setEditStatus('edit');
        const newNominalAccount = {
            nominal: {
                nominalCode: newNominal.values.find(x => x.id === 'nominalCode')?.value,
                description: newNominal.values.find(x => x.id === 'description')?.value
            },
            department: {
                departmentCode: newNominal.values.find(x => x.id === 'departmentCode')?.value,
                description: newNominal.values.find(x => x.id === 'departmentDescription')?.value
            },
            accountId: newNominal.id
        };
        dispatch({
            payload: newNominalAccount,
            lineNumber,
            type: 'nominalChange'
        });
    };

    const [orderPdfEmailDialogOpen, setOrderPdfEmailDialogOpen] = useState(false);

    const orderPdfEmailMessageVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[sendPurchaseOrderPdfEmail.item])
    );

    const orderPdfEmailMessage = useSelector(state =>
        processSelectorHelpers.getMessageText(state[sendPurchaseOrderPdfEmail.item])
    );

    const handleOrderPdfEmailClick = () => {
        setOrderPdfEmailDialogOpen(false);
        reduxDispatch(sendPurchaseOrderPdfEmailActionTypes.clearProcessData);
        reduxDispatch(
            sendPurchaseOrderPdfEmailActionTypes.requestProcessStart('', {
                orderNumber: order.orderNumber,
                emailAddress: purchaseOrderEmailState.email,
                bcc: purchaseOrderEmailState.bcc
            })
        );
    };

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
                    <Grid container spacing={1} justifyContent="center">
                        <SnackbarMessage
                            visible={snackbarVisible}
                            onClose={() =>
                                reduxDispatch(purchaseOrderActions.setSnackbarVisible(false))
                            }
                            message="Save successful"
                        />
                        <SnackbarMessage
                            visible={orderPdfEmailMessageVisible}
                            onClose={() =>
                                reduxDispatch(
                                    sendPurchaseOrderPdfEmailActionTypes.setMessageVisible(false)
                                )
                            }
                            message={orderPdfEmailMessage}
                        />
                        {/* <SnackbarMessage
                            visible={authEmailMessageVisible}
                            onClose={() =>
                                dispatch(sendReqAuthEmailActions.setMessageVisible(false))
                            }
                            message={authEmailMessage}
                        /> */}
                        {itemError && (
                            <Grid item xs={12}>
                                <ErrorCard
                                    errorMessage={itemError?.details ?? itemError.statusText}
                                />
                            </Grid>
                        )}
                        <Dialog open={false && authEmailDialogOpen} fullWidth maxWidth="md">
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
                            <Typography variant="h6">Purchase Order Utility </Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                fullWidth
                                value={order.orderNumber}
                                label="Order Number"
                                propertyName="orderNumber"
                                onChange={handleFieldChange}
                                disabled
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                fullWidth
                                value={order.orderDate}
                                label="Order Date"
                                propertyName="orderDate"
                                onChange={handleFieldChange}
                                type="date"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={3}>
                            {/* will be dropdown like this:
                            <Dropdown
                                items={orderTypes
                                    ?.sort((a, b) => a.displayOrder - b.displayOrder)
                                    .map(e => ({
                                        displayText: e.description,
                                        id: e.name
                                    }))}
                                propertyName="state"
                                label="State"
                                value={`${order?.documentType?.name} - ${order?.documentType?.description}`}
                                onChange={handleDocumentTypeChange}
                                disabled={!allowedToUpdateed()}
                                fullwidth
                                allowNoValue={false}
                                required
                            /> */}
                            <InputField
                                fullWidth
                                value={`${order?.documentType?.name} - ${order?.documentType?.description}`}
                                label="Order Type"
                                propertyName="documentType"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <InputField
                                fullWidth
                                value={`${order?.orderMethod?.name} - ${order?.orderMethod?.description}`}
                                label="Order Method"
                                propertyName="orderMethod"
                                disabled
                            />
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
                            <div className={classes.centeredIcon}>
                                {allowedToUpdate ? (
                                    <Tooltip
                                        title={`You can ${
                                            creating ? 'create' : 'edit'
                                        } purchase orders`}
                                    >
                                        <ModeEditIcon color="primary" />
                                    </Tooltip>
                                ) : (
                                    <Tooltip title="cannot edit order">
                                        <EditOffIcon color="secondary" />
                                    </Tooltip>
                                )}
                            </div>
                        </Grid>

                        <Grid item xs={3}>
                            <Typeahead
                                // onSelect={newValue => {
                                //     handleSupplierChange(newValue);
                                // }}
                                label="Supplier"
                                modal
                                propertyName="supplierId"
                                items={suppliersSearchResults}
                                value={order.supplier ? order.supplier.id : null}
                                loading={suppliersSearchLoading}
                                fetchItems={searchSuppliers}
                                links={false}
                                text
                                clearSearch={() => {}}
                                placeholder="Search Suppliers"
                                minimumSearchTermLength={3}
                                fullWidth
                                disabled
                                required
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={order.supplier?.name}
                                label="Supplier Name"
                                number
                                propertyName="supplierContact"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <InputField
                                fullWidth
                                value={order.issuePartsToSupplier}
                                label="Issue Parts to Supplier"
                                number
                                propertyName="issuePartsToSupplier"
                                onChange={handleFieldChange}
                                disabled
                            />
                        </Grid>

                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={order.orderContactName}
                                label="Order Contact"
                                number
                                propertyName="orderContactName"
                                onChange={handleFieldChange}
                                disabled
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={order.supplierContactPhone}
                                label="Phone Number"
                                propertyName="phoneNumber"
                                onChange={handleFieldChange}
                                disabled
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={order.supplierContactEmail}
                                label="Email Address"
                                propertyName="email"
                                onChange={handleFieldChange}
                                disabled
                            />
                        </Grid>

                        <Grid item xs={3}>
                            <Dropdown
                                fullWidth
                                value={order.currency?.code}
                                label="Currency"
                                propertyName="currency"
                                items={currencies.map(c => ({
                                    displayText: c.code,
                                    id: c.code
                                }))}
                                allowNoValue
                                onChange={(propertyName, newValue) => {
                                    dispatch(a => ({
                                        ...a,
                                        currency: {
                                            code: newValue,
                                            name: currencies.find(x => x.code === newValue)?.name
                                        }
                                    }));
                                }}
                                disabled
                                required
                            />
                        </Grid>
                        <Grid item xs={5}>
                            <InputField
                                fullWidth
                                value={order?.currency?.name}
                                label="Name"
                                propertyName="currencyName"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={order.exchangeRate}
                                label="Exchange rate"
                                number
                                propertyName="exchangeRate"
                                onChange={handleFieldChange}
                                disabled
                                type="number"
                            />
                        </Grid>

                        <Grid container item spacing={1} xs={4}>
                            <Grid item xs={12}>
                                <InputField
                                    fullWidth
                                    value={order.deliveryAddress?.addressId}
                                    label="Delivery Address Id"
                                    propertyName="deliveryAddressId"
                                    onChange={handleFieldChange}
                                    disabled
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <InputField
                                    fullWidth
                                    value={order.sentByMethod}
                                    label="Sent by method"
                                    propertyName="sentByMethod"
                                    onChange={handleFieldChange}
                                    disabled
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <InputField
                                    fullWidth
                                    value={order.quotationRef}
                                    label="Quote Ref"
                                    propertyName="quotationRef"
                                    onChange={handleFieldChange}
                                    disabled
                                    rows={2}
                                />
                            </Grid>
                        </Grid>

                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={order.deliveryAddress?.address}
                                label="Delivery Address"
                                propertyName="deliveryAddress"
                                onChange={handleFieldChange}
                                rows={8}
                                disabled
                            />
                        </Grid>
                        <Grid item xs={12} />
                        <Grid container item spacing={1} xs={7}>
                            <Grid item xs={6}>
                                <InputField
                                    fullWidth
                                    value={`${order.requestedBy?.fullName} (${order.requestedBy?.id})`}
                                    label="Requested By"
                                    disabled
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <InputField
                                    fullWidth
                                    value={`${order.enteredBy?.fullName} (${order.enteredBy?.id})`}
                                    label="Entered By"
                                    disabled
                                />
                            </Grid>
                            <Grid item xs={4}>
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
                            <Grid item xs={2}>
                                <Tooltip title="Email to request authorisation">
                                    <IconButton
                                        className={classes.buttonMarginTop}
                                        aria-label="Email"
                                        onClick={() => setAuthEmailDialogOpen(true)}
                                        disabled
                                    >
                                        <Email />
                                    </IconButton>
                                </Tooltip>
                            </Grid>
                            <Grid item xs={6}>
                                <InputField
                                    fullWidth
                                    value={
                                        order.authorisedBy
                                            ? `${order.authorisedBy?.fullName} (${order.authorisedBy?.id})`
                                            : ''
                                    }
                                    label="Authorised by"
                                    disabled
                                />
                            </Grid>
                        </Grid>
                        <Grid item xs={5}>
                            <InputField
                                fullWidth
                                value={order.remarks}
                                label="Remarks"
                                propertyName="remarks"
                                onChange={handleFieldChange}
                                rows={4}
                                disabled={!allowedToUpdate()}
                            />
                        </Grid>

                        {order.details
                            ?.sort((a, b) => a.line - b.line)
                            .map(detail => (
                                <>
                                    <Grid container item spacing={1} xs={4}>
                                        <Grid item xs={4}>
                                            <InputField
                                                fullWidth
                                                value={detail.line}
                                                label="Order Line No"
                                                propertyName="line"
                                                disabled
                                            />
                                        </Grid>
                                        <Grid item xs={8}>
                                            <InputField
                                                fullWidth
                                                value={detail.partNumber}
                                                label="Part Number"
                                                propertyName="partNumber"
                                                onChange={handleDetailFieldChange}
                                                disabled
                                            />
                                        </Grid>
                                        <Grid item xs={6}>
                                            <InputField
                                                fullWidth
                                                value={detail.ourQty}
                                                label="Our quantity"
                                                propertyName="ourQty"
                                                onChange={(propertyName, newValue) =>
                                                    handleDetailFieldChange(
                                                        propertyName,
                                                        newValue,
                                                        detail
                                                    )
                                                }
                                                disabled //until check what totals etc these influence and add currency conversion for base fields
                                                // disabled={!allowedToUpdate()}
                                                type="number"
                                                required
                                            />
                                        </Grid>
                                        <Grid item xs={6}>
                                            <InputField
                                                fullWidth
                                                value={detail.orderQty}
                                                label="Order quantity"
                                                propertyName="orderQty"
                                                onChange={(propertyName, newValue) =>
                                                    handleDetailFieldChange(
                                                        propertyName,
                                                        newValue,
                                                        detail
                                                    )
                                                }
                                                disabled
                                                // disabled={!allowedToUpdate()}
                                                type="number"
                                                required
                                            />
                                        </Grid>

                                        <Grid item xs={6}>
                                            <InputField
                                                fullWidth
                                                value={detail.ourUnitPriceCurrency}
                                                label="Our price (unit, currency)"
                                                propertyName="ourUnitPriceCurrency"
                                                onChange={(propertyName, newValue) =>
                                                    handleDetailFieldChange(
                                                        propertyName,
                                                        newValue,
                                                        detail
                                                    )
                                                }
                                                disabled
                                                // disabled={!allowedToUpdate()}
                                                type="number"
                                                required
                                            />
                                        </Grid>
                                        <Grid item xs={6}>
                                            <InputField
                                                fullWidth
                                                value={detail.orderUnitPriceCurrency}
                                                label="Order price (currency)"
                                                propertyName="orderUnitPriceCurrency"
                                                onChange={(propertyName, newValue) =>
                                                    handleDetailFieldChange(
                                                        propertyName,
                                                        newValue,
                                                        detail
                                                    )
                                                }
                                                disabled
                                                // disabled={!allowedToUpdate()}
                                                type="number"
                                                required
                                            />
                                        </Grid>
                                    </Grid>
                                    <Grid item xs={8}>
                                        <InputField
                                            fullWidth
                                            value={detail.suppliersDesignation}
                                            label="Supplier Designation"
                                            propertyName="suppliersDesignation"
                                            onChange={(propertyName, newValue) =>
                                                handleDetailFieldChange(
                                                    propertyName,
                                                    newValue,
                                                    detail
                                                )
                                            }
                                            rows={8}
                                            disabled={!allowedToUpdate()}
                                        />
                                    </Grid>

                                    <Grid item xs={4}>
                                        <InputField
                                            fullWidth
                                            value={detail.netTotalCurrency}
                                            label="Net total (currency)"
                                            propertyName="netTotalCurrency"
                                            onChange={handleDetailFieldChange}
                                            disabled
                                            type="number"
                                            required
                                        />
                                    </Grid>
                                    <Grid item xs={4}>
                                        <InputField
                                            fullWidth
                                            value={detail.vatTotalCurrency}
                                            label="Vat total (currency)"
                                            propertyName="vatTotalCurrency"
                                            onChange={handleDetailFieldChange}
                                            disabled
                                            type="number"
                                            required
                                        />
                                    </Grid>
                                    <Grid item xs={4}>
                                        <InputField
                                            fullWidth
                                            value={detail.detailTotalCurrency}
                                            label="detail total (currency)"
                                            propertyName="detailTotalCurrency"
                                            onChange={handleDetailFieldChange}
                                            disabled
                                            type="number"
                                            required
                                        />
                                    </Grid>

                                    <Grid item xs={4}>
                                        <InputField
                                            fullWidth
                                            value={detail.baseNetTotal}
                                            label="Base Net total"
                                            propertyName="baseNetTotal"
                                            onChange={handleDetailFieldChange}
                                            disabled
                                            type="number"
                                            required
                                        />
                                    </Grid>
                                    <Grid item xs={4}>
                                        <InputField
                                            fullWidth
                                            value={detail.baseVatTotal}
                                            label="Base vat total"
                                            propertyName="baseVatTotal"
                                            onChange={handleDetailFieldChange}
                                            disabled
                                            type="number"
                                            required
                                        />
                                    </Grid>
                                    <Grid item xs={4}>
                                        <InputField
                                            fullWidth
                                            value={detail.baseDetailTotal}
                                            label="Base detail total"
                                            propertyName="baseDetailTotal"
                                            onChange={handleDetailFieldChange}
                                            disabled
                                            type="number"
                                            required
                                        />
                                    </Grid>

                                    <Grid item xs={4}>
                                        <InputField
                                            fullWidth
                                            value={detail.ourUnitOfMeasure}
                                            label="Our Unit Of Measure"
                                            propertyName="ourUnitOfMeasure"
                                            onChange={handleDetailFieldChange}
                                            disabled
                                            type="number"
                                            required
                                        />
                                    </Grid>
                                    <Grid item xs={4}>
                                        <InputField
                                            fullWidth
                                            value={detail.orderUnitOfMeasure}
                                            label="Order Unit Of Measure"
                                            propertyName="orderUnitOfMeasure"
                                            onChange={handleDetailFieldChange}
                                            disabled
                                            type="number"
                                            required
                                        />
                                    </Grid>
                                    <Grid item xs={4}>
                                        <></>
                                    </Grid>

                                    <Grid item xs={4}>
                                        <TypeaheadTable
                                            table={nominalAccountsTable}
                                            columnNames={['Nominal', 'Description', 'Dept', 'Name']}
                                            fetchItems={searchTerm =>
                                                reduxDispatch(nominalsActions.search(searchTerm))
                                            }
                                            modal
                                            placeholder="Search Dept/Nominal"
                                            links={false}
                                            clearSearch={() =>
                                                reduxDispatch(nominalsActions.clearSearch)
                                            }
                                            loading={nominalsSearchLoading}
                                            label="Department"
                                            title="Search Department"
                                            value={
                                                detail.orderPosting?.nominalAccount?.department
                                                    ?.departmentCode
                                            }
                                            onSelect={newValue =>
                                                handleNominalUpdate(newValue, detail.line)
                                            }
                                            debounce={1000}
                                            minimumSearchTermLength={2}
                                            disabled
                                            // disabled={!allowedToUpdate}
                                            required
                                        />
                                    </Grid>
                                    <Grid item xs={8}>
                                        <InputField
                                            fullWidth
                                            value={
                                                detail.orderPosting?.nominalAccount?.department
                                                    ?.description
                                            }
                                            label="Description"
                                            disabled
                                            propertyName="nominalDescription"
                                        />
                                    </Grid>
                                    <Grid item xs={4}>
                                        <InputField
                                            fullWidth
                                            value={
                                                detail.orderPosting?.nominalAccount?.nominal
                                                    ?.nominalCode
                                            }
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
                                            value={
                                                detail.orderPosting?.nominalAccount?.nominal
                                                    ?.description
                                            }
                                            label="Description"
                                            propertyName="nominalDescription"
                                            onChange={() => {}}
                                            disabled
                                        />
                                    </Grid>

                                    <Grid item xs={12}>
                                        <InputField
                                            fullWidth
                                            value={detail.deliveryInstructions}
                                            label="Delivery instructions"
                                            propertyName="deliveryInstructions"
                                            onChange={() => {}}
                                            disabled
                                            rows={2}
                                        />
                                    </Grid>

                                    {detail.purchaseDeliveries
                                        ?.sort((a, b) => a.line - b.line)
                                        ?.map(delivery => (
                                            <>
                                                <Grid item xs={2}>
                                                    <InputField
                                                        fullWidth
                                                        value={delivery.deliverySeq}
                                                        label="Delivery Seq"
                                                        propertyName="deliverySeq"
                                                        disabled
                                                    />
                                                </Grid>
                                                <Grid item xs={2}>
                                                    <InputField
                                                        fullWidth
                                                        value={delivery.dateRequested}
                                                        label="Date requested"
                                                        propertyName="dateRequested"
                                                        required
                                                        type="date"
                                                        onChange={(propertyName, newValue) =>
                                                            handleDeliveryFieldChange(
                                                                propertyName,
                                                                newValue,
                                                                delivery
                                                            )
                                                        }
                                                        disabled={!allowedToUpdate()}
                                                    />
                                                </Grid>
                                                <Grid item xs={2}>
                                                    <InputField
                                                        fullWidth
                                                        value={delivery.dateAdvised}
                                                        label="Date advised"
                                                        onChange={() => {}}
                                                        propertyName="dateAdvised"
                                                        required
                                                        disabled
                                                    />
                                                </Grid>
                                                <Grid item xs={6}>
                                                    <InputField
                                                        fullWidth
                                                        value={delivery.supplierConfirmationComment}
                                                        label="Supplier confirmation comment"
                                                        onChange={() => {}}
                                                        propertyName="supplierConfirmationComment"
                                                        required
                                                        disabled
                                                    />
                                                </Grid>
                                            </>
                                        ))}

                                    <Grid item xs={12}>
                                        <InputField
                                            fullWidth
                                            value={detail.internalComments}
                                            label="Internal comments"
                                            propertyName="internalComments"
                                            onChange={(propertyName, newValue) =>
                                                handleDetailFieldChange(
                                                    propertyName,
                                                    newValue,
                                                    detail
                                                )
                                            }
                                            rows={4}
                                            disabled={!allowedToUpdate()}
                                        />
                                    </Grid>
                                </>
                            ))}

                        {/* 
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
                                    value={order.partNumber ? `${order.partNumber}` : null}
                                    modal
                                    links={false}
                                    debounce={1000}
                                    minimumSearchTermLength={2}
                                    disabled={!allowedToUpdate()}
                                    placeholder="click to set part"
                                />
                            </Grid>
                        </Grid>
                        <Grid item xs={7}>
                            <InputField
                                fullWidth
                                value={order.part?.description}
                                label="Description"
                                propertyName="description"
                                onChange={handleFieldChange}
                                rows={8}
                                disabled={!allowedToUpdate()}
                            />
                        </Grid> */}
                        <Grid item xs={6}>
                            <SaveBackCancelButtons
                                saveDisabled={!canSave()}
                                backClick={() => handleBackClick(previousPaths, history.goBack)}
                                saveClick={() => {
                                    setEditStatus('view');
                                    clearErrors();
                                    reduxDispatch(
                                        purchaseOrderActions.update(order.orderNumber, order)
                                    );
                                }}
                                cancelClick={() => {
                                    setEditStatus('view');
                                    // if (creating) {
                                    //     setOrder(defaultCreatingOrder);
                                    // } else {
                                    dispatch(item);
                                }}
                            />
                        </Grid>
                    </Grid>
                )}
            </Page>
        </>
    );
}

PurchaseOrderUtility.propTypes = { creating: PropTypes.bool };
PurchaseOrderUtility.defaultProps = { creating: false };
export default PurchaseOrderUtility;
