import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { InputField, Dropdown } from '@linn-it/linn-form-components-library';
import { Button } from '@mui/material';

function OrderDetailsTab({
    handleFieldChange,
    unitsOfMeasure,
    unitOfMeasure,
    deliveryAddresses,
    deliveryAddress,
    orderMethod,
    orderMethodDescription,
    orderMethods,
    currencies,
    currency,
    currencyUnitPrice,
    ourCurrencyPriceToShowOnOrder,
    baseOurUnitPrice,
    minimumOrderQty,
    minimumDeliveryQty,
    orderIncrement,
    orderConversionFactor,
    reelOrBoxQty,
    fullAddress,
    setPriceChangeDialogOpen
}) {
    return (
        <Grid container spacing={3}>
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
                <Button variant="outlined" onClick={() => setPriceChangeDialogOpen(true)}>
                    Change Prices
                </Button>
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={currency}
                    label="Currency"
                    propertyName="currencyCode"
                    items={currencies.map(x => x.code)}
                    allowNoValue
                    disabled
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={currencyUnitPrice}
                    label="Currency Unit Price"
                    type="number"
                    disabled
                    propertyName="currencyUnitPrice"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={ourCurrencyPriceToShowOnOrder}
                    label="Our Price to Show On Order (ex duty)"
                    type="number"
                    disabled
                    propertyName="ourCurrencyPriceToShowOnOrder"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={baseOurUnitPrice}
                    label="Base Our Unit Price"
                    type="number"
                    disabled
                    propertyName="baseOurUnitPrice"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={minimumOrderQty}
                    label="Minumum Order Qty"
                    type="number"
                    required
                    propertyName="minimumOrderQty"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={minimumDeliveryQty}
                    label="Minumum Delivery Qty"
                    type="number"
                    propertyName="minimumDeliveryQty"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={orderIncrement}
                    label="Order Increment"
                    type="number"
                    required
                    propertyName="orderIncrement"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={reelOrBoxQty}
                    label="Reel or Box Qty"
                    type="number"
                    propertyName="reelOrBoxQty"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
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
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={orderConversionFactor}
                    label="Order Conversion Factor"
                    type="number"
                    propertyName="orderConversionFactor"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
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
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={fullAddress}
                    label="fullAddress"
                    type="number"
                    rows={10}
                    propertyName="fullAddress"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={2} />
        </Grid>
    );
}

OrderDetailsTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    unitsOfMeasure: PropTypes.arrayOf(PropTypes.shape({})),
    deliveryAddresses: PropTypes.arrayOf(PropTypes.shape({})),
    orderMethods: PropTypes.arrayOf(PropTypes.shape({})),
    unitOfMeasure: PropTypes.string,
    deliveryAddress: PropTypes.string,
    orderMethod: PropTypes.string,
    orderMethodDescription: PropTypes.string,
    currencies: PropTypes.arrayOf(PropTypes.shape({})),
    currency: PropTypes.string,
    currencyUnitPrice: PropTypes.number,
    ourCurrencyPriceToShowOnOrder: PropTypes.number,
    baseOurUnitPrice: PropTypes.number,
    minimumOrderQty: PropTypes.number,
    minimumDeliveryQty: PropTypes.number,
    orderIncrement: PropTypes.number,
    orderConversionFactor: PropTypes.number,
    reelOrBoxQty: PropTypes.number,
    fullAddress: PropTypes.string,
    setPriceChangeDialogOpen: PropTypes.func.isRequired
};

OrderDetailsTab.defaultProps = {
    unitsOfMeasure: [],
    deliveryAddresses: [],
    orderMethods: [],
    currencies: [],
    unitOfMeasure: null,
    deliveryAddress: null,
    orderMethod: null,
    currency: null,
    orderMethodDescription: null,
    currencyUnitPrice: null,
    ourCurrencyPriceToShowOnOrder: null,
    baseOurUnitPrice: null,
    minimumOrderQty: null,
    minimumDeliveryQty: null,
    orderIncrement: null,
    orderConversionFactor: null,
    reelOrBoxQty: null,
    fullAddress: null
};

export default OrderDetailsTab;
