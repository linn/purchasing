import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { InputField } from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';

function AssemblyChange({ item }) {
    return (
        <>
            <Grid item xs={4}>
                <Typography>Assembly Change</Typography>
            </Grid>
            <Grid item xs={8}>
                <InputField
                    value={item?.newPartNumber}
                    label="Part Number"
                    propertyName="proposedBy"
                    disabled
                />
                <Typography>{item?.newPartDescription}</Typography>
            </Grid>
        </>
    );
}

AssemblyChange.propTypes = {
    item: PropTypes.shape({
        newPartNumber: PropTypes.string,
        newPartDescription: PropTypes.string
    })
};

AssemblyChange.defaultProps = {
    item: {
        newPartNumber: null,
        newPartDescription: null
    }
};

export default AssemblyChange;
