import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { collectionSelectorHelpers, itemSelectorHelpers, Page, Typeahead } from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';

import supplierActions from '../../actions/supplierActions';
import history from '../../history';
import config from '../../config';

function Supplier() {
    const dispatch = useDispatch();

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h3">Supplier</Typography>
                </Grid>
            </Grid>
        </Page>
    );
}

export default Supplier;
