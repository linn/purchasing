import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { itemSelectorHelpers, Page, Loading } from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { useParams } from 'react-router-dom';

import supplierActions from '../../actions/supplierActions';
import history from '../../history';
import config from '../../config';

function Supplier() {
    const dispatch = useDispatch();

    const { id } = useParams();
    const supplier = useSelector(state => itemSelectorHelpers.getItem(state.supplier));
    const supplierLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.supplier)
    );

    useEffect(() => {
        if (id) {
            dispatch(supplierActions.fetch(id));
        }
    }, [id, dispatch]);
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                {supplierLoading ? (
                    <>
                        <Grid item xs={12}>
                            <Loading />
                        </Grid>
                    </>
                ) : (
                    supplier && (
                        <>
                            <Grid item xs={12}>
                                <Typography variant="h3">Supplier</Typography>
                                <Typography variant="h3">{supplier.id}</Typography>
                            </Grid>
                        </>
                    )
                )}
            </Grid>
        </Page>
    );
}

export default Supplier;
