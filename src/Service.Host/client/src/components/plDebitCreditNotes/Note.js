import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { itemSelectorHelpers, Loading, Page } from '@linn-it/linn-form-components-library';
import { useParams } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import config from '../../config';
import history from '../../history';
import { plCreditDebitNote } from '../../itemTypes';
import plCreditDebitNoteActions from '../../actions/plCreditDebitNoteActions';

function Notes() {
    const dispatch = useDispatch();
    const item = useSelector(state => itemSelectorHelpers.getItem(state[plCreditDebitNote.item]));
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[plCreditDebitNote.item])
    );

    const [note, setNote] = useState(null);
    const { id } = useParams();

    useEffect(() => {
        if (id) {
            dispatch(plCreditDebitNoteActions.fetch(id));
        }
    }, [id, dispatch]);

    useEffect(() => {
        if (item) {
            setNote(item);
        }
    }, [item]);
    const Content = () => (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                <Typography variant="h6">
                    {`Linn ${note.noteType === 'C' ? 'Credit' : 'Debit'} Note ${note.noteNumber}`}
                </Typography>
            </Grid>
            <Grid item xs={1}>
                <Typography variant="subtitle2">Supplier:</Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{note.supplierName}</Typography>
            </Grid>
            <Grid item xs={2} />
            <Grid item xs={1}>
                <Typography variant="subtitle2">Date:</Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">
                    {new Date(note.dateCreated)?.toLocaleDateString()}
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle2">Your Ref:</Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{note.orderContactName}</Typography>
            </Grid>
            <Grid item xs={6} />
            <Grid item xs={1}>
                <Typography variant="subtitle2">Qty:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">
                    {`${note.orderQty} in ${note.orderUnitOfMeasure}`}
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle2">Ret Order No:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{note.returnsOrderNumber}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle2">Line:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{note.returnsOrderLine}</Typography>
            </Grid>
            <Grid item xs={8} />

            <Grid item xs={1}>
                <Typography variant="subtitle2">Line:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{note.returnsOrderLine}</Typography>
            </Grid>
            <Grid item xs={2} />
        </Grid>
    );
    return (
        note && (
            <Page history={history} homeUrl={config.appRoot}>
                {loading ? <Loading /> : <Content />}
            </Page>
        )
    );
}

export default Notes;
