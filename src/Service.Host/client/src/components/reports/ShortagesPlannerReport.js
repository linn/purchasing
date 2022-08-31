import React, { useEffect, useMemo } from 'react';
import {
    Loading,
    MultiReportTable,
    BackButton,
    Page,
    Title
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import shortagesPlannerReportActions from '../../actions/shortagesPlannerReportActions';
import { shortagesPlannerReport } from '../../reportTypes';
import config from '../../config';

function ShortagesPlannerReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state => state[shortagesPlannerReport.item]?.results.loading);

    const reportData = useSelector(state => state[shortagesPlannerReport.item]?.results.data);
    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = '/purchasing/reports/shortages/';
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(shortagesPlannerReportActions.fetchReport(options));
        }
    }, [options, dispatch]);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Part Shortages" />
            <Grid container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
                <Grid item xs={12}>
                    {loading || !reportData ? (
                        <Loading />
                    ) : (
                        reportData && (
                            <MultiReportTable
                                reportData={reportData}
                                showTitle
                                showRowTitles={false}
                                showTotals={false}
                                placeholderRows={10}
                                placeholderColumns={4}
                            />
                        )
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

ShortagesPlannerReport.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default ShortagesPlannerReport;
