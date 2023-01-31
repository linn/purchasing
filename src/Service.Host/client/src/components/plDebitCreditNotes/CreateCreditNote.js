import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import Grid from '@mui/material/Grid';
import {
    Page,
    Search,
    collectionSelectorHelpers,
    InputField,
    itemSelectorHelpers,
    Dropdown,
    SaveBackCancelButtons,
    Loading,
    ErrorCard,
    getItemError
} from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import partsActions from '../../actions/partsActions';
import plCreditDebitNoteActions from '../../actions/plCreditDebitNoteActions';
import { purchaseOrders, parts, plCreditDebitNote } from '../../itemTypes';

function CreateCreditNote() {
    const [note, setNote] = useState({
        creditOrReplace: 'CREDIT',
        orderUnitOfMeasure: 'ONES',
        currency: 'GBP',
        partNumber: 'SUNDRY'
    });
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
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[plCreditDebitNote.item])
    );
    const itemError = useSelector(reduxState => getItemError(reduxState, plCreditDebitNote.item));
    const [type, setType] = useState('F');

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                {itemError && (
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={itemError.details?.error || itemError.details} />
                    </Grid>
                )}
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={12}>
                            <Dropdown
                                label="Note Type"
                                value={type}
                                onChange={(_, newVal) => setType(newVal)}
                                items={['C', 'F']}
                                allowNoValue={false}
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <Search
                                propertyName="returnsOrder"
                                label="Returns Order"
                                disabled={type === 'F'}
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
                                disabled={type === 'F' || !order}
                                autoFocus={false}
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
                                autoFocus={false}
                                disabled={type === 'F'}
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
                                disabled={type === 'F'}
                                onChange={(_, newVal) => {
                                    setNote(n => ({
                                        ...n,
                                        originalOrderLine: newVal
                                    }));
                                }}
                            />
                        </Grid>
                        <Grid iten xs={8} />

                        <Grid item xs={12}>
                            <InputField
                                value={note.supplierId}
                                propertyName="supplierId"
                                label="Supplier"
                                type="number"
                                required
                                onChange={(_, newVal) => {
                                    setNote(n => ({ ...n, supplierId: newVal }));
                                }}
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <Search
                                propertyName="partNumber"
                                label="Part"
                                required
                                autoFocus={false}
                                value={note.partNumber}
                                handleValueChange={(_, newVal) =>
                                    setNote(n => ({ ...n, partNumber: newVal }))
                                }
                                search={() => dispatch(partsActions.search(note.partNumber))}
                                searchResults={partsResults}
                                loading={partsSearchLoading}
                                resultsInModal
                                priorityFunction="closestMatchesFirst"
                                onResultSelect={res =>
                                    setNote(n => ({ ...n, partNumber: res.partNumber }))
                                }
                                clearSearch={() => dispatch(partsActions.clearSearch)}
                            />
                        </Grid>
                        <Grid item xs={1}>
                            <InputField
                                value={note.orderQty}
                                propertyName="orderQty"
                                label="Qty"
                                required
                                type="number"
                                onChange={(_, newVal) => setNote(n => ({ ...n, orderQty: newVal }))}
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <InputField
                                value={note.orderUnitOfMeasure}
                                propertyName="orderUnitOfMeasure"
                                required
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
                                required
                                type="number"
                                onChange={(_, newVal) =>
                                    setNote(n => ({ ...n, orderUnitPrice: newVal }))
                                }
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                value={note.currency}
                                propertyName="currency"
                                required
                                label="Currency Code"
                                onChange={(_, newVal) => setNote(n => ({ ...n, currency: newVal }))}
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                value={note.netTotal}
                                propertyName="netTotal"
                                required
                                label="Net"
                                type="number"
                                onChange={(_, newVal) => setNote(n => ({ ...n, netTotal: newVal }))}
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                value={note.vatTotal}
                                propertyName="vatTotal"
                                required
                                label="VAT"
                                type="number"
                                onChange={(_, newVal) => setNote(n => ({ ...n, vatTotal: newVal }))}
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                value={note.total}
                                propertyName="total"
                                required
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
                                allowNoValue={false}
                                items={['CREDIT', 'REPLACE']}
                                onChange={(_, newval) =>
                                    setNote(n => ({ ...n, creditOrReplace: newval }))
                                }
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
                                    dispatch(plCreditDebitNoteActions.clearErrorsForItem());
                                    dispatch(plCreditDebitNoteActions.add(note));
                                }}
                                backClick={() => {
                                    history.push('/purchasing/pl-credit-debit-notes');
                                }}
                                saveDisabled={
                                    !note.partNumber ||
                                    !note.creditOrReplace ||
                                    !note.total ||
                                    !note.vatTotal ||
                                    !note.netTotal ||
                                    !note.supplierId ||
                                    !note.orderUnitPrice ||
                                    !note.orderQty ||
                                    !note.currency
                                }
                                cancelClick={() => {
                                    setNote({});
                                    setOrder({});
                                }}
                            />
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default CreateCreditNote;
