import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { useSelector, useDispatch } from 'react-redux';
import moment from 'moment';
import {
    collectionSelectorHelpers,
    InputField,
    Typeahead,
    DatePicker
} from '@linn-it/linn-form-components-library';

import employeesActions from '../../../actions/employeesActions';

function LifecycleTab({
    handleFieldChange,
    openedById,
    openedByName,
    dateOpened,
    closedById,
    closedByName,
    dateClosed,
    reasonClosed
}) {
    const dispatch = useDispatch();

    const searchEmployees = searchTerm => dispatch(employeesActions.search(searchTerm));
    const employeesSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.employees, 100, 'id', 'fullName', 'fullName')
    );
    const employeesSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.employees)
    );

    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('openedById', newValue.id);
                        handleFieldChange('openedByName', newValue.fullName);
                    }}
                    label="Opened By"
                    modal
                    propertyName="openedById"
                    items={employeesSearchResults}
                    value={openedById}
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
                    value={openedByName}
                    label="Name"
                    propertyName="openedByName"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={4}>
                <DatePicker
                    label="Date"
                    value={moment(dateOpened) || moment()}
                    onChange={newValue => handleFieldChange('dateOpened', newValue)}
                    format="DD/MM/YYYY"
                    disabled
                />
            </Grid>

            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('closedById', newValue.id);
                        handleFieldChange('closedByName', newValue.fullName);
                    }}
                    label="Closed By"
                    modal
                    propertyName="closedById"
                    items={employeesSearchResults}
                    value={closedById}
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
                    value={closedByName}
                    label="Name"
                    propertyName="closedByName"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={4}>
                <DatePicker
                    label="Date"
                    value={dateClosed ? moment(dateClosed) : null}
                    onChange={newValue => handleFieldChange('dateClosed', newValue)}
                    format="DD/MM/YYYY"
                    disabled
                />
            </Grid>
            <Grid item xs={12}>
                <InputField
                    fullWidth
                    value={reasonClosed}
                    label="Reason Closed"
                    propertyName="reasonClosed"
                    onChange={handleFieldChange}
                />
            </Grid>
        </Grid>
    );
}

LifecycleTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    openedById: PropTypes.number,
    openedByName: PropTypes.string,
    dateOpened: PropTypes.string,
    closedById: PropTypes.number,
    closedByName: PropTypes.string,
    dateClosed: PropTypes.string,
    reasonClosed: PropTypes.string
};

LifecycleTab.defaultProps = {
    openedByName: null,
    openedById: null,
    dateOpened: null,
    closedById: null,
    closedByName: null,
    dateClosed: null,
    reasonClosed: null
};

export default LifecycleTab;
