import React from 'react';
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

import plCreditDebitNotesActions from '../../actions/plCreditDebitNotesActions';
import history from '../../history';
import config from '../../config';

function Search() {
    const dispatch = useDispatch();

    const search = searchTerm => dispatch(plCreditDebitNotesActions.search(searchTerm));
    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state.plCreditDebitNotes,
            100,
            'noteNumber',
            'noteNumber',
            'supplierName'
        )
    );
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.plCreditDebitNotes)
    );

    const item = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.plCreditDebitNotes)
    );

    const createUrl = utilities.getHref(item, 'create');

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={10}>
                    <Typography variant="h3">PL Credit/Debit Notes Utility</Typography>
                </Grid>
                <Grid item xs={2}>
                    <CreateButton createUrl={createUrl} disabled={!createUrl} />
                </Grid>
                <Grid item xs={12}>
                    <Typeahead
                        items={searchResults}
                        fetchItems={search}
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

export default Search;
