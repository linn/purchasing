import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    collectionSelectorHelpers,
    Page,
    Typeahead,
    utilities
} from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import history from '../../history';
import config from '../../config';

function OverbooksSearch() {
    const dispatch = useDispatch();
    useEffect(() => dispatch(purchaseOrdersActions.fetch()), [dispatch]);
    useEffect(() => dispatch(purchaseOrdersActions.fetchState()), [dispatch]);

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

    const item = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.purchaseOrders)
    );
    const canSearch = utilities.getHref(item, 'allow-over-book-search');
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.purchaseOrders)
    );

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={11}>
                    <Typography variant="h3">Allow Overbook UT</Typography>
                </Grid>
                <Grid item xs={1}>
                    {canSearch ? (
                        <Tooltip title="You have write access to allow overbooking">
                            <ModeEditIcon fontSize="large" color="primary" />
                        </Tooltip>
                    ) : (
                        <Tooltip title="You do not have write access to allow overbooking">
                            <EditOffIcon fontSize="large" color="secondary" />
                        </Tooltip>
                    )}
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
                        disabled={!canSearch}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default OverbooksSearch;
