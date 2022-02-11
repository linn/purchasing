import React, { useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    InputField,
    Typeahead,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import Dialog from '@mui/material/Dialog';
import { makeStyles } from '@mui/styles';
import IconButton from '@mui/material/IconButton';
import Close from '@mui/icons-material/Close';
import Button from '@mui/material/Button';
import addressesActions from '../../../actions/addressesActions';
import AddressUtility from '../../AdressUtility';

function WhereTab({
    orderAddressId,
    orderFullAddress,
    invoiceAddressId,
    invoiceFullAddress,
    handleFieldChange
}) {
    const dispatch = useDispatch();
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

    const [addressDialogOpen, setAddressDialogOpen] = useState(false);
    return (
        <Grid container spacing={3}>
            <Dialog open={addressDialogOpen} fullWidth maxWidth="md">
                <div>
                    <IconButton
                        className={classes.pullRight}
                        aria-label="Close"
                        onClick={() => setAddressDialogOpen(false)}
                    >
                        <Close />
                    </IconButton>
                    <div className={classes.dialog}>
                        <AddressUtility
                            inDialogBox
                            closeDialog={() => setAddressDialogOpen(false)}
                        />
                    </div>
                </div>
            </Dialog>
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
            <Grid item xs={2}>
                <Button variant="outlined" onClick={() => setAddressDialogOpen(true)}>
                    Address Utility
                </Button>
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
