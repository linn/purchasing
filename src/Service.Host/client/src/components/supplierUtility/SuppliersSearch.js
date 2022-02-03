import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { collectionSelectorHelpers, Page, Typeahead } from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';

import suppliersActions from '../../actions/suppliersActions';
import history from '../../history';
import config from '../../config';

function SuppliersSearch() {
    const dispatch = useDispatch();

    const searchSuppliers = searchTerm => dispatch(suppliersActions.search(searchTerm));
    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'id', 'name')
    );
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h3">Suppliers Utility</Typography>
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
