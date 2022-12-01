import React from 'react';
import { Page } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import history from '../../history';

function CreateChangeRequest() {
    const create = () => {
        console.log('Create');
    };

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={12}>
                    <Typography variant="h3">Raise Change Request</Typography>
                </Grid>
                create
                <Grid item xs={12}>
                    <>hey</>
                </Grid>
                <Grid item xs={2}>
                    <Button variant="outlined" color="primary" onClick={create}>
                        Create
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default CreateChangeRequest;
