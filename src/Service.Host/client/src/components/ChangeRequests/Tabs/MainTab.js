import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { InputField, utilities } from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';

function MainTab({ item, approve }) {
    const approveUri = utilities.getHref(item, 'approve');
    console.log(`approveUri ${approveUri}`);

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
                    value={item?.proposedBy.fullName}
                    label="Proposed By"
                    propertyName="proposedBy"
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
                    value={item?.enteredBy.fullName}
                    label="Entered By"
                    propertyName="enteredBy"
                    disabled
                />
            </Grid>
            <Grid item xs={4}>
                {item?.dateAccepted ? (
                    <InputField
                        value={item?.dateAccepted}
                        label="Date Accepted"
                        propertyName="dateAccepted"
                        type="date"
                        disabled
                    />
                ) : (
                    <Button
                        variant="contained"
                        disabled={!approveUri}
                        onClick={() => approve(item)}
                    >
                        Approve
                    </Button>
                )}
            </Grid>
        </Grid>
    );
}

MainTab.propTypes = {
    item: PropTypes.shape({
        documentNumber: PropTypes.string,
        dateEntered: PropTypes.string,
        dateAccepted: PropTypes.string,
        changeState: PropTypes.string,
        reasonForChange: PropTypes.string,
        descriptionOfChange: PropTypes.string,
        enteredBy: PropTypes.shape({
            id: PropTypes.number,
            fullName: PropTypes.string
        }),
        proposedBy: PropTypes.shape({
            id: PropTypes.number,
            fullName: PropTypes.string
        })
    }),
    approve: PropTypes.func
};

MainTab.defaultProps = {
    item: {
        documentNumber: null,
        dateEntered: null,
        changeState: null,
        reasonForChange: null,
        descriptionOfChange: null
    },
    approve: null
};

export default MainTab;
