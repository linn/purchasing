import React, { Fragment, useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    itemSelectorHelpers,
    Loading,
    Page,
    InputField,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import { useParams } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import { makeStyles } from '@mui/styles';
import IconButton from '@mui/material/IconButton';
import Close from '@mui/icons-material/Close';
import config from '../../config';
import history from '../../history';
import { plCreditDebitNote } from '../../itemTypes';
import plCreditDebitNoteActions from '../../actions/plCreditDebitNoteActions';

function Notes() {
    const useStyles = makeStyles(theme => ({
        dialog: {
            margin: theme.spacing(6),
            minWidth: theme.spacing(62)
        },
        total: {
            float: 'right'
        }
    }));
    const classes = useStyles();
    const dispatch = useDispatch();
    const item = useSelector(state => itemSelectorHelpers.getItem(state[plCreditDebitNote.item]));
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[plCreditDebitNote.item])
    );

    const [note, setNote] = useState(null);
    const [cancelDialogOpen, setCancelDialogOpen] = useState(false);
    const [cancelReason, setCancelReason] = useState('');

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
            {note.cancelled && (
                <Grid item xs={12}>
                    <Typography color="secondary" variant="h6">
                        CANCELLED
                    </Typography>
                </Grid>
            )}
            <Grid item xs={2}>
                <Typography variant="subtitle2">Supplier:</Typography>
            </Grid>
            <Grid item xs={5}>
                <Typography variant="subtitle1">{note.supplierName}</Typography>
            </Grid>
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
            <Grid item xs={4} />
            {note.orderDetails?.map(d => (
                <Fragment key={d.line}>
                    <Grid item xs={2}>
                        <Typography variant="subtitle2">Orig Order No:</Typography>
                    </Grid>
                    <Grid item xs={2}>
                        <Typography variant="subtitle1">{note.originalOrderNumber}</Typography>
                    </Grid>
                    <Grid item xs={2}>
                        <Typography variant="subtitle2">Line:</Typography>
                    </Grid>
                    <Grid item xs={2}>
                        <Typography variant="subtitle1">{d.line}</Typography>
                    </Grid>
                    <Grid item xs={4} />
                    <Grid item xs={2}>
                        <Typography variant="subtitle2">Part:</Typography>
                    </Grid>
                    <Grid item xs={4}>
                        <Typography variant="subtitle1">{d.partNumber}</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <Typography variant="subtitle1">{d.partDescription}</Typography>
                    </Grid>
                </Fragment>
            ))}
            <Grid item xs={2}>
                <Typography variant="subtitle2">Qty:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">
                    {`${note.orderQty} in ${note.orderUnitOfMeasure}`}
                </Typography>
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Unit Price:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{note.orderUnitPrice}</Typography>
            </Grid>
            <Grid item xs={2} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Currency:</Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{note.currency}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle2">Total Ex-Vat:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{note.netTotal}</Typography>
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Vat Total:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">
                    {`${note.vatTotal} at ${note.vatRate}%`}
                </Typography>
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Total Value:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{note.total}</Typography>
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Notes:</Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography variant="subtitle1">{note.notes}</Typography>
            </Grid>
            <Grid item xs={2} />

            {note.noteType === 'D' && (
                <>
                    <Grid item xs={10}>
                        <Typography variant="subtitle1">
                            THESE ITEMS ARE RETURNED FOR CREDIT
                        </Typography>
                        <Typography variant="subtitle1">DO NOT RESSUPPLY</Typography>
                    </Grid>
                    <Grid item xs={2} />
                </>
            )}

            <Grid item xs={10}>
                <Typography variant="subtitle2">
                    {`THIS IS A PURCHASE LEDGER ${note.noteType === 'D' ? 'DEBIT' : 'CREDIT'} NOTE`}
                </Typography>
            </Grid>
        </Grid>
    );
    return (
        note && (
            <>
                <div style={{ width: '874px', margin: '0 auto' }}>
                    <Page history={history} homeUrl={config.appRoot}>
                        {loading ? <Loading /> : <Content />}
                    </Page>
                </div>
                <Grid container spacing={3}>
                    <Dialog open={cancelDialogOpen} fullWidth maxWidth="md">
                        <div>
                            <IconButton
                                className={classes.pullRight}
                                aria-label="Close"
                                onClick={() => setCancelDialogOpen(false)}
                            >
                                <Close />
                            </IconButton>
                            <div className={classes.dialog}>
                                <Grid item xs={12}>
                                    <InputField
                                        fullWidth
                                        value={cancelReason}
                                        label="Must give a reason:"
                                        propertyName="holdReason"
                                        onChange={(_, newValue) => setCancelReason(newValue)}
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <SaveBackCancelButtons
                                        saveDisabled={!cancelReason}
                                        backClick={() => setCancelDialogOpen(false)}
                                        cancelClick={() => setCancelDialogOpen(false)}
                                        saveClick={() => {
                                            dispatch(
                                                plCreditDebitNoteActions.update(id, {
                                                    noteNumber: id,
                                                    reasonCancelled: cancelReason
                                                })
                                            );
                                        }}
                                    />
                                </Grid>
                            </div>
                        </div>
                    </Dialog>
                    <Grid item xs={10} />

                    <Grid item xs={2}>
                        <Button
                            onClick={() => setCancelDialogOpen(true)}
                            variant="outlined"
                            color="secondary"
                        >
                            Cancel
                        </Button>
                    </Grid>
                </Grid>
            </>
        )
    );
}

export default Notes;
