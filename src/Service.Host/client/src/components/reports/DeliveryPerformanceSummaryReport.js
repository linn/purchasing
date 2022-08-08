import React from 'react';
import { Title, Page } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import history from '../../history';
import config from '../../config';

function DeliveryPerformanceSummaryReport() {
    return (
        <Page history={history} homeUrl={config.appRoot} width="s">
            <Title text="Delivery Performance Report" />
            <Grid container>
                <Grid item xs={12} />
            </Grid>
        </Page>
    );
}

export default DeliveryPerformanceSummaryReport;
