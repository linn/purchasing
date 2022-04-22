import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Page, collectionSelectorHelpers } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';

import history from '../history';

import mrpRunLogActions from '../actions/mrpRunLogActions';
import mrpRunLogsActions from '../actions/mrpRunLogsActions';
import runMrpApplicationStateActions from '../actions/runMrpApplicationStateActions';
import employeesActions from '../actions/employeesActions';

function RunMrp() {
    const [editingAllowed, setEditingAllowed] = useState(false);

    const mrpRunLogs = useSelector(state => collectionSelectorHelpers.getItems(state.mrpRunLogs));
    const mrpRunLogsLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.mrpRunLogs)
    );
    const mrpRunLogsApplicationState = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.mrpRunLogs)
    );

    const dispatch = useDispatch();
    useEffect(() => dispatch(mrpRunLogsActions.fetch()), [dispatch]);
    useEffect(() => dispatch(runMrpApplicationStateActions.fetchState()), [dispatch]);
    useEffect(() => dispatch(employeesActions.fetch()), [dispatch]);

    useEffect(() => {
        if (mrpRunLogsApplicationState && !mrpRunLogsApplicationState.loading) {
            if (mrpRunLogsApplicationState.links?.find(a => a.rel === 'edit')) {
                setEditingAllowed(true);
            } else {
                setEditingAllowed(false);
            }
        }
    }, [mrpRunLogsApplicationState]);

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={8} />
                <Grid item xs={4}>
                    hello
                </Grid>
            </Grid>
        </Page>
    );
}

export default RunMrp;
