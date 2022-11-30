import React, { useEffect, useReducer, useState } from 'react';
import { useMediaQuery } from 'react-responsive';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { useLocation } from 'react-router-dom';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import { makeStyles } from '@mui/styles';
import {
    Page,
    collectionSelectorHelpers,
    Dropdown,
    Typeahead,
    InputField,
    itemSelectorHelpers,
    Loading,
    utilities
} from '@linn-it/linn-form-components-library';
import queryString from 'query-string';
import LinearProgress from '@mui/material/LinearProgress';
import suppliersActions from '../../actions/suppliersActions';
import partsActions from '../../actions/partsActions';
import partSuppliersActions from '../../actions/partSuppliersActions';
import history from '../../history';
import config from '../../config';
import suggestedPurchaseOrderValuesActions from '../../actions/suggestedPurchaseOrderValuesActions';
import reducer from './purchaseOrderReducer';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import purchaseOrderActions from '../../actions/purchaseOrderActions';

function CreatePurchaseOrderUt() {
    const reduxDispatch = useDispatch();

    const item = useSelector(reduxState =>
        itemSelectorHelpers.getApplicationState(reduxState.purchaseOrder)
    );
    const loading = useSelector(state =>
        itemSelectorHelpers.getApplicationStateLoading(state.purchaseOrderApplicationState)
    );

    const [order, dispatch] = useReducer(reducer, { details: [{}] });

    const { search } = useLocation();
    const { supplierId, supplierName, partNumber, qty, currencyUnitPrice, dateRequired } =
        queryString.parse(search);

    const [stockControlledPart, setStockControlled] = useState(null);

    useEffect(() => {
        reduxDispatch(purchaseOrderActions.fetchState());
    }, [reduxDispatch]);

    useEffect(() => {
        if (item) {
            const initialOrder = {
                ...item,
                documentType: { name: 'PO' },
                exchangeRate: 1,
                supplier: { id: supplierId, name: supplierName },
                details: [
                    {
                        line: 1,
                        partNumber,
                        ourQty: qty ?? 0,
                        orderQty: qty ?? 0,
                        ourUnitPriceCurrency: currencyUnitPrice ?? 0,
                        orderUnitPriceCurrency: currencyUnitPrice ?? 0,
                        vatTotalCurrency: 0,
                        baseNetTotal: 0,
                        purchaseDeliveries: [{ dateRequested: dateRequired }]
                    }
                ]
            };
            dispatch({ type: 'initialise', payload: initialOrder });
        }
    }, [
        item,
        partNumber,
        qty,
        dispatch,
        supplierId,
        supplierName,
        currencyUnitPrice,
        dateRequired
    ]);

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'id', 'name')
    );
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );
    const searchSuppliers = searchTerm => reduxDispatch(suppliersActions.search(searchTerm));

    const previousOrderResults = useSelector(state => state.purchaseOrders.searchItems);
    const previousOrderLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.purchaseOrders)
    );

    const allowedToCreate = () => item?.links?.some(l => l.rel === 'create');

    const inputIsValid = () =>
        order.supplier?.id && order.details[0].partNumber && order.details[0].ourQty;

    const canSave = () => allowedToCreate() && inputIsValid();

    const handleDetailFieldChange = (propertyName, newValue, detail) => {
        dispatch({ payload: { ...detail, [propertyName]: newValue }, type: 'detailFieldChange' });
    };

    const handleDetailQtyFieldChange = (propertyName, newValue, detail) => {
        if (newValue && newValue > 0 && newValue !== order[propertyName]) {
            dispatch({
                payload: {
                    ...detail,
                    [propertyName]: newValue
                },
                type: 'detailCalculationFieldChange'
            });
        }
    };

    const handleSupplierChange = newSupplier => {
        dispatch({
            payload: { id: newSupplier.id, name: newSupplier.description },
            type: 'supplierChange'
        });
    };

    const handleOrderTypeChange = (_, newOrderType) => {
        dispatch({
            payload: newOrderType,
            type: 'orderTypeChange'
        });
    };

    const partsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.parts)
    ).map?.(c => ({
        id: c.partNumber,
        name: c.partNumber,
        partNumber: c.partNumber,
        description: c.description,
        stockControlled: c.stockControlled
    }));

    const partSuppliersSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.partSuppliers)
    );

    const partsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.parts)
    );

    const useStyles = makeStyles(theme => ({
        buttonMarginTop: {
            marginTop: '28px',
            height: '40px'
        },
        buttonMarginLineUp: {
            marginTop: '28px'
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

    const detail = order ? order.details[0] : {};

    useEffect(() => {
        if (previousOrderResults && previousOrderResults[0]) {
            const prevOrder = previousOrderResults[0];
            dispatch({
                payload: { id: prevOrder.supplier.id, name: prevOrder.supplier.name },
                type: 'supplierChange'
            });
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'partNumber',
                    value: prevOrder.details[0].partNumber
                },
                type: 'detailFieldUpdate'
            });
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'ourQty',
                    value: prevOrder.details[0].ourQty
                },
                type: 'detailFieldUpdate'
            });
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'ourUnitPriceCurrency',
                    value: prevOrder.details[0].ourUnitPriceCurrency
                },
                type: 'detailFieldUpdate'
            });
        } else {
            dispatch({
                payload: { id: null, name: null },
                type: 'supplierChange'
            });
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'partNumber',
                    value: null
                },
                type: 'detailFieldUpdate'
            });
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'ourQty',
                    value: 0
                },
                type: 'detailFieldUpdate'
            });
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'ourUnitPriceCurrency',
                    value: 0
                },
                type: 'detailFieldUpdate'
            });
        }
    }, [previousOrderResults]);

    useEffect(() => {
        if (partSuppliersSearchResults?.length) {
            handleSupplierChange({
                id: `${partSuppliersSearchResults.find(s => s.supplierRanking === 1).supplierId}`
            });
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'ourUnitPriceCurrency',
                    value: partSuppliersSearchResults.find(s => s.supplierRanking === 1)
                        .currencyUnitPrice
                },
                type: 'detailFieldUpdate'
            });
        }
    }, [partSuppliersSearchResults]);

    const progressToFullCreate = () => {
        reduxDispatch(suggestedPurchaseOrderValuesActions.add(order));

        history.push(utilities.getHref(item, 'create'));
    };

    const orderTypes = [
        { id: 'PO', displayText: 'Purchase Order' },
        { id: 'CO', displayText: 'Credit Order' },
        { id: 'RO', displayText: 'Returns Order' }
    ];

    const isCreditOrReturn = () =>
        order?.documentType?.name === 'CO' || order?.documentType?.name === 'RO';
    const lookUpOriginalOrder = () => {
        reduxDispatch(purchaseOrdersActions.clearSearch());
        reduxDispatch(
            purchaseOrdersActions.searchWithOptions(
                '',
                `&numberToTake=1&searchTerm=${detail.originalOrderNumber}`
            )
        );
    };

    return (
        <>
            <Page history={history} homeUrl={config.appRoot} width={screenIsSmall ? 'l' : 's'}>
                {loading ? (
                    <Loading />
                ) : (
                    <Grid container spacing={1} justifyContent="center">
                        <Grid item xs={12}>
                            <Typography variant="h6">Create Purchase Order Wizard</Typography>
                        </Grid>
                        <Grid item xs={12}>
                            <Dropdown
                                items={orderTypes}
                                value={order.documentType?.name}
                                allowNoValue={false}
                                propertyName="documentType"
                                label="Order Type"
                                onChange={handleOrderTypeChange}
                            />
                        </Grid>
                        {isCreditOrReturn() && (
                            <>
                                <Grid item xs={3}>
                                    <InputField
                                        fullWidth
                                        value={detail.originalOrderNumber}
                                        label="Original Order"
                                        propertyName="originalOrderNumber"
                                        onChange={(propertyName, newValue) =>
                                            handleDetailQtyFieldChange(
                                                propertyName,
                                                newValue,
                                                detail
                                            )
                                        }
                                        disabled={!allowedToCreate()}
                                        type="number"
                                        required
                                    />
                                </Grid>
                                <Grid item xs={2}>
                                    <InputField
                                        fullWidth
                                        value={detail.originalOrderLine}
                                        label="Original Line"
                                        propertyName="originalOrderLine"
                                        onChange={(propertyName, newValue) =>
                                            handleDetailQtyFieldChange(
                                                propertyName,
                                                newValue,
                                                detail
                                            )
                                        }
                                        disabled={!allowedToCreate()}
                                        type="number"
                                        required
                                    />
                                </Grid>
                                <Grid item xs={2}>
                                    <Button
                                        className={classes.buttonMarginLineUp}
                                        color="primary"
                                        variant="contained"
                                        onClick={lookUpOriginalOrder}
                                        disabled={
                                            !detail.originalOrderNumber || !detail.originalOrderLine
                                        }
                                    >
                                        Look Up
                                    </Button>
                                </Grid>
                                <Grid item xs={5}>
                                    {previousOrderLoading && (
                                        <LinearProgress
                                            style={{ marginTop: '20px', marginBottom: '20px' }}
                                        />
                                    )}
                                </Grid>
                            </>
                        )}
                        <Grid item xs={11}>
                            <Typeahead
                                label="Part"
                                title="Search for a part"
                                onSelect={newPart => {
                                    handleDetailFieldChange(
                                        'partNumber',
                                        newPart.id,
                                        order.details[0]
                                    );
                                    if (newPart.stockControlled === 'Y') {
                                        setStockControlled(true);
                                        reduxDispatch(
                                            partSuppliersActions.searchWithOptions(
                                                '',
                                                `&partNumber=${newPart.partNumber}`
                                            )
                                        );
                                    } else {
                                        setStockControlled(false);
                                    }
                                }}
                                items={partsSearchResults}
                                loading={partsSearchLoading}
                                fetchItems={searchTerm =>
                                    reduxDispatch(partsActions.search(searchTerm))
                                }
                                clearSearch={() => reduxDispatch(partsActions.clearSearch)}
                                value={detail.partNumber}
                                modal
                                links={false}
                                debounce={1000}
                                minimumSearchTermLength={4}
                                disabled={!allowedToCreate() || isCreditOrReturn()}
                                placeholder="click to set part"
                            />
                        </Grid>
                        <Grid item xs={1}>
                            <div className={classes.centeredIcon}>
                                {allowedToCreate() ? (
                                    <Tooltip title={`You can ${'create'} purchase orders`}>
                                        <ModeEditIcon fontSize="large" color="primary" />
                                    </Tooltip>
                                ) : (
                                    <Tooltip title="You cannot create orders">
                                        <EditOffIcon color="secondary" />
                                    </Tooltip>
                                )}
                            </div>
                        </Grid>

                        {stockControlledPart ? (
                            <Grid item xs={4}>
                                <Dropdown
                                    value={order.supplier?.id}
                                    label="Supplier"
                                    propertyName="supplierId"
                                    disabled={!allowedToCreate()}
                                    items={partSuppliersSearchResults
                                        .filter(a => a.partNumber === order.details[0].partNumber)
                                        .sort(
                                            (a, b) =>
                                                (a.supplierRanking ?? 100) -
                                                (b.supplierRanking ?? 100)
                                        )
                                        .map(s => ({
                                            id: s.supplierId,
                                            displayText: `${s.supplierName}  ${
                                                s.supplierRanking === 1 ? '(P)' : ''
                                            }`
                                        }))}
                                    onChange={(propertyName, selected) => {
                                        handleSupplierChange({ id: selected });
                                        dispatch({
                                            payload: {
                                                lineNumber: 1,
                                                fieldName: 'ourUnitPriceCurrency',
                                                value: partSuppliersSearchResults.find(
                                                    x => x.supplierId === Number(selected)
                                                ).currencyUnitPrice
                                            },
                                            type: 'detailFieldUpdate'
                                        });
                                    }}
                                    allowNoValue={false}
                                />
                            </Grid>
                        ) : (
                            <Grid item xs={4}>
                                <Typeahead
                                    label="Supplier"
                                    modal
                                    propertyName="supplierId"
                                    items={suppliersSearchResults}
                                    value={order.supplier?.id}
                                    loading={suppliersSearchLoading}
                                    fetchItems={searchSuppliers}
                                    links={false}
                                    on
                                    text
                                    clearSearch={() => {}}
                                    placeholder="Search Suppliers"
                                    onSelect={newValue => {
                                        handleSupplierChange(newValue);
                                    }}
                                    minimumSearchTermLength={3}
                                    fullWidth
                                    disabled={
                                        !allowedToCreate() ||
                                        isCreditOrReturn() ||
                                        !order.details[0].partNumber
                                    }
                                    required
                                />
                            </Grid>
                        )}

                        {stockControlledPart ? (
                            <Grid item xs={8} />
                        ) : (
                            <Grid item xs={8}>
                                <InputField
                                    fullWidth
                                    value={order.supplier?.name}
                                    label="Supplier Name"
                                    number
                                    propertyName="supplierName"
                                    disabled
                                />
                            </Grid>
                        )}

                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={detail.ourQty}
                                label="Quantity"
                                propertyName="ourQty"
                                onChange={(propertyName, newValue) =>
                                    handleDetailQtyFieldChange(propertyName, newValue, detail)
                                }
                                disabled={!allowedToCreate()}
                                type="number"
                                required
                            />
                        </Grid>
                        <Grid item xs={6} />

                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={detail.ourUnitPriceCurrency}
                                label="Our price (currency)"
                                propertyName="ourUnitPriceCurrency"
                                onChange={(propertyName, newValue) =>
                                    dispatch({
                                        payload: {
                                            lineNumber: 1,
                                            fieldName: 'ourUnitPriceCurrency',
                                            value: newValue
                                        },
                                        type: 'detailFieldUpdate'
                                    })
                                }
                                disabled={!allowedToCreate()}
                                type="number"
                                required
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <></>
                        </Grid>
                        <Grid item xs={6}>
                            <Button
                                className={classes.buttonMarginTop}
                                color="primary"
                                variant="contained"
                                disabled={!canSave()}
                                onClick={progressToFullCreate}
                            >
                                Next
                            </Button>
                        </Grid>
                    </Grid>
                )}
            </Page>
        </>
    );
}

export default CreatePurchaseOrderUt;
