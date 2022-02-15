import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    collectionSelectorHelpers,
    CreateButton,
    Page,
    Typeahead,
    utilities
} from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';

import suppliersActions from '../../actions/suppliersActions';
import history from '../../history';
import config from '../../config';

function SuppliersSearch() {
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(suppliersActions.fetchState());
    }, [dispatch]);

    const searchSuppliers = searchTerm => dispatch(suppliersActions.search(searchTerm));
    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'id', 'name')
    );
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );

    const item = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.suppliers)
    );

    const createUrl = utilities.getHref(item, 'create');

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={10}>
                    <Typography variant="h3">Suppliers Utility</Typography>
                </Grid>
                <Grid item xs={2}>
                    <CreateButton createUrl={createUrl} disabled={!createUrl} />
                </Grid>
                <Grid item xs={12}>
                    <Typeahead
                        items={searchResults}
                        fetchItems={searchSuppliers}
                        clearSearch={() => {}}
                        resultLimit={100}
                        loading={searchLoading}
                        history={history}
                        links
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default SuppliersSearch;
