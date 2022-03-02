import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    collectionSelectorHelpers,
    CreateButton,
    Page,
    TypeaheadTable,
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

    const searchResultsTable = {
        totalItemCount: searchResults.length,
        rows: searchResults?.map((res, i) => ({
            id: res.noteNumber,
            values: [
                { id: `${i}-0`, value: `${res.noteNumber}` },
                { id: `${i}-1`, value: `${res.noteType}` },
                { id: `${i}-2`, value: `${res.originalOrderNumber || ''}` },
                { id: `${i}-3`, value: `${res.supplierId || ''}` },
                { id: `${i}-4`, value: `${res.supplierName || ''}` },
                { id: `${i}-5`, value: `${res.partNumber || ''}` }
            ],
            links: res.links
        }))
    };

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
                    <TypeaheadTable
                        table={searchResultsTable}
                        columnNames={['#', 'Type', 'Order', 'Supplier', 'Name', 'Part']}
                        links={false}
                        fetchItems={search}
                        placeholder="Note Number or Supplier"
                        onSelect={newVal => history.push(utilities.getSelfHref(newVal))}
                        clearSearch={() => {}}
                        loading={searchLoading}
                        debounce={1000}
                        minimumSearchTermLength={2}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default Search;
