import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField } from '@linn-it/linn-form-components-library';

function PreferredSupplier({
    partNumber,
    partDescription,
    oldSupplierId,
    oldSupplierDescription,
    oldPrice,
    baseOldPrice,
    oldCurrencyCode,
    oldCurrencyDescription,
    newSupplierId,
    newSupplierName,
    newPrice,
    baseNewPrice,
    newCurrencyName,
    newCurrencyDescription,
    dateChanged,
    changedBy,
    changeReason,
    changeReasons,
    remarks,
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
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={partDescription}
                    label="Description"
                    propertyName="partDescription"
                    onChange={() => {}}
                />
            </Grid>
        </Grid>
    );
}

PreferredSupplier.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    partNumber: PropTypes.string.isRequired,
    partDescription: PropTypes.string.isRequired,
    oldSupplierId: PropTypes.number.isRequired,
    oldSupplierDescription: PropTypes.string.isRequired,
    oldPrice: PropTypes.number,
    baseOldPrice: PropTypes.number,
    oldCurrencyCode: PropTypes.number,
    oldCurrencyDescription: PropTypes.string,
    newSupplierId: PropTypes.number,
    newSupplierName: PropTypes.string,
    newPrice: PropTypes.number,
    baseNewPrice: PropTypes.number,
    newCurrencyName: PropTypes.string,
    newCurrencyDescription: PropTypes.string,
    dateChanged: PropTypes.string,
    changedBy: PropTypes.number,
    changeReason: PropTypes.string,
    changeReasons: PropTypes.arrayOf(PropTypes.string),
    remarks: PropTypes.string
};

PreferredSupplier.defaultProps = {
    oldPrice: null,
    baseOldPrice: null,
    oldCurrencyCode: null,
    oldCurrencyDescription: null,
    newSupplierId: null,
    newSupplierName: null,
    newPrice: null,
    baseNewPrice: null,
    newCurrencyName: null,
    newCurrencyDescription: null,
    dateChanged: null,
    changedBy: null,
    changeReason: null,
    changeReasons: PropTypes.arrayOf(null),
    remarks: null
};

export default PreferredSupplier;
