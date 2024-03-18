import React, { useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Dialog from '@mui/material/Dialog';
import Button from '@mui/material/Button';

import { makeStyles } from '@mui/styles';
import IconButton from '@mui/material/IconButton';
import Close from '@mui/icons-material/Close';
import Typography from '@mui/material/Typography';

import {
    SaveBackCancelButtons,
    InputField,
    Search,
    Loading
} from '@linn-it/linn-form-components-library';

function AddressUtility({
    createAddress,
    createAddressLoading,
    selectAddress,
    searchCountries,
    searchAddresses,
    countriesSearchResults,
    countriesSearchLoading,
    addressSearchResults,
    addressSearchLoading,
    clearAddressesSearch,
    clearCountriesSearch,
    defaultAddressee
}) {
    const [address, setAddress] = useState({ addressee: defaultAddressee });
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

    const [popUpOpen, setPopUpOpen] = useState(false);

    const handleFieldChange = (propertyName, newValue) => {
        setAddress(a => ({ ...a, [propertyName]: newValue }));
    };

    const close = () => {
        setPopUpOpen(false);
    };

    const open = () => setPopUpOpen(true);

    const [addressSearchTerm, setAddressSearchTerm] = useState('');

    const [countrySearchTerm, setCountrySearchTerm] = useState('');

    const chips = a => {
        const result = [{ text: a.addressId }];
        if (a.postCode) {
            result.push({ text: a.postCode });
        }
        if (a.countryCode) {
            result.push({ text: a.countryCode });
        }
        return result;
    };
    return (
        <>
            <Grid item xs={4}>
                <Button variant="outlined" onClick={open}>
                    Create Or Look Up Address
                </Button>
            </Grid>
            <Grid item xs={8} />
            <Dialog open={popUpOpen} maxWidth="md">
                <div>
                    <IconButton className={classes.pullRight} aria-label="Close" onClick={close}>
                        <Close />
                    </IconButton>
                    <div className={classes.dialog}>
                        <Grid container spacing={3}>
                            <Grid item xs={12}>
                                <Typography variant="h6">Create or Look Up Address</Typography>
                            </Grid>
                            <Grid item xs={12}>
                                <Search
                                    propertyName="salesAccount"
                                    label="Look up an address"
                                    value={addressSearchTerm}
                                    handleValueChange={(_, newVal) => setAddressSearchTerm(newVal)}
                                    search={searchAddresses}
                                    searchResults={addressSearchResults.map(a => ({
                                        ...a,
                                        chips: chips(a)
                                    }))}
                                    loading={addressSearchLoading}
                                    displayChips
                                    resultsInModal
                                    onResultSelect={x => {
                                        selectAddress(x);
                                        close();
                                    }}
                                    clearSearch={clearAddressesSearch}
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <Typography variant="subtitle1">
                                    Or Enter details and click save to create a new address
                                </Typography>
                            </Grid>
                            {createAddressLoading ? (
                                <Loading />
                            ) : (
                                <>
                                    <Grid item xs={8}>
                                        <InputField
                                            fullWidth
                                            value={address?.addressee}
                                            label="Addressee"
                                            propertyName="addressee"
                                            onChange={handleFieldChange}
                                        />
                                    </Grid>
                                    <Grid item xs={4} />
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
                                    <Grid item xs={4} />
                                    <Grid item xs={4}>
                                        <Search
                                            propertyName="countryCode"
                                            label="Look up Countries"
                                            value={countrySearchTerm}
                                            handleValueChange={(_, newVal) =>
                                                setCountrySearchTerm(newVal)
                                            }
                                            search={searchCountries}
                                            searchResults={countriesSearchResults}
                                            loading={countriesSearchLoading}
                                            autoFocus={false}
                                            resultsInModal
                                            priorityFunction={(i, searchTerm) => {
                                                if (i.countryCode === searchTerm?.toUpperCase()) {
                                                    return 1;
                                                }
                                                return 0;
                                            }}
                                            onResultSelect={newValue => {
                                                setCountrySearchTerm(newValue.countryCode);
                                                setAddress(a => ({
                                                    ...a,
                                                    countryCode: newValue.countryCode,
                                                    countryName: newValue.countryName
                                                }));
                                            }}
                                            clearSearch={clearCountriesSearch}
                                        />
                                    </Grid>
                                    <Grid item xs={8}>
                                        <InputField
                                            fullWidth
                                            value={address?.countryName}
                                            label="Name"
                                            propertyName="countryName"
                                            onChange={() => {}}
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <SaveBackCancelButtons
                                            saveDisabled={
                                                !address?.addressee || !address?.countryCode
                                            }
                                            backClick={close}
                                            saveClick={() => {
                                                createAddress(address);
                                                close();
                                            }}
                                            cancelClick={close}
                                        />
                                    </Grid>
                                </>
                            )}
                        </Grid>
                    </div>
                </div>
            </Dialog>
        </>
    );
}

AddressUtility.propTypes = {
    createAddress: PropTypes.func.isRequired,
    selectAddress: PropTypes.func.isRequired,
    searchCountries: PropTypes.func.isRequired,
    searchAddresses: PropTypes.func.isRequired,
    countriesSearchResults: PropTypes.arrayOf(PropTypes.shape({})),
    countriesSearchLoading: PropTypes.bool,
    addressSearchResults: PropTypes.arrayOf(PropTypes.shape({})),
    addressSearchLoading: PropTypes.bool,
    clearAddressesSearch: PropTypes.func,
    clearCountriesSearch: PropTypes.func,
    createAddressLoading: PropTypes.bool,
    defaultAddressee: PropTypes.string
};

AddressUtility.defaultProps = {
    countriesSearchResults: [],
    countriesSearchLoading: false,
    addressSearchResults: [],
    addressSearchLoading: false,
    clearAddressesSearch: () => {},
    clearCountriesSearch: () => {},
    createAddressLoading: false,
    defaultAddressee: false
};

export default AddressUtility;
