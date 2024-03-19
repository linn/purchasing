import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    collectionSelectorHelpers,
    itemSelectorHelpers
} from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import AddressUtility from './AddressUtility';

// this container just wraps the component in all the redux gubbins necessary to make it work
// I wanted to common-ise the component and not couple it to redux dispatching and selecting
// such that it can be reused in future apps that might not use redux
function AddressUtilityReduxContainer({
    onCreateSuccess,
    onSelectAddress,
    addressActions,
    addressesActions,
    countriesActions,
    defaultAddressee,
    addressItemType,
    addressesItemType,
    countriesItemType
}) {
    const dispatch = useDispatch();
    const addressStoreItem = useSelector(state => state[addressItemType.item]);
    const addressesStoreItem = useSelector(state => state[addressesItemType.item]);
    const countriesStoreItem = useSelector(state => state[countriesItemType.item]);
    const address = addressStoreItem.item;

    if (address?.addressId) {
        onCreateSuccess(address);
        dispatch(addressActions.clearItem());
    }

    return (
        <AddressUtility
            defaultAddressee={defaultAddressee}
            createAddress={data => dispatch(addressActions.add(data))}
            selectAddress={onSelectAddress}
            searchCountries={searchTerm => dispatch(countriesActions.search(searchTerm))}
            countriesSearchResults={collectionSelectorHelpers.getSearchItems(
                countriesStoreItem,
                100,
                'countryCode',
                'countryCode',
                'countryName'
            )}
            countriesSearchLoading={collectionSelectorHelpers.getSearchLoading(countriesStoreItem)}
            clearCountriesSearch={() => dispatch(countriesActions.clearSearch())}
            searchAddresses={searchTerm => dispatch(addressesActions.search(searchTerm))}
            addressSearchResults={collectionSelectorHelpers.getSearchItems(
                addressesStoreItem,
                100,
                'addressId',
                'addressee',
                'line1'
            )}
            addressSearchLoading={collectionSelectorHelpers.getSearchLoading(addressesStoreItem)}
            clearAddressesSearch={() => dispatch(addressesActions.clearSearch())}
            createAddressLoading={itemSelectorHelpers.getItemLoading(addressStoreItem)}
        />
    );
}

AddressUtilityReduxContainer.propTypes = {
    defaultAddressee: PropTypes.string,
    onCreateSuccess: PropTypes.func.isRequired,
    onSelectAddress: PropTypes.func.isRequired,
    addressActions: PropTypes.shape({ add: PropTypes.func, clearItem: PropTypes.func }).isRequired,
    addressesActions: PropTypes.shape({ search: PropTypes.func, clearSearch: PropTypes.func })
        .isRequired,
    countriesActions: PropTypes.shape({ search: PropTypes.func, clearSearch: PropTypes.func })
        .isRequired,
    addressItemType: PropTypes.shape({ item: PropTypes.string }),
    addressesItemType: PropTypes.shape({ item: PropTypes.string }),
    countriesItemType: PropTypes.shape({ item: PropTypes.string })
};

AddressUtilityReduxContainer.defaultProps = {
    defaultAddressee: null,
    addressItemType: { item: 'address' },
    addressesItemType: { item: 'addresses' },
    countriesItemType: { item: 'countries' }
};

export default AddressUtilityReduxContainer;
