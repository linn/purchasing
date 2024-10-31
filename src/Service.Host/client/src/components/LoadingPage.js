import React from 'react';
import { Loading, Page } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import history from '../history';
import config from '../config';

function LoadingPage() {
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Loading />
                </Grid>
            </Grid>
        </Page>
    );
}

export default LoadingPage;
