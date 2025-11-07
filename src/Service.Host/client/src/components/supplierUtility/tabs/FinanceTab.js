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

function FinanceTab({
    handleFieldChange,
    vatNumber,
    invoiceGoesToId,
    invoiceGoesToName,
    expenseAccount,
    paymentDays,
    paymentMethod,
    currencyCode,
    paysInFc,
    approvedCarrier,
    paymentDaysTerms
}) {
    const reduxDispatch = useDispatch();
    useEffect(() => {
        reduxDispatch(currenciesActions.fetch());
    }, [reduxDispatch]);

    const currencies = useSelector(state =>
        collectionSelectorHelpers.getItems(state.currencies)
    ).map(x => x.code);
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
                        handleFieldChange('invoiceGoesToId', newValue.id);
                        handleFieldChange('invoiceGoesToName', newValue.description);
                    }}
                    label="Invoice Goes To"
                    modal
                    propertyName="invoiceGoesToId"
                    items={suppliersSearchResults}
                    value={invoiceGoesToId}
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
            <Grid item xs={2}>
                <InputField
                    fullWidth
                    value={paymentDays}
                    label="Payment Days"
                    propertyName="paymentDays"
                    type="number"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={2}>
                <InputField
                    fullWidth
                    value={paymentDaysTerms}
                    label="Payment Terms"
                    propertyName="paymentDaysTerms"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
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
                    items={['BACS', 'CHEQUE', 'FORPAY', 'NONE', 'OTHER', 'FORPAY MANUAL']}
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
                    label="Pays In Foreign Currency?"
                    items={[
                        { id: 'A', displayText: 'Always' },
                        { id: 'N', displayText: 'Never' }
                    ]}
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
            <Grid item xs={8} />
        </Grid>
    );
}

FinanceTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    vatNumber: PropTypes.string,
    invoiceGoesToId: PropTypes.number,
    invoiceGoesToName: PropTypes.string,
    expenseAccount: PropTypes.string,
    paymentDays: PropTypes.number,
    paymentMethod: PropTypes.string,
    currencyCode: PropTypes.string,
    paysInFc: PropTypes.string,
    approvedCarrier: PropTypes.string,
    paymentDaysTerms: PropTypes.string
};

FinanceTab.defaultProps = {
    invoiceGoesToId: null,
    vatNumber: null,
    invoiceGoesToName: null,
    expenseAccount: null,
    paymentDays: null,
    paymentMethod: null,
    currencyCode: null,
    paysInFc: null,
    approvedCarrier: null,
    paymentDaysTerms: null
};

export default FinanceTab;
