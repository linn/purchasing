import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    Typeahead,
    collectionSelectorHelpers,
    reportSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import suppliersActions from '../../actions/suppliersActions';

function SupplierLeadTimesReportOptions() {
    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers)
    )?.map(c => ({
        id: c.id,
        name: c.id.toString(),
        description: c.name
    }));
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );
    const prevOptions = useSelector(state =>
        reportSelectorHelpers.getReportOptions(state.supplierLeadTimesReport)
    );

    const dispatch = useDispatch();

    const [supplier, setSupplier] = useState(
        prevOptions?.id
            ? { id: prevOptions.id, name: '' }
            : { id: '', name: 'click to set supplier' }
    );

    const handleSupplierChange = selectedsupplier => {
        setSupplier(selectedsupplier);
    };

    const handleClick = () =>
        history.push({
            pathname: `/purchasing/reports/leadtimes/report`,
            search: `?supplier=${supplier.id}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot} width="s">
            <Title text="Lead Times By Supplier" />
            <Grid container spacing={3} justifyContent="center">
                <Grid item xs={12} data-testid="supplierSearch">
                    <Typeahead
                        label="Supplier"
                        title="Search for a supplier"
                        onSelect={handleSupplierChange}
                        items={suppliersSearchResults}
                        loading={suppliersSearchLoading}
                        fetchItems={searchTerm => dispatch(suppliersActions.search(searchTerm))}
                        clearSearch={() => dispatch(suppliersActions.clearSearch)}
                        value={`${supplier?.id} - ${supplier?.description}`}
                        modal
                        links={false}
                        debounce={1000}
                        minimumSearchTermLength={2}
                    />
                </Grid>
                <Grid item xs={12}>
                    <Button color="primary" variant="contained" onClick={handleClick}>
                        Run Report
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default SupplierLeadTimesReportOptions;
