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
import boardDifferenceReportActions from '../../actions/boardDifferenceReportActions';
import config from '../../config';
import { boardDifferenceReport } from '../../reportTypes';

function BoardDifferenceReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state => state[boardDifferenceReport.item]?.loading);
    const reportData = useSelector(state => state[boardDifferenceReport.item]?.data);

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/reports/board-difference`;
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(boardDifferenceReportActions.fetchReport(options));
        }
    }, [options, dispatch]);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Board Difference" />
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
                                title={reportData?.title}
                                showTotals={false}
                                showRowCount
                                showRowTitles
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

export default BoardDifferenceReport;
