import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { collectionSelectorHelpers, Page } from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import partsActions from '../../actions/partsActions';
import history from '../../history';
import config from '../../config';
import Search from '../Search';

function BomUtilitySearch() {
    const dispatch = useDispatch();

    const [searchTerm, setSearchTerm] = useState('');

    const searchParts = term =>
        dispatch(partsActions.searchWithOptions('', `&partNumberSearchTerm=${term}`));
    const clearSearch = () => dispatch(partsActions.clearSearch());

    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state.parts,
            100,
            'id',
            'partNumber',
            'description'
        )
    );

    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.parts)
    );

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={10}>
                    <Typography variant="h3">Bom Utility</Typography>
                </Grid>
                <Grid item xs={12}>
                    <Search
                        propertyName="supplier"
                        label="Search for an Assembly or Phantom BOM type part. You can use an asterisk as a wildcard."
                        value={searchTerm}
                        handleValueChange={(_, newVal) => setSearchTerm(newVal)}
                        search={searchParts}
                        searchResults={searchResults}
                        loading={searchLoading}
                        priorityFunction={null}
                        onResultSelect={res =>
                            history.push(`/purchasing/boms/bom-utility?bomName=${res.partNumber}`, {
                                searchResults: searchResults?.map(s => s.partNumber)
                            })
                        }
                        clearSearch={clearSearch}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default BomUtilitySearch;
