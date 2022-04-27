import React, { useState } from 'react';
import { useLocation } from 'react-router-dom';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    Loading,
    DatePicker,
    ExportButton,
    ReportTable
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import config from '../../config';
import { prefSupReceiptsReport } from '../../reportTypes';
import prefSupReceiptsReportActions from '../../actions/prefSupReceiptsReportActions';

function PrefSupReceiptsReport() {
    const dispatch = useDispatch();

    const location = useLocation();
    const queryOptions = queryString.parse(location.search);
    const [options, setOptions] = useState({
        fromDate: queryOptions.fromDate ? new Date(queryOptions.fromDate) : new Date(),
        toDate: queryOptions.toDate ? new Date(queryOptions.toDate) : new Date()
    });

    const loading = useSelector(state => state[prefSupReceiptsReport.item]?.loading);

    const reportData = useSelector(state => state[prefSupReceiptsReport.item]?.data);

    console.log(options.fromDate.toISOString());

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Pref Sup Receipts Report" />
                    <ExportButton
                        href={
                            `${config.appRoot}/purchasing/reports/pref-sup-receipts/export` +
                            `?fromDate=${options.fromDate.toISOString()}` +
                            `&toDate=${options.toDate.toISOString()}`
                        }
                    />
                </Grid>

                <Grid item xs={3}>
                    <DatePicker
                        label="From Date"
                        value={options.fromDate}
                        propertyName="fromDate"
                        minDate="01/01/2000"
                        maxDate={options.toDate?.toString()}
                        onChange={newVal => setOptions(o => ({ ...o, fromDate: newVal }))}
                    />
                </Grid>
                <Grid item xs={3}>
                    <DatePicker
                        label="To Date"
                        propertyName="toDate"
                        value={options.toDate}
                        maxDate={new Date()}
                        minDate={options.fromDate?.toString() || '01/01/2000'}
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
