import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { InputField } from '@linn-it/linn-form-components-library';

function MainTab({ item }) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <InputField
                    value={item?.documentNumber}
                    label="Change Request"
                    disabled
                    propertyName="documentNumber"
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    value={item?.dateEntered}
                    label="Date Entered"
                    propertyName="dateEntered"
                    type="date"
                    disabled
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    value={item?.changeState}
                    label="Change State"
                    propertyName="changeState"
                    disabled
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={item?.reasonForChange}
                    label="Reason For Change"
                    propertyName="reasonForChange"
                    rows={4}
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={item?.descriptionOfChange}
                    label="Description Of Change"
                    propertyName="descriptionOfChange"
                    rows={4}
                />
            </Grid>
        </Grid>
    );
}

MainTab.propTypes = {
    item: PropTypes.shape({
        documentNumber: PropTypes.string,
        dateEntered: PropTypes.string,
        changeState: PropTypes.string,
        reasonForChange: PropTypes.string,
        descriptionOfChange: PropTypes.string
    })
};

MainTab.defaultProps = {
    item: {
        documentNumber: null,
        dateEntered: null,
        changeState: null,
        reasonForChange: null,
        descriptionOfChange: null
    }
};

export default MainTab;
