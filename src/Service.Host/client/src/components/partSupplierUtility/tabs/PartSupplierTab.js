import React from 'react';
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
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={partNumber}
                    label="Part Number"
                    propertyName="partNumber"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={partDescription}
                    label="Description"
                    propertyName="partDescription"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={supplierId}
                    label="Supplier"
                    propertyName="supplierId"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={supplierName}
                    label="Name"
                    propertyName="supplierName"
                    onChange={handleFieldChange}
                />
            </Grid>
        </Grid>
    );
}

export default PartSupplierTab;
