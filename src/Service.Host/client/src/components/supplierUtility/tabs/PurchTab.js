import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';

import {
    InputField,
    collectionSelectorHelpers,
    Dropdown,
    Typeahead
} from '@linn-it/linn-form-components-library';

import suppliersActions from '../../../actions/suppliersActions';
import currenciesActions from '../../../actions/currenciesActions';

function PurchTab({
    handleFieldChange,
    partCategory,
    partCategoryDescription,
    orderHold,
    notesForBuyer,
    deliveryDay,
    refersToFcId,
    refersToFcName,
    pmDeliveryDaysGrace
}) {
    const reduxDispatch = useDispatch();
    useEffect(() => {
        reduxDispatch(currenciesActions.fetch());
    }, [reduxDispatch]);

    const searchSuppliers = searchTerm => reduxDispatch(suppliersActions.search(searchTerm));
    const suppliersSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.suppliers, 100, 'id', 'name', 'name')
    );
    const suppliersSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.suppliers)
    );
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('refersToFcId', newValue.id);
                        handleFieldChange('refersToFcName', newValue.description);
                    }}
                    label="Refers to Fc Supplier"
                    modal
                    propertyName="refersToFcId"
                    items={suppliersSearchResults}
                    value={refersToFcId}
                    loading={suppliersSearchLoading}
                    fetchItems={searchSuppliers}
                    links={false}
                    text
                    clearSearch={() => {}}
                    placeholder="Search Suppliers"
                    minimumSearchTermLength={3}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={refersToFcName}
                    label="Name"
                    propertyName="refersToFcName"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={orderHold}
                    label="Order Hold"
                    items={['Y', 'N']}
                    propertyName="orderHold"
                    onChange={handleFieldChange}
                    allowNoValue={false}
                />
            </Grid>
            <Grid item xs={8}>
            {/* <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={invoiceGoesToName}
                    label="Name"
                    propertyName="invoiceGoesToName"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={expenseAccount}
                    label="Expense Account"
                    items={['Y', 'N']}
                    propertyName="expenseAccount"
                    onChange={handleFieldChange}
                    allowNoValue
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={paymentDays}
                    label="Payment Days"
                    propertyName="paymentDays"
                    type="number"
                    onChange={handleFieldChange}
                />
            </Grid>

            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={vatNumber}
                    label="VAT Number"
                    propertyName="vatNumber"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={paymentMethod}
                    label="Payment Method"
                    items={['BACS', 'CHEQUE', 'FORPAY', 'NONE', 'OTHER']}
                    propertyName="paymentMethod"
                    onChange={handleFieldChange}
                    allowNoValue
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={currencyCode}
                    label="Currency"
                    items={currencies}
                    propertyName="currencyCode"
                    onChange={handleFieldChange}
                    allowNoValue={false}
                />
            </Grid>
            <Grid item xs={2}>
                <Dropdown
                    fullWidth
                    value={paysInFc}
                    label="Payment Method"
                    items={['A', 'S', 'N']}
                    propertyName="paysInFc"
                    onChange={handleFieldChange}
                    allowNoValue
                />
            </Grid>
            <Grid item xs={6} />
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={approvedCarrier}
                    label="Approved Carrier"
                    items={['Y', 'N']}
                    propertyName="approvedCarrier"
                    onChange={handleFieldChange}
                    allowNoValue={false}
                />
            </Grid>
            <Grid item xs={8} /> */}
        </Grid>
    );
}

PurchTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    partCategory: PropTypes.string,
    partCategoryDescription: PropTypes.string,
    orderHold: PropTypes.string,
    notesForBuyer: PropTypes.string,
    deliveryDay: PropTypes.string,
    refersToFcId: PropTypes.number,
    refersToFcName: PropTypes.string,
    pmDeliveryDaysGrace: PropTypes.number
};

PurchTab.defaultProps = {
    partCategory: null,
    partCategoryDescription: null,
    orderHold: null,
    notesForBuyer: null,
    deliveryDay: null,
    refersToFcId: null,
    refersToFcName: null,
    pmDeliveryDaysGrace: null
};

export default PurchTab;
