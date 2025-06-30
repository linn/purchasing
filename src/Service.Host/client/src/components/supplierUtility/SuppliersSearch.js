import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    collectionSelectorHelpers,
    CreateButton,
    Page,
    Search,
    utilities
} from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import suppliersActions from '../../actions/suppliersActions';
import history from '../../history';
import config from '../../config';

function SuppliersSearch() {
    const dispatch = useDispatch();
    const [searchTerm, setSearchTerm] = useState('');

    useEffect(() => {
        dispatch(suppliersActions.fetchState());
    }, [dispatch]);

    const searchSuppliers = term => dispatch(suppliersActions.search(term));
    const clearSearch = () => dispatch(suppliersActions.clearSearch());

    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'name', 'name')
    );
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );

    const item = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.suppliers)
    );

    const createUrl = utilities.getHref(item, 'create');

    const getClosedString = dateClosed =>
        dateClosed ? `closed: ${new Date(dateClosed).toLocaleDateString('en-GB')}` : '';

    return (
        <Page history={history} homeUrl={config.appRoot} title="Suppliers">
            <Grid container spacing={3}>
                <Grid item xs={10}>
                    <Typography variant="h3">Suppliers Utility</Typography>
                </Grid>
                <Grid item xs={2}>
                    <CreateButton createUrl={createUrl} disabled={!createUrl} />
                </Grid>
                <Grid item xs={12}>
                    <Search
                        propertyName="supplier"
                        label="Search for a Supplier"
                        value={searchTerm}
                        handleValueChange={(_, newVal) => setSearchTerm(newVal)}
                        search={searchSuppliers}
                        searchResults={searchResults.map(s => ({
                            ...s,
                            chips: [
                                {
                                    text: s.dateClosed
                                        ? `${getClosedString(s.dateClosed)}`
                                        : 'open',
                                    color: s.dateClosed ? 'red' : 'green'
                                }
                            ]
                        }))}
                        displayChips
                        loading={searchLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={res => history.push(utilities.getSelfHref(res))}
                        clearSearch={clearSearch}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default SuppliersSearch;
