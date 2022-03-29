import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { collectionSelectorHelpers, Page, Typeahead } from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';

import purchaseOrdersActions from '../actions/purchaseOrdersActions';
import history from '../history';
import config from '../config';

function OverbooksSearch() {
    const dispatch = useDispatch();

    const searchOverbookItems = searchTerm => dispatch(purchaseOrdersActions.search(searchTerm));
    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state.purchaseOrders,
            100,
            'orderNumber',
            'orderNumber',
            'orderNumber'
        )
    );

    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.purchaseOrders)
    );

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h3">Allow Overbook UT</Typography>
                </Grid>
                <Grid item xs={12}>
                    <Typeahead
                        items={searchResults.map(x => ({
                            ...x,
                            href: x.links.find(l => l.rel === 'allow-over-book')?.href
                        }))}
                        fetchItems={searchOverbookItems}
                        placeholder="Search by Order Number"
                        clearSearch={() => {}}
                        resultLimit={100}
                        loading={searchLoading}
                        history={history}
                        links
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default OverbooksSearch;
