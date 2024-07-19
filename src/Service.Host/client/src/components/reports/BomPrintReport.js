import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    Loading,
    ExportButton,
    Search,
    collectionSelectorHelpers,
    MultiReportTable
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation } from 'react-router-dom';
import queryString from 'query-string';

import history from '../../history';
import config from '../../config';

import { bomPrintReport } from '../../reportTypes';
import bomPrintActions from '../../actions/bomPrintActions';
import partsActions from '../../actions/partsActions';
import { parts } from '../../itemTypes';

function PrintBomReport() {
    const dispatch = useDispatch();
    const [searchTerm, setSearchTerm] = useState();

    const loading = useSelector(state => state[bomPrintReport.item]?.loading);
    const { search } = useLocation();
    const { bomName } = queryString.parse(search);

    const [prevBomName, setPrevBomName] = useState();
    if (bomName && bomName !== prevBomName) {
        setPrevBomName(bomName);
        setSearchTerm(bomName);
        dispatch(
            bomPrintActions.fetchReport({
                bomName
            })
        );
    }

    const reportData = useSelector(state => state[bomPrintReport.item]?.data);
    console.log(reportData);
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
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Print Bom" />
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
                        onClick={() =>
                            history.push(`/purchasing/boms/reports/bom-print?bomName=${searchTerm}`)
                        }
                    >
                        Run
                    </Button>
                </Grid>
                <Grid item xs={9} />
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        {reportData && (
                            <>
                                <Grid item xs={12}>
                                    <ExportButton
                                        href={`${config.appRoot}/purchasing/boms/reports/bom-print?bomName=${searchTerm}`}
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <MultiReportTable
                                        reportData={reportData}
                                        showTotals={false}
                                        showRowTitles={false}
                                        showTitle
                                        placeholderRows={4}
                                        placeholderColumns={4}
                                    />
                                </Grid>
                            </>
                        )}
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default PrintBomReport;
