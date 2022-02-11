import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    InputField,
    Typeahead,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';

import addressesActions from '../../../actions/addressesActions';

function WhereTab({
    orderAddressId,
    orderFullAddress,
    invoiceAddressId,
    invoiceFullAddress,
    handleFieldChange
}) {
    const dispatch = useDispatch();
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
    return (
        <Grid container spacing={3}>
            <Grid item xs={3}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('orderAddressId', newValue.addressId);
                        handleFieldChange('orderFullAddress', newValue.fullAddress);
                    }}
                    label="Order Addressee"
                    modal
                    propertyName="orderAddressId"
                    items={addressesSearchResults}
                    value={orderAddressId}
                    loading={addressesSearchLoading}
                    fetchItems={searchAddresses}
                    links={false}
                    text
                    clearSearch={() => dispatch(addressesActions.clearSearch())}
                    placeholder="Search by Addressee"
                    minimumSearchTermLength={3}
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={orderFullAddress}
                    label="Address"
                    propertyName="addressee"
                    onChange={() => {}}
                    rows={7}
                />
            </Grid>
            <Grid item xs={3} />

            <Grid item xs={3}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('invoiceAddressId', newValue.addressId);
                        handleFieldChange('invoiceFullAddress', newValue.fullAddress);
                    }}
                    label="Invoice Addressee"
                    modal
                    propertyName="invoiceAddressId"
                    items={addressesSearchResults}
                    value={invoiceAddressId}
                    loading={addressesSearchLoading}
                    fetchItems={searchAddresses}
                    links={false}
                    text
                    clearSearch={() => dispatch(addressesActions.clearSearch())}
                    placeholder="Search by Addressee"
                    minimumSearchTermLength={3}
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={invoiceFullAddress}
                    label="Full Address"
                    propertyName="invoiceFullAddress"
                    onChange={() => {}}
                    rows={7}
                />
            </Grid>
        </Grid>
    );
}

WhereTab.propTypes = {
    orderAddressId: PropTypes.number,
    orderFullAddress: PropTypes.string,
    invoiceAddressId: PropTypes.number,
    invoiceFullAddress: PropTypes.string,
    handleFieldChange: PropTypes.func.isRequired
};
WhereTab.defaultProps = {
    orderAddressId: null,
    orderFullAddress: null,
    invoiceAddressId: null,
    invoiceFullAddress: null
};
export default WhereTab;
