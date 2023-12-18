import React, { useState } from 'react';
import { useLocation } from 'react-router-dom';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    Loading,
    ExportButton,
    ReportTable,
    DatePicker
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import moment from 'moment';
import history from '../../history';
import config from '../../config';
import { prefSupReceiptsReport } from '../../reportTypes';
import prefSupReceiptsReportActions from '../../actions/prefSupReceiptsReportActions';

function PrefSupReceiptsReport() {
    const dispatch = useDispatch();

    const location = useLocation();
    const queryOptions = queryString.parse(location.search);
    const [options, setOptions] = useState({
        fromDate: queryOptions.fromDate ? moment(queryOptions.fromDate) : moment(),
        toDate: queryOptions.toDate ? moment(queryOptions.toDate) : moment()
    });
    const [queryLaunched, setQueryLaunched] = useState(false);

    // if come from dashboard
    if (queryOptions.fromDate && queryOptions.toDate && !queryLaunched) {
        setQueryLaunched(true);
        dispatch(
            prefSupReceiptsReportActions.fetchReport({
                fromDate: options.fromDate.toISOString(),
                toDate: options.toDate.toISOString()
            })
        );
    }

    const loading = useSelector(state => state[prefSupReceiptsReport.item]?.loading);

    const reportData = useSelector(state => state[prefSupReceiptsReport.item]?.data);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Pref Sup Receipts Report" />
                    <ExportButton
                        href={`${
                            config.appRoot
                        }/purchasing/reports/pref-sup-receipts/report?fromDate=${moment(
                            options.fromDate
                        ).format('DD-MMM-YYYY')}&toDate=${moment(options.toDate).format(
                            'DD-MMM-YYYY'
                        )}`}
                    />
                </Grid>

                <Grid item xs={3}>
                    <DatePicker
                        label="From Date"
                        value={options.fromDate}
                        propertyName="fromDate"
                        format="DD/MM/YYYY"
                        maxDate={options.toDate}
                        onChange={newVal => setOptions(o => ({ ...o, fromDate: newVal }))}
                    />
                </Grid>
                <Grid item xs={3}>
                    <DatePicker
                        label="To Date"
                        propertyName="toDate"
                        value={options.toDate}
                        maxDate={moment()}
                        minDate={options.fromDate}
                        onChange={newVal => setOptions(o => ({ ...o, toDate: newVal }))}
                    />
                </Grid>
                <Grid item xs={6}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={() =>
                            dispatch(
                                prefSupReceiptsReportActions.fetchReport({
                                    fromDate: options.fromDate.toISOString(),
                                    toDate: options.toDate.toISOString()
                                })
                            )
                        }
                    >
                        Run Report
                    </Button>
                </Grid>

                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        {reportData && (
                            <Grid item xs={12}>
                                <ReportTable
                                    reportData={reportData}
                                    title={reportData.title}
                                    showTitle
                                    showTotals
                                    placeholderRows={4}
                                    placeholderColumns={4}
                                />
                            </Grid>
                        )}
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default PrefSupReceiptsReport;
