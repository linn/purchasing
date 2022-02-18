import React, { useEffect } from 'react';
import Grid from '@mui/material/Grid';
import {
    Page,
    reportSelectorHelpers,
    Loading,
    ReportTable,
    BackButton
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation } from 'react-router-dom';
import queryString from 'query-string';
import history from '../../history';
import config from '../../config';
import unacknowledgedOrdersReportActions from '../../actions/unacknowledgedOrdersReportActions';

function UnacknowledgedOrdersReport() {
    const options = useLocation();
    const reportLoading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.unacknowledgedOrdersReport)
    );
    const reportData = useSelector(state =>
        reportSelectorHelpers.getReportData(state.unacknowledgedOrdersReport)
    );

    const dispatch = useDispatch();
    useEffect(() => {
        if (options.search) {
            dispatch(
                unacknowledgedOrdersReportActions.fetchReport(queryString.parse(options.search))
            );
        }
    }, [dispatch, options]);

    const handleBackClick = () => {
        const uri = '/purchasing/reports/suppliers-with-unacknowledged-orders';
        history.push(uri);
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container>
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
                <Grid item xs={12}>
                    {reportLoading ? (
                        <Loading />
                    ) : (
                        <ReportTable
                            reportData={reportData}
                            title={reportData?.title}
                            showTitle
                            showTotals={false}
                            placeholderRows={4}
                            placeholderColumns={2}
                        />
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default UnacknowledgedOrdersReport;
