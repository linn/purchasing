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
import queryString from 'query-string';
import Grid from '@mui/material/Grid';
import history from '../../history';
import config from '../../config';
import deliveryPerformanceSupplierReportActions from '../../actions/deliveryPerformanceSupplierReportActions';

function DeliveryPerformanceSupplierReport() {
    const dispatch = useDispatch();
    const options = useLocation();

    useEffect(() => {
        if (options && options.search) {
            const query = queryString.parse(options.search);
            dispatch(deliveryPerformanceSupplierReportActions.fetchReport(query));
        }
    }, [dispatch, options]);

    const loading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.deliveryPerformanceSupplierReport)
    );
    const data = useSelector(state =>
        reportSelectorHelpers.getReportData(state.deliveryPerformanceSupplierReport)
    );

    const handleBackClick = () => {
        history.push('/purchasing/reports/delivery-performance-summary/report');
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container>
                <Grid item xs={12}>
                    {loading || !data ? (
                        <Loading />
                    ) : (
                        <>
                            <ReportTable
                                reportData={data}
                                title={data.title}
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

export default DeliveryPerformanceSupplierReport;
