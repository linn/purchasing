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
import { shortagesReport } from '../../reportTypes';
import shortagesReportActions from '../../actions/shortagesReportActions';
import config from '../../config';

function ShortagesReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state => state[shortagesReport.item]?.results.loading);

    const reportData = useSelector(state => state[shortagesReport.item]?.results.data);

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
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Shortages By Vendor Manager" />
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
                                showTotals
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

ShortagesReport.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default ShortagesReport;
