import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import { InputField } from '@linn-it/linn-form-components-library';

function WhereTab({ orderAddressId, orderFullAddress, invoiceAddressId, invoiceFullAddress }) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={2}>
                <InputField
                    label="Order Address Id"
                    propertyName="orderAddressId"
                    value={orderAddressId}
                    type="number"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={10}>
                <InputField
                    label="Order Address"
                    propertyName="orderFullAddress"
                    value={orderFullAddress}
                    onChange={() => {}}
                    fullWidth
                    rows={7}
                />
            </Grid>
            <Grid item xs={2}>
                <InputField
                    label="Invoice Address Id"
                    propertyName="invoiceAddressId"
                    value={invoiceAddressId}
                    type="number"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={10}>
                <InputField
                    label="Invoice Address"
                    fullWidth
                    propertyName="invoiceFullAddress"
                    value={invoiceFullAddress}
                    onChange={() => {}}
                    rows={7}
                />
            </Grid>
        </Grid>
    );
}

WhereTab.propTypes = {
    orderAddressId: PropTypes.number,
    orderFullAddress: PropTypes.string,
    invoiceAddressId: PropTypes.number,
    invoiceFullAddress: PropTypes.string
};
WhereTab.defaultProps = {
    orderAddressId: null,
    orderFullAddress: null,
    invoiceAddressId: null,
    invoiceFullAddress: null
};
export default WhereTab;
