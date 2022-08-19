import React, { useEffect, useMemo } from 'react';
import {
    Loading,
    ReportTable,
    BackButton,
    ExportButton,
    reportSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import config from '../../config';
import spendBySupplierByDateRangeActions from '../../actions/spendBySupplierByDateRangeActions';

function SpendBySupplierByDateRangeReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.spendBySupplierByDateRangeReport)
    );
    const reportData = useSelector(state =>
        reportSelectorHelpers.getReportData(state.spendBySupplierByDateRangeReport)
    );

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/reports/spend-by-supplier-by-date-range/`;
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(spendBySupplierByDateRangeActions.fetchReport(options));
        }
    }, [options, dispatch]);

    return (
        <>
            <Grid style={{ marginTop: 40 }} container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
                <Grid item xs={12}>
                    {!loading && reportData ? (
                        <ExportButton
                            href={
                                `${config.appRoot}/purchasing/reports/spend-by-supplier-by-date-range/export` +
                                `?&vm=${options.vm}` +
                                `&fromDate=${options.fromDate}` +
                                `&toDate=${options.toDate}`
                            }
                        />
                    ) : (
                        <></>
                    )}
                </Grid>
                <Grid item xs={12}>
                    {loading || !reportData ? (
                        <Loading />
                    ) : (
                        <>
                            <ReportTable
                                reportData={reportData}
                                title={reportData.title}
                                showTitle
                                showTotals
                                showRowTitles
                                placeholderRows={4}
                                placeholderColumns={4}
                            />
                            <p>Total number of suppliers: {reportData?.results?.length}</p>
                        </>
                    )}
                </Grid>
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
            </Grid>
        </>
    );
}

export default SpendBySupplierByDateRangeReport;
