import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';

import {
    InputField,
    collectionSelectorHelpers,
    Dropdown
} from '@linn-it/linn-form-components-library';

import accountingCompaniesActions from '../../../actions/accountingCompaniesActions';

function GeneralTab({
    name,
    handleFieldChange,
    phoneNumber,
    webAddress,
    suppliersReference,
    orderContactMethod,
    invoiceContactMethod,
    accountingCompany
}) {
    const reduxDispatch = useDispatch();
    useEffect(() => {
        reduxDispatch(accountingCompaniesActions.fetch());
    }, [reduxDispatch]);

    const accountingCompanies = useSelector(state =>
        collectionSelectorHelpers.getItems(state.accountingCompanies)
    ).map(x => x.name);

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
                <Dropdown
                    fullWidth
                    value={orderContactMethod}
                    label="Order Contact Method"
                    propertyName="orderContactMethod"
                    allowNoValue
                    items={['EDI', 'EMAIL', 'FAX', 'NONE', 'POST']}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={8}>
                <Dropdown
                    fullWidth
                    value={invoiceContactMethod}
                    label="Invoice Contact Method"
                    propertyName="invoiceContactMethod"
                    allowNoValue
                    items={['EDI', 'EMAIL', 'FAX', 'NONE', 'POST']}
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
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={accountingCompany}
                    label="Accounting Company"
                    items={accountingCompanies}
                    propertyName="accountingCompany"
                    onChange={handleFieldChange}
                    allowNoValue={false}
                />
            </Grid>
            <Grid item xs={8} />
        </Grid>
    );
}

GeneralTab.propTypes = {
    name: PropTypes.string,
    phoneNumber: PropTypes.string,
    webAddress: PropTypes.string,
    orderContactMethod: PropTypes.string,
    invoiceContactMethod: PropTypes.string,
    suppliersReference: PropTypes.string,
    handleFieldChange: PropTypes.func.isRequired,
    accountingCompany: PropTypes.string
};

GeneralTab.defaultProps = {
    name: null,
    phoneNumber: null,
    webAddress: null,
    orderContactMethod: null,
    invoiceContactMethod: null,
    suppliersReference: null,
    accountingCompany: null
};

export default GeneralTab;
