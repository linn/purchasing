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

function SpendByPartReportOptions() {
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
        reportSelectorHelpers.getReportOptions(state.spendByPartReport)
    );

    const dispatch = useDispatch();

    const defaultStartDate = new Date();
    defaultStartDate.setMonth(defaultStartDate.getMonth() - 1);

    const [supplier, setSupplier] = useState(
        prevOptions?.id
            ? { id: prevOptions.id, description: '' }
            : { id: '', description: 'click to set supplier' }
    );

    const handleSupplierChange = selectedsupplier => {
        setSupplier(selectedsupplier);
    };

    const handleClick = () =>
        history.push({
            pathname: `/purchasing/reports/spend-by-part/report`,
            search: `?id=${supplier.id}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot} width="s" title="Spend By Part">
            <Title text="Spend By Part" />
            <Grid container spacing={3} justifyContent="center">
                <Grid item xs={12}>
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

export default SpendByPartReportOptions;
