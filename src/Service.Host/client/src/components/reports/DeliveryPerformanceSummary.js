import React from 'react';
import { Title, Page } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import history from '../../history';
import config from '../../config';

function DeliveryPerformanceSummary() {
    return (
        <Page history={history} homeUrl={config.appRoot} width="s">
            <Title text="Delivery Performance" />
            <Grid container>
                <Grid item xs={12} />
            </Grid>
        </Page>
    );
}

export default DeliveryPerformanceSummary;
