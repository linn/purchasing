import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    Search,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation } from 'react-router-dom';
import queryString from 'query-string';
import history from '../../history';
import config from '../../config';
import bomCostReportActions from '../../actions/bomCostReportActions';
import partsActions from '../../actions/partsActions';
import { parts } from '../../itemTypes';

function BomCostReportOptions() {
    const dispatch = useDispatch();
    const [searchTerm, setSearchTerm] = useState();

    const { search } = useLocation();
    const { bomName } = queryString.parse(search);

    const [prevBomName, setPrevBomName] = useState();
    if (bomName && bomName !== prevBomName) {
        setPrevBomName(bomName);
        setSearchTerm(bomName);
        dispatch(
            bomCostReportActions.fetchReport({
                bomName,
                splitBySubAssembly: true,
                levels: 999,
                labourHourlyRate: 15
            })
        );
    }
    const partsSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(
            reduxState[parts.item],
            100,
            'id',
            'partNumber',
            'description'
        )
    );
    const partsSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState[parts.item])
    );
    return (
        <Page history={history} homeUrl={config.appRoot} title="BOM Cost">
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Bom Cost Report" />
                </Grid>
                <Grid item xs={12}>
                    <Search
                        propertyName="searchTerm"
                        label="Bom Name"
                        resultsInModal
                        resultLimit={100}
                        value={searchTerm}
                        handleValueChange={(_, newVal) => setSearchTerm(newVal)}
                        search={partNumber => {
                            dispatch(partsActions.search(partNumber));
                        }}
                        searchResults={partsSearchResults}
                        helperText="Enter a value. Press the enter key if you want to search parts."
                        loading={partsSearchLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={newValue => {
                            setSearchTerm(newValue.partNumber);
                        }}
                        clearSearch={() => {}}
                    />
                </Grid>
                <Grid item xs={3}>
                    <Button
                        variant="contained"
                        color="primary"
                        disabled={!searchTerm}
                        onClick={() =>
                            history.push(`/purchasing/boms/reports/cost?bomName=${searchTerm}`)
                        }
                    >
                        Run
                    </Button>
                </Grid>
                <Grid item xs={9} />
            </Grid>
        </Page>
    );
}

export default BomCostReportOptions;
