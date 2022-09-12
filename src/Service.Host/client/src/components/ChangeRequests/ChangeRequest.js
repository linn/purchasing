import React, { useState } from 'react';
import { Page, InputField } from '@linn-it/linn-form-components-library';
import { makeStyles } from '@mui/styles';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';

import history from '../../history';

function ChangeRequest() {
    const [documentNumber, setDocumentNumber] = useState('');

    const useStyles = makeStyles(theme => ({
        button: {
            marginLeft: theme.spacing(1),
            marginTop: theme.spacing(4)
        },
        a: {
            textDecoration: 'none'
        }
    }));
    const classes = useStyles();

    const search = () => history.push(`/purchasing/change-requests/${documentNumber}/`);

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={11}>
                    <Typography variant="h3">Search Change Request</Typography>
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        fullWidth
                        placeholder="Go to Change Request"
                        value={documentNumber}
                        label="Change Request"
                        propertyName="changeRequest"
                        onChange={(_, newValue) => setDocumentNumber(newValue)}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        color="primary"
                        className={classes.button}
                        onClick={search}
                    >
                        Go
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default ChangeRequest;
