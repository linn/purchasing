import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField } from '@linn-it/linn-form-components-library';

function PartSupplierTab({
    partNumber,
    partDescription,
    supplierDesignation,
    supplierId,
    supplierName,
    handleFieldChange
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={partNumber}
                    label="Part Number"
                    propertyName="partNumber"
                    onChange={handleFieldChange}
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
                    value={supplierDesignation}
                    label="Designation"
                    rows={3}
                    propertyName="supplierDesignation"
                    onChange={handleFieldChange}
                />
            </Grid>
        </Grid>
    );
}

PartSupplierTab.propTypes = {
    partNumber: PropTypes.string,
    partDescription: PropTypes.string,
    supplierDesignation: PropTypes.string,
    supplierId: PropTypes.number,
    supplierName: PropTypes.string,
    handleFieldChange: PropTypes.func.isRequired
};

PartSupplierTab.defaultProps = {
    partNumber: null,
    partDescription: null,
    supplierDesignation: null,
    supplierId: null,
    supplierName: null
};

export default PartSupplierTab;
