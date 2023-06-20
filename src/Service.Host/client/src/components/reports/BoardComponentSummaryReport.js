import React, { useEffect, useMemo } from 'react';
import {
    Loading,
    ReportTable,
    BackButton,
    Page,
    Title
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import boardComponentSummaryReportActions from '../../actions/boardComponentSummaryReportActions';
import config from '../../config';
import { boardComponentSummaryReport } from '../../reportTypes';

function BoardComponentSummaryReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state => state[boardComponentSummaryReport.item]?.loading);
    const reportData = useSelector(state => state[boardComponentSummaryReport.item]?.data);

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/boms/reports/board-component-summary`;
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(boardComponentSummaryReportActions.fetchReport(options));
        }
    }, [options, dispatch]);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Board Component Summary Report" />
            <Grid container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
                <Grid item xs={12}>
                    {loading ? (
                        <Loading />
                    ) : (
                        reportData && (
                            <ReportTable
                                reportData={reportData}
                                showTitle
                                showTotals={false}
                                showRowCount
                                placeholderRows={4}
                                placeholderColumns={4}
                            />
                        )
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default BoardComponentSummaryReport;
