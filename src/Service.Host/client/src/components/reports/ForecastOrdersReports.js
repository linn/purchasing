import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';

import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    ExportButton,
    Typeahead,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';
import suppliersActions from '../../actions/suppliersActions';

const ForecastOrdersReport = () => {
    const [supplierId, setSupplierId] = useState(140692);

    const dispatch = useDispatch();

    const searchSuppliers = searchTerm => dispatch(suppliersActions.search(searchTerm));
    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers, 100, 'id', 'id', 'name')
    );
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Forecast Orders Reports" />
                </Grid>
                <Grid item xs={6}>
                    <Grid item xs={6}>
                        <Typeahead
                            onSelect={newValue => setSupplierId(newValue.id)}
                            label="Enter a Supplier Id or Click the Magnifying glass to search"
                            modal
                            openModalOnClick={false}
                            handleFieldChange={(_, newValue) => setSupplierId(newValue)}
                            propertyName="supplierId"
                            items={searchResults}
                            value={supplierId}
                            loading={searchLoading}
                            fetchItems={searchSuppliers}
                            links={false}
                            title="Search Suppliers"
                            clearSearch={() => {}}
                            placeholder="Type supplierId or click search icon"
                            minimumSearchTermLength={3}
                        />
                    </Grid>
                    <ExportButton
                        disabled={!supplierId}
                        href={`${config.appRoot}/purchasing/reports/monthly-forecast-orders/export?supplierId=${supplierId}`}
                        buttonText="Monthly Forecast"
                    />
                    <ExportButton
                        disabled={!supplierId}
                        href={`${config.appRoot}/purchasing/reports/mr-order-book/export?supplierId=${supplierId}`}
                        buttonText="Order Book"
                    />
                </Grid>
            </Grid>
        </Page>
    );
};

export default ForecastOrdersReport;
