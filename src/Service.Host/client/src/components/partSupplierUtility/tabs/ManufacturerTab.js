import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { Dropdown, DatePicker, InputField, Typeahead } from '@linn-it/linn-form-components-library';

function ManufacturerTab({
    handleFieldChange,
    manufacturer,
    manufacturerName,
    manufacturersSearchResults,
    manufacturersSearchLoading,
    searchManufacturers,
    manufacturerPartNumber,
    vendorPartNumber,
    rohsCategory,
    dateRohsCompliant,
    rohsCompliant,
    rohsComments
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('manufacturerCode', newValue.code);
                        handleFieldChange('manufacturerName', newValue.description);
                    }}
                    label="Manufacturer"
                    modal
                    propertyName="manufacturerId"
                    items={manufacturersSearchResults}
                    value={manufacturer}
                    loading={manufacturersSearchLoading}
                    fetchItems={searchManufacturers}
                    links={false}
                    text
                    clearSearch={() => {}}
                    placeholder="Search manufacturers"
                    minimumSearchTermLength={3}
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
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={rohsCategory}
                    label="Rohs Category"
                    items={[
                        { id: 'YES', displayText: 'COMPLIANT' },
                        { id: 'EXEMPT', displayText: 'EXEMPT FROM ROHS DIRECTIVE' },
                        { id: 'NOREP', displayText: 'NO REPLACEMENT' },
                        { id: 'NO', displayText: 'NON COMPLIANT' }
                    ]}
                    propertyName="rohsCategory"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <DatePicker
                    label="Date"
                    value={dateRohsCompliant || new Date()}
                    onChange={newValue => handleFieldChange('dateRohsCompliant', newValue)}
                    minDate="01/01/1900"
                    maxDate="01/01/2100"
                />
            </Grid>
            <Grid item xs={4} />

            {/* <Grid item xs={8}>
                <Dropdown
                    fullWidth
                    value={createdBy}
                    label="Created By"
                    items={employees.map(x => ({ id: x.id, displayText: x.fullName }))}
                    propertyName="createdBy"
                    onChange={handleFieldChange}
                />
            </Grid> */}
            {/* <Grid item xs={4}>
                <DatePicker
                    label="Date"
                    value={dateCreated || new Date()}
                    onChange={newValue => handleFieldChange('dateCreated', newValue)}
                    minDate="01/01/1970"
                    maxDate="01/01/2100"
                />
            </Grid> */}
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
    vendorPartNumber: PropTypes.string,
    rohsCategory: PropTypes.string,
    dateRohsCompliant: PropTypes.string,
    rohsCompliant: PropTypes.string,
    rohsComments: PropTypes.string
};

ManufacturerTab.defaultProps = {
    manufacturer: null,
    manufacturerName: null,
    manufacturerPartNumber: null,
    vendorPartNumber: null,
    rohsCategory: null,
    dateRohsCompliant: null,
    rohsCompliant: null,
    rohsComments: null,
    manufacturersSearchResults: [],
    manufacturersSearchLoading: false
};

export default ManufacturerTab;
