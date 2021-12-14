import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField, Dropdown } from '@linn-it/linn-form-components-library';
import { deliveryAddresses } from '../../../itemTypes';

function OtherDetailsTab({
    handleFieldChange,
    leadTimeWeeks,
    contractLeadTimeWeeks,
    overbookingAllowed,
    damagesPercent,
    webAddress,
    deliveryInstructions,
    notesForBuyer,
    tariffId,
    tariffCode,
    dutyPercent,
    tariffDescription,
    packWasteStatus,
    packagingGroupId,
    packagingGroupDescription,
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
                <InputField
                    fullWidth
                    value={contractLeadTimeWeeks}
                    type="number"
                    label="Contract Lead Time Weeks"
                    propertyName="contractLeadTimeWeeks"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
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
                    label="Notes For Buyter"
                    value={notesForBuyer}
                    propertyName="notesForBuyer"
                    rows={3}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6} />

            {/* <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={}
                    label=""
                    propertyName=""
                    items={}
                    allowNoValue={false}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={}
                    label=""
                    propertyName="orderMethdDescription"
                    onChange={() => {}}
                />
            </Grid> */}
            <Grid item xs={4} />
        </Grid>
    );
}

OtherDetailsTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    leadTimeWeeks: PropTypes.number,
    contractLeadTimeWeeks: PropTypes.number,
    overbookingAllowed: PropTypes.string,
    damagesPercent: PropTypes.number,
    webAddress: PropTypes.string,
    deliveryInstructions: PropTypes.string,
    notesForBuyer: PropTypes.string,
    tariffId: PropTypes.number,
    tariffCode: PropTypes.string,
    dutyPercent: PropTypes.number,
    tariffDescription: PropTypes.string,
    packWasteStatus: PropTypes.string,
    packagingGroupId: PropTypes.number,
    packagingGroupDescription: PropTypes.string
};

OtherDetailsTab.defaultProps = {
    leadTimeWeeks: null,
    contractLeadTimeWeeks: null,
    overbookingAllowed: null,
    damagesPercent: null,
    webAddress: null,
    deliveryInstructions: null,
    notesForBuyer: null,
    tariffId: null,
    tariffCode: null,
    dutyPercent: null,
    tariffDescription: null,
    packWasteStatus: null,
    packagingGroupId: null,
    packagingGroupDescription: null
};

export default OtherDetailsTab;
