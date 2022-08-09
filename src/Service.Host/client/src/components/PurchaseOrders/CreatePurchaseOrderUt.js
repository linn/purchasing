import React, { useEffect, useState, useReducer, useMemo } from 'react';
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
    Typeahead,
    InputField,
    SnackbarMessage,
    itemSelectorHelpers,
    Loading,
    TypeaheadTable,
    getItemError,
    ErrorCard,
    utilities
} from '@linn-it/linn-form-components-library';
import queryString from 'query-string';
import moment from 'moment';
import currenciesActions from '../../actions/currenciesActions';
import nominalsActions from '../../actions/nominalsActions';
import suppliersActions from '../../actions/suppliersActions';
import partsActions from '../../actions/partsActions';
import history from '../../history';
import config from '../../config';
import purchaseOrderActions from '../../actions/purchaseOrderActions';
import reducer from './purchaseOrderReducer';
import { exchangeRates } from '../../itemTypes';
import exchangeRatesActions from '../../actions/exchangeRatesActions';
import currencyConvert from '../../helpers/currencyConvert';

function CreatePurchaseOrderUt() {
    const reduxDispatch = useDispatch();

    useEffect(() => {
        reduxDispatch(purchaseOrderActions.clearErrorsForItem());
        reduxDispatch(purchaseOrderActions.fetchState());
        reduxDispatch(currenciesActions.fetch());
    }, [reduxDispatch]);

    const item = useSelector(reduxState =>
        itemSelectorHelpers.getApplicationState(reduxState.purchaseOrder)
    );
    const loading = useSelector(state =>
        itemSelectorHelpers.getApplicationStateLoading(state.purchaseOrderApplicationState)
    );

    const itemError = useSelector(state => getItemError(state, 'purchaseOrder'));

    const [order, dispatch] = useReducer(reducer, { details: [{}] });

    const { search } = useLocation();
    const { supplierId, supplierName, partNumber, qty } = queryString.parse(search);

    useEffect(() => {
        if (item) {
            const initialOrder = {
                ...item,
                exchangeRate: 1,
                dateRequired: new Date(),
                supplier: { id: supplierId, name: supplierName },
                details: [
                    {
                        line: 1,
                        partNumber,
                        ourQty: qty,
                        orderQty: qty,
                        ourUnitPriceCurrency: 0,
                        orderUnitPriceCurrency: 0,
                        vatTotalCurrency: 0,
                        baseNetTotal: 0
                    }
                ]
            };
            dispatch({ type: 'initialise', payload: initialOrder });
        }
    }, [item, partNumber, qty, reduxDispatch, supplierId, supplierName]);

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'id', 'name')
    );
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );
    const searchSuppliers = searchTerm => reduxDispatch(suppliersActions.search(searchTerm));

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

    const allowedToCreate = () => item?.links?.some(l => l.rel === 'create');

    const inputIsInvalid = () =>
        !order.supplier &&
        !order.partNumber &&
        order.department &&
        order.nominal &&
        order.details[0].ourQty &&
        order.details[0].ourUnitPriceCurrency;

    const canSave = () => editStatus !== 'view' && allowedToCreate() && !inputIsInvalid();

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        dispatch({ payload: newValue, propertyName, type: 'orderFieldChange' });
    };

    const handleDetailFieldChange = (propertyName, newValue, detail) => {
        setEditStatus('edit');

        dispatch({ payload: { ...detail, [propertyName]: newValue }, type: 'detailFieldChange' });
    };

    const dateToDdMmmYyyy = date => (date ? moment(date).format('DD-MMM-YYYY') : '-');

    useEffect(() => {
        if (order?.dateCreated) {
            reduxDispatch(exchangeRatesActions.search(dateToDdMmmYyyy(order?.dateCreated)));
        }
    }, [order, reduxDispatch]);

    const exchangeRatesItems = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state[exchangeRates.item])
    );

    const currentExchangeRate = useMemo(() => {
        if (exchangeRatesItems.length) {
            if (order?.currency?.code) {
                return exchangeRatesItems.find(
                    x => x.exchangeCurrency === order.currency.code && x.baseCurrency === 'GBP'
                )?.exchangeRate;
            }
        }
        return '';
    }, [order?.currency?.code, exchangeRatesItems]);

    useEffect(() => {
        if (currentExchangeRate && order?.exchangeRate !== currentExchangeRate) {
            handleFieldChange('exchangeRate', currentExchangeRate);
        }
    }, [order.exchangeRate, currentExchangeRate]);

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

    useEffect(() => {
        if (currentExchangeRate && order?.exchangeRate !== currentExchangeRate) {
            handleFieldChange('exchangeRate', currentExchangeRate);
        }
    }, [order.exchangeRate, currentExchangeRate]);

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

    const handleSupplierChange = newSupplier => {
        setEditStatus('edit');
        dispatch({
            payload: { id: newSupplier.id, name: newSupplier.name },
            type: 'supplierChange'
        });
    };

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

    const detail = order ? order.details[0] : {};

    const progressToFullCreate = () => {
        //post to get supplier info fill out address etc, then reduxDispatch the action
        // reduxDispatch(purchaseOrderActions. thingy ());

        history.push(utilities.getHref(order, 'create'));
    };

    return (
        <>
            <Page history={history} homeUrl={config.appRoot} width={screenIsSmall ? 'l' : 's'}>
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
                        {itemError && (
                            <Grid item xs={12}>
                                <ErrorCard
                                    errorMessage={itemError?.details ?? itemError.statusText}
                                />
                            </Grid>
                        )}
                        <Grid item xs={12}>
                            <Typography variant="h6">Purchase Order Quick Create Ut </Typography>
                        </Grid>
                        <Grid item xs={4}>
                            <Typeahead
                                label="Part"
                                title="Search for a part"
                                onSelect={newPart => {
                                    handleDetailFieldChange(
                                        'partNumber',
                                        newPart.id,
                                        order.details[0]
                                    );
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
                                minimumSearchTermLength={2}
                                disabled={!allowedToCreate()}
                                placeholder="click to set part"
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <InputField
                                fullWidth
                                value={detail.ourQty}
                                label="Quantity"
                                propertyName="ourQty"
                                onChange={(propertyName, newValue) =>
                                    handleDetailQtyFieldChange(
                                        propertyName,
                                        newValue,
                                        order.details[0]
                                    )
                                }
                                disabled={!allowedToCreate()}
                                type="number"
                                required
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={detail.ourUnitPriceCurrency}
                                label="Our price (currency)"
                                propertyName="ourUnitPriceCurrency"
                                onChange={(propertyName, newValue) =>
                                    handleDetailValueFieldChange(
                                        propertyName,
                                        'baseUnitPrice',
                                        newValue,
                                        detail
                                    )
                                }
                                disabled={!allowedToCreate()}
                                type="number"
                                required
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

                        <Grid item xs={4}>
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
                                onSelect={newValue => {
                                    handleSupplierChange(newValue);
                                }}
                                minimumSearchTermLength={3}
                                fullWidth
                                disabled={!allowedToCreate()}
                                required
                            />
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={order.supplier?.name}
                                label="Supplier Name"
                                number
                                propertyName="supplierContact"
                                disabled
                            />
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
                                clearSearch={() => reduxDispatch(nominalsActions.clearSearch)}
                                loading={nominalsSearchLoading}
                                label="Department"
                                title="Search Department"
                                value={
                                    detail.orderPosting?.nominalAccount?.department?.departmentCode
                                }
                                onSelect={newValue => handleNominalUpdate(newValue, detail.line)}
                                debounce={1000}
                                minimumSearchTermLength={2}
                                disabled={!allowedToCreate()}
                                required
                            />
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={detail.orderPosting?.nominalAccount?.department?.description}
                                label="Dept Description"
                                disabled
                                propertyName="nominalDescription"
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={detail.orderPosting?.nominalAccount?.nominal?.nominalCode}
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
                                value={detail.orderPosting?.nominalAccount?.nominal?.description}
                                label="Nominal Description"
                                propertyName="nominalDescription"
                                onChange={() => {}}
                                disabled
                            />
                        </Grid>

                        <Grid item xs={6}>
                            <Button
                                className={classes.buttonMarginTop}
                                color="primary"
                                variant="contained"
                                disabled={!canSave()}
                                onClick={() => progressToFullCreate()}
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
