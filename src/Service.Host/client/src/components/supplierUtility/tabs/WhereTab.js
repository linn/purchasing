import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    InputField,
    Typeahead,
    collectionSelectorHelpers,
    itemSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import Dialog from '@mui/material/Dialog';
import { makeStyles } from '@mui/styles';
import IconButton from '@mui/material/IconButton';
import Close from '@mui/icons-material/Close';
import Button from '@mui/material/Button';
import addressesActions from '../../../actions/addressesActions';
import AddressUtility from '../../AddressUtility';
import addressActions from '../../../actions/addressActions';
import countriesActions from '../../../actions/countriesActions';

function WhereTab({
    orderAddressId,
    orderFullAddress,
    invoiceAddressId,
    invoiceFullAddress,
    country,
    handleFieldChange
}) {
    const dispatch = useDispatch();
    const useStyles = makeStyles(theme => ({
        dialog: {
            margin: theme.spacing(6),
            minWidth: theme.spacing(62)
        },
        pullRight: {
            float: 'right'
        }
    }));
    const classes = useStyles();
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
    const address = useSelector(state => itemSelectorHelpers.getItem(state.address));
    const [orderAddressDialogOpen, setOrderAddressDialogOpen] = useState(false);
    const [invoiceAddressDialogOpen, setInvoiceAddressDialogOpen] = useState(false);
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
    useEffect(() => {
        if (address?.addressId) {
            if (orderAddressDialogOpen) {
                handleFieldChange('orderAddressId', address.addressId);
                handleFieldChange('orderFullAddress', address.fullAddress);
                setOrderAddressDialogOpen(false);
                dispatch(addressActions.clearItem());
            }
            if (invoiceAddressDialogOpen) {
                handleFieldChange('invoiceAddressId', address.addressId);
                handleFieldChange('invoiceFullAddress', address.fullAddress);
                setInvoiceAddressDialogOpen(false);
                dispatch(addressActions.clearItem());
            }
        }
    }, [address, handleFieldChange, orderAddressDialogOpen, invoiceAddressDialogOpen, dispatch]);
    return (
        <Grid container spacing={3}>
            <Dialog open={orderAddressDialogOpen} fullWidth maxWidth="md">
                <div>
                    <IconButton
                        className={classes.pullRight}
                        aria-label="Close"
                        onClick={() => setOrderAddressDialogOpen(false)}
                    >
                        <Close />
                    </IconButton>
                    <div className={classes.dialog}>
                        <AddressUtility
                            inDialogBox
                            closeDialog={() => setOrderAddressDialogOpen(false)}
                        />
                    </div>
                </div>
            </Dialog>
            <Dialog open={invoiceAddressDialogOpen} fullWidth maxWidth="md">
                <div>
                    <IconButton
                        className={classes.pullRight}
                        aria-label="Close"
                        onClick={() => setInvoiceAddressDialogOpen(false)}
                    >
                        <Close />
                    </IconButton>
                    <div className={classes.dialog}>
                        <AddressUtility
                            inDialogBox
                            closeDialog={() => setInvoiceAddressDialogOpen(false)}
                        />
                    </div>
                </div>
            </Dialog>
            <Grid item xs={4}>
                <Button variant="outlined" onClick={() => setOrderAddressDialogOpen(true)}>
                    Create New Order Address
                </Button>
            </Grid>
            <Grid item xs={8} />

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
                    label=" Full Address"
                    propertyName="addressee"
                    onChange={() => {}}
                    rows={7}
                />
            </Grid>
            <Grid item xs={3} />
            <Grid item xs={4}>
                <Button variant="outlined" onClick={() => setOrderAddressDialogOpen(true)}>
                    Create New Invoice Address
                </Button>
            </Grid>
            <Grid item xs={8} />

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
            <Grid item xs={3} />
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
    invoiceAddressId: PropTypes.number,
    invoiceFullAddress: PropTypes.string,
    handleFieldChange: PropTypes.func.isRequired,
    country: PropTypes.string
};
WhereTab.defaultProps = {
    orderAddressId: null,
    orderFullAddress: null,
    invoiceAddressId: null,
    invoiceFullAddress: null,
    country: null
};
export default WhereTab;
