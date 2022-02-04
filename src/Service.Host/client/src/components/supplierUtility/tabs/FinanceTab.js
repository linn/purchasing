import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';

import {
    InputField,
    collectionSelectorHelpers,
    Dropdown
} from '@linn-it/linn-form-components-library';

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
    approvedCarrier
}) {
    const reduxDispatch = useDispatch();
    // useEffect(() => {
    //     reduxDispatch(accountingCompaniesActions.fetch());
    // }, [reduxDispatch]);

    // const accountingCompanies = useSelector(state =>
    //     collectionSelectorHelpers.getItems(state.accountingCompanies)
    // ).map(x => x.name);

    return (
        <Grid container spacing={3}>
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
                    value={expenseAccount}
                    label="Expense Account"
                    items={['Y', 'N']}
                    propertyName="accountingCompany"
                    onChange={expenseAccount}
                    allowNoValue
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
    approvedCarrier: PropTypes.string
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
    approvedCarrier: null
};

export default FinanceTab;
