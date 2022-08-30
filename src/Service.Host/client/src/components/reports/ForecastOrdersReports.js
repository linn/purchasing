import React, { useState } from 'react';
import Grid from '@mui/material/Grid';
import { Page, Title, ExportButton, InputField } from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';

const ForecastOrdersReport = () => {
    const [supplierId, setSupplierId] = useState(140726);
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Forecast Orders Reports" />
                </Grid>
                <Grid item xs={6}>
                    <InputField
                        type="number"
                        propertyName="supplierId"
                        onChange={(_, newVal) => setSupplierId(newVal)}
                        value={supplierId}
                        label="Supplier Id"
                    />
                    <ExportButton
                        href={`${config.appRoot}/purchasing/reports/Monthly-forecast-orders/export?supplierId=${supplierId}`}
                        buttonText="Monthly"
                    />
                </Grid>
            </Grid>
        </Page>
    );
};

export default ForecastOrdersReport;
