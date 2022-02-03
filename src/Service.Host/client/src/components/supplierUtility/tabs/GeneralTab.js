import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField } from '@linn-it/linn-form-components-library';

function GeneralTab({
    name,
    handleFieldChange,
    phoneNumber,
    webAddress,
    suppliersReference,
    orderContactMethod,
    invoiceContactMethod,
    liveOnOracle
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={name}
                    label="Name"
                    propertyName="name"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={phoneNumber}
                    label="Phone Number"
                    propertyName="phoneNumber"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={webAddress}
                    label="Website"
                    propertyName="webAddress"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={liveOnOracle}
                    label="Live On Oracle"
                    propertyName="liveOnOracle"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={orderContactMethod}
                    label="Order Contact Method"
                    propertyName="orderContactMethod"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={invoiceContactMethod}
                    label="Invoice Contact Method"
                    propertyName="invoiceContactMethod"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={suppliersReference}
                    label="Reference (For Us)"
                    propertyName="suppliersReference"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
        </Grid>
    );
}

GeneralTab.propTypes = {
    name: PropTypes.string,
    phoneNumber: PropTypes.string,
    webAddress: PropTypes.string,
    orderContactMethod: PropTypes.string,
    invoiceContactMethod: PropTypes.string,
    liveOnOracle: PropTypes.string,
    suppliersReference: PropTypes.string,
    handleFieldChange: PropTypes.func.isRequired
};

GeneralTab.defaultProps = {
    name: null,
    phoneNumber: null,
    webAddress: null,
    orderContactMethod: null,
    invoiceContactMethod: null,
    suppliersReference: null,
    liveOnOracle: null
};

export default GeneralTab;
