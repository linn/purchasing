import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import Grid from '@mui/material/Grid';
import { Page, Search, collectionSelectorHelpers } from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import { purchaseOrders } from '../../itemTypes';

function CreateNote() {
    const [roSearchTerm, setRoSearchTerm] = useState('');
    const dispatch = useDispatch();
    const searchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state[purchaseOrders.item],
            100,
            'orderNumber',
            'orderNumber',
            'documentType'
        )
    );
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state[purchaseOrders.item])
    );
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Search
                        propertyName="purchaseOrder"
                        label="Returns Order"
                        value={roSearchTerm}
                        handleValueChange={(_, newVal) => setRoSearchTerm(newVal)}
                        search={() => dispatch(purchaseOrdersActions.search(roSearchTerm))}
                        searchResults={searchResults.map(s => ({
                            ...s,
                            name: s.orderNumber.toString(),
                            description: s.documentType?.description
                        }))}
                        loading={searchLoading}
                        resultsInModal
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={res => console.log(res)}
                        clearSearch={() => dispatch(purchaseOrdersActions.clearSearch)}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default CreateNote;
