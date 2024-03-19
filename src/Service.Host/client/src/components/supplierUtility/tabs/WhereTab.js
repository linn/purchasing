import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    InputField,
    Typeahead,
    collectionSelectorHelpers,
    AddressUtilityReduxContainer
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import addressesActions from '../../../actions/addressesActions';
import addressActions from '../../../actions/addressActions';
import countriesActions from '../../../actions/countriesActions';

function WhereTab({
    orderAddressId,
    orderFullAddress,
    invoiceFullAddress,
    country,
    handleFieldChange,
    supplierName
}) {
    const dispatch = useDispatch();

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
    const updateOrderAddress = newAddress => {
        handleFieldChange('orderAddressId', newAddress.addressId);
        handleFieldChange('orderFullAddress', newAddress.fullAddress);
        handleFieldChange('country', newAddress.countryCode);
    };
    const updateInvoiceAddress = newAddress => {
        handleFieldChange('invoiceAddressId', newAddress.addressId);
        handleFieldChange('invoiceFullAddress', newAddress.fullAddress);
    };
    return (
        <Grid container spacing={3}>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={orderFullAddress}
                    label=" Order Address"
                    propertyName="orderFullAddress"
                    onChange={() => {}}
                    rows={7}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={12}>
                <AddressUtilityReduxContainer
                    defaultAddressee={supplierName}
                    addressActions={addressActions}
                    addressesActions={addressesActions}
                    countriesActions={countriesActions}
                    onCreateSuccess={updateOrderAddress}
                    onSelectAddress={updateOrderAddress}
                />
            </Grid>
            <Grid item xs={4}>
                <Button
                    variant="outlined"
                    disabled={!orderAddressId}
                    onClick={() => {
                        handleFieldChange('invoiceAddressId', orderAddressId);
                        handleFieldChange('invoiceFullAddress', orderFullAddress);
                    }}
                >
                    Copy Order Address To Invoice Address
                </Button>
            </Grid>
            <Grid item xs={8} />

            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={invoiceFullAddress}
                    label="Invoice Address"
                    propertyName="invoiceFullAddress"
                    onChange={() => {}}
                    rows={7}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={12}>
                <AddressUtilityReduxContainer
                    defaultAddressee={supplierName}
                    addressActions={addressActions}
                    addressesActions={addressesActions}
                    countriesActions={countriesActions}
                    onCreateSuccess={updateInvoiceAddress}
                    onSelectAddress={updateInvoiceAddress}
                />
            </Grid>

            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('country', newValue.countryCode);
                    }}
                    label="Country Lookup"
                    modal
                    propertyName="country"
                    items={countriesSearchResults}
                    value={country}
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
            <Grid item xs={8} />
        </Grid>
    );
}

WhereTab.propTypes = {
    orderAddressId: PropTypes.number,
    orderFullAddress: PropTypes.string,
    invoiceFullAddress: PropTypes.string,
    handleFieldChange: PropTypes.func.isRequired,
    country: PropTypes.string,
    supplierName: PropTypes.string
};
WhereTab.defaultProps = {
    orderAddressId: null,
    orderFullAddress: null,
    invoiceFullAddress: null,
    country: null,
    supplierName: null
};
export default WhereTab;
