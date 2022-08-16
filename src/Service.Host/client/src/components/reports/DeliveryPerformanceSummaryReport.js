import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation } from 'react-router';
import {
    Title,
    Page,
    ReportTable,
    BackButton,
    Loading,
    reportSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import history from '../../history';
import config from '../../config';
import deliveryPerformanceSummaryReportActions from '../../actions/deliveryPerformanceSummaryReportActions';

function DeliveryPerformanceSummaryReport() {
    const dispatch = useDispatch();
    const options = useLocation();

    useEffect(() => {
        if (options && options.state) {
            dispatch(deliveryPerformanceSummaryReportActions.fetchReport(options.state));
        }
    }, [dispatch, options]);

    const loading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.deliveryPerformanceSummaryReport)
    );
    const data = useSelector(state =>
        reportSelectorHelpers.getReportData(state.deliveryPerformanceSummaryReport)
    );

    const handleBackClick = () => {
        history.push('/purchasing/reports/delivery-performance-summary');
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Delivery Performance Report" />
            <Grid container>
                <Grid item xs={12}>
                    {loading || !data ? (
                        <Loading />
                    ) : (
                        <>
                            <ReportTable
                                reportData={data}
                                title={data.title}
                                showTitle={false}
                                showTotals
                                showRowTitles={false}
                                placeholderRows={4}
                                placeholderColumns={4}
                            />
                        </>
                    )}
                </Grid>
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick()} />
                </Grid>
            </Grid>
        </Page>
    );
}

export default DeliveryPerformanceSummaryReport;
