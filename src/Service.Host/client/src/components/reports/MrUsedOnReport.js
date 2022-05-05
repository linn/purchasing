import React, { useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import queryString from 'query-string';

import { Page, Loading, ReportTable, Title } from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import { mrusedOnReport } from '../../reportTypes';
import mrUsedOnReportActions from '../../actions/mrUsedOnReportActions';

function MrUsedOnReport() {
    const dispatch = useDispatch();

    const location = useLocation();
    const partNumber = queryString.parse(location.search)?.partNumber;
    useEffect(() => {
        dispatch(mrUsedOnReportActions.fetchReport({ partNumber }));
    }, [partNumber, dispatch]);

    const loading = useSelector(state => state[mrusedOnReport.item]?.loading);

    const reportData = useSelector(state => state[mrusedOnReport.item]?.data);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Mr Used On Report" />
                </Grid>
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <Grid item xs={12}>
                        <ReportTable
                            reportData={reportData}
                            title={reportData.title}
                            showTitle
                            showTotals
                            placeholderRows={4}
                            placeholderColumns={4}
                        />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default MrUsedOnReport;
