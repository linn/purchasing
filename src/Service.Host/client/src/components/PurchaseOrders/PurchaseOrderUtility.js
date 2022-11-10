import React, { useEffect, useState, useReducer, Fragment } from 'react';
import { useMediaQuery } from 'react-responsive';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { useParams, useLocation } from 'react-router-dom';
import Dialog from '@mui/material/Dialog';
import LinearProgress from '@mui/material/LinearProgress';
import IconButton from '@mui/material/IconButton';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import PrintIcon from '@mui/icons-material/Print';
import Tooltip from '@mui/material/Tooltip';
import Close from '@mui/icons-material/Close';
import Email from '@mui/icons-material/Email';
import moment from 'moment';
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
    getItemError,
    ErrorCard,
    utilities,
    SaveBackCancelButtons,
    OnOffSwitch,
    processSelectorHelpers,
    getPreviousPaths
} from '@linn-it/linn-form-components-library';
import queryString from 'query-string';
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
import vendorManagersActions from '../../actions/vendorManagersActions';
import CancelUnCancelDialog from './CancelUnCancelDialog';
import FilCancelUnCancelDialog from './FilCancelUnCancelDialog';
import PlInvRecDialog from './PlInvRecDialog';
import handleBackClick from '../../helpers/handleBackClick';

function PurchaseOrderUtility({ creating }) {
    const reduxDispatch = useDispatch();
    const clearErrors = () => reduxDispatch(purchaseOrderActions.clearErrorsForItem());

    const { orderNumber } = useParams();
    const loc = useLocation();

    const [deptCode, setDeptCode] = useState('');
    const [deptDesc, setDeptDesc] = useState('');

    useEffect(() => {
        if (orderNumber) {
            reduxDispatch(sendPurchaseOrderDeptEmailActions.clearErrorsForItem());
            reduxDispatch(purchaseOrderActions.fetch(orderNumber));
        }
    }, [orderNumber, reduxDispatch]);

    useEffect(() => {
        if (creating) {
            reduxDispatch(purchaseOrderActions.fetchState());
        }
    }, [reduxDispatch, creating]);

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
        if (creating) {
            reduxDispatch(purchaseOrderActions.setEditStatus('create'));
        }
    }, [creating, reduxDispatch]);

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
            reduxDispatch(purchaseOrderActions.clearErrorsForItem());
            dispatch({ type: 'initialise', payload: suggestedValues });
            if (utilities.getHref(applicationState, 'create-for-other-user')) {
                reduxDispatch(vendorManagersActions.fetch());
            }
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

    const vendorManagers = useSelector(reduxState =>
        collectionSelectorHelpers.getItems(reduxState.vendorManagers)
    );

    const deptSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state.nominals,
            100,
            'departmentCode',
            'departmentCode',
            'departmentDescription'
        )
    );

    const nominalSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state.nominals,
            100,
            'nominalCode',
            'nominalCode',
            'description'
        )
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

    const editStatus = useSelector(state =>
        itemSelectorHelpers.getItemEditStatus(state[purchaseOrder.item])
    );
    const [authEmailDialogOpen, setAuthEmailDialogOpen] = useState(false);

    const [invRecDialogOpen, setInvRecDialogOpen] = useState(
        !!queryString.parse(loc.search).invRecDialogOpen
    );

    const allowedToAuthorise = () => !creating && utilities.getHref(order, 'authorise');

    const allowedToUpdate = () => {
        if (creating) {
            return utilities.getHref(order, 'create');
        }
        return utilities.getHref(order, 'edit');
    };

    const allowedToFilCancel = () => !creating && utilities.getHref(order, 'fil-cancel');

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

    const canSave = editStatus !== 'view' && allowedToUpdate() && inputIsValid();

    const handleAuthorise = () => {
        reduxDispatch(purchaseOrderActions.setEditStatus('edit'));
        if (allowedToAuthorise()) {
            clearErrors();
            reduxDispatch(purchaseOrderActions.postByHref(utilities.getHref(item, 'authorise')));
        }
    };

    const handleFieldChange = (propertyName, newValue) => {
        reduxDispatch(purchaseOrderActions.setEditStatus('edit'));
        dispatch({ payload: newValue, propertyName, type: 'orderFieldChange' });
    };

    const handleDetailFieldChange = (propertyName, newValue, detail) => {
        reduxDispatch(purchaseOrderActions.setEditStatus('edit'));
        dispatch({ payload: { ...detail, [propertyName]: newValue }, type: 'detailFieldChange' });
    };

    const handleCurrencyChange = (propertyName, newCurrencyCode) => {
        reduxDispatch(purchaseOrderActions.setEditStatus('edit'));

        const name = currencies.find(x => x.code === newCurrencyCode)?.name;
        dispatch({
            newCurrency: { code: newCurrencyCode, name },
            propertyName: 'currency',
            type: 'currencyChange'
        });
    };
    const previousPaths = useSelector(state => getPreviousPaths(state));

    const handleDetailValueFieldChange = (propertyName, basePropertyName, newValue, detail) => {
        const { exchangeRate } = order;

        if (exchangeRate && newValue && newValue > 0 && newValue !== order[propertyName]) {
            reduxDispatch(purchaseOrderActions.setEditStatus('edit'));

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
            reduxDispatch(purchaseOrderActions.setEditStatus('edit'));

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
        reduxDispatch(purchaseOrderActions.setEditStatus('edit'));
        const newNominalAccount = {
            nominal: {
                nominalCode: newNominal.nominalCode,
                description: newNominal.description
            },
            department: {
                departmentCode: newNominal.departmentCode,
                description: newNominal.departmentDescription
            },
            accountId: newNominal.nominalAccountId
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

    const getDateString = isoString => {
        if (!isoString) {
            return null;
        }
        const date = new Date(isoString);
        return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
    };

    const [cancelDialogOpen, setCancelDialogOpen] = useState(false);
    const [currentLine, setCurrentLine] = useState(1);
    const [filCancelDialogOpen, setFilCancelDialogOpen] = useState(false);

    const filCancelLine = lineNumber => {
        setCurrentLine(lineNumber);
        setFilCancelDialogOpen(true);
    };

    const getDistinctVendorManagers = () => {
        const distinctUserNumbers = [...new Set(vendorManagers.map(x => x.userNumber))];
        const distinctVendorManagers = distinctUserNumbers.map(e =>
            vendorManagers.find(v => v.userNumber === e)
        );
        return distinctVendorManagers.map(v => ({
            id: v.userNumber.toString(),
            displayText: v.name
        }));
    };

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
                                {!creating && item && (
                                    <CancelUnCancelDialog
                                        open={cancelDialogOpen}
                                        setOpen={setCancelDialogOpen}
                                        mode={item.cancelled === 'Y' ? 'uncancel' : 'cancel'}
                                        order={item.orderNumber}
                                    />
                                )}
                                {!creating && (
                                    <FilCancelUnCancelDialog
                                        open={filCancelDialogOpen}
                                        setOpen={setFilCancelDialogOpen}
                                        mode={
                                            item.details.find(a => a.line === currentLine)
                                                .filCancelled === 'Y'
                                                ? 'uncancel'
                                                : 'cancel'
                                        }
                                        order={item.orderNumber}
                                        line={currentLine}
                                    />
                                )}
                                {!creating && (
                                    <PlInvRecDialog
                                        open={invRecDialogOpen}
                                        setOpen={setInvRecDialogOpen}
                                        ledgerEntries={order.ledgerEntries}
                                        inDialog
                                    />
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
                                                    dateRequested: d.dateRequested,
                                                    dateAdvised: d.dateAdvised
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
                                    <Typography variant="h6" display="inline">
                                        Purchase Order {!creating && item?.orderNumber}
                                    </Typography>
                                    {item?.cancelled === 'Y' && (
                                        <>
                                            {' '}
                                            <Typography
                                                variant="h6"
                                                display="inline"
                                                color="secondary"
                                            >
                                                (CANCELLED)
                                            </Typography>
                                        </>
                                    )}
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
                                <Grid item xs={12}>
                                    <Button
                                        className={classes.buttonMarginTop}
                                        aria-label="Pl Inv/Rec"
                                        onClick={() => setInvRecDialogOpen(true)}
                                        disabled={creating}
                                    >
                                        Pl Inv/Rec
                                    </Button>
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
                                            <span>
                                                <IconButton
                                                    className={classes.buttonMarginTop}
                                                    aria-label="Email"
                                                    onClick={() =>
                                                        handleSupplierAssEmailClick(true)
                                                    }
                                                    disabled={creating}
                                                >
                                                    <Email />
                                                </IconButton>
                                            </span>
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

                                <Grid item xs={4}>
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
                                        <Dropdown
                                            fullWidth
                                            value={order.sentByMethod}
                                            label="Sent by method"
                                            propertyName="sentByMethod"
                                            onChange={handleFieldChange}
                                            items={['EMAIL', 'FAX', 'POST', 'EDI', 'NONE']}
                                            allowNoValue
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
                                {creating && order.details?.[0].purchaseDeliveries && (
                                    <Grid item xs={12}>
                                        <InputField
                                            value={
                                                order.details?.[0].purchaseDeliveries[0]
                                                    .dateRequested
                                                    ? moment(
                                                          order.details?.[0].purchaseDeliveries[0]
                                                              .dateRequested
                                                      ).format('YYYY-MM-DDTHH:mm:ss')
                                                    : null
                                            }
                                            label="Date Requested"
                                            propertyName="dateRequested"
                                            onChange={(_, newValue) =>
                                                dispatch({
                                                    type: 'dateRequestedChange',
                                                    payload: { line: 1, newValue }
                                                })
                                            }
                                            type="date"
                                        />
                                    </Grid>
                                )}
                                <Grid container spacing={1} xs={7}>
                                    <Grid item xs={6}>
                                        {creating &&
                                        utilities.getHref(
                                            applicationState,
                                            'create-for-other-user'
                                        ) ? (
                                            <Dropdown
                                                fullWidth
                                                value={order.requestedBy?.id}
                                                label="Requested By (Leave blank to set as you)"
                                                onChange={handleFieldChange}
                                                items={getDistinctVendorManagers()}
                                                propertyName="requestedBy"
                                            />
                                        ) : (
                                            <InputField
                                                fullWidth
                                                value={`${order.requestedBy?.fullName} (${order.requestedBy?.id})`}
                                                label="Requested By"
                                                disabled
                                                propertyName="requestedBy"
                                            />
                                        )}
                                    </Grid>
                                    <Grid item xs={6}>
                                        <InputField
                                            fullWidth
                                            value={`${order.enteredBy?.fullName} (${order.enteredBy?.id})`}
                                            label="Entered By"
                                            propertyName="enteredBy"
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
                                            <span>
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
                                            </span>
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
                                            propertyName="authorisedBy"
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
                                                    utilities.getHref(order, 'email-dept'),
                                                    {}
                                                )
                                            );
                                        }}
                                        disabled={
                                            creating || !utilities.getHref(order, 'email-dept')
                                        }
                                        startIcon={<Email />}
                                    >
                                        Email Dept
                                    </Button>
                                </Grid>
                                <Grid item xs={9} justify="flex-end" alignItems="center">
                                    {deptEmailLoading && <LinearProgress />}
                                    {deptEmailError && (
                                        <ErrorCard
                                            errorMessage={
                                                deptEmailError?.details ?? itemError.statusText
                                            }
                                        />
                                    )}
                                </Grid>
                                <Grid item xs={3}>
                                    {!creating && item && (
                                        <Button
                                            className={classes.buttonMarginTop}
                                            aria-label={
                                                item.cancelled === 'N' ? 'Cancel' : 'UnCancel'
                                            }
                                            color={item.cancelled === 'N' ? 'secondary' : 'primary'}
                                            variant="contained"
                                            onClick={() => setCancelDialogOpen(true)}
                                        >
                                            {item.cancelled === 'N'
                                                ? 'Cancel Order'
                                                : 'UnCancel Order'}
                                        </Button>
                                    )}
                                </Grid>
                                {!creating && item?.cancelled === 'Y' ? (
                                    <>
                                        <Grid item xs={3}>
                                            <InputField
                                                fullWidth
                                                value={item.cancelledByName}
                                                label="Cancelled By"
                                                propertyName="cancelledByName"
                                                onChange={() => {}}
                                                disabled
                                            />
                                        </Grid>
                                        <Grid item xs={3}>
                                            <InputField
                                                fullWidth
                                                value={item.dateCancelled}
                                                label="Date"
                                                propertyName="dateCancelled"
                                                onChange={() => {}}
                                                disabled
                                            />
                                        </Grid>
                                        <Grid item xs={3}>
                                            <InputField
                                                fullWidth
                                                value={item.reasonCancelled}
                                                label="Reason"
                                                propertyName="reasonCancelled"
                                                onChange={() => {}}
                                                disabled
                                            />
                                        </Grid>
                                    </>
                                ) : (
                                    <Grid item xs={9} />
                                )}
                                {order.details
                                    ?.sort((a, b) => a.line - b.line)
                                    .map(detail => (
                                        <Fragment key={detail.line}>
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
                                                            <Grid container spacing={1}>
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
                                                            <Grid container spacing={1}>
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
                                            <Grid item xs={6}>
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
                                            <Grid item xs={1} />
                                            {detail.filCancelled === 'Y' && (
                                                <>
                                                    <Grid item xs={3} />
                                                    <Grid item xs={2}>
                                                        <InputField
                                                            fullWidth
                                                            value={detail.dateFilCancelled}
                                                            label="Date Fil Cancelled"
                                                            propertyName="dateFilCancelled"
                                                            disabled
                                                        />
                                                    </Grid>
                                                    <Grid item xs={3}>
                                                        <InputField
                                                            fullWidth
                                                            value={detail.filCancelledByName}
                                                            label="By"
                                                            propertyName="filCancelledByName"
                                                            disabled
                                                        />
                                                    </Grid>
                                                    <Grid item xs={4}>
                                                        <InputField
                                                            fullWidth
                                                            value={detail.reasonFilCancelled}
                                                            label="Reason Fil Cancelled"
                                                            propertyName="reasonFilCancelled"
                                                            disabled
                                                        />
                                                    </Grid>
                                                </>
                                            )}
                                            <Grid item xs={3}>
                                                {!creating && (
                                                    <Button
                                                        className={classes.buttonMarginTop}
                                                        aria-label={
                                                            detail.filCancelled === 'N'
                                                                ? 'Fil Cancel'
                                                                : 'UnFilCancel'
                                                        }
                                                        color={
                                                            detail.filCancelled === 'N'
                                                                ? 'secondary'
                                                                : 'primary'
                                                        }
                                                        variant="contained"
                                                        disabled={!allowedToFilCancel()}
                                                        onClick={() => filCancelLine(detail.line)}
                                                    >
                                                        {detail.filCancelled === 'N'
                                                            ? 'Fil Cancel'
                                                            : 'Un Fil Cancel'}
                                                    </Button>
                                                )}
                                            </Grid>
                                            <Grid item xs={4}>
                                                <Typeahead
                                                    onSelect={newValue => {
                                                        setDeptCode(newValue.departmentCode);
                                                        setDeptDesc(newValue.departmentDescription);
                                                    }}
                                                    label="Search Departments"
                                                    modal
                                                    openModalOnClick={false}
                                                    handleFieldChange={(_, newValue) => {
                                                        setDeptCode(newValue);
                                                    }}
                                                    propertyName="deptCode"
                                                    items={[
                                                        ...new Set(
                                                            deptSearchResults.map(
                                                                n => n.departmentCode
                                                            )
                                                        )
                                                    ].map(r => ({
                                                        ...deptSearchResults.find(
                                                            s => s.departmentCode === r
                                                        )
                                                    }))}
                                                    value={deptCode}
                                                    loading={nominalsSearchLoading}
                                                    fetchItems={searchTerm =>
                                                        reduxDispatch(
                                                            nominalsActions.search(searchTerm)
                                                        )
                                                    }
                                                    links={false}
                                                    title="Search Departments"
                                                    clearSearch={() => {}}
                                                    placeholder="Enter Dept Code or click search icon to look up"
                                                    minimumSearchTermLength={3}
                                                />
                                            </Grid>

                                            <Grid item xs={8}>
                                                <InputField
                                                    fullWidth
                                                    value={
                                                        detail.orderPosting?.nominalAccount
                                                            ?.department?.description || deptDesc
                                                    }
                                                    label="Description"
                                                    disabled
                                                    propertyName="nominalDescription"
                                                />
                                            </Grid>
                                            <Grid item xs={4}>
                                                <Typeahead
                                                    onSelect={newValue => {
                                                        handleNominalUpdate(newValue, detail.line);
                                                    }}
                                                    label="Search Nominals"
                                                    modal
                                                    propertyName="nominalCode"
                                                    items={nominalSearchResults.filter(
                                                        x =>
                                                            !deptCode ||
                                                            x.departmentCode.endsWith(deptCode)
                                                    )}
                                                    value={
                                                        detail.orderPosting?.nominalAccount?.nominal
                                                            ?.nominalCode
                                                    }
                                                    loading={nominalsSearchLoading}
                                                    fetchItems={searchTerm =>
                                                        reduxDispatch(
                                                            nominalsActions.search(searchTerm)
                                                        )
                                                    }
                                                    links={false}
                                                    title="Search Nominals"
                                                    clearSearch={() => {}}
                                                    placeholder="Search Nominals"
                                                    minimumSearchTermLength={3}
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
                                            <Grid item xs={9}>
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
                                                    maxLength={200}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <InputField
                                                    fullWidth
                                                    value={detail.drawingReference}
                                                    label="Drawing Ref"
                                                    propertyName="drawingReference"
                                                    onChange={(propertyName, newValue) =>
                                                        handleDetailFieldChange(
                                                            propertyName,
                                                            newValue,
                                                            detail
                                                        )
                                                    }
                                                    maxLength={100}
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
                                        </Fragment>
                                    ))}
                                <Grid item xs={6}>
                                    <SaveBackCancelButtons
                                        saveDisabled={!canSave}
                                        saveClick={() => {
                                            clearErrors();
                                            reduxDispatch(
                                                purchaseOrderActions.setEditStatus('view')
                                            );

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
                                            if (!creating) {
                                                reduxDispatch(
                                                    purchaseOrderActions.setEditStatus('view')
                                                );

                                                dispatch(item);
                                            }
                                        }}
                                        backClick={() =>
                                            handleBackClick(previousPaths, history.goBack)
                                        }
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
