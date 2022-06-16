import React, { useEffect, useMemo } from 'react';
import {
    Loading,
    ReportTable,
    BackButton,
    reportSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import shortagesReportActions from '../../actions/shortagesReportActions';

function ShortagesReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.shortagesReport)
    );
    const reportData = useSelector(state =>
        reportSelectorHelpers.getReportData(state.shortagesReport)
    );

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/reports/shortages/`;
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(shortagesReportActions.fetchReport(options));
        }
    }, [options, dispatch]);

    return (
        <>
            <Grid style={{ marginTop: 40 }} container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
                <Grid item xs={12}>
                    {loading || !reportData ? (
                        <Loading />
                    ) : (
                        <>
                            {reportData.map(report => (
                                <ReportTable
                                    reportData={report.reportResults[0]}
                                    title={report.reportResults[0].title}
                                    key={report.reportResults[0].PlannerName}
                                    showTitle
                                    showTotals
                                    placeholderRows={4}
                                    placeholderColumns={4}
                                />
                            ))}
                        </>
                    )}
                </Grid>
            </Grid>
        </>
    );
}

ShortagesReport.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default ShortagesReport;