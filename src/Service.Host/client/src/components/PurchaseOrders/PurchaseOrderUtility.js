import React, { useEffect, useState, useReducer } from 'react';
import { useMediaQuery } from 'react-responsive';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { useParams } from 'react-router-dom';
import Dialog from '@mui/material/Dialog';
import LinearProgress from '@mui/material/LinearProgress';
import IconButton from '@mui/material/IconButton';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import PrintIcon from '@mui/icons-material/Print';
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
    getItemError,
    ErrorCard,
    utilities,
    SaveBackCancelButtons,
    OnOffSwitch,
    processSelectorHelpers
} from '@linn-it/linn-form-components-library';
import currenciesActions from '../../actions/currenciesActions';
import nominalsActions from '../../actions/nominalsActions';
import suppliersActions from '../../actions/suppliersActions';
import history from '../../history';
import config from '../../config';
import purchaseOrderActions from '../../actions/purchaseOrderActions';
import reducer from './purchaseOrderReducer';
import unitsOfMeasureActions from '../../actions/unitsOfMeasureActions';
import sendPurchaseOrderPdfEmailActionTypes from '../../actions/sendPurchaseOrderPdfEmailActions';
import sendPurchaseOrderSupplierAssActionTypes from '../../actions/sendPurchaseOrderSupplierAssEmailActions';
import {
    purchaseOrder,
    sendPurchaseOrderPdfEmail,
    sendPurchaseOrderAuthEmail,
    sendPurchaseOrderDeptEmail,
    purchaseOrderDeliveries,
    suggestedPurchaseOrderValues
} from '../../itemTypes';
import currencyConvert from '../../helpers/currencyConvert';
import PurchaseOrderDeliveriesUtility from '../PurchaseOrderDeliveriesUtility';
import sendOrderAuthEmailActions from '../../actions/sendPurchaseOrderAuthEmailActions';
import purchaseOrderDeliveriesActions from '../../actions/purchaseOrderDeliveriesActions';
import sendPurchaseOrderDeptEmailActions from '../../actions/sendPurchaseOrderDeptEmailActions';

function PurchaseOrderUtility({ creating }) {
    const reduxDispatch = useDispatch();
    const clearErrors = () => reduxDispatch(purchaseOrderActions.clearErrorsForItem());

    const { orderNumber } = useParams();

    useEffect(() => {
        if (orderNumber) {
            reduxDispatch(sendPurchaseOrderDeptEmailActions.clearErrorsForItem());
            reduxDispatch(purchaseOrderActions.fetch(orderNumber));
        } else if (creating) {
            reduxDispatch(purchaseOrderActions.fetchState());
        }
    }, [orderNumber, reduxDispatch, creating]);

    useEffect(() => {
        reduxDispatch(currenciesActions.fetch());
    }, [reduxDispatch]);
    useEffect(() => {
        reduxDispatch(unitsOfMeasureActions.fetch());
    }, [reduxDispatch]);

    const deliveryTableColumns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'deliverySeq', headerName: 'Delivery', width: 100 },
        { field: 'ourDeliveryQty', headerName: 'Qty', width: 100 },
        {
            field: 'dateRequested',
            headerName: 'Request Date',
            width: 200,
            type: 'date'
        },
        {
            field: 'dateAdvised',
            headerName: 'Advised Date',
            width: 200
        },
        {
            field: 'availableAtSupplier',
            headerName: 'Available at Supplier?',
            width: 200,
            valueOptions: ['Y', 'N']
        }
    ];

    useEffect(() => {
        reduxDispatch(currenciesActions.fetch());
    }, [reduxDispatch]);
    useEffect(() => {
        reduxDispatch(unitsOfMeasureActions.fetch());
    }, [reduxDispatch]);

    const item = useSelector(state => itemSelectorHelpers.getItem(state[purchaseOrder.item]));
    const applicationState = useSelector(state =>
        itemSelectorHelpers.getApplicationState(state[purchaseOrder.item])
    );

    const suggestedValues = useSelector(state =>
        itemSelectorHelpers.getItem(state[suggestedPurchaseOrderValues.item])
    );

    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[purchaseOrder.item])
    );

    const suggestedValuesLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[suggestedPurchaseOrderValues.item])
    );

    const deliveriesLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[purchaseOrderDeliveries.item])
    );

    const itemError = useSelector(state => getItemError(state, purchaseOrder.item));

    const deptEmailError = useSelector(state =>
        getItemError(state, sendPurchaseOrderDeptEmail.item)
    );

    const [order, dispatch] = useReducer(reducer, null);

    const [purchaseOrderEmailState, setPurchaseOrderEmailState] = useState({
        email: '',
        bcc: false
    });

    useEffect(() => {
        if (!creating && item?.supplier?.id) {
            reduxDispatch(purchaseOrderActions.clearErrorsForItem());
            dispatch({ type: 'initialise', payload: item });
            setPurchaseOrderEmailState({ bcc: false, email: item.supplierContactEmail?.trim() });
        } else if (creating && applicationState && !suggestedValues) {
            reduxDispatch(purchaseOrderActions.clearErrorsForItem());
            dispatch({
                type: 'initialise',
                payload: applicationState
            });
        } else if (creating && suggestedValues) {
            console.log(suggestedValues);
            reduxDispatch(purchaseOrderActions.clearErrorsForItem());
            dispatch({ type: 'initialise', payload: suggestedValues });
        }
    }, [item, applicationState, creating, reduxDispatch, suggestedValues]);

    const [printHtml, setPrintHtml] = useState(<span>loading</span>);

    useEffect(() => {
        if (item?.orderNumber) {
            fetch(config.appRoot + utilities.getHref(item, 'html'))
                .then(response => response?.text?.())
                .then(html => {
                    setPrintHtml(html);
                });
        }
    }, [item?.orderNumber, item]);

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'id', 'name')
    );
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );
    const searchSuppliers = searchTerm => reduxDispatch(suppliersActions.search(searchTerm));

    const currencies = useSelector(state => collectionSelectorHelpers.getItems(state.currencies));
    const unitsOfMeasure = useSelector(reduxState =>
        collectionSelectorHelpers.getItems(reduxState.unitsOfMeasure)
    );

    const nominalsSearchItems = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.nominals)
    );
    const nominalsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.nominals)
    );

    const snackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state[purchaseOrder.item])
    );

    const deliveriesSnackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state[purchaseOrderDeliveries.item])
    );

    const [editStatus, setEditStatus] = useState('view');
    const [authEmailDialogOpen, setAuthEmailDialogOpen] = useState(false);

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

    const allowedToAuthorise = () => !creating && utilities.getHref(order, 'authorise');

    const allowedToUpdate = () => {
        if (creating) {
            return utilities.getHref(order, 'create');
        }
        return utilities.getHref(order, 'edit');
    };

    const inputIsValid = () =>
        order.supplier?.id &&
        order.details.every(
            d =>
                d.partNumber &&
                d.ourQty &&
                d.ourUnitPriceCurrency &&
                d.orderUnitPriceCurrency &&
                d.ourUnitOfMeasure &&
                d.orderPosting?.nominalAccount?.department?.departmentCode &&
                d.orderPosting?.nominalAccount?.nominal?.nominalCode &&
                d.netTotalCurrency &&
                d.detailTotalCurrency &&
                d.baseNetTotal &&
                d.baseDetailTotal
        ) &&
        order.supplierContactEmail &&
        order.currency.code &&
        order.deliveryAddress?.addressId;

    const canSave = () =>
        editStatus !== 'view' && allowedToUpdate() && inputIsValid() && order !== item;

    const handleAuthorise = () => {
        setEditStatus('edit');
        if (allowedToAuthorise()) {
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

    const handleCurrencyChange = (propertyName, newCurrencyCode) => {
        setEditStatus('edit');
        const name = currencies.find(x => x.code === newCurrencyCode)?.name;
        dispatch({
            newCurrency: { code: newCurrencyCode, name },
            propertyName: 'currency',
            type: 'currencyChange'
        });
    };

    const handleDetailValueFieldChange = (propertyName, basePropertyName, newValue, detail) => {
        const { exchangeRate } = order;

        if (exchangeRate && newValue && newValue > 0 && newValue !== order[propertyName]) {
            setEditStatus('edit');
            const convertedValue = currencyConvert(newValue, exchangeRate);

            dispatch({
                payload: {
                    ...detail,
                    [propertyName]: newValue,
                    [basePropertyName]: convertedValue
                },
                type: 'detailCalculationFieldChange'
            });
        }
    };

    const handleDetailQtyFieldChange = (propertyName, newValue, detail) => {
        if (newValue && newValue > 0 && newValue !== order[propertyName]) {
            setEditStatus('edit');

            dispatch({
                payload: {
                    ...detail,
                    [propertyName]: newValue
                },
                type: 'detailCalculationFieldChange'
            });
        }
    };

    const handleSendAuthoriseEmail = () => {
        setAuthEmailDialogOpen(false);
        reduxDispatch(sendOrderAuthEmailActions.clearProcessData);
        reduxDispatch(
            sendOrderAuthEmailActions.requestProcessStart('', {
                orderNumber: order?.orderNumber
            })
        );
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

    const [selectedDeliveries, setSelectedDeliveries] = useState();

    const [deliveriesDialogOpen, setDeliveriesDialogOpen] = useState(false);
    const [selectedOrderLine, setSelectedOrderLine] = useState();
    const updateDeliveries = orderLine => {
        setSelectedOrderLine(orderLine);
        setSelectedDeliveries(order.details.find(d => d.line === orderLine).purchaseDeliveries);
        setDeliveriesDialogOpen(true);
    };

    const orderPdfEmailMessageVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[sendPurchaseOrderPdfEmail.item])
    );

    const orderPdfEmailMessage = useSelector(state =>
        processSelectorHelpers.getMessageText(state[sendPurchaseOrderPdfEmail.item])
    );

    const authEmailMessageVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[sendPurchaseOrderAuthEmail.item])
    );

    const authEmailMessage = useSelector(state =>
        processSelectorHelpers.getMessageText(state[sendPurchaseOrderAuthEmail.item])
    );

    const deptEmailMessageVisible = useSelector(
        state => state[sendPurchaseOrderDeptEmail.item].snackbarVisible
    );

    const deptEmailText = useSelector(
        state => state[sendPurchaseOrderDeptEmail.item].item?.message
    );

    const deptEmailLoading = useSelector(state => state[sendPurchaseOrderDeptEmail.item].loading);

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

    const handleSupplierAssEmailClick = () => {
        reduxDispatch(
            sendPurchaseOrderSupplierAssActionTypes.requestProcessStart('', {
                orderNumber: order.orderNumber
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
    const screenIsSmall = useMediaQuery({ query: `(max-width: 1200px)` });
    const [overridingOrderPrice, setOverridingOrderPrice] = useState(false);
    const [overridingOrderQty, setOverridingOrderQty] = useState(false);

    const getDateString = isoString =>
        isoString ? new Date(isoString).toLocaleDateString('en-GB') : null;

    return (
        <>
            <div className="hide-when-printing">
                <Page history={history} homeUrl={config.appRoot} width={screenIsSmall ? 'xl' : 'm'}>
                    {loading || deliveriesLoading || suggestedValuesLoading ? (
                        <Loading />
                    ) : (
                        order && (
                            <Grid container spacing={1} justifyContent="center">
                                <SnackbarMessage
                                    visible={snackbarVisible || deliveriesSnackbarVisible}
                                    onClose={() => {
                                        reduxDispatch(
                                            purchaseOrderActions.setSnackbarVisible(false)
                                        );
                                        reduxDispatch(
                                            purchaseOrderDeliveriesActions.setSnackbarVisible(false)
                                        );
                                    }}
                                    message="Save successful"
                                />
                                <SnackbarMessage
                                    visible={orderPdfEmailMessageVisible}
                                    onClose={() =>
                                        reduxDispatch(
                                            sendPurchaseOrderPdfEmailActionTypes.setMessageVisible(
                                                false
                                            )
                                        )
                                    }
                                    message={orderPdfEmailMessage}
                                />
                                <SnackbarMessage
                                    visible={authEmailMessageVisible}
                                    onClose={() =>
                                        reduxDispatch(
                                            sendPurchaseOrderPdfEmailActionTypes.setMessageVisible(
                                                false
                                            )
                                        )
                                    }
                                    message={authEmailMessage}
                                />
                                <SnackbarMessage
                                    visible={deptEmailMessageVisible}
                                    onClose={() =>
                                        reduxDispatch(
                                            sendPurchaseOrderDeptEmailActions.setSnackbarVisible(
                                                false
                                            )
                                        )
                                    }
                                    message={deptEmailText}
                                />
                                {itemError && (
                                    <Grid item xs={12}>
                                        <ErrorCard
                                            errorMessage={
                                                itemError?.details ?? itemError.statusText
                                            }
                                        />
                                    </Grid>
                                )}
                                <Dialog open={authEmailDialogOpen} fullWidth maxWidth="md">
                                    <div className={classes.centerTextInDialog}>
                                        <IconButton
                                            className={classes.pullRight}
                                            aria-label="Close"
                                            onClick={() => setAuthEmailDialogOpen(false)}
                                        >
                                            <Close />
                                        </IconButton>
                                        <Typography variant="h6">Finance Auth Request</Typography>
                                        <Typography variant="body1" gutterBottom>
                                            <Grid container spacing={1}>
                                                <Grid item xs={8}>
                                                    <i>Sends email with a link to Finance</i>
                                                </Grid>
                                                <Grid item xs={4}>
                                                    <Tooltip
                                                        title="Send"
                                                        placement="top"
                                                        className={classes.cursorPointer}
                                                    >
                                                        <Send
                                                            className={classes.buttonMarginTop}
                                                            onClick={() =>
                                                                handleSendAuthoriseEmail()
                                                            }
                                                        />
                                                    </Tooltip>
                                                </Grid>
                                            </Grid>
                                        </Typography>
                                    </div>
                                </Dialog>
                                {!creating && selectedDeliveries && (
                                    <Dialog open={deliveriesDialogOpen} fullWidth maxWidth="md">
                                        <div className={classes.centerTextInDialog}>
                                            <IconButton
                                                className={classes.pullRight}
                                                aria-label="Close"
                                                onClick={() => setDeliveriesDialogOpen(false)}
                                            >
                                                <Close />
                                            </IconButton>
                                            <PurchaseOrderDeliveriesUtility
                                                orderNumber={order.orderNumber}
                                                orderLine={selectedOrderLine}
                                                inDialogBox
                                                deliveries={selectedDeliveries.map(d => ({
                                                    ...d,
                                                    id: `${d.orderNumber}/${d.line}/${d.deliverySeq}`,
                                                    dateRequested: getDateString(d.dateRequested),
                                                    dateAdvised: getDateString(d.dateAdvised)
                                                }))}
                                                backClick={() => setDeliveriesDialogOpen(false)}
                                                closeOnSave
                                            />
                                        </div>
                                    </Dialog>
                                )}
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
                                                                email: newEmail.trim()
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
                                                            onClick={() =>
                                                                handleOrderPdfEmailClick()
                                                            }
                                                        />
                                                    </Tooltip>
                                                </Grid>
                                            </Grid>
                                        </Typography>
                                    </div>
                                </Dialog>
                                <Grid item xs={10}>
                                    <Typography variant="h6">Purchase Order Utility </Typography>
                                </Grid>
                                <Grid item xs={2}>
                                    <div className={classes.centeredIcon}>
                                        {allowedToUpdate() ? (
                                            <Tooltip
                                                title={`You can ${
                                                    creating ? 'create' : 'edit'
                                                } purchase orders`}
                                            >
                                                <ModeEditIcon fontSize="large" color="primary" />
                                            </Tooltip>
                                        ) : (
                                            <Tooltip
                                                title={`You cannot ${
                                                    creating ? 'create' : 'edit'
                                                } purchase orders`}
                                            >
                                                <EditOffIcon color="secondary" />
                                            </Tooltip>
                                        )}
                                    </div>
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
                                    <InputField
                                        fullWidth
                                        value={`${order?.documentType?.name} - ${order?.documentType?.description}`}
                                        label="Order Type"
                                        propertyName="documentType"
                                        disabled
                                    />
                                </Grid>
                                <Grid item xs={2}>
                                    <InputField
                                        fullWidth
                                        value={`${order?.orderMethod?.name} - ${order?.orderMethod?.description}`}
                                        label="Order Method"
                                        propertyName="orderMethod"
                                        disabled
                                    />
                                </Grid>
                                <Grid item xs={1}>
                                    <Button
                                        className={classes.buttonMarginTop}
                                        aria-label="Print"
                                        variant="contained"
                                        onClick={window.print}
                                        disabled={creating}
                                        startIcon={<PrintIcon />}
                                    />
                                </Grid>

                                <Grid item xs={2}>
                                    <Button
                                        className={classes.buttonMarginTop}
                                        aria-label="Email"
                                        variant="outlined"
                                        onClick={() => setOrderPdfEmailDialogOpen(true)}
                                        disabled={creating}
                                        startIcon={<Email />}
                                    >
                                        Supplier
                                    </Button>
                                </Grid>
                                <Grid item xs={3}>
                                    <Typeahead
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
                                <Grid item xs={2}>
                                    <Dropdown
                                        fullWidth
                                        value={order.issuePartsToSupplier}
                                        label="Issue Parts to Supplier"
                                        number
                                        propertyName="issuePartsToSupplier"
                                        onChange={handleFieldChange}
                                        disabled={!creating}
                                        allowNoValue={false}
                                        items={['Y', 'N']}
                                        required
                                    />
                                </Grid>
                                <Grid item xs={1}>
                                    {order.issuePartsToSupplier === 'Y' && (
                                        <Tooltip title="Email kitting to logistics">
                                            <IconButton
                                                className={classes.buttonMarginTop}
                                                aria-label="Email"
                                                onClick={() => handleSupplierAssEmailClick(true)}
                                                disabled={creating}
                                            >
                                                <Email />
                                            </IconButton>
                                        </Tooltip>
                                    )}
                                </Grid>

                                <Grid item xs={4}>
                                    <InputField
                                        fullWidth
                                        value={order.orderContactName}
                                        label="Order Contact"
                                        propertyName="orderContactName"
                                        onChange={handleFieldChange}
                                        disabled={!creating}
                                    />
                                </Grid>
                                <Grid item xs={4}>
                                    <InputField
                                        fullWidth
                                        value={order.supplierContactPhone}
                                        label="Phone Number"
                                        number
                                        propertyName="supplierContactPhone"
                                        onChange={handleFieldChange}
                                        disabled={!creating}
                                    />
                                </Grid>
                                <Grid item xs={4}>
                                    <InputField
                                        fullWidth
                                        value={order.supplierContactEmail}
                                        label="Email Address"
                                        propertyName="supplierContactEmail"
                                        onChange={(propertyName, newValue) =>
                                            handleFieldChange(propertyName, newValue.trim())
                                        }
                                        disabled={!creating}
                                        required
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
                                        onChange={handleCurrencyChange}
                                        disabled={!creating}
                                        required
                                    />
                                </Grid>
                                <Grid item xs={5}>
                                    <InputField
                                        fullWidth
                                        value={order.currency?.name}
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
                                            disabled={!creating}
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
                                    <Grid item xs={3}>
                                        <Tooltip title="Email to request authorisation">
                                            <Button
                                                className={classes.buttonMarginTop}
                                                aria-label="Email Finance"
                                                variant="outlined"
                                                onClick={() => setAuthEmailDialogOpen(true)}
                                                disabled={creating || !allowedToAuthorise()}
                                                startIcon={<Email />}
                                            >
                                                Finance
                                            </Button>
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
                                            <Grid container item spacing={1} xs={6}>
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
                                                            handleDetailQtyFieldChange(
                                                                propertyName,
                                                                newValue,
                                                                detail
                                                            )
                                                        }
                                                        disabled={!allowedToUpdate()}
                                                        type="number"
                                                        required
                                                    />
                                                </Grid>
                                                <Grid item xs={6}>
                                                    {overridingOrderQty ? (
                                                        <Tooltip
                                                            title="Order qty is set automatically based on Our qty.
                                                            Only change this if you have a good reason to override it."
                                                            placement="top"
                                                            className={classes.cursorPointer}
                                                        >
                                                            <Grid item xs={12}>
                                                                <InputField
                                                                    fullWidth
                                                                    value={detail.orderQty}
                                                                    label="Order quantity"
                                                                    propertyName="orderQty"
                                                                    onChange={(
                                                                        propertyName,
                                                                        newValue
                                                                    ) =>
                                                                        handleDetailFieldChange(
                                                                            propertyName,
                                                                            newValue,
                                                                            detail
                                                                        )
                                                                    }
                                                                    type="number"
                                                                    required
                                                                />
                                                            </Grid>
                                                        </Tooltip>
                                                    ) : (
                                                        <Tooltip
                                                            title="Order qty is set automatically based on Our qty.
                                                Only change this if you have a good reason to override it."
                                                            placement="top"
                                                            className={classes.cursorPointer}
                                                        >
                                                            <Grid container item>
                                                                <Grid item xs={4}>
                                                                    <InputField
                                                                        fullWidth
                                                                        value={detail.orderQty}
                                                                        label="Order quantity"
                                                                        propertyName="orderQty"
                                                                        disabled
                                                                        type="number"
                                                                        required
                                                                    />
                                                                </Grid>
                                                                <Grid item xs={8}>
                                                                    <Button
                                                                        className={
                                                                            classes.buttonMarginTop
                                                                        }
                                                                        color="primary"
                                                                        variant="contained"
                                                                        disabled={
                                                                            !allowedToUpdate()
                                                                        }
                                                                        onClick={() =>
                                                                            setOverridingOrderQty(
                                                                                true
                                                                            )
                                                                        }
                                                                    >
                                                                        Override
                                                                    </Button>
                                                                </Grid>
                                                            </Grid>
                                                        </Tooltip>
                                                    )}
                                                </Grid>

                                                <Grid item xs={6}>
                                                    <InputField
                                                        fullWidth
                                                        value={detail.ourUnitPriceCurrency}
                                                        label="Our price (unit, currency)"
                                                        propertyName="ourUnitPriceCurrency"
                                                        onChange={(propertyName, newValue) =>
                                                            handleDetailValueFieldChange(
                                                                propertyName,
                                                                'baseUnitPrice',
                                                                newValue,
                                                                detail
                                                            )
                                                        }
                                                        disabled={!allowedToUpdate()}
                                                        type="number"
                                                        required
                                                    />
                                                </Grid>
                                                <Grid item xs={6}>
                                                    {overridingOrderPrice ? (
                                                        <Tooltip
                                                            title="Order price is set automatically based on Our price. Only change this if you have a good reason to override it."
                                                            placement="top"
                                                            className={classes.cursorPointer}
                                                        >
                                                            <Grid item xs={12}>
                                                                <InputField
                                                                    fullWidth
                                                                    value={
                                                                        detail.orderUnitPriceCurrency
                                                                    }
                                                                    label="Order price (currency)"
                                                                    propertyName="orderUnitPriceCurrency"
                                                                    onChange={(
                                                                        propertyName,
                                                                        newValue
                                                                    ) =>
                                                                        handleDetailFieldChange(
                                                                            propertyName,
                                                                            newValue,
                                                                            detail
                                                                        )
                                                                    }
                                                                    type="number"
                                                                    required
                                                                />
                                                            </Grid>
                                                        </Tooltip>
                                                    ) : (
                                                        <Tooltip
                                                            title="Order price is set automatically based on Our price.
                                                Only change this if you have a good reason to override it."
                                                            placement="top"
                                                            className={classes.cursorPointer}
                                                        >
                                                            <Grid container xs={12}>
                                                                <Grid item xs={4}>
                                                                    <InputField
                                                                        fullWidth
                                                                        value={
                                                                            detail.orderUnitPriceCurrency
                                                                        }
                                                                        label="Order price (currency)"
                                                                        propertyName="orderUnitPriceCurrency"
                                                                        disabled
                                                                        type="number"
                                                                        required
                                                                    />
                                                                </Grid>
                                                                <Grid item xs={8}>
                                                                    <Button
                                                                        className={
                                                                            classes.buttonMarginTop
                                                                        }
                                                                        color="primary"
                                                                        variant="contained"
                                                                        disabled={
                                                                            !allowedToUpdate()
                                                                        }
                                                                        onClick={() =>
                                                                            setOverridingOrderPrice(
                                                                                true
                                                                            )
                                                                        }
                                                                    >
                                                                        Override
                                                                    </Button>
                                                                </Grid>
                                                            </Grid>
                                                        </Tooltip>
                                                    )}
                                                </Grid>
                                            </Grid>
                                            <Grid item xs={6} spacing={1}>
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
                                            <Grid item xs={3}>
                                                <Button
                                                    className={classes.buttonMarginTop}
                                                    aria-label="Email Dept"
                                                    variant="outlined"
                                                    onClick={() => {
                                                        reduxDispatch(
                                                            sendPurchaseOrderDeptEmailActions.clearErrorsForItem()
                                                        );
                                                        reduxDispatch(
                                                            sendPurchaseOrderDeptEmailActions.postByHref(
                                                                utilities.getHref(
                                                                    order,
                                                                    'email-dept'
                                                                ),
                                                                {}
                                                            )
                                                        );
                                                    }}
                                                    disabled={
                                                        creating ||
                                                        !utilities.getHref(order, 'email-dept')
                                                    }
                                                    startIcon={<Email />}
                                                >
                                                    Email Dept
                                                </Button>
                                            </Grid>
                                            <Grid
                                                item
                                                xs={9}
                                                justify="flex-end"
                                                alignItems="center"
                                                spacing={2}
                                            >
                                                {deptEmailLoading && <LinearProgress />}
                                                {deptEmailError && (
                                                    <ErrorCard
                                                        errorMessage={
                                                            deptEmailError?.details ??
                                                            itemError.statusText
                                                        }
                                                    />
                                                )}
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
                                                    label="Vat total (currency) - calculated on save"
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
                                                    label="Base vat total - calculated on save"
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
                                                <Dropdown
                                                    fullWidth
                                                    value={detail.ourUnitOfMeasure}
                                                    label="Our Unit Of Measure"
                                                    propertyName="ourUnitOfMeasure"
                                                    items={unitsOfMeasure.map(x => x.unit)}
                                                    onChange={(propertyName, newValue) =>
                                                        handleDetailFieldChange(
                                                            propertyName,
                                                            newValue,
                                                            detail
                                                        )
                                                    }
                                                    disabled={!creating}
                                                    required
                                                />
                                            </Grid>

                                            <Grid item xs={4}>
                                                <Dropdown
                                                    fullWidth
                                                    value={detail.orderUnitOfMeasure}
                                                    label="Order Unit Of Measure"
                                                    propertyName="orderUnitOfMeasure"
                                                    items={unitsOfMeasure.map(x => x.unit)}
                                                    onChange={(propertyName, newValue) =>
                                                        handleDetailFieldChange(
                                                            propertyName,
                                                            newValue,
                                                            detail
                                                        )
                                                    }
                                                    disabled={!creating}
                                                    required
                                                />
                                            </Grid>
                                            <Grid item xs={4} />

                                            <Grid item xs={4}>
                                                <TypeaheadTable
                                                    table={nominalAccountsTable}
                                                    columnNames={[
                                                        'Nominal',
                                                        'Description',
                                                        'Dept',
                                                        'Name'
                                                    ]}
                                                    fetchItems={searchTerm =>
                                                        reduxDispatch(
                                                            nominalsActions.search(searchTerm)
                                                        )
                                                    }
                                                    modal
                                                    placeholder="Search Dept/Nominal"
                                                    links={false}
                                                    clearSearch={() =>
                                                        reduxDispatch(nominalsActions.clearSearch)
                                                    }
                                                    loading={nominalsSearchLoading}
                                                    label="Department"
                                                    title="Search on Department or Nominal"
                                                    value={
                                                        detail.orderPosting?.nominalAccount
                                                            ?.department?.departmentCode
                                                    }
                                                    onSelect={newValue =>
                                                        handleNominalUpdate(newValue, detail.line)
                                                    }
                                                    debounce={1000}
                                                    minimumSearchTermLength={2}
                                                    disabled={!allowedToUpdate}
                                                    required
                                                />
                                            </Grid>
                                            <Grid item xs={8}>
                                                <InputField
                                                    fullWidth
                                                    value={
                                                        detail.orderPosting?.nominalAccount
                                                            ?.department?.description
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
                                            {detail.originalOrderNumber && (
                                                <>
                                                    <Grid item xs={3}>
                                                        <InputField
                                                            fullWidth
                                                            value={detail.originalOrderNumber}
                                                            label="Original Order Number"
                                                            propertyName="originalOrderNumber"
                                                            onChange={(propertyName, newValue) =>
                                                                handleDetailFieldChange(
                                                                    propertyName,
                                                                    newValue,
                                                                    detail
                                                                )
                                                            }
                                                            disabled={!creating}
                                                        />
                                                    </Grid>
                                                    <Grid item xs={3}>
                                                        <InputField
                                                            fullWidth
                                                            value={detail.originalOrderLine}
                                                            label="Original Order Line"
                                                            propertyName="originalOrderLine"
                                                            onChange={(propertyName, newValue) =>
                                                                handleDetailFieldChange(
                                                                    propertyName,
                                                                    newValue,
                                                                    detail
                                                                )
                                                            }
                                                            disabled={!creating}
                                                        />
                                                    </Grid>
                                                    <Grid item xs={6} />
                                                </>
                                            )}
                                            <Grid item xs={12}>
                                                <InputField
                                                    fullWidth
                                                    value={detail.deliveryInstructions}
                                                    label="Delivery Instructions"
                                                    propertyName="deliveryInstructions"
                                                    onChange={(propertyName, newValue) =>
                                                        handleDetailFieldChange(
                                                            propertyName,
                                                            newValue,
                                                            detail
                                                        )
                                                    }
                                                    disabled={!creating}
                                                    rows={2}
                                                />
                                            </Grid>
                                            {!creating && detail.purchaseDeliveries && (
                                                <>
                                                    <Grid
                                                        item
                                                        xs={12}
                                                        style={{ paddingTop: '40px' }}
                                                    >
                                                        <div>
                                                            <DataGrid
                                                                rows={detail.purchaseDeliveries.map(
                                                                    x => ({
                                                                        ...x,
                                                                        id: `${x.deliverySeq}`,
                                                                        dateRequested:
                                                                            getDateString(
                                                                                x.dateRequested
                                                                            ),
                                                                        dateAdvised: getDateString(
                                                                            x.dateAdvised
                                                                        )
                                                                    })
                                                                )}
                                                                columns={deliveryTableColumns}
                                                                density="compact"
                                                                rowHeight={34}
                                                                autoHeight
                                                                loading={loading}
                                                                columnBuffer={8}
                                                                hideFooter
                                                            />
                                                        </div>
                                                    </Grid>
                                                    <Grid item xs={12}>
                                                        <Button
                                                            onClick={() =>
                                                                updateDeliveries(detail.line)
                                                            }
                                                        >
                                                            EDIT DELIVERIES
                                                        </Button>
                                                    </Grid>
                                                </>
                                            )}
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
                                <Grid item xs={6}>
                                    <SaveBackCancelButtons
                                        saveDisabled={!canSave()}
                                        saveClick={() => {
                                            clearErrors();
                                            setEditStatus('view');
                                            if (creating) {
                                                reduxDispatch(purchaseOrderActions.add(order));
                                            } else {
                                                reduxDispatch(
                                                    purchaseOrderActions.update(
                                                        order.orderNumber,
                                                        order
                                                    )
                                                );
                                            }
                                        }}
                                        cancelClick={() => {
                                            setEditStatus('view');
                                            dispatch(item);
                                        }}
                                    />
                                </Grid>
                            </Grid>
                        )
                    )}
                </Page>
            </div>
            <div
                className="show-only-when-printing"
                // eslint-disable-next-line react/no-danger
                dangerouslySetInnerHTML={{ __html: printHtml }}
                style={{ paddingTop: '40px', size: 'portrait' }}
            />
        </>
    );
}

PurchaseOrderUtility.propTypes = { creating: PropTypes.bool };
PurchaseOrderUtility.defaultProps = { creating: false };
export default PurchaseOrderUtility;
