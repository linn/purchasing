import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { useSelector, useDispatch } from 'react-redux';

import {
    collectionSelectorHelpers,
    Dropdown,
    InputField,
    Typeahead
} from '@linn-it/linn-form-components-library';

import plannersActions from '../../../actions/plannersActions';
import vendorManagersActions from '../../../actions/vendorManagersActions';
import employeesActions from '../../../actions/employeesActions';

function WhoseTab({
    handleFieldChange,
    accountControllerId,
    accountControllerName,
    vendorManagerId,
    plannerId
}) {
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(plannersActions.fetch());
        dispatch(vendorManagersActions.fetch());
    }, [dispatch]);

    const searchEmployees = searchTerm => dispatch(employeesActions.search(searchTerm));
    const employeesSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.employees, 100, 'id', 'name', 'name')
    );
    const employeesSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.employees)
    );

    return (
        <Grid container spacing={3}>
            <Grid item xs={8}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('accountControllerId', newValue.id);
                        handleFieldChange('accountControllerName', newValue.fullName);
                    }}
                    label="Employeee Lookup"
                    modal
                    propertyName="accountControllerId"
                    items={employeesSearchResults}
                    value={accountControllerId}
                    loading={employeesSearchLoading}
                    fetchItems={searchEmployees}
                    links={false}
                    text
                    clearSearch={() => dispatch(employeesActions.clearSearch())}
                    placeholder="Search by Name"
                    minimumSearchTermLength={3}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={accountControllerName}
                    label="Addressee"
                    propertyName="addressee"
                    onChange={() => {}}
                />
            </Grid>
        </Grid>
    );
}

WhoseTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    accountControllerId: PropTypes.number,
    accountControllerName: PropTypes.string,
    vendorManagerId: PropTypes.number,
    plannerId: PropTypes.number
};

WhoseTab.defaultProps = {
    accountControllerName: null,
    accountControllerId: null,
    vendorManagerId: null,
    plannerId: null
};

export default WhoseTab;
