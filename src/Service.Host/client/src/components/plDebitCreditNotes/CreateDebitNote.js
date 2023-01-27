import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import Grid from '@mui/material/Grid';
import {
    Page,
    Search,
    collectionSelectorHelpers,
    InputField
} from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import partsActions from '../../actions/partsActions';
import { purchaseOrders, parts } from '../../itemTypes';

function CreateDebitNote() {
    const [note, setNote] = useState({});
    const dispatch = useDispatch();
    const poSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state[purchaseOrders.item],
            100,
            'orderNumber',
            'orderNumber',
            'documentType'
        )
    );
    const partsResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(
            state[parts.item],
            100,
            'partNumber',
            'partNumber',
            'description'
        )
    );
    const searchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state[purchaseOrders.item])
    );
    const partsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state[parts.item])
    );
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={6}>
                    <Search
                        propertyName="returnsOrder"
                        label="Returns Order"
                        value={note?.returnsOrderNumber}
                        handleValueChange={(_, newVal) =>
                            setNote(n => ({ ...n, returnsOrderNumber: newVal }))
                        }
                        search={() =>
                            dispatch(purchaseOrdersActions.search(note?.returnsOrderNumber))
                        }
                        searchResults={poSearchResults.map(s => ({
                            ...s,
                            name: s.orderNumber.toString(),
                            description: s.documentType?.description
                        }))}
                        loading={searchLoading}
                        resultsInModal
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={res =>
                            setNote(n => ({
                                ...n,
                                returnsOrderNumber: res.orderNumber,
                                partNumber: res.details[0]?.partNumber,
                                orderQty: res.details[0]?.ourQty
                            }))
                        }
                        clearSearch={() => dispatch(purchaseOrdersActions.clearSearch)}
                    />
                </Grid>
                <Grid item xs={6}>
                    <Search
                        propertyName="purchaseOrder"
                        label="Orig Order Number"
                        value={note.originalOrderNumber}
                        handleValueChange={(_, newVal) =>
                            setNote(n => ({ ...n, originalOrderNumber: newVal }))
                        }
                        search={() =>
                            dispatch(purchaseOrdersActions.search(note.originalOrderNumber))
                        }
                        searchResults={poSearchResults.map(s => ({
                            ...s,
                            name: s.orderNumber.toString(),
                            description: s.documentType?.description
                        }))}
                        loading={searchLoading}
                        resultsInModal
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={res =>
                            setNote(n => ({
                                ...n,
                                originalOrderNumber: res.orderNumber
                            }))
                        }
                        clearSearch={() => dispatch(purchaseOrdersActions.clearSearch)}
                    />
                </Grid>
                <Grid item xs={4}>
                    <Search
                        propertyName="partNumber"
                        label="Part"
                        value={note.partNumber}
                        handleValueChange={(_, newVal) =>
                            setNote(n => ({ ...n, partNumber: newVal }))
                        }
                        search={() => dispatch(partsActions.search(note.partNumber))}
                        searchResults={partsResults}
                        loading={partsSearchLoading}
                        resultsInModal
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={res => console.log(res)}
                        clearSearch={() => dispatch(partsActions.clearSearch)}
                    />
                </Grid>
                <Grid item xs={4}>
                    <InputField
                        value={note.orderQty}
                        propertyName="orderQty"
                        label="Qty"
                        onChange={(_, newVal) => setNote(n => ({ ...n, orderQty: newVal }))}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default CreateDebitNote;
