import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import Grid from '@mui/material/Grid';
import {
    Page,
    Search,
    collectionSelectorHelpers,
    InputField,
    Dropdown,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import partsActions from '../../actions/partsActions';
import plCreditDebitNoteActions from '../../actions/plCreditDebitNoteActions';
import { purchaseOrders, parts } from '../../itemTypes';

function CreateDebitNote() {
    const [note, setNote] = useState({});
    const [order, setOrder] = useState({});
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
                <Grid item xs={3}>
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
                        onResultSelect={res => {
                            setNote(n => ({
                                ...n,
                                returnsOrderNumber: res.orderNumber,
                                returnsOrderLine: 1,
                                partNumber: res.details[0]?.partNumber,
                                orderQty: res.details[0]?.ourQty,
                                originalOrderNumber: res.details[0]?.originalOrderNumber,
                                originalOrderLine: res.details[0]?.originalOrderLine,
                                netTotal: res.details[0]?.netTotalCurrency,
                                vatTotal: res.details[0]?.vatTotalCurrency,
                                total: res.details[0]?.detailTotalCurrency,
                                orderUnitOfMeasure: res.details[0]?.ourUnitOfMeasure,
                                orderUnitPrice: res.details[0]?.orderUnitPriceCurrency,
                                currency: res.currency?.code,
                                supplierId: res.supplier?.id
                            }));
                            setOrder(res);
                        }}
                        clearSearch={() => dispatch(purchaseOrdersActions.clearSearch)}
                    />
                </Grid>
                <Grid item xs={1}>
                    <InputField
                        label="Line"
                        value={note?.returnsOrderLine}
                        propertyName="returnsOrderLine"
                        disabled={!order}
                        type="number"
                        onChange={(_, newVal) => {
                            const line = order.details?.find(d => d.line === newVal);
                            setNote(n => ({
                                ...n,
                                returnsOrderLine: newVal,
                                partNumber: line?.partNumber,
                                orderQty: line?.ourQty,
                                originalOrderNumber: line?.originalOrderNumber,
                                originalOrderLine: line?.originalOrderLine,
                                netTotal: line?.netTotalCurrency,
                                vatTotal: line?.vatTotalCurrency,
                                total: line?.detailTotalCurrency,
                                orderUnitOfMeasure: line?.ourUnitOfMeasure,
                                orderUnitPrice: line?.orderUnitPriceCurrency
                            }));
                        }}
                    />
                </Grid>
                <Grid iten xs={8} />

                <Grid item xs={3}>
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
                <Grid item xs={1}>
                    <InputField
                        label="Line"
                        value={note?.originalOrderLine}
                        propertyName="originalOrderLine"
                        type="number"
                        onChange={(_, newVal) => {
                            setNote(n => ({
                                ...n,
                                originalOrderLine: newVal
                            }));
                        }}
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        value={note.supplierId}
                        propertyName="supplierId"
                        label="Supplier"
                        onChange={() => {}}
                    />
                </Grid>
                <Grid iten xs={5} />
                <Grid item xs={3}>
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
                        onResultSelect={res => setNote(n => ({ ...n, partNumber: res.partNumber }))}
                        clearSearch={() => dispatch(partsActions.clearSearch)}
                    />
                </Grid>
                <Grid item xs={1}>
                    <InputField
                        value={note.orderQty}
                        propertyName="orderQty"
                        label="Qty"
                        type="number"
                        onChange={(_, newVal) => setNote(n => ({ ...n, orderQty: newVal }))}
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        value={note.orderUnitOfMeasure}
                        propertyName="orderUnitOfMeasure"
                        label="UoM"
                        onChange={(_, newVal) =>
                            setNote(n => ({ ...n, orderUnitOfMeasure: newVal }))
                        }
                    />
                </Grid>
                <Grid item xs={2}>
                    <InputField
                        value={note.orderUnitPrice}
                        propertyName="orderUnitPrice"
                        label="Unit Price"
                        type="number"
                        onChange={(_, newVal) => setNote(n => ({ ...n, orderUnitPrice: newVal }))}
                    />
                </Grid>
                <Grid item xs={2}>
                    <InputField
                        value={note.currency}
                        propertyName="currency"
                        label="Currency"
                        onChange={(_, newVal) => setNote(n => ({ ...n, currency: newVal }))}
                    />
                </Grid>
                <Grid item xs={2}>
                    <InputField
                        value={note.netTotal}
                        propertyName="netTotal"
                        label="Net"
                        type="number"
                        onChange={(_, newVal) => setNote(n => ({ ...n, netTotal: newVal }))}
                    />
                </Grid>
                <Grid item xs={2}>
                    <InputField
                        value={note.vatTotal}
                        propertyName="vatTotal"
                        label="VAT"
                        type="number"
                        onChange={(_, newVal) => setNote(n => ({ ...n, vatTotal: newVal }))}
                    />
                </Grid>
                <Grid item xs={2}>
                    <InputField
                        value={note.total}
                        propertyName="total"
                        label="Total"
                        type="number"
                        onChange={(_, newVal) => setNote(n => ({ ...n, total: newVal }))}
                    />
                </Grid>
                <Grid item xs={6} />
                <Grid item xs={3}>
                    <Dropdown
                        label="Credit/Replace"
                        propertyName="creditOrReplace"
                        value={note.creditOrReplace}
                        fullWidth
                        items={['CREDIT', 'REPLACE']}
                        onChange={(_, newval) => setNote(n => ({ ...n, creditOrReplace: newval }))}
                    />
                </Grid>
                <Grid item xs={9}>
                    <InputField
                        value={note.notes}
                        propertyName="notes"
                        fullWidth
                        label="Notes"
                        onChange={(_, newVal) => setNote(n => ({ ...n, notes: newVal }))}
                    />
                </Grid>
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveClick={() => {
                            dispatch(plCreditDebitNoteActions.add(note));
                        }}
                        backClick={() => {
                            history.push('/purchasing/pl-credit-debit-notes');
                        }}
                        saveDisabled={!order?.orderNumber}
                        cancelClick={() => {
                            setNote({});
                            setOrder({});
                        }}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default CreateDebitNote;
