import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField, Dropdown, Typeahead } from '@linn-it/linn-form-components-library';

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
    searchTariffs,
    tariffsSearchResults,
    tariffsSearchLoading,
    dutyPercent,
    tariffDescription,
    packWasteStatus,
    packagingGroupId,
    packagingGroups
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={leadTimeWeeks}
                    type="number"
                    required
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
                    label="Notes For Buyer"
                    value={notesForBuyer}
                    propertyName="notesForBuyer"
                    rows={3}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6} />
            <Grid item xs={2}>
                <InputField
                    fullWidth
                    label="Duty %"
                    value={dutyPercent}
                    propertyName="dutyPercent"
                    type="number"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={10} />

            <Grid item xs={2}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('tariffId', newValue.id);
                        handleFieldChange('tariffCode', newValue.code);

                        handleFieldChange('tariffDescription', newValue.description);
                    }}
                    label="Tariff"
                    modal
                    propertyName="tariffId"
                    items={tariffsSearchResults}
                    value={tariffId}
                    loading={tariffsSearchLoading}
                    fetchItems={searchTariffs}
                    links={false}
                    text
                    clearSearch={() => {}}
                    placeholder="Search tariffs"
                    minimumSearchTermLength={3}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    label="Code"
                    value={tariffCode}
                    propertyName="tariffCode"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    label="Description"
                    value={tariffDescription}
                    rows={6}
                    propertyName="tariffDescription"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={packWasteStatus}
                    label="Packaging Waste Status"
                    propertyName="packWasteStatus"
                    items={[
                        { id: 'A', displayText: 'Applicable' },
                        { id: 'N', displayText: 'Not Applicable' }
                    ]}
                    allowNoValue
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={packagingGroupId}
                    label="Group"
                    propertyName="packagingGroupId"
                    items={packagingGroups.map(g => ({ ...g, displayText: g.description }))}
                    allowNoValue
                    onChange={handleFieldChange}
                />
            </Grid>
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
    searchTariffs: PropTypes.func.isRequired,
    tariffsSearchResults: PropTypes.arrayOf(PropTypes.shape({})),
    tariffsSearchLoading: PropTypes.bool,
    packagingGroups: PropTypes.arrayOf(PropTypes.shape({}))
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
    tariffsSearchResults: [],
    tariffsSearchLoading: false,
    packagingGroups: []
};

export default OtherDetailsTab;
