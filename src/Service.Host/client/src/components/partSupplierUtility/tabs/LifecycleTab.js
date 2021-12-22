import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { Dropdown, DatePicker } from '@linn-it/linn-form-components-library';

function LifecycleTab({
    handleFieldChange,
    createdBy,
    employees,
    dateCreated,
    madeInvalidBy,
    dateInvalid
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={8}>
                <Dropdown
                    fullWidth
                    value={createdBy}
                    label="Created By"
                    items={employees.map(x => ({ id: x.id, displayText: x.fullName }))}
                    propertyName="createdBy"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <DatePicker
                    label="Date"
                    value={dateCreated || new Date()}
                    onChange={newValue => handleFieldChange('dateCreated', newValue)}
                    minDate="01/01/1970"
                    maxDate="01/01/2100"
                />
            </Grid>
            <Grid item xs={8}>
                <Dropdown
                    fullWidth
                    value={madeInvalidBy}
                    label="Invalidated By"
                    items={employees.map(x => ({ id: x.id, displayText: x.fullName }))}
                    propertyName="madeInvalidBy"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <DatePicker
                    label="Date"
                    value={dateInvalid}
                    onChange={newValue => handleFieldChange('dateInvalid', newValue)}
                    minDate="01/01/1970"
                    maxDate="01/01/2100"
                />
            </Grid>
        </Grid>
    );
}

LifecycleTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    createdBy: PropTypes.number,
    employees: PropTypes.arrayOf(PropTypes.shape({})),
    dateCreated: PropTypes.shape({}),
    dateInvalid: PropTypes.shape({}),
    madeInvalidBy: PropTypes.number
};

LifecycleTab.defaultProps = {
    createdBy: null,
    employees: [],
    dateCreated: null,
    dateInvalid: undefined,
    madeInvalidBy: null
};

export default LifecycleTab;
