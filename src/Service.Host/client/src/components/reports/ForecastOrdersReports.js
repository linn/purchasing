import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { Page, Title, Loading } from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';
import DownloadButton from './DownloadButton';

const ForecastOrdersReport = () => (
    <Page history={history} homeUrl={config.appRoot}>
        <Grid container spacing={3}>
            <Grid item xs={12}>
                <DownloadButton
                    endpoint={`${config.appRoot}/purchasing/reports/weekly-forecast-orders/export?supplierId=120674`}
                    fileName="test.csv"
                />
            </Grid>
        </Grid>
    </Page>
);

export default ForecastOrdersReport;
