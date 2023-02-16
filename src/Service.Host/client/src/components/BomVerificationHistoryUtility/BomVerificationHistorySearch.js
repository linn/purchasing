import React, { useState } from 'react';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import { useSelector, useDispatch } from 'react-redux';
import { Page, collectionSelectorHelpers, Search } from '@linn-it/linn-form-components-library';
import bomVerificationHistoryEntriesSearchActions from '../../actions/bomVerificationHistoryEntriesActions';
import history from '../../history';
import config from '../../config';
import { bomVerificationHistoryEntries as bomVerificationHistoryEntriesItemType } from '../../itemTypes';

function BomVerificationHistorySearch() {
    const dispatch = useDispatch();

    const [searchTerm, setSearchTerm] = useState('');

    const searchEntries = term => dispatch(bomVerificationHistoryEntriesSearchActions.search(term));
    const clearSearch = () => dispatch(bomVerificationHistoryEntriesSearchActions.clearSearch());

    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state[bomVerificationHistoryEntriesItemType.item],
            100,
            'tRef',
            'partNumber',
            'remarks'
        )
    );
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.bomVerificationHistoryEntries)
    );

    const goToSelectedEntry = selectedEntry => {
        history.push(`${bomVerificationHistoryEntriesItemType.uri}/${selectedEntry.tRef}`);
    };

    const goToCreate = () => {
        history.push(`${bomVerificationHistoryEntriesItemType.uri}/create`);
    };

    return (
        <Page history={history} style={{ paddingBottom: '20px' }} homeUrl={config.appRoot}>
            <Typography variant="h5" gutterBottom>
                Search Bom Verification History Entries
            </Typography>
            <Grid container spacing={2}>
                <Grid item xs={9}>
                    <Stack direction="row" spacing={2}>
                        <Search
                            propertyName="partNumber"
                            label="Select Entry"
                            resultsInModal
                            resultLimit={100}
                            value={searchTerm}
                            handleValueChange={(_, e) => setSearchTerm(e)}
                            search={searchEntries}
                            helperText="Press <ENTER> to search"
                            searchResults={searchResults}
                            loading={searchLoading}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={newValue => {
                                goToSelectedEntry(newValue);
                            }}
                            clearSearch={clearSearch}
                        />
                    </Stack>
                </Grid>
                <Grid item xs={3}>
                    <Button
                        variant="outlined"
                        onClick={goToCreate}
                        size="small"
                        style={{ marginBottom: '25px' }}
                    >
                        Create New Verification
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default BomVerificationHistorySearch;
