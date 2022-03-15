import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import { Link as RouterLink } from 'react-router-dom';
import { InputField } from '@linn-it/linn-form-components-library';

function OtherDetailsTab({
    handleFieldChange,
    leadTimeWeeks,
    damagesPercent,
    deliveryInstructions,
    notesForBuyer,
    bulkUpdateLeadTimesUrl
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
            <Grid item xs={4}>
                {bulkUpdateLeadTimesUrl && (
                    <RouterLink to={bulkUpdateLeadTimesUrl}>
                        <Typography variant="button"> Bulk Update Lead Times </Typography>
                    </RouterLink>
                )}
            </Grid>
            <Grid item xs={4} />
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
    damagesPercent: PropTypes.number,
    deliveryInstructions: PropTypes.string,
    notesForBuyer: PropTypes.string,
    bulkUpdateLeadTimesUrl: PropTypes.string
};

OtherDetailsTab.defaultProps = {
    leadTimeWeeks: null,
    damagesPercent: null,
    deliveryInstructions: null,
    notesForBuyer: null,
    bulkUpdateLeadTimesUrl: null
};

export default OtherDetailsTab;
