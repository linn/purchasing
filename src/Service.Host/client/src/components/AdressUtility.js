import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import Typography from '@mui/material/Typography';
import {
    Page,
    SaveBackCancelButtons,
    collectionSelectorHelpers,
    Typeahead,
    InputField,
    SnackbarMessage,
    itemSelectorHelpers,
    Loading
} from '@linn-it/linn-form-components-library';
import addressesActions from '../actions/addressesActions';
import addressActions from '../actions/addressActions';
import history from '../history';
import config from '../config';

function AddressUtility({ inDialogBox, closeDialog }) {
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

    const [address, setAddress] = useState({});
    const snackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state.address)
    );
    const loading = useSelector(state => itemSelectorHelpers.getItemLoading(state.address));
    const item = useSelector(state => itemSelectorHelpers.getItem(state.address));
    const [editStatus, setEditStatus] = useState('view');
    useEffect(() => {
        if (item?.addressId) {
            setAddress(item);
        }
    }, [item]);

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        setAddress(a => ({ ...a, [propertyName]: newValue }));
    };

    const content = () =>
        loading ? (
            <Loading />
        ) : (
            <Grid container spacing={3}>
                <SnackbarMessage
                    visible={snackbarVisible}
                    onClose={() => dispatch(addressActions.setSnackbarVisible(false))}
                    message="Save Successful"
                />
                <Grid item xs={12}>
                    <Typography variant="h6">Address Utility</Typography>
                </Grid>
                <Grid item xs={8}>
                    <Typeahead
                        onSelect={newValue => {
                            setAddress(newValue);
                        }}
                        label="Address Lookup (Leave blank if creating new)"
                        modal
                        propertyName="partNumber"
                        items={addressesSearchResults}
                        value={address?.addressId}
                        loading={addressesSearchLoading}
                        fetchItems={searchAddresses}
                        links={false}
                        text
                        clearSearch={handleFieldChange}
                        placeholder="Search by Addressee"
                        minimumSearchTermLength={3}
                    />
                </Grid>
                <Grid item xs={8}>
                    <InputField
                        fullWidth
                        value={address?.addressee}
                        label="Addressee"
                        propertyName="addressee"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={8}>
                    <InputField
                        fullWidth
                        value={address?.addressee2}
                        label="Addressee 2"
                        propertyName="addressee2"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={4} />
                <Grid item xs={8}>
                    <InputField
                        fullWidth
                        value={address?.line1}
                        label="Line 1"
                        propertyName="line1"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={4} />
                <Grid item xs={8}>
                    <InputField
                        fullWidth
                        value={address?.line2}
                        label="Line 2"
                        propertyName="line2"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={4} />
                <Grid item xs={8}>
                    <InputField
                        fullWidth
                        value={address?.line3}
                        label="Line 3"
                        propertyName="line3"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={4} />
                <Grid item xs={8}>
                    <InputField
                        fullWidth
                        value={address?.line4}
                        label="Line 4"
                        propertyName="line4"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={4} />
                <Grid item xs={8}>
                    <InputField
                        fullWidth
                        value={address?.postCode}
                        label="Postcode"
                        propertyName="postCode"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={8}>
                    <InputField
                        fullWidth
                        value={address?.countryCode}
                        label="Country"
                        propertyName="countryCode"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={4} />
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveDisabled={
                            editStatus === 'view' || !address?.addressee || !address?.country
                        }
                        backClick={() =>
                            closeDialog ? closeDialog() : history.push('/purchasing')
                        }
                        saveClick={() =>
                            address?.addressId
                                ? dispatch(addressActions.update(address.addressId, address))
                                : dispatch(addressActions.add(address))
                        }
                        cancelClick={() => {
                            setEditStatus('view');
                            return closeDialog ? closeDialog() : setAddress({});
                        }}
                    />
                </Grid>
            </Grid>
        );
    return (
        <>
            {inDialogBox ? (
                content()
            ) : (
                <Page history={history} homeUrl={config.appRoot}>
                    {content()}
                </Page>
            )}
        </>
    );
}

AddressUtility.propTypes = { inDialogBox: PropTypes.bool, closeDialog: PropTypes.func };
AddressUtility.defaultProps = { inDialogBox: false, closeDialog: null };
export default AddressUtility;
