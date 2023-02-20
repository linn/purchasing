import React, { useState } from 'react';
import {
    Loading,
    ReportTable,
    BackButton,
    Page,
    Title,
    Search
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';

import Button from '@mui/material/Button';

import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import bomDifferenceReportActions from '../../actions/bomDifferenceReportActions';
import config from '../../config';
import { bomDifferenceReport } from '../../reportTypes';

function BomDifferenceReport() {
    const loading = useSelector(state => state[bomDifferenceReport.item]?.loading);
    const reportData = useSelector(state => state[bomDifferenceReport.item]?.data);

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
                        onResultSelect={selected => handleOptionChange('bom1', selected.partNumber)}
                        search={() => {}}
                        clearSearch={() => {}}
                        label="Bom 1"
                        searchResults={[]}
                        loading={false}
                    />
                </Grid>
                <Grid item xs={6}>
                    <Search
                        propertyName="bom2"
                        value={options.bom2}
                        resultsInModal
                        handleValueChange={handleOptionChange}
                        onResultSelect={selected => handleOptionChange('bom2', selected.partNumber)}
                        search={() => {}}
                        clearSearch={() => {}}
                        label="Bom 2"
                        searchResults={[]}
                        loading={false}
                    />
                </Grid>
                <Grid item xs={12}>
                    <Button
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
