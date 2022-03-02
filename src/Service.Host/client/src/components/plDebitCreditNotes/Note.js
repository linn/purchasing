import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { itemSelectorHelpers, Loading, Page } from '@linn-it/linn-form-components-library';
import { useParams } from 'react-router-dom';
import Typography from '@mui/material/Typography';
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
    if (loading) {
        return (
            <Page history={history} homeUrl={config.appRoot}>
                <Loading />
            </Page>
        );
    }
    return (
        note && (
            <Page history={history} homeUrl={config.appRoot}>
                <Typography variant="h4">
                    {`${note.noteType === 'C' ? 'Credit' : 'Debit'} Note ${note.noteNumber}`}
                </Typography>
            </Page>
        )
    );
}

export default Notes;
