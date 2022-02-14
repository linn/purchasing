import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';

import { useSelector, useDispatch } from 'react-redux';

import {
    collectionSelectorHelpers,
    Dropdown,
    Typeahead
} from '@linn-it/linn-form-components-library';

import plannersActions from '../../../actions/plannersActions';
import vendorManagersActions from '../../../actions/vendorManagersActions';
import employeesActions from '../../../actions/employeesActions';

function WhoseTab({handleFieldChange}) {
    const reduxDispatch = useDispatch();
    useEffect(() => {
        reduxDispatch(plannersActions.fetch());
        reduxDispatch(vendorManagersActions.fetch());
    }, [reduxDispatch]);

    const searchEmployees = searchTerm => reduxDispatch(employeesActions.search(searchTerm));
    const employeesSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.employees, 100, 'id', 'name', 'name')
    );
    const employeesSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.employees)
    );

    return <Grid container spacing={3}></Grid>;
}

WhoseTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired
};

WhoseTab.defaultProps = {};

export default WhoseTab;
