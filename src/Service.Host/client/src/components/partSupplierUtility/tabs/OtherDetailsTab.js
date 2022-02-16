import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField, Dropdown } from '@linn-it/linn-form-components-library';

function OtherDetailsTab({
    handleFieldChange,
    leadTimeWeeks,
    overbookingAllowed,
    damagesPercent,
    webAddress,
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
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={overbookingAllowed}
                    label="Overbooking Allowed"
                    propertyName="overbookingAllowed"
                    items={['Y', 'N']}
                    allowNoValue
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={damagesPercent}
                    type="number"
                    label="Damages Percent"
                    propertyName="damagesPercent"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    label="Web Address"
                    value={webAddress}
                    propertyName="webAddress"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6} />
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
    overbookingAllowed: PropTypes.string,
    damagesPercent: PropTypes.number,
    webAddress: PropTypes.string,
    deliveryInstructions: PropTypes.string,
    notesForBuyer: PropTypes.string
};

OtherDetailsTab.defaultProps = {
    leadTimeWeeks: null,
    overbookingAllowed: null,
    damagesPercent: null,
    webAddress: null,
    deliveryInstructions: null,
    notesForBuyer: null
};

export default OtherDetailsTab;
