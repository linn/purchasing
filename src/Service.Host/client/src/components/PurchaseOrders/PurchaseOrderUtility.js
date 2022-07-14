import React, { useEffect, useState } from 'react';
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
//import PrintIcon from '@mui/icons-material/Print';
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
    SaveBackCancelButtons
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

function PurchaseOrderUtility({ creating }) {
    const dispatch = useDispatch();
    const clearErrors = () => dispatch(purchaseOrderActions.clearErrorsForItem());

    const { orderNumber } = useParams();

    useEffect(() => {
        if (orderNumber) {
            dispatch(purchaseOrderActions.fetch(orderNumber));
        } else if (creating) {
            dispatch(purchaseOrderActions.fetchState());
        }
    }, [orderNumber, dispatch, creating]);

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.purchaseOrder));
    const loading = useSelector(state =>
        creating
            ? itemSelectorHelpers.getApplicationStateLoading(state.purchaseOrderApplicationState)
            : itemSelectorHelpers.getItemLoading(state.purchaseOrder)
    );

    const itemError = useSelector(state => getItemError(state, 'purchaseOrder'));
    const [order, setOrder] = useState({});

    useEffect(() => {
        if (item?.orderNumber) {
            setOrder(item);
        } else {
            dispatch(purchaseOrderActions.clearErrorsForItem());
        }
    }, [item, dispatch]);

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'id', 'name')
    );
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );
    const searchSuppliers = searchTerm => dispatch(suppliersActions.search(searchTerm));

    // const partsSearchResults = useSelector(state =>
    //     collectionSelectorHelpers.getSearchItems(state.parts)
    // ).map(c => ({
    //     id: c.partNumber,
    //     name: c.partNumber,
    //     partNumber: c.partNumber,
    //     description: c.description
    // }));

    // const partsSearchLoading = useSelector(state =>
    //     collectionSelectorHelpers.getSearchLoading(state.parts)
    // );

    // const countriesSearchResults = useSelector(state =>
    //     collectionSelectorHelpers.getSearchItems(
    //         state.countries,
    //         100,
    //         'countryCode',
    //         'countryCode',
    //         'countryName'
    //     )
    // );
    // const countriesSearchLoading = useSelector(state =>
    //     collectionSelectorHelpers.getSearchLoading(state.countries)
    // );
    // const searchCountries = searchTerm => dispatch(countriesActions.search(searchTerm));

    const currencies = useSelector(state => collectionSelectorHelpers.getItems(state.currencies));
    const employees = useSelector(state => collectionSelectorHelpers.getItems(state.employees));

    const nominalsSearchItems = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.nominals)
    );
    const nominalsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.nominals)
    );

    // const currentUserId = useSelector(state => userSelectors.getUserNumber(state));
    // const currentUserName = useSelector(state => userSelectors.getName(state));

    const snackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state.purchaseOrder)
    );

    const [editStatus, setEditStatus] = useState('view');
    const [authEmailDialogOpen, setAuthEmailDialogOpen] = useState(false);
    const [employeeToEmail, setEmployeeToEmail] = useState();

    useEffect(() => dispatch(currenciesActions.fetch()), [dispatch]);
    useEffect(() => dispatch(employeesActions.fetch()), [dispatch]);

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

    // const allowedToCancel = () => !creating && order.links?.some(l => l.rel === 'cancel');
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
            dispatch(purchaseOrderActions.postByHref(utilities.getHref(item, 'authorise')));
        }
    };

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        setOrder(a => ({ ...a, [propertyName]: newValue }));
    };

    // const handleCancelClick = () => {
    //     if (allowedToCancel) {
    //         clearErrors();
    //         dispatch(purchaseOrderActions.postByHref(utilities.getHref(item, 'cancel')));
    //     }
    // };

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

    const handleNominalUpdate = newNominal => {
        setEditStatus('edit');

        setOrder(r => ({
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
                            onClose={() => dispatch(purchaseOrderActions.setSnackbarVisible(false))}
                            message="Save successful"
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
                                                />handleFieldChange
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
                        <Grid item xs={1} />
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
                                    setOrder(a => ({
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

                        {order.details?.map(detail => (
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
                                            onChange={handleFieldChange}
                                            disabled
                                        />
                                    </Grid>
                                    <Grid item xs={6}>
                                        <InputField
                                            fullWidth
                                            value={detail.ourQty}
                                            label="Our quantity"
                                            propertyName="ourQty"
                                            onChange={handleFieldChange}
                                            disabled={!allowedToUpdate()}
                                            type="number"
                                            required
                                        />
                                    </Grid>
                                    <Grid item xs={6}>
                                        <InputField
                                            fullWidth
                                            value={detail.orderQty}
                                            label="Order quantity"
                                            propertyName="Order qty"
                                            onChange={handleFieldChange}
                                            disabled={!allowedToUpdate()}
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
                                            onChange={handleFieldChange}
                                            disabled
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
                                            onChange={handleFieldChange}
                                            disabled={!allowedToUpdate()}
                                            type="number"
                                            required
                                        />
                                    </Grid>
                                </Grid>
                                <Grid item xs={8}>
                                    <InputField
                                        fullWidth
                                        value={detail.partDescription}
                                        label="Description"
                                        propertyName="partDescription"
                                        onChange={handleFieldChange}
                                        rows={8}
                                        disabled
                                    />
                                </Grid>

                                <Grid item xs={4}>
                                    <InputField
                                        fullWidth
                                        value={detail.netTotalCurrency}
                                        label="Net total (currency)"
                                        propertyName="netTotalCurrency"
                                        onChange={handleFieldChange}
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
                                        onChange={handleFieldChange}
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
                                        onChange={handleFieldChange}
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
                                        onChange={handleFieldChange}
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
                                        onChange={handleFieldChange}
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
                                        onChange={handleFieldChange}
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
                                        onChange={handleFieldChange}
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
                                        onChange={handleFieldChange}
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
                                            dispatch(nominalsActions.search(searchTerm))
                                        }
                                        modal
                                        placeholder="Search Dept/Nominal"
                                        links={false}
                                        clearSearch={() => dispatch(nominalsActions.clearSearch)}
                                        loading={nominalsSearchLoading}
                                        label="Department"
                                        title="Search Department"
                                        value={detail.department?.departmentCode}
                                        onSelect={newValue => handleNominalUpdate(newValue)}
                                        debounce={1000}
                                        minimumSearchTermLength={2}
                                        disabled={!allowedToUpdate}
                                        required
                                    />
                                </Grid>
                                <Grid item xs={8}>
                                    <InputField
                                        fullWidth
                                        value={detail.department?.description}
                                        label="Description"
                                        disabled
                                        propertyName="nominalDescription"
                                    />
                                </Grid>
                                <Grid item xs={4}>
                                    <InputField
                                        fullWidth
                                        value={detail.nominal?.nominalCode}
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
                                        value={detail.nominal?.description}
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

                                {detail.purchaseDeliveries?.map(delivery => (
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
                                                onChange={() => {}}
                                                propertyName="dateRequested"
                                                required
                                                type="date"
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
                                                disabled={!allowedToUpdate()}
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
                                        onChange={handleFieldChange}
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
                                    dispatch(purchaseOrderActions.update(order.orderNumber, order));
                                }}
                                cancelClick={() => {
                                    setEditStatus('view');
                                    // if (creating) {
                                    //     setOrder(defaultCreatingOrder);
                                    // } else {
                                    setOrder(item);
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
