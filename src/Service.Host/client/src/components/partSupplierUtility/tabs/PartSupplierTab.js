import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField, Typeahead } from '@linn-it/linn-form-components-library';

function PartSupplierTab({
    partNumber,
    partDescription,
    partsSearchResults,
    partsSearchLoading,
    searchParts,
    designation,
    supplierId,
    supplierName,
    handleFieldChange
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => handleFieldChange('partNumber', newValue.id)}
                    label="Part"
                    modal
                    handleFieldChange={(_, newValue) => {
                        handleFieldChange('partNumber', newValue);
                    }}
                    propertyName="partNumber"
                    items={partsSearchResults}
                    value={partNumber}
                    loading={partsSearchLoading}
                    fetchItems={searchParts}
                    links={false}
                    text
                    clearSearch={() => {}}
                    placeholder="Search Locations"
                    minimumSearchTermLength={3}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={partDescription}
                    label="Description"
                    propertyName="partDescription"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={supplierId}
                    label="Supplier"
                    propertyName="supplierId"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={supplierName}
                    label="Name"
                    propertyName="supplierName"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={designation}
                    label="Designation"
                    rows={3}
                    propertyName="designation"
                    onChange={handleFieldChange}
                />
            </Grid>
        </Grid>
    );
}

PartSupplierTab.propTypes = {
    partNumber: PropTypes.string,
    partDescription: PropTypes.string,
    designation: PropTypes.string,
    supplierId: PropTypes.number,
    supplierName: PropTypes.string,
    handleFieldChange: PropTypes.func.isRequired,
    partsSearchResults: PropTypes.arrayOf(PropTypes.shape({})),
    partsSearchLoading: PropTypes.bool,
    searchParts: PropTypes.func.isRequired
};

PartSupplierTab.defaultProps = {
    partNumber: null,
    partDescription: null,
    designation: null,
    supplierId: null,
    supplierName: null,
    partsSearchResults: [],
    partsSearchLoading: false
};

export default PartSupplierTab;
