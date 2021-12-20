import React, { useEffect, useState, useReducer } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Box from '@mui/material/Box';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    Loading,
    SaveBackCancelButtons,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import getQuery from '../../selectors/routerSelelctors';
import partSupplierActions from '../../actions/partSupplierActions';
import history from '../../history';
import config from '../../config';
import partSupplierReducer from './partSupplierReducer';
import { partSupplier } from '../../itemTypes';
import PartSupplierTab from './tabs/PartSupplierTab';
import partsActions from '../../actions/partsActions';
import {
    getSearchItems,
    getSearchLoading,
    getItems
} from '../../selectors/CollectionSelectorHelpers';
import { getSnackbarVisible, getItem, getEditStatus } from '../../selectors/ItemSelectorsHelpers';
import deliveryAddressesActions from '../../actions/deliveryAddressesActions';
import unitsOfMeasureActions from '../../actions/unitsOfMeasureActions';
import orderMethodsactions from '../../actions/orderMethodActions';
import suppliersActions from '../../actions/suppliersActions';
import currenciesActions from '../../actions/currenciesActions';
import OrderDetailsTab from './tabs/OrderDetailsTab';
import OtherDetailsTab from './tabs/OtherDetailsTab';
import tariffsActions from '../../actions/tariffsActions';
import packagingGroupActions from '../../actions/packagingGroupActions';
import LifecycleTab from './tabs/LifecycleTab';
import employeesActions from '../../actions/employeesActions';

function PartSupplier() {
    const reduxDispatch = useDispatch();

    const searchParts = searchTerm => reduxDispatch(partsActions.search(searchTerm));
    const partsSearchResults = useSelector(reduxState =>
        getSearchItems(reduxState.parts, 100, 'partNumber', 'partNumber', 'description')
    );
    const partsSearchLoading = useSelector(reduxState => getSearchLoading(reduxState.parts));

    const searchSuppliers = searchTerm => reduxDispatch(suppliersActions.search(searchTerm));
    const suppliersSearchResults = useSelector(reduxState =>
        getSearchItems(reduxState.suppliers, 100, 'id', 'name', 'name')
    );
    const suppliersSearchLoading = useSelector(reduxState =>
        getSearchLoading(reduxState.suppliers)
    );

    const searchTariffs = searchTerm => reduxDispatch(tariffsActions.search(searchTerm));
    const tariffsSearchResults = useSelector(reduxState =>
        getSearchItems(reduxState.tariffs, 100, 'id', 'code', 'description')
    );
    const tariffsSearchLoading = useSelector(reduxState => getSearchLoading(reduxState.tariffs));

    const unitsOfMeasure = useSelector(reduxState => getItems(reduxState.unitsOfMeasure));
    const deliveryAddresses = useSelector(reduxState => getItems(reduxState.deliveryAddresses));
    const orderMethods = useSelector(reduxState => getItems(reduxState.orderMethods));
    const currencies = useSelector(reduxState => getItems(reduxState.currencies));
    const packagingGroups = useSelector(reduxState => getItems(reduxState.packagingGroups));
    const employees = useSelector(reduxState => getItems(reduxState.employees));
    const updatePartSupplier = body => reduxDispatch(partSupplierActions.update(null, body));

    const creating = () => false;

    const [state, dispatch] = useReducer(partSupplierReducer, {
        partSupplier: creating() ? {} : {},
        prevPart: {}
    });

    const query = useSelector(reduxState => getQuery(reduxState));
    const loading = useSelector(reduxState => reduxState.partSupplier.loading);
    const snackbarVisible = useSelector(reduxState => getSnackbarVisible(reduxState.partSupplier));
    const editStatus = useSelector(reduxState => getEditStatus(reduxState.partSupplier));

    const item = useSelector(reduxState => getItem(reduxState.partSupplier));

    const setEditStatus = status => reduxDispatch(partSupplierActions.setEditStatus(status));

    const [value, setValue] = useState(0);

    useEffect(() => {
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
        }
        if (query.tab) {
            const tabs = {
                partAndSupplier: 0,
                orderDetails: 1,
                otherDetails: 2,
                lifecycle: 3,
                manufacturers: 4
            };
            setValue(tabs[query.tab]);
        }
    }, [query, reduxDispatch]);

    useEffect(() => {
        if (item) {
            dispatch({ type: 'initialise', payload: item });
        }
    }, [item]);

    const handleFieldChange = (propertyName, newValue) => {
        let formatted = newValue;
        if (['addressId', 'packagingGroupId'].includes(propertyName)) {
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

    const canEdit = () => item?.links.some(l => l.rel === 'edit' || l.rel === 'create');

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <SnackbarMessage
                visible={snackbarVisible}
                onClose={() => reduxDispatch(partSupplierActions.setSnackbarVisible(false))}
                message="Save Successful"
            />
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
                                    borderColor: 'divider',
                                    marginBottom: '20px'
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
                                            handleFieldChange={handleFieldChange}
                                            partNumber={state.partSupplier?.partNumber}
                                            partDescription={state.partSupplier?.partDescription}
                                            supplierId={state.partSupplier?.supplierId}
                                            supplierName={state.partSupplier?.supplierName}
                                            designation={state.partSupplier?.designation}
                                            partsSearchResults={partsSearchResults}
                                            partsSearchLoading={partsSearchLoading}
                                            searchParts={searchParts}
                                            suppliersSearchResults={suppliersSearchResults}
                                            suppliersSearchLoading={suppliersSearchLoading}
                                            searchSuppliers={searchSuppliers}
                                            editStatus={editStatus}
                                        />
                                    </Box>
                                )}
                                {value === 1 && (
                                    <Box sx={{ paddingTop: 3 }}>
                                        <OrderDetailsTab
                                            handleFieldChange={handleFieldChange}
                                            editStatus={editStatus}
                                            unitsOfMeasure={unitsOfMeasure}
                                            unitOfMeasure={state.partSupplier?.unitOfMeasure}
                                            deliveryAddresses={deliveryAddresses}
                                            deliveryAddress={state.partSupplier?.addressId}
                                            fullAddress={state.partSupplier?.fullAddress}
                                            orderMethods={orderMethods}
                                            orderMethod={state.partSupplier?.orderMethodName}
                                            orderMethodDescription={
                                                state.partSupplier?.orderMethodDescription
                                            }
                                            currencies={currencies}
                                            currency={state.partSupplier?.currencyCode}
                                            currencyUnitPrice={
                                                state.partSupplier?.currencyUnitPrice
                                            }
                                            ourCurrencyPriceToShowOnOrder={
                                                state.partSupplier?.ourCurrencyPriceToShowOnOrder
                                            }
                                            baseOurUnitPrice={state.partSupplier?.baseOurUnitPrice}
                                            minimumOrderQty={state.partSupplier?.minimumOrderQty}
                                            minimumDeliveryQty={
                                                state.partSupplier?.minimumDeliveryQty
                                            }
                                            orderIncrement={state.partSupplier?.orderIncrement}
                                            orderConversionFactor={
                                                state.partSupplier?.orderConversionFactor
                                            }
                                            reelOrBoxQty={state.partSupplier?.reelOrBoxQty}
                                        />
                                    </Box>
                                )}
                                {value === 2 && (
                                    <Box sx={{ paddingTop: 3 }}>
                                        <OtherDetailsTab
                                            handleFieldChange={handleFieldChange}
                                            leadTimeWeeks={state.partSupplier?.leadTimeWeeks}
                                            contractLeadTimeWeeks={
                                                state.partSupplier?.contractLeadTimeWeeks
                                            }
                                            overbookingAllowed={
                                                state.partSupplier?.overbookingAllowed
                                            }
                                            damagesPercent={state.partSupplier?.damagesPercent}
                                            webAddress={state.partSupplier?.webAddress}
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
                                            packWasteStatus={state.partSupplier?.packWasteStatus}
                                            packagingGroupId={state.partSupplier?.packagingGroupId}
                                            packagingGroupDescription={
                                                state.partSupplier?.packagingGroupDescription
                                            }
                                            tariffsSearchResults={tariffsSearchResults}
                                            tariffsSearchLoading={tariffsSearchLoading}
                                            searchTariffs={searchTariffs}
                                            packagingGroups={packagingGroups}
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
                                            madeInvalidB={state.partSupplier?.madeInvalidB}
                                            dateInvalid={
                                                state.partSupplier?.dateInvalid
                                                    ? new Date(state.partSupplier?.dateInvalid)
                                                    : null
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
                        saveDisabled={!canEdit() || editStatus === 'view'}
                        saveClick={() =>
                            editStatus === 'create'
                                ? () => {}
                                : updatePartSupplier(state.partSupplier)
                        }
                        cancelClick={() => {
                            dispatch({ type: 'initialise', payload: item });
                            setEditStatus('view');
                        }}
                        backClick={() => history.push('/purchasing/part-suppliers')}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default PartSupplier;
