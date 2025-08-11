import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { Page, Title, InputField } from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';

function BoardComponentSummaryReportOptions() {
    const [boardCode, setBoardCode] = useState(null);
    const [revisionCode, setRevisionCode] = useState('L1R1');

    const handleClick = () =>
        history.push({
            pathname: `/purchasing/boms/reports/board-component-summary/report`,
            search: `?boardCode=${boardCode}&revisionCode=${revisionCode}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot} title="Component Summary">
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Board Component Summary Report" />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        propertyName="boardCode"
                        label="Board Code"
                        value={boardCode}
                        onChange={(_, value) => setBoardCode(value)}
                    />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        propertyName="revisionCode"
                        label="Revision Code"
                        value={revisionCode}
                        onChange={(_, value) => setRevisionCode(value)}
                    />
                </Grid>
                <Grid item xs={2} />
                <Grid item xs={3}>
                    <Button
                        variant="contained"
                        color="primary"
                        disabled={!boardCode || !revisionCode}
                        onClick={handleClick}
                    >
                        Run Report
                    </Button>
                </Grid>
                <Grid item xs={9} />
            </Grid>
        </Page>
    );
}

export default BoardComponentSummaryReportOptions;
