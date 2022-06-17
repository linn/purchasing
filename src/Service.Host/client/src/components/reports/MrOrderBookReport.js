import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    Typeahead,
    collectionSelectorHelpers,
    Loading,
    MultiReportTable
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import suppliersActions from '../../actions/suppliersActions';
import { suppliers } from '../../itemTypes';
import { mrOrderBookReport } from '../../reportTypes';
import mrOrderBookReportActions from '../../actions/mrOrderBookReportActions';

function MrOrderBookReport() {
    const dispatch = useDispatch();
    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state[suppliers.item])
    )?.map(c => ({
        id: c.id,
        name: c.id.toString(),
        description: c.name
    }));
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );
    const [options, setOptions] = useState({
        supplierId: 38577
    });
    const loading = useSelector(state => state[mrOrderBookReport.item]?.loading);

    const reportData = useSelector(state => state[mrOrderBookReport.item]?.data);
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="MR Order Book" />
                </Grid>
                <Grid item xs={12}>
                    <Typeahead
                        label="Supplier (leave blank for all)"
                        title="Search for a supplier"
                        onSelect={newValue => setOptions({ ...options, supplierId: newValue.id })}
                        items={suppliersSearchResults}
                        loading={suppliersSearchLoading}
                        fetchItems={searchTerm => dispatch(suppliersActions.search(searchTerm))}
                        clearSearch={() => dispatch(suppliersActions.clearSearch)}
                        value={options.supplierId}
                        modal
                        links={false}
                        debounce={1000}
                        minimumSearchTermLength={2}
                    />
                </Grid>
                <Grid item xs={3}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={() => dispatch(mrOrderBookReportActions.fetchReport(options))}
                    >
                        Run Report
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    {loading ? (
                        <Loading />
                    ) : (
                        reportData && (
                            <MultiReportTable
                                reportData={reportData}
                                showTotals
                                placeholderRows={10}
                                placeholderColumns={3}
                                showRowTitles={false}
                                showTitle
                            />
                        )
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default MrOrderBookReport;
