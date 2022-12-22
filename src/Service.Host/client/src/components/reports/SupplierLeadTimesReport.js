import React, { useEffect, useMemo } from 'react';
import {
    Loading,
    BackButton,
    reportSelectorHelpers,
    ReportTable
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import supplierLeadTimesReportActions from '../../actions/supplierLeadTimesReportActions';

function SupplierLeadTimesReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.supplierLeadTimesReport)
    );
    const reportData = useSelector(state =>
        reportSelectorHelpers.getReportData(state.supplierLeadTimesReport)
    );

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/reports/leadtimes/`;
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(supplierLeadTimesReportActions.fetchReport(options));
        }
    }, [options, dispatch]);

    return (
        <>
            <Grid style={{ marginTop: 40 }} container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
                <Grid item xs={6}>
                    {loading || !reportData ? (
                        <Loading />
                    ) : (
                        <>
                            <ReportTable
                                reportData={reportData}
                                title={reportData.title}
                                showTitle
                                showTotals={false}
                                placeholderRows={4}
                                placeholderColumns={4}
                            />
                        </>
                    )}
                </Grid>
                <Grid item xs={6} />

                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
            </Grid>
        </>
    );
}
export default SupplierLeadTimesReport;
