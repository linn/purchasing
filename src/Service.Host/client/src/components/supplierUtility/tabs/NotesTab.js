import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField } from '@linn-it/linn-form-components-library';

function NotesTab({ handleFieldChange, organisationId, notes }) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={notes}
                    label="Notes"
                    rows={4}
                    propertyName="notes"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={organisationId}
                    disabled
                    label="Org Id"
                    rows={4}
                    propertyName="organisationId"
                    onChange={handleFieldChange}
                />
            </Grid>
        </Grid>
    );
}

NotesTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    organisationId: PropTypes.number,
    notes: PropTypes.string
};

NotesTab.defaultProps = {
    notes: null,
    organisationId: null
};

export default NotesTab;
