import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    processSelectorHelpers,
    SnackbarMessage,
    InputField,
    Loading
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { Button } from '@mui/material';
import Typography from '@mui/material/Typography';
import { applyForecastingPercentageChange } from '../itemTypes';

import history from '../history';

import applyForecastingPercentageChangeActions from '../actions/applyForecastingPercentageChangeActions';

function ApplyForecastingPercentageChange() {
    const [options, setOptions] = useState({});
    const dispatch = useDispatch();

    const loading = useSelector(state =>
        processSelectorHelpers.getWorking(state[applyForecastingPercentageChange.item])
    );

    const result = useSelector(state =>
        processSelectorHelpers.getData(state[applyForecastingPercentageChange.item])
    );

    const message = useSelector(state =>
        processSelectorHelpers.getMessageText(state[applyForecastingPercentageChange.item])
    );

    const snackbarVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[applyForecastingPercentageChange.item])
    );

    const handleFieldChange = (propertyName, newVal) => {
        setOptions(o => ({ ...o, [propertyName]: newVal }));
    };

    const setSnackbarVisible = visible =>
        dispatch(applyForecastingPercentageChange.setMessageVisible(visible));

    return (
        <Page history={history}>
            <SnackbarMessage
                visible={snackbarVisible && result?.success}
                onClose={() => setSnackbarVisible(false)}
                message={message}
            />
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h6">Apply Forecasting Percentage Change</Typography>
                </Grid>
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={12}>
                            <InputField
                                type="number"
                                propertyName="change"
                                label="% Change"
                                fullWidth
                                value={options.change}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                propertyName="startPeriod"
                                label="Start Period"
                                fullWidth
                                value={options.startPeriod}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                propertyName="endPeriod"
                                label="End Period"
                                fullWidth
                                value={options.endPeriod}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Button
                                variant="outlined"
                                onClick={() =>
                                    dispatch(
                                        applyForecastingPercentageChangeActions.requestProcessStart(
                                            options
                                        )
                                    )
                                }
                            >
                                Apply
                            </Button>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default ApplyForecastingPercentageChange;
