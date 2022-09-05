import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    processSelectorHelpers,
    SnackbarMessage,
    InputField,
    Loading,
    Dropdown,
    ErrorCard,
    ReportTable
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { Button } from '@mui/material';
import Typography from '@mui/material/Typography';
import { applyForecastingPercentageChange } from '../itemTypes';
import { forecastWeekChangesReport } from '../reportTypes';
import history from '../history';
import forecastWeekChangesReportActions from '../actions/forecastWeekChangesReportActions';
import applyForecastingPercentageChangeActions from '../actions/applyForecastingPercentageChangeActions';

function ApplyForecastingPercentageChange() {
    const today = new Date();
    const [options, setOptions] = useState({
        startMonth: today.getMonth().toString(),
        startYear: today.getFullYear(),
        endMonth: today.getMonth() === 11 ? '1' : (today.getMonth() + 1).toString(),
        endYear: today.getMonth() === 11 ? today.getFullYear() + 1 : today.getFullYear(),
        change: -10
    });
    const dispatch = useDispatch();

    const loading = useSelector(state =>
        processSelectorHelpers.getWorking(state[applyForecastingPercentageChange.item])
    );

    const reportLoading = useSelector(state => state[forecastWeekChangesReport.item]?.loading);
    const reportData = useSelector(state => state[forecastWeekChangesReport.item]?.data);

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

    const months = [
        { id: '0', displayText: 'JAN' },
        { id: '1', displayText: 'FEB' },
        { id: '2', displayText: 'MAR' },
        { id: '3', displayText: 'APR' },
        { id: '4', displayText: 'MAY' },
        { id: '5', displayText: 'JUN' },
        { id: '6', displayText: 'JUL' },
        { id: '7', displayText: 'AUG' },
        { id: '8', displayText: 'SEP' },
        { id: '9', displayText: 'OCT' },
        { id: '10', displayText: 'NOV' },
        { id: '11', displayText: 'DEC' }
    ];

    useEffect(() => {
        dispatch(forecastWeekChangesReportActions.fetchReport());
    }, [dispatch]);

    useEffect(() => {
        if (result?.success) {
            dispatch(forecastWeekChangesReportActions.fetchReport());
        }
    }, [result, dispatch]);

    return (
        <Page history={history}>
            <SnackbarMessage
                visible={snackbarVisible && result?.success}
                onClose={() => setSnackbarVisible(false)}
                message={message}
            />
            {result && !result.success && (
                <Grid item xs={12}>
                    <ErrorCard errorMessage={result.message} />
                </Grid>
            )}
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h6">Apply Forecasting Percentage Change</Typography>
                </Grid>
                {loading || reportLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={2}>
                            <InputField
                                type="number"
                                propertyName="change"
                                label="% Change"
                                error={options.change > 0}
                                helperText={
                                    options.change > 0
                                        ? 'Are you sure you want to apply a positive change?'
                                        : ''
                                }
                                fullWidth
                                value={options.change}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={10} />
                        <Grid item xs={2}>
                            <Dropdown
                                propertyName="startMonth"
                                label="Start Month"
                                fullWidth
                                value={options.startMonth}
                                items={months}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                propertyName="startYear"
                                label="Start Year"
                                type="number"
                                fullWidth
                                value={options.startYear}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={8} />

                        <Grid item xs={2}>
                            <Dropdown
                                propertyName="endMonth"
                                label="End Month"
                                fullWidth
                                value={options.endMonth}
                                items={months}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                propertyName="endYear"
                                label="End Year"
                                type="number"
                                fullWidth
                                value={options.endYear}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={8} />
                        <Grid item xs={12}>
                            <Button
                                variant="outlined"
                                onClick={() => {
                                    dispatch(
                                        applyForecastingPercentageChangeActions.clearProcessData()
                                    );
                                    dispatch(
                                        applyForecastingPercentageChangeActions.requestProcessStart(
                                            options
                                        )
                                    );
                                }}
                            >
                                Apply
                            </Button>
                        </Grid>
                        <Grid item xs={12}>
                            {reportData && (
                                <ReportTable
                                    reportData={reportData}
                                    showTotals
                                    placeholderRows={10}
                                    placeholderColumns={3}
                                    showRowTitles={false}
                                    showTitle
                                />
                            )}
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default ApplyForecastingPercentageChange;
