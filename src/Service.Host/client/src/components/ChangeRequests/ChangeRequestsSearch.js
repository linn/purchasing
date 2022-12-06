import React, { useState } from 'react';
import { Page, InputField } from '@linn-it/linn-form-components-library';
import { makeStyles } from '@mui/styles';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';

import history from '../../history';

function ChangeRequestSearch() {
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

    const create = () => history.push('/purchasing/change-requests/create');

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={10}>
                    <Typography variant="h3">Search Change Request</Typography>
                </Grid>
                <Grid item xs={2}>
                    <Button variant="outlined" className={classes.button} onClick={create}>
                        Create
                    </Button>
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

export default ChangeRequestSearch;
