import React, { useEffect, useState, useReducer, useCallback } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { makeStyles } from '@mui/styles';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import Tabs from '@mui/material/Tabs';
import { useLocation } from 'react-router';
import queryString from 'query-string';
import Tab from '@mui/material/Tab';
import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import Close from '@mui/icons-material/Close';
import PropTypes from 'prop-types';
import Dialog from '@mui/material/Dialog';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    Loading,
    SaveBackCancelButtons,
    SnackbarMessage,
    utilities,
    ErrorCard,
    collectionSelectorHelpers,
    itemSelectorHelpers,
    getItemError,
    userSelectors,
    LinkButton
} from '@linn-it/linn-form-components-library';
import partSupplierActions from '../../actions/partSupplierActions';
import history from '../../history';
import config from '../../config';
import partSupplierReducer from './partSupplierReducer';
import { partSupplier, partPriceConversions } from '../../itemTypes';
import PartSupplierTab from './tabs/PartSupplierTab';
import partsActions from '../../actions/partsActions';
import partActions from '../../actions/partActions';
import partSuppliersActions from '../../actions/partSuppliersActions';
import deliveryAddressesActions from '../../actions/deliveryAddressesActions';
import unitsOfMeasureActions from '../../actions/unitsOfMeasureActions';
import orderMethodsactions from '../../actions/orderMethodActions';
import suppliersActions from '../../actions/suppliersActions';
import currenciesActions from '../../actions/currenciesActions';
import OrderDetailsTab from './tabs/OrderDetailsTab';
import OtherDetailsTab from './tabs/OtherDetailsTab';
import packagingGroupActions from '../../actions/packagingGroupActions';
import LifecycleTab from './tabs/LifecycleTab';
import employeesActions from '../../actions/employeesActions';
import ManufacturerTab from './tabs/ManufacturerTab';
import manufacturersActions from '../../actions/manufacturersActions';
import PreferredSupplier from './PreferredSupplier';
import PriceChange from './PriceChange';
import partPriceConversionsActions from '../../actions/partPriceConversionsActions';

function PartSupplier({ creating }) {
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
    useEffect(
        () => () => {
            reduxDispatch(partPriceConversionsActions.clearItem());
        },
        [reduxDispatch]
    );
    const [errorMessage, setErrorMessage] = useState(null);

    const searchParts = searchTerm => reduxDispatch(partsActions.search(searchTerm));
    const partsSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(
            reduxState.parts,
            100,
            'id',
            'partNumber',
            'description'
        )
    );
    const partsSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.parts)
    );

    const searchSuppliers = searchTerm => reduxDispatch(suppliersActions.search(searchTerm));
    const suppliersSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.suppliers, 100, 'id', 'id', 'name')
    );
    const suppliersSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.suppliers)
    );

    const searchManufacturers = searchTerm =>
        reduxDispatch(manufacturersActions.search(searchTerm));
    const manufacturersSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(
            reduxState.manufacturers,
            100,
            'code',
            'code',
            'name'
        )
    );
    const manufacturersSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.manufacturers)
    );

    const currentUserNumber = useSelector(reduxState => userSelectors.getUserNumber(reduxState));
    const unitsOfMeasure = useSelector(reduxState =>
        collectionSelectorHelpers.getItems(reduxState.unitsOfMeasure)
    );
    const deliveryAddresses = useSelector(reduxState =>
        collectionSelectorHelpers.getItems(reduxState.deliveryAddresses)
    );
    const orderMethods = useSelector(reduxState =>
        collectionSelectorHelpers.getItems(reduxState.orderMethods)
    );
    const currencies = useSelector(reduxState =>
        collectionSelectorHelpers.getItems(reduxState.currencies)
    );

    const employees = useSelector(reduxState =>
        collectionSelectorHelpers.getItems(reduxState.employees)
    );

    const updatePartSupplier = body => reduxDispatch(partSupplierActions.update(null, body));
    const createPartSupplier = body => reduxDispatch(partSupplierActions.add(body));
    const clearErrors = () => reduxDispatch(partSupplierActions.clearErrorsForItem());

    const applicationState = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.partSuppliers)
    );

    const [state, dispatch] = useReducer(partSupplierReducer, {
        partSupplier: {},
        prevPart: {}
    });
    const { search } = useLocation();

    const query = queryString.parse(search);
    const loading = useSelector(reduxState => reduxState.partSupplier.loading);
    const snackbarVisible = useSelector(reduxState =>
        itemSelectorHelpers.getSnackbarVisible(reduxState.partSupplier)
    );
    const editStatus = useSelector(reduxState =>
        itemSelectorHelpers.getItemEditStatus(reduxState.partSupplier)
    );

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.partSupplier));

    const part = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.part));
    const partLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.part)
    );

    const itemError = useSelector(reduxState => getItemError(reduxState, 'partSupplier'));

    const setEditStatus = status => reduxDispatch(partSupplierActions.setEditStatus(status));

    const [value, setValue] = useState(0);
    const [preferredSupplierDialogOpen, setPreferredSupplierDialogOpen] = useState(false);
    const [priceChangeDialogOpen, setPriceChangeDialogOpen] = useState(false);

    const refreshPart = useCallback(
        () => reduxDispatch(partActions.fetch(query.partId)),
        [query.partId, reduxDispatch]
    );

    const fetchBasePriceConversion = (currency, currencyUnitPrice) => {
        reduxDispatch(
            partPriceConversionsActions.fetchByHref(
                `${partPriceConversions.uri}?partNumber=&newPrice=${currencyUnitPrice}&newCurrency=${currency}&ledger=PL&round=FALSE`
            )
        );
    };

    useEffect(() => {
        reduxDispatch(partSuppliersActions.fetchState());
        reduxDispatch(unitsOfMeasureActions.fetch());
        reduxDispatch(deliveryAddressesActions.fetch());
        reduxDispatch(orderMethodsactions.fetch());
        reduxDispatch(currenciesActions.fetch());
        reduxDispatch(packagingGroupActions.fetch());
        reduxDispatch(employeesActions.fetch());
    }, [reduxDispatch]);

    useEffect(() => {
        if (query.partId && query.supplierId) {
            reduxDispatch(
                partSupplierActions.fetchByHref(
                    `${partSupplier.uri}?partId=${query.partId}&supplierId=${query.supplierId}`
                )
            );
            reduxDispatch(partActions.fetch(query.partId));
        }
        if (query.tab) {
            const tabs = {
                partAndSupplier: 0,
                orderDetails: 1,
                otherDetails: 2,
                lifecycle: 3,
                manufacturer: 4
            };
            setValue(tabs[query.tab]);
        }
    }, [query.partId, query.tab, query.supplierId, reduxDispatch]);

    useEffect(() => {
        if (creating) {
            setErrorMessage(null);
        }
    }, [creating, reduxDispatch]);

    useEffect(() => {
        if (creating && !state.partSupplier.dateCreated) {
            dispatch({
                type: 'initialise',
                payload: {
                    createdBy: Number(currentUserNumber),
                    dateCreated: new Date(),
                    currencyCode: 'GBP',
                    orderMethodName: 'MANUAL',
                    unitOfMeasure: 'ONES',
                    addressId: 405284
                }
            });
        } else if (item && !creating) {
            dispatch({ type: 'initialise', payload: item });
        }
    }, [item, creating, currentUserNumber, state.partSupplier.dateCreated]);

    useEffect(() => {
        if (
            creating &&
            state.partSupplier.partNumber &&
            item?.partNumber === state.partSupplier.partNumber &&
            item?.supplierId === state.partSupplier.supplierId
        ) {
            setErrorMessage('Record for this part and supplier already exists');
        } else {
            setErrorMessage(null);
        }
    }, [
        item,
        creating,
        reduxDispatch,
        state.partSupplier.partNumber,
        state.partSupplier.supplierId
    ]);

    const handleFieldChange = (propertyName, newValue) => {
        let formatted = newValue;
        if (['addressId', 'packagingGroupId', 'createdBy'].includes(propertyName)) {
            formatted = Number(newValue);
        }
        setEditStatus('edit');
        if (propertyName === 'orderMethodName') {
            dispatch({
                type: 'fieldChange',
                fieldName: 'orderMethodDescription',
                payload: orderMethods.find(x => x.name === formatted).description
            });
        }
        if (propertyName === 'addressId') {
            dispatch({
                type: 'fieldChange',
                fieldName: 'fullAddress',
                payload: deliveryAddresses.find(x => x.addressId === formatted).address
            });
            dispatch({ type: 'fieldChange', fieldName: propertyName, payload: formatted });
            return;
        }
        dispatch({ type: 'fieldChange', fieldName: propertyName, payload: formatted });
    };

    const canEdit = () =>
        item?.links.some(l => l.rel === 'edit') || !!utilities.getHref(applicationState, 'create');

    const invalid = () =>
        !state.partSupplier?.partNumber ||
        !state.partSupplier?.supplierId ||
        !state.partSupplier?.orderMethodName ||
        !state.partSupplier?.currencyCode ||
        !state.partSupplier?.minimumOrderQty ||
        !state.partSupplier?.orderIncrement ||
        !state.partSupplier?.leadTimeWeeks;

    return (
        <Page history={history} homeUrl={config.appRoot} title="Part Suppliers">
            <Grid container spacing={3}>
                {!creating && (
                    <Dialog open={preferredSupplierDialogOpen} fullWidth maxWidth="md">
                        <div>
                            <IconButton
                                className={classes.pullRight}
                                aria-label="Close"
                                onClick={() => setPreferredSupplierDialogOpen(false)}
                            >
                                <Close />
                            </IconButton>
                            <div className={classes.dialog}>
                                <PreferredSupplier
                                    currentSupplier={state.partSupplier?.supplierId}
                                    partLoading={partLoading}
                                    partNumber={part?.partNumber}
                                    partDescription={part?.description}
                                    baseOldPrice={part?.baseUnitPrice}
                                    oldPrice={part?.currencyUnitPrice}
                                    oldCurrencyCode={part?.currency}
                                    oldSupplierId={part?.preferredSupplier}
                                    oldSupplierName={part?.preferredSupplierName}
                                    close={() => setPreferredSupplierDialogOpen(false)}
                                    refreshPart={refreshPart}
                                    safetyCriticalPart={part?.safetyCriticalPart === 'Y'}
                                />
                            </div>
                        </div>
                    </Dialog>
                )}
                <Dialog open={priceChangeDialogOpen} fullWidth maxWidth="md">
                    <div>
                        <IconButton
                            className={classes.pullRight}
                            aria-label="Close"
                            onClick={() => setPriceChangeDialogOpen(false)}
                        >
                            <Close />
                        </IconButton>
                        <div className={classes.dialog}>
                            <PriceChange
                                partNumber={part?.partNumber}
                                partDescription={part?.description}
                                baseOldPrice={state.partSupplier?.baseOurUnitPrice}
                                oldPrice={state.partSupplier?.currencyUnitPrice}
                                oldCurrencyCode={state.partSupplier?.currencyCode}
                                supplierId={state.partSupplier?.supplierId}
                                supplierName={state.partSupplier?.supplierName}
                                close={() => setPriceChangeDialogOpen(false)}
                                changePrices={newValues =>
                                    updatePartSupplier({
                                        ...state.partSupplier,
                                        currencyUnitPrice: newValues.newPrice,
                                        ourCurrencyPriceToShowOnOrder: newValues.newPrice,
                                        baseOurUnitPrice: newValues.baseNewPrice,
                                        currencyCode: newValues.newCurrency
                                    })
                                }
                            />
                        </div>
                    </div>
                </Dialog>
                <SnackbarMessage
                    visible={snackbarVisible}
                    onClose={() => reduxDispatch(partSupplierActions.setSnackbarVisible(false))}
                    message="Save Successful"
                />
                {itemError && (
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={itemError.details?.error || itemError.details} />
                    </Grid>
                )}
                {errorMessage && (
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={errorMessage.toString()} />
                    </Grid>
                )}
                <Grid container spacing={3}>
                    {loading ? (
                        <Grid item xs={12}>
                            <Loading />
                        </Grid>
                    ) : (
                        <>
                            <Grid item xs={3}>
                                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                    <Typography variant="h6">
                                        {state.partSupplier?.partNumber}
                                    </Typography>
                                </Box>
                            </Grid>
                            <Grid item xs={7}>
                                <Box
                                    sx={{
                                        borderBottom: 1,
                                        borderColor: 'divider'
                                    }}
                                >
                                    <Typography variant="h6">
                                        {state.partSupplier?.supplierName}
                                    </Typography>
                                </Box>
                            </Grid>
                            <Grid item xs={1} />
                            <Grid item xs={1}>
                                {canEdit() ? (
                                    <Tooltip title="You have write access to Part Suppliers">
                                        <ModeEditIcon fontSize="large" color="primary" />
                                    </Tooltip>
                                ) : (
                                    <Tooltip title="You do not have write access to Part Suppliers">
                                        <EditOffIcon fontSize="large" color="secondary" />
                                    </Tooltip>
                                )}
                            </Grid>
                            <Grid item xs={3}>
                                {!creating && (
                                    <LinkButton
                                        external
                                        newTab
                                        to={`${config.proxyRoot}${utilities.getHref(item, 'part')}`}
                                        text="View Part"
                                    />
                                )}
                            </Grid>
                            <Grid item xs={3}>
                                {!creating && (
                                    <LinkButton
                                        external
                                        newTab
                                        to={`${config.proxyRoot}${utilities.getHref(
                                            item,
                                            'supplier'
                                        )}`}
                                        text="View Supplier"
                                    />
                                )}
                            </Grid>
                            <Grid item xs={6} />
                            <Grid item xs={12}>
                                <Box sx={{ width: '100%' }}>
                                    <Box sx={{ borderBottom: 0, borderColor: 'divider' }}>
                                        <Tabs
                                            value={value}
                                            onChange={(event, newValue) => {
                                                setValue(newValue);
                                            }}
                                        >
                                            <Tab label="Part and Supplier" />
                                            <Tab label="Order Details" />
                                            <Tab label="Other Details" />
                                            <Tab label="Lifecycle" />
                                            <Tab label="Manufacturer" />
                                        </Tabs>
                                    </Box>

                                    {value === 0 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <PartSupplierTab
                                                setPreferredSupplierDialogOpen={
                                                    setPreferredSupplierDialogOpen
                                                }
                                                handleFieldChange={handleFieldChange}
                                                partNumber={state.partSupplier?.partNumber}
                                                partDescription={
                                                    state.partSupplier?.partDescription
                                                }
                                                supplierId={state.partSupplier?.supplierId}
                                                supplierName={state.partSupplier?.supplierName}
                                                designation={state.partSupplier?.designation}
                                                partsSearchResults={partsSearchResults}
                                                partsSearchLoading={partsSearchLoading}
                                                searchParts={searchParts}
                                                suppliersSearchResults={suppliersSearchResults}
                                                suppliersSearchLoading={suppliersSearchLoading}
                                                searchSuppliers={searchSuppliers}
                                                part={part}
                                                canEdit={canEdit}
                                                creating={creating}
                                            />
                                        </Box>
                                    )}
                                    {value === 1 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <OrderDetailsTab
                                                handleFieldChange={handleFieldChange}
                                                creating={creating}
                                                unitsOfMeasure={unitsOfMeasure}
                                                unitOfMeasure={state.partSupplier?.unitOfMeasure}
                                                deliveryAddresses={deliveryAddresses}
                                                deliveryAddress={state.partSupplier?.addressId}
                                                orderMethods={orderMethods}
                                                orderMethod={state.partSupplier?.orderMethodName}
                                                orderMethodDescription={
                                                    state.partSupplier?.orderMethodDescription
                                                }
                                                currencies={currencies}
                                                currencyCode={state.partSupplier?.currencyCode}
                                                currencyUnitPrice={
                                                    state.partSupplier?.currencyUnitPrice
                                                }
                                                ourCurrencyPriceToShowOnOrder={
                                                    state.partSupplier
                                                        ?.ourCurrencyPriceToShowOnOrder
                                                }
                                                baseOurUnitPrice={
                                                    state.partSupplier?.baseOurUnitPrice
                                                }
                                                minimumOrderQty={
                                                    state.partSupplier?.minimumOrderQty
                                                }
                                                minimumDeliveryQty={
                                                    state.partSupplier?.minimumDeliveryQty
                                                }
                                                orderIncrement={state.partSupplier?.orderIncrement}
                                                reelOrBoxQty={state.partSupplier?.reelOrBoxQty}
                                                setPriceChangeDialogOpen={setPriceChangeDialogOpen}
                                                fetchBasePriceConversion={fetchBasePriceConversion}
                                                canEdit={canEdit}
                                            />
                                        </Box>
                                    )}
                                    {value === 2 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <OtherDetailsTab
                                                handleFieldChange={handleFieldChange}
                                                leadTimeWeeks={state.partSupplier?.leadTimeWeeks}
                                                deliveryInstructions={
                                                    state.partSupplier?.deliveryInstructions
                                                }
                                                notesForBuyer={state.partSupplier?.notesForBuyer}
                                                tariffId={state.partSupplier?.tariffId}
                                                tariffCode={state.partSupplier?.tariffCode}
                                                dutyPercent={state.partSupplier?.dutyPercent}
                                                tariffDescription={
                                                    state.partSupplier?.tariffDescription
                                                }
                                                packWasteStatus={
                                                    state.partSupplier?.packWasteStatus
                                                }
                                            />
                                        </Box>
                                    )}
                                    {value === 3 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <LifecycleTab
                                                handleFieldChange={handleFieldChange}
                                                createdBy={state.partSupplier?.createdBy}
                                                employees={employees}
                                                dateCreated={
                                                    state.partSupplier?.dateCreated
                                                        ? new Date(state.partSupplier?.dateCreated)
                                                        : null
                                                }
                                                madeInvalidBy={state.partSupplier?.madeInvalidBy}
                                                dateInvalid={
                                                    state.partSupplier?.dateInvalid
                                                        ? new Date(state.partSupplier?.dateInvalid)
                                                        : null
                                                }
                                            />
                                        </Box>
                                    )}
                                    {value === 4 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <ManufacturerTab
                                                handleFieldChange={handleFieldChange}
                                                manufacturerPartNumber={
                                                    state.partSupplier?.manufacturerPartNumber
                                                }
                                                manufacturer={state.partSupplier?.manufacturerCode}
                                                manufacturerName={
                                                    state.partSupplier?.manufacturerName
                                                }
                                                manufacturersSearchResults={
                                                    manufacturersSearchResults
                                                }
                                                manufacturersSearchLoading={
                                                    manufacturersSearchLoading
                                                }
                                                searchManufacturers={searchManufacturers}
                                                vendorPartNumber={
                                                    state.partSupplier?.vendorPartNumber
                                                }
                                            />
                                        </Box>
                                    )}
                                </Box>
                            </Grid>
                        </>
                    )}
                    <Grid item xs={12}>
                        <SaveBackCancelButtons
                            saveDisabled={!canEdit() || invalid() || editStatus === 'view'}
                            saveClick={() => {
                                clearErrors();
                                setErrorMessage(null);
                                if (creating) {
                                    createPartSupplier(state.partSupplier);
                                } else {
                                    updatePartSupplier(state.partSupplier);
                                }
                            }}
                            cancelClick={() => {
                                dispatch({
                                    type: 'initialise',
                                    payload: !creating ? item : { dateCreated: new Date() }
                                });
                                setEditStatus('view');
                            }}
                            backClick={() => history.push('/purchasing/part-suppliers')}
                        />
                    </Grid>
                </Grid>
            </Grid>
        </Page>
    );
}

PartSupplier.propTypes = {
    creating: PropTypes.bool
};

PartSupplier.defaultProps = {
    creating: false
};

export default PartSupplier;
