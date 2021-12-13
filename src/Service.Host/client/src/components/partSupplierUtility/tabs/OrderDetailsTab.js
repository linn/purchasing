import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField, Dropdown } from '@linn-it/linn-form-components-library';

function OrderDetailsTab({
    handleFieldChange,
    editStatus,
    unitsOfMeasure,
    unitOfMeasure,
    deliveryAddresses,
    deliveryAddress,
    orderMethod,
    orderMethodDescription,
    orderMethods,
    currencies,
    currency
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                <InputField
                    fullWidth
                    value={editStatus}
                    label="thing"
                    propertyName="editStatus"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={orderMethod}
                    label="Order Method"
                    propertyName="orderMethodName"
                    items={orderMethods.map(x => x.name)}
                    allowNoValue={false}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={orderMethodDescription}
                    label="Desc"
                    propertyName="orderMethodDescription"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={currency}
                    label="Currency"
                    propertyName="currencyCode"
                    items={currencies.map(x => x.code)}
                    allowNoValue
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={unitOfMeasure}
                    label="Units"
                    propertyName="unitOfMeasure"
                    items={unitsOfMeasure.map(x => x.unit)}
                    allowNoValue
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={deliveryAddress}
                    label="Delivery Address"
                    propertyName="addressId"
                    items={deliveryAddresses.map(x => ({
                        id: x.addressId.toString(),
                        displayText: x.description
                    }))}
                    allowNoValue
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
        </Grid>
    );
}

OrderDetailsTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    unitsOfMeasure: PropTypes.arrayOf(PropTypes.shape({})),
    deliveryAddresses: PropTypes.arrayOf(PropTypes.shape({})),
    orderMethods: PropTypes.arrayOf(PropTypes.shape({})),
    editStatus: PropTypes.string,
    unitOfMeasure: PropTypes.string,
    deliveryAddress: PropTypes.string,
    orderMethod: PropTypes.string,
    orderMethodDescription: PropTypes.string,
    currencies: PropTypes.arrayOf(PropTypes.shape({})),
    currency: PropTypes.string
};

OrderDetailsTab.defaultProps = {
    editStatus: 'view',
    unitsOfMeasure: [],
    deliveryAddresses: [],
    orderMethods: [],
    currencies: [],
    unitOfMeasure: null,
    deliveryAddress: null,
    orderMethod: null,
    currency: null,
    orderMethodDescription: null
};

export default OrderDetailsTab;
