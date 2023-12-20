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
    InputField,
    itemSelectorHelpers,
    Loading,
    utilities,
    Search
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

    const [supplierSearchTerm, setSupplierSearchTerm] = useState();

    const [partSearchTerm, setPartSearchTerm] = useState();

    useEffect(() => {
        reduxDispatch(purchaseOrderActions.fetchState());
    }, [reduxDispatch]);

    useEffect(() => {
        if (item) {
            if (partNumber) {
                setPartSearchTerm(partNumber);
            }

            if (supplierId) {
                setSupplierSearchTerm(supplierId);
            }

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
    const suppliersStoreItem = useSelector(state => state.suppliers);
    const suppliersSearchResults = collectionSelectorHelpers.getSearchItems(
        suppliersStoreItem,
        100,
        'id',
        'id',
        'name'
    );
    const suppliersSearchLoading = collectionSelectorHelpers.getSearchLoading(suppliersStoreItem);

    const searchSuppliers = searchTerm => reduxDispatch(suppliersActions.search(searchTerm));

    const purchaseOrderStoreItem = useSelector(state => state.purchaseOrder);
    const previousOrderResult = purchaseOrderStoreItem.item;
    const previousOrderLoading = collectionSelectorHelpers.getLoading(purchaseOrderStoreItem);

    const allowedToCreate = () => item?.links?.some(l => l.rel === 'create');

    const inputIsValid = () =>
        order.supplier?.id && order.details[0].partNumber && order.details[0].ourQty;

    const canSave = () => allowedToCreate() && inputIsValid();

    const handleDetailFieldChange = (propertyName, newValue, detail) => {
        dispatch({ payload: { ...detail, [propertyName]: newValue }, type: 'detailFieldChange' });
    };

    const handleDetailQtyFieldChange = (propertyName, newValue, detail) => {
        if (newValue >= 0 && newValue !== order[propertyName]) {
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
    const partsStoreItem = useSelector(state => state.parts);
    const partsSearchResults = collectionSelectorHelpers
        .getSearchItems(partsStoreItem)
        .map?.(c => ({
            id: c.partNumber,
            name: c.partNumber,
            partNumber: c.partNumber,
            description: c.description,
            stockControlled: c.stockControlled
        }));
    const partsSearchLoading = collectionSelectorHelpers.getSearchLoading(partsStoreItem);

    const partSuppliersStoreItem = useSelector(state => state.partSuppliers);
    const partSuppliersSearchResults =
        collectionSelectorHelpers.getSearchItems(partSuppliersStoreItem);

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
        if (previousOrderResult) {
            dispatch({
                payload: {
                    id: previousOrderResult.supplier.id,
                    name: previousOrderResult.supplier.name
                },
                type: 'supplierChange'
            });
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'partNumber',
                    value: previousOrderResult.details[0].partNumber
                },
                type: 'detailFieldUpdate'
            });
            setPartSearchTerm(previousOrderResult.details[0].partNumber);
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'ourQty',
                    value: previousOrderResult.details[0].ourQty
                },
                type: 'detailFieldUpdate'
            });
            dispatch({
                payload: {
                    lineNumber: 1,
                    fieldName: 'ourUnitPriceCurrency',
                    value: previousOrderResult.details[0].ourUnitPriceCurrency
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
            setPartSearchTerm(null);
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
    }, [previousOrderResult]);

    useEffect(() => {
        const partSuppliersWithCorrectPart = partSuppliersSearchResults?.filter(
            a => a.partNumber === detail?.partNumber
        );
        if (partSuppliersWithCorrectPart?.length) {
            const preferredSupplier = partSuppliersWithCorrectPart.find(
                s => s.supplierRanking === 1
            );
            if (preferredSupplier) {
                handleSupplierChange({
                    id: `${preferredSupplier.supplierId}`
                });
                setStockControlled(true);

                dispatch({
                    payload: {
                        lineNumber: 1,
                        fieldName: 'ourUnitPriceCurrency',
                        value: preferredSupplier.currencyUnitPrice
                    },
                    type: 'detailFieldUpdate'
                });
            } else {
                setStockControlled(false);
            }
        }
    }, [detail?.partNumber, partSuppliersSearchResults]);

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
        reduxDispatch(purchaseOrderActions.fetch(detail.originalOrderNumber));
    };

    const setPartWithoutSearch = () => {
        handleDetailFieldChange('partNumber', partSearchTerm?.toUpperCase(), order.details[0]);
        setStockControlled(false);
        reduxDispatch(
            partSuppliersActions.searchWithOptions(
                '',
                `&partNumber=${partSearchTerm?.toUpperCase()}`
            )
        );
    };

    const setSupplierWithoutSearch = () => {
        handleSupplierChange({ id: supplierSearchTerm, description: order.supplier?.name });
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
                            <Search
                                propertyName="searchTerm"
                                label="Part Number"
                                resultsInModal
                                resultLimit={100}
                                value={partSearchTerm}
                                handleValueChange={(_, newVal) => setPartSearchTerm(newVal)}
                                search={() => reduxDispatch(partsActions.search(partSearchTerm))}
                                searchResults={partsSearchResults}
                                loading={partsSearchLoading}
                                helperText="Press ENTER to search or TAB to proceed"
                                priorityFunction="closestMatchesFirst"
                                onKeyPressFunctions={[{ keyCode: 9, action: setPartWithoutSearch }]}
                                onResultSelect={newPart => {
                                    handleDetailFieldChange(
                                        'partNumber',
                                        newPart.id,
                                        order.details[0]
                                    );
                                    setPartSearchTerm(newPart.partNumber);
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
                                clearSearch={() => {}}
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
                                    onChange={(_, selected) => {
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
                                <Search
                                    value={supplierSearchTerm}
                                    handleValueChange={(_, newVal) => setSupplierSearchTerm(newVal)}
                                    search={searchSuppliers}
                                    searchResults={suppliersSearchResults.filter(
                                        s => !s.dateClosed
                                    )}
                                    label="Supplier"
                                    resultsInModal
                                    propertyName="supplierId"
                                    items={suppliersSearchResults.filter(s => !s.dateClosed)}
                                    loading={suppliersSearchLoading}
                                    clearSearch={() => {}}
                                    placeholder="Search Suppliers"
                                    onResultSelect={newValue => {
                                        handleSupplierChange(newValue);
                                        setSupplierSearchTerm(newValue.name);
                                    }}
                                    minimumSearchTermLength={3}
                                    onKeyPressFunctions={[
                                        { keyCode: 9, action: setSupplierWithoutSearch }
                                    ]}
                                    fullWidth
                                    disabled={
                                        !allowedToCreate() ||
                                        isCreditOrReturn() ||
                                        !order.details[0].partNumber
                                    }
                                    required
                                    helperText="Press ENTER to search or TAB to proceed"
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
                                disabled={!allowedToCreate() || isCreditOrReturn()}
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
