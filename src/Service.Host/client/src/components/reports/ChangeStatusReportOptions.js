import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { InputField, Page, Title } from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';

function ChangeStatusReportOptions() {
    const [months, setMonths] = useState('3');
    const handleClick = () =>
        history.push({
            pathname: `/purchasing/reports/change-status/report`,
            search: `?months=${months}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot} width="s">
            <Title text="Change Status Report Options" />
            <Grid container spacing={3} justifyContent="center">
                <>
                    <Grid item xs={5}>
                        <InputField
                            propertyName="months"
                            label="Months"
                            type="number"
                            fullWidth
                            value={months}
                            onChange={(_, value) => setMonths(value)}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <Button color="primary" variant="contained" onClick={handleClick}>
                            Run Report
                        </Button>
                    </Grid>
                </>
            </Grid>
        </Page>
    );
}

export default ChangeStatusReportOptions;
