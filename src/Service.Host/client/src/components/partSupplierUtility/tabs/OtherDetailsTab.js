import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField } from '@linn-it/linn-form-components-library';

function OtherDetailsTab({
    handleFieldChange,
    leadTimeWeeks,
    deliveryInstructions,
    notesForBuyer
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={leadTimeWeeks}
                    type="number"
                    label="Lead Time Weeks"
                    propertyName="leadTimeWeeks"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    label="Delivery Instructions"
                    value={deliveryInstructions}
                    propertyName="deliveryInstructions"
                    rows={3}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6} />
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    label="Notes For Buyer"
                    value={notesForBuyer}
                    propertyName="notesForBuyer"
                    rows={3}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6} />
        </Grid>
    );
}

OtherDetailsTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    leadTimeWeeks: PropTypes.number,
    deliveryInstructions: PropTypes.string,
    notesForBuyer: PropTypes.string
};

OtherDetailsTab.defaultProps = {
    leadTimeWeeks: null,
    deliveryInstructions: null,
    notesForBuyer: null
};

export default OtherDetailsTab;
