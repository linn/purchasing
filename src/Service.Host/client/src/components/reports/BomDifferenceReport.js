import React, { useState } from 'react';
import {
    Loading,
    ReportTable,
    Page,
    Title,
    Search,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';

import Button from '@mui/material/Button';

import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import bomDifferenceReportActions from '../../actions/bomDifferenceReportActions';
import config from '../../config';
import { bomDifferenceReport } from '../../reportTypes';
import partsActions from '../../actions/partsActions';
import { parts } from '../../itemTypes';

function BomDifferenceReport() {
    const loading = useSelector(state => state[bomDifferenceReport.item]?.loading);
    const reportData = useSelector(state => state[bomDifferenceReport.item]?.data);

    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state[parts.item])
    )
        .map?.(c => ({
            id: c.partNumber,
            name: c.partNumber,
            description: c.description,
            type: c.bomType
        }))
        .filter(p => p.type !== 'C');

    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state[parts.item])
    );

    const [options, setOptions] = useState({});

    const handleOptionChange = (propName, newVal) =>
        setOptions(o => ({ ...o, [propName]: newVal }));

    const dispatch = useDispatch();

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Bom Difference" />
            <Grid container spacing={3} justifyContent="center">
                <Grid item xs={6}>
                    <Search
                        propertyName="bom1"
                        value={options.bom1}
                        resultsInModal
                        handleValueChange={handleOptionChange}
                        onResultSelect={selected => handleOptionChange('bom1', selected.name)}
                        search={() => {
                            dispatch(partsActions.search(options.bom1));
                        }}
                        clearSearch={() => {
                            dispatch(partsActions.clearSearch());
                        }}
                        label="Bom 1"
                        searchResults={searchResults}
                        loading={searchLoading}
                    />
                </Grid>
                <Grid item xs={6}>
                    <Search
                        propertyName="bom2"
                        value={options.bom2}
                        resultsInModal
                        handleValueChange={handleOptionChange}
                        onResultSelect={selected => handleOptionChange('bom2', selected.name)}
                        search={() => {
                            dispatch(partsActions.search(options.bom2));
                        }}
                        clearSearch={() => {
                            dispatch(partsActions.clearSearch());
                        }}
                        label="Bom 2"
                        searchResults={searchResults}
                        loading={searchLoading}
                    />
                </Grid>
                <Grid item xs={12}>
                    <Button
                        variant="contained"
                        onClick={() => dispatch(bomDifferenceReportActions.fetchReport(options))}
                    >
                        RUN
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    {loading ? (
                        <Loading />
                    ) : (
                        reportData && (
                            <ReportTable
                                reportData={reportData}
                                showTitle
                                title={reportData?.title}
                                showTotals={false}
                                placeholderRows={4}
                                placeholderColumns={4}
                            />
                        )
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default BomDifferenceReport;
