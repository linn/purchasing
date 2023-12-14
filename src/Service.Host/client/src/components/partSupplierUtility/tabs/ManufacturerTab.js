import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField, Search } from '@linn-it/linn-form-components-library';

function ManufacturerTab({
    handleFieldChange,
    manufacturer,
    manufacturerName,
    manufacturersSearchResults,
    manufacturersSearchLoading,
    searchManufacturers,
    manufacturerPartNumber,
    vendorPartNumber
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <Search
                    propertyName="manufacturerCode"
                    label="Manufacturer"
                    resultsInModal
                    resultLimit={100}
                    value={manufacturer}
                    handleValueChange={handleFieldChange}
                    search={searchManufacturers}
                    searchResults={manufacturersSearchResults}
                    loading={manufacturersSearchLoading}
                    priorityFunction="closestMatchesFirst"
                    onResultSelect={newValue => {
                        handleFieldChange('manufacturerCode', newValue.code);
                        handleFieldChange('manufacturerName', newValue.description);
                    }}
                    clearSearch={() => {}}
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    label="Name"
                    value={manufacturerName}
                    onChange={() => {}}
                    propertyName="manufacturerName"
                    fullWidth
                />
            </Grid>
            <Grid item xs={2} />

            <Grid item xs={6}>
                <InputField
                    label="Manufacturer Part Number"
                    value={manufacturerPartNumber}
                    onChange={handleFieldChange}
                    propertyName="manufacturerPartNumber"
                    fullWidth
                />
            </Grid>
            <Grid item xs={6} />

            <Grid item xs={6}>
                <InputField
                    label="Vendor Part Number"
                    value={vendorPartNumber}
                    onChange={handleFieldChange}
                    propertyName="vendorPartNumber"
                    fullWidth
                />
            </Grid>
            <Grid item xs={6} />
        </Grid>
    );
}

ManufacturerTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    manufacturer: PropTypes.string,
    manufacturerName: PropTypes.string,
    manufacturerPartNumber: PropTypes.string,
    manufacturersSearchResults: PropTypes.arrayOf(PropTypes.shape({})),
    manufacturersSearchLoading: PropTypes.bool,
    searchManufacturers: PropTypes.func.isRequired,
    vendorPartNumber: PropTypes.string
};

ManufacturerTab.defaultProps = {
    manufacturer: null,
    manufacturerName: null,
    manufacturerPartNumber: null,
    vendorPartNumber: null,
    manufacturersSearchResults: [],
    manufacturersSearchLoading: false
};

export default ManufacturerTab;
