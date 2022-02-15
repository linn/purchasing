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
import spendByPartActions from '../../actions/spendByPartActions';

function SpendByPartReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.spendByPartReport)
    );
    const reportData = useSelector(state =>
        reportSelectorHelpers.getReportData(state.spendByPartReport)
    );

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/reports/spend-by-part/`;
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(spendByPartActions.fetchReport(options));
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
                                `${config.appRoot}/purchasing/reports/spend-by-supplier/export` +
                                `?&vm=${options.vm}`
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
                        <ReportTable
                            reportData={reportData}
                            title={reportData.title}
                            showTitle
                            showTotals
                            placeholderRows={4}
                            placeholderColumns={4}
                        />
                    )}
                </Grid>
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
            </Grid>
        </>
    );
}

SpendByPartReport.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default SpendByPartReport;
