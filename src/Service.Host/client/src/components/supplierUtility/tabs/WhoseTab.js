import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { useSelector, useDispatch } from 'react-redux';

import {
    collectionSelectorHelpers,
    Dropdown,
    InputField,
    Loading,
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

    const searchEmployees = searchTerm => dispatch(employeesActions.search(searchTerm));
    const employeesSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.employees, 100, 'id', 'fullName', 'fullName')
    );
    const employeesSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.employees)
    );
    const vendorManagers = useSelector(state =>
        collectionSelectorHelpers.getItems(state.vendorManagers)
    );
    const planners = useSelector(state => collectionSelectorHelpers.getItems(state.planners));
    const vendorManagersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.vendorManagers)
    );
    const plannersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.planners)
    );

    useEffect(() => {
        if (!planners || planners.length === 0) {
            dispatch(plannersActions.fetch());
        }
        if (!planners || planners.length === 0) {
            dispatch(vendorManagersActions.fetch());
        }
    }, [dispatch, planners, vendorManagers]);

    if (plannersLoading || vendorManagersLoading) {
        return <Loading />;
    }
    return (
        <Grid container spacing={3}>
            <Grid item xs={3}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('accountControllerId', newValue.id);
                        handleFieldChange('accountControllerName', newValue.fullName);
                    }}
                    label="Account Controller"
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
                    label="Name"
                    propertyName="addressee"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={5} />

            <Grid item xs={8}>
                <Dropdown
                    items={vendorManagers.map(v => ({ id: v.vmId, displayText: v.name }))}
                    value={vendorManagerId}
                    allowNoValue
                    propertyName="vendorManagerId"
                    label="Vendor Manager"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
            <Grid item xs={8}>
                <Dropdown
                    items={planners.map(v => ({ id: v.id, displayText: v.employeeName }))}
                    value={plannerId}
                    allowNoValue
                    propertyName="plannerId"
                    label="Planner"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4} />
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
