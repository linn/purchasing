import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { Page, Title, InputField } from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';

function BoardDifferenceReportOptions() {
    const [boardCode1, setBoardCode1] = useState(null);
    const [revisionCode1, setRevisionCode1] = useState(null);
    const [boardCode2, setBoardCode2] = useState(null);
    const [revisionCode2, setRevisionCode2] = useState(null);

    const handleClick = () =>
        history.push({
            pathname: `/purchasing/reports/board-difference/report`,
            search: `?boardCode1=${boardCode1}&revisionCode1=${revisionCode1}&boardCode2=${boardCode2}&revisionCode2=${revisionCode2}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Board Difference Report" />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        propertyName="boardCode1"
                        label="Board Code"
                        value={boardCode1}
                        onChange={(_, value) => setBoardCode1(value)}
                    />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        propertyName="revisionCode1"
                        label="Revision Code"
                        value={revisionCode1}
                        onChange={(_, value) => setRevisionCode1(value)}
                    />
                </Grid>
                <Grid item xs={2} />
                <Grid item xs={5}>
                    <InputField
                        propertyName="boardCode2"
                        label="Board Code"
                        value={boardCode2}
                        onChange={(_, value) => setBoardCode2(value)}
                    />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        propertyName="revisionCode2"
                        label="Revision Code"
                        value={revisionCode2}
                        onChange={(_, value) => setRevisionCode2(value)}
                    />
                </Grid>
                <Grid item xs={2} />
                <Grid item xs={3}>
                    <Button
                        variant="contained"
                        color="primary"
                        disabled={!boardCode1 || !boardCode2 || !revisionCode1 || !revisionCode2}
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

export default BoardDifferenceReportOptions;
