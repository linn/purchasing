import React, { Fragment, useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    Loading,
    ReportTable,
    Search,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation } from 'react-router-dom';
import queryString from 'query-string';

import history from '../../history';
import config from '../../config';

import { bomCostReport } from '../../reportTypes';
import bomCostReportActions from '../../actions/bomCostReportActions';
import partsActions from '../../actions/partsActions';
import { parts } from '../../itemTypes';

function BomCostReport() {
    const dispatch = useDispatch();
    const [searchTerm, setSearchTerm] = useState();

    const loading = useSelector(state => state[bomCostReport.item]?.loading);
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

    const reportData = useSelector(state => state[bomCostReport.item].data);
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
                    <Title text="Bom Print" />
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
                            history.push(`/purchasing/boms/reports/cost?bomName=${searchTerm}`)
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
                        {reportData &&
                            reportData.map(d => (
                                <Fragment key={d.subAssembly}>
                                    <Grid item xs={12}>
                                        <ReportTable
                                            reportData={d.breakdown.reportResults[0]}
                                            title={d.subAssembly}
                                            showTitle
                                            placeholderRows={4}
                                            placeholderColumns={4}
                                        />
                                    </Grid>
                                </Fragment>
                            ))}
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default BomCostReport;
