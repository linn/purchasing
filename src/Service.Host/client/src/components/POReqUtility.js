import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { useParams } from 'react-router-dom';
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
    userSelectors
} from '@linn-it/linn-form-components-library';
import addressesActions from '../actions/addressesActions';
import currenciesActions from '../actions/currenciesActions';
import employeesActions from '../actions/employeesActions';
import nominalsActions from '../actions/nominalsActions';
import countriesActions from '../actions/countriesActions';
import suppliersActions from '../actions/suppliersActions';
import partsActions from '../actions/partsActions';
import poReqActions from '../actions/purchaseOrderReqActions';
import history from '../history';
import config from '../config';

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

    const addressesSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state.addresses,
            100,
            'addressId',
            'addressId',
            'addressee'
        )
    );
    const addressesSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.addresses)
    );
    const searchAddresses = searchTerm => dispatch(addressesActions.search(searchTerm));

    const currencies = useSelector(state => collectionSelectorHelpers.getItems(state.currencies));
    const employees = useSelector(state => collectionSelectorHelpers.getItems(state.employees));

    const departments = useSelector(state => collectionSelectorHelpers.getItems(state.departments));
    const departmentsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.departments)
    );

    const nominalsSearchItems = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.nominals)
    );
    const nominalsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.nominals)
    );
    // .getSearchItems(state)
    // .map(i => ({ ...i, name: i.departmentCode, id: i.departmentCode })),

    const currentUserId = useSelector(reduxState => userSelectors.getUserNumber(reduxState));
    const currentUserName = useSelector(reduxState => userSelectors.getUserNumber(reduxState));

    const [req, setReq] = useState({
        requestedBy: {
            id: currentUserId,
            fullName: currentUserName
        }
    });
    const snackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state.purchaseOrderReq)
    );
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.purchaseOrderReq)
    );
    const item = useSelector(state => itemSelectorHelpers.getItem(state.purchaseOrderReq));
    const [editStatus, setEditStatus] = useState('view');

    useEffect(() => {
        if (item?.reqNumber) {
            setReq(item);
        }
    }, [item]);

    useEffect(() => dispatch(currenciesActions.fetch()), [dispatch]);
    useEffect(() => dispatch(employeesActions.fetch()), [dispatch]);

    const { id } = useParams();
    useEffect(() => {
        if (id) {
            dispatch(poReqActions.fetch(id));
        }
    }, [id, dispatch]);

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        setReq(a => ({ ...a, [propertyName]: newValue }));
    };

    const handleEmployeeFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        setReq(a => ({ ...a, [propertyName]: { id: newValue.id, fullName: newValue.fullName } }));
    };

    const handleSupplierChange = newSupplier => {
        setEditStatus('edit');
        setReq(a => ({ ...a, supplier: { id: newSupplier.id, name: newSupplier.description } }));
        console.info(newSupplier);

        
        // handleFieldChange('supplierId', newValue.id);
        // handleFieldChange('supplierName', newValue.name);
        // todo set rest of supplier stuff,
        // supplier contact
        // set country codes
        // set currency
        // email
        // phone number
        // also lookup address    };
    };

    const reqStates = [{ id: 0, state: 'todo' }];

    const nominalAccountsTable = {
        totalItemCount: nominalsSearchItems.length,
        rows: nominalsSearchItems?.map((nom, i) => ({
            id: nom.nominalAccountId,
            values: [
                { id: `${i}-0`, value: `${nom.nominalCode}` },
                { id: `${i}-1`, value: `${nom.description || ''}` },
                { id: `${i}-2`, value: `${nom.departmentCode || ''}` },
                { id: `${i}-3`, value: `${nom.departmentDescription || ''}` }
            ],
            links: nom.links
        }))
    };

    const allowedToAuthorise = () => !creating && req?.links.some(l => l.rel === 'authorise');
    const allowedToFinanceCheck = () =>
        !creating && req?.links.some(l => l.rel === 'finance-check');
    const allowedToCreateOrder = () =>
        !creating && req?.links.some(l => l.rel === 'create-purchase-order');

    const editingAllowed = creating
        ? req?.links?.some(l => l.rel === 'create')
        : req?.links?.some(l => l.rel === 'edit');

    const inputIsValid = () => req?.reqDate?.length && req?.partNumber?.length;

    const canSave = () => editStatus !== 'view' && editingAllowed && inputIsValid;

    return (
        <>
            <Page history={history} homeUrl={config.appRoot} width="xl">
                {loading ? (
                    <Loading />
                ) : (
                    <Grid container spacing={1} justifyContent="center">
                        <SnackbarMessage
                            visible={snackbarVisible}
                            onClose={() => dispatch(poReqActions.setSnackbarVisible(false))}
                            message="Save Successful"
                        />
                        <Grid item xs={12}>
                            <Typography variant="h6">Purchase Order Req Utility</Typography>
                        </Grid>

                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req?.reqNumber}
                                label="PO Req Number"
                                propertyName="reqNumber"
                                onChange={handleFieldChange}
                                disabled
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <Dropdown
                                items={reqStates.map(e => ({
                                    displayText: `${e.state}`,
                                    id: parseInt(e.id, 10)
                                }))}
                                propertyName="state"
                                label="State"
                                onChange={(propertyName, newValue) =>
                                    handleFieldChange(propertyName, newValue)
                                }
                                disabled={!editingAllowed}
                                fullwidth
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <Button>explain states</Button>
                        </Grid>
                        <Grid item xs={2}>
                            {/* <DatePicker
                             
                                value={req.reqDate?.toString()}
                                onChange={newValue => {
                                    handleFieldChange('reqDate', newValue);
                                }}
                            /> */}
                            <InputField
                                fullWidth
                                value={req?.reqDate}
                                label="Req Date"
                                propertyName="reqDate"
                                onChange={handleFieldChange}
                                type="date"
                            />
                        </Grid>

                        <Grid item xs={5} container spacing={1}>
                            <Grid item xs={12}>
                                <Typeahead
                                    label="Part"
                                    title="Search for a part"
                                    onSelect={newPart => {
                                        handleFieldChange('partNumber', newPart.id);
                                        handleFieldChange('partDescription', newPart.description);
                                    }}
                                    items={partsSearchResults}
                                    loading={partsSearchLoading}
                                    fetchItems={searchTerm =>
                                        dispatch(partsActions.search(searchTerm))
                                    }
                                    clearSearch={() => dispatch(partsActions.clearSearch)}
                                    value={`${req?.partNumber}`}
                                    modal
                                    links={false}
                                    debounce={1000}
                                    minimumSearchTermLength={2}
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <InputField
                                    fullWidth
                                    value={req?.qty}
                                    label="Quantity"
                                    propertyName="qty"
                                    onChange={handleFieldChange}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <Dropdown
                                    fullWidth
                                    value={req?.currency}
                                    label="Currency"
                                    propertyName="currency"
                                    items={currencies.map(x => x.code)}
                                    allowNoValue
                                    onChange={(propertyName, newValue) => {
                                        handleFieldChange(propertyName, newValue);
                                    }}
                                />
                            </Grid>
                        </Grid>
                        <Grid item xs={7}>
                            <InputField
                                fullWidth
                                value={req?.partDescription}
                                label="Part Description"
                                propertyName="partDescription"
                                onChange={handleFieldChange}
                                rows={7}
                            />
                        </Grid>

                        <Grid item xs={3}>
                            <InputField
                                fullWidth
                                value={req?.unitPrice}
                                label="Unit Price"
                                number
                                propertyName="unitPrice"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <InputField
                                fullWidth
                                value={req?.carriage}
                                label="Carriage"
                                number
                                propertyName="carriage"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <InputField
                                fullWidth
                                value={req?.totalReqPrice}
                                label="Total Req Price"
                                number
                                propertyName="totalReqPrice"
                                onChange={handleFieldChange}
                            />
                        </Grid>

                        <Grid item xs={6}>
                            <Typeahead
                                onSelect={newValue => {
                                    handleSupplierChange(newValue);
                                }}
                                label="Supplier"
                                modal
                                propertyName="supplierId"
                                items={suppliersSearchResults}
                                value={`${req?.supplier?.id}: ${req?.supplier?.name}`}
                                loading={suppliersSearchLoading}
                                fetchItems={searchSuppliers}
                                links={false}
                                text
                                clearSearch={() => {}}
                                placeholder="Search Suppliers"
                                minimumSearchTermLength={3}
                                fullWidth
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.supplierContact}
                                label="Supplier Contact"
                                number
                                propertyName="supplierContact"
                                onChange={handleFieldChange}
                            />
                        </Grid>

                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.addressLine1}
                                label="Line 1"
                                propertyName="addressLine1"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.addressLine2}
                                label="Line 2"
                                propertyName="addressLine2"
                                onChange={handleFieldChange}
                            />
                        </Grid>

                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.addressLine3}
                                label="Line 3"
                                propertyName="addressLine3"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.addressLine4}
                                label="Line 4"
                                propertyName="addressLine4"
                                onChange={handleFieldChange}
                            />
                        </Grid>

                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req?.postCode}
                                label="Postcode"
                                propertyName="postCode"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <Typeahead
                                onSelect={newValue => {
                                    setReq(a => ({
                                        ...a,
                                        countryCode: newValue.countryCode,
                                        countryName: newValue.countryName
                                    }));
                                }}
                                label="Country Lookup"
                                modal
                                propertyName="countryCode"
                                items={countriesSearchResults}
                                value={req?.country?.countryCode}
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
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req?.country?.name}
                                label="Name"
                                propertyName="countryName"
                                disabled
                            />
                        </Grid>

                        <Grid item xs={12} />

                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.phoneNumber}
                                label="Phone Number"
                                propertyName="phoneNumber"
                                onChange={handleFieldChange}
                            />
                        </Grid>

                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.email}
                                label="Email Address"
                                propertyName="email"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.quoteRef}
                                label="Quote Ref"
                                propertyName="quoteRef"
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            {/* <DatePicker
                                label="Date Required"
                                value={req.dateRequired?.toString()}
                                onChange={newValue => {
                                    handleFieldChange('dateRequired', newValue);
                                }}
                            /> */}
                            <InputField
                                fullWidth
                                value={req?.dateRequired}
                                label="Date Required"
                                propertyName="dateRequired"
                                onChange={handleFieldChange}
                                type="date"
                            />
                            {/* Maybe input would be better and fix format? */}
                        </Grid>

                        {/* <Grid item xs={12}>
                            <Typeahead
                                label="Department"
                                title="Search for department"
                                onSelect={newValue => {
                                    handleFieldChange('department', newValue.departmentCode);
                                }}
                                modal
                                items={departments}
                                value={req.department}
                                loading={departmentsSearchLoading}
                                fetchItems={searchTerm =>
                                    dispatch(departmentsActions.search(searchTerm))
                                }
                                links={false}
                                clearSearch={() => dispatch(departmentsActions.clearSearch)}
                                placeholder=""
                            />
                        </Grid> */}

                        <Grid item xs={4}>
                            <TypeaheadTable
                                table={nominalAccountsTable}
                                columnNames={['Nominal', 'Description', 'Dept', 'Name']}
                                fetchItems={nominalsActions.search}
                                modal
                                placeholder="Search Nominal/Dept"
                                links={false}
                                clearSearch={nominalsActions.clearSearch}
                                loading={nominalsSearchLoading}
                                label="Nominal"
                                title="Search Nominals"
                                value={req?.nominal?.nominalCode}
                                onSelect={newValue => handleFieldChange('nominalAccount', newValue)}
                                debounce={1000}
                                minimumSearchTermLength={2}
                            />
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={req?.nominal?.description}
                                label="Description"
                                disabled
                                onChange={handleFieldChange}
                                propertyName="nominalDescription"
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                fullWidth
                                value={req?.department?.departmentCode}
                                label="Dept"
                                onChange={handleFieldChange}
                                propertyName="department"
                                disabled
                            />
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                fullWidth
                                value={req?.department?.description}
                                label="Description"
                                disabled
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            {/* set on create to current user and don't allow editing after */}
                            <InputField
                                fullWidth
                                value={`${req?.requestedBy?.fullName} (${req?.requestedBy?.id})`}
                                label="Description"
                                disabled
                                onChange={handleFieldChange}
                            />

                            {/* <Dropdown
                                fullWidth
                                value={req?.requestedBy?.id}
                                label="Requested by"
                                items={employees.map(e => ({
                                    displayText: `${e.fullName} (${e.id})`,
                                    id: e.id
                                }))}
                                propertyName="requestedBy"
                                onChange={handleEmployeeFieldChange}
                            /> */}
                        </Grid>

                        <Grid item xs={4}>
                            <Button
                                color="primary"
                                variant="contained"
                                disabled={allowedToAuthorise}
                                // onClick={handleAuthorise}
                            >
                                Authorise
                            </Button>
                        </Grid>

                        <Grid item xs={8}>
                            <Dropdown
                                fullWidth
                                value={req?.authorisedBy?.id}
                                label="Authorised by"
                                items={employees.map(e => ({
                                    displayText: `${e.fullName} (${e.id})`,
                                    id: e.id
                                }))}
                                propertyName="authorisedBy"
                                onChange={handleEmployeeFieldChange}
                            />
                        </Grid>

                        <Grid item xs={4}>
                            <Button
                                color="primary"
                                variant="contained"
                                disabled={allowedToAuthorise}
                                // onClick={handleAuthorise}
                            >
                                Authorise (secondary)
                            </Button>
                        </Grid>
                        <Grid item xs={8}>
                            <Dropdown
                                fullWidth
                                value={req?.secondAuthBy?.id}
                                label="Second auth by"
                                items={employees.map(e => ({
                                    displayText: `${e.fullName} (${e.id})`,
                                    id: e.id
                                }))}
                                propertyName="secondAuthBy"
                                onChange={handleEmployeeFieldChange}
                            />
                        </Grid>

                        <Grid item xs={4}>
                            <Button
                                color="primary"
                                variant="contained"
                                disabled={allowedToFinanceCheck}
                                // onClick={handleAuthorise}
                            >
                                Sign off (finance)
                            </Button>
                        </Grid>
                        <Grid item xs={8}>
                            <Dropdown
                                fullWidth
                                value={req?.financeCheckBy?.id}
                                label="Finance check by"
                                items={employees.map(e => ({
                                    displayText: `${e.fullName} (${e.id})`,
                                    id: e.id
                                }))}
                                propertyName="financeCheckBy"
                                onChange={handleEmployeeFieldChange}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <Button
                                color="primary"
                                variant="contained"
                                disabled={allowedToCreateOrder}
                                // onClick={handleAuthorise}
                            >
                                Create Order
                            </Button>
                            <Grid item xs={8}>
                                <Dropdown
                                    fullWidth
                                    value={req?.turnedIntoOrderBy?.id}
                                    label="Turned into order by"
                                    items={employees.map(e => ({
                                        displayText: `${e.fullName} (${e.id})`,
                                        id: e.id
                                    }))}
                                    propertyName="turnedIntoOrderBy"
                                    onChange={handleEmployeeFieldChange}
                                />
                            </Grid>
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.orderNumber}
                                label="Order Number"
                                propertyName="orderNumber"
                                disabled
                            />
                        </Grid>

                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.remarksForOrder}
                                label="Remarks to print on order"
                                propertyName="remarksForOrder"
                                onChange={handleFieldChange}
                            />
                        </Grid>

                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={req?.internalNotes}
                                label="Internal order remarks"
                                propertyName="internalNotes"
                                onChange={handleFieldChange}
                            />
                        </Grid>

                        {/* 
public string Nominal { get; set; }
public string Department { get; set; } */}

                        <SaveBackCancelButtons
                            saveDisabled={canSave}
                            // backClick={() =>
                            //     closeDialog ? closeDialog() : history.push('/purchasing')
                            // }
                            saveClick={() =>
                                creating
                                    ? dispatch(poReqActions.add(req))
                                    : dispatch(poReqActions.update(req?.reqNumber, req))
                            }
                            cancelClick={() => {
                                setEditStatus('view');
                                if (creating) {
                                    setReq({
                                        requestedBy: {
                                            id: currentUserId,
                                            fullName: currentUserName
                                        }
                                    });
                                } else {
                                    setReq(item);
                                }
                            }}
                        />
                    </Grid>
                )}
            </Page>
        </>
    );
}

POReqUtility.propTypes = { creating: PropTypes.bool };
POReqUtility.defaultProps = { creating: false };
export default POReqUtility;
