import React, { Fragment, useEffect, useState, useRef } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    itemSelectorHelpers,
    Loading,
    Page,
    InputField,
    SaveBackCancelButtons,
    SnackbarMessage,
    processSelectorHelpers,
    ErrorCard,
    getItemError
} from '@linn-it/linn-form-components-library';
import { useParams } from 'react-router-dom';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import html2canvas from 'html2canvas';
import { jsPDF as JsPDF } from 'jspdf';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import { makeStyles } from '@mui/styles';
import IconButton from '@mui/material/IconButton';
import Close from '@mui/icons-material/Close';
import config from '../../config';
import history from '../../history';
import { plCreditDebitNote, sendPlNoteEmail as sendPlNoteEmailItemType } from '../../itemTypes';
import plCreditDebitNoteActions from '../../actions/plCreditDebitNoteActions';
import {
    sendPlNoteEmail,
    setMessageVisible,
    clearProcessData
} from '../../actions/sendPlNoteEmailActions';

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
    const pdfRef = useRef();

    const itemError = useSelector(state => getItemError(state, plCreditDebitNote.item));

    const [cancelDialogOpen, setCancelDialogOpen] = useState(false);
    const [cancelReason, setCancelReason] = useState('');

    const [pdfLoading, setPdfLoading] = useState(false);

    const { id } = useParams();

    const snackbarVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[sendPlNoteEmailItemType.item])
    );

    const message = useSelector(state =>
        processSelectorHelpers.getMessageText(state[sendPlNoteEmailItemType.item])
    );

    const processResult = useSelector(state =>
        processSelectorHelpers.getData(state[sendPlNoteEmailItemType.item])
    );

    const processLoading = useSelector(state =>
        processSelectorHelpers.getWorking(state[sendPlNoteEmailItemType.item])
    );

    useEffect(() => {
        if (processResult) {
            setPdfLoading(false);
        }
    }, [processResult]);

    const setSnackbarVisible = () => dispatch(setMessageVisible(false));

    useEffect(() => {
        if (id) {
            dispatch(plCreditDebitNoteActions.fetch(id));
        }
    }, [id, dispatch]);

    const toPdf = async email => {
        const element = pdfRef.current;
        const canvas = await html2canvas(element, {
            quality: 4,
            scale: 5
        });
        const data = canvas.toDataURL('image/png');

        const pdf = new JsPDF('p', 'pt', 'a4', true);
        const imgProperties = pdf.getImageProperties(data);
        const pdfWidth = pdf.internal.pageSize.getWidth();
        const pdfHeight = (imgProperties.height * pdfWidth) / imgProperties.width;

        pdf.addImage(data, 'PNG', 0, 0, pdfWidth, pdfHeight, '', 'FAST');
        if (email) {
            const blob = pdf.output('blob');
            dispatch(sendPlNoteEmail(blob, id));
            return;
        }
        pdf.save();
    };

    const Content = () => (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                <SnackbarMessage
                    visible={snackbarVisible && processResult?.success}
                    onClose={() => setSnackbarVisible(false)}
                    message={message}
                />
                <Typography variant="h6">
                    {`Linn ${item.itemType === 'C' ? 'Credit' : 'Debit'} Note ${item.noteNumber}`}
                </Typography>
            </Grid>
            {item.cancelled && (
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
                <Typography variant="subtitle1">{item.supplierName}</Typography>
            </Grid>
            <Grid item xs={1}>
                <Typography variant="subtitle2">Date:</Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">
                    {new Date(item.dateCreated)?.toLocaleDateString()}
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle2">Your Ref:</Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.orderContactName}</Typography>
            </Grid>
            <Grid item xs={6} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Ret Order No:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.returnsOrderNumber}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle2">Line:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.returnsOrderLine}</Typography>
            </Grid>
            <Grid item xs={4} />
            {item.orderDetails?.map(d => (
                <Fragment key={d.line}>
                    <Grid item xs={2}>
                        <Typography variant="subtitle2">Orig Order No:</Typography>
                    </Grid>
                    <Grid item xs={2}>
                        <Typography variant="subtitle1">{item.originalOrderNumber}</Typography>
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
                    {`${item.orderQty} in ${item.orderUnitOfMeasure}`}
                </Typography>
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Unit Price:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.orderUnitPrice}</Typography>
            </Grid>
            <Grid item xs={2} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Currency:</Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.currency}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle2">Total Ex-Vat:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.netTotal}</Typography>
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Vat Total:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">
                    {`${item.vatTotal} at ${item.vatRate}%`}
                </Typography>
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Total Value:</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.total}</Typography>
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={2}>
                <Typography variant="subtitle2">Notes:</Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography variant="subtitle1">{item.items}</Typography>
            </Grid>
            <Grid item xs={2} />

            {item.itemType === 'D' && (
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
                    {`THIS IS A PURCHASE LEDGER ${item.itemType === 'D' ? 'DEBIT' : 'CREDIT'} NOTE`}
                </Typography>
            </Grid>
        </Grid>
    );
    return (
        item && (
            <>
                {processResult && !processResult.success && (
                    <Grid style={{ paddingTop: '100px' }} item xs={12}>
                        <ErrorCard errorMessage={processResult.message} />
                    </Grid>
                )}

                {itemError && (
                    <Grid style={{ paddingTop: '100px' }} item xs={12}>
                        <ErrorCard errorMessage={itemError.details} />
                    </Grid>
                )}

                <div style={{ width: '90%', minWidth: '1200px', margin: '0 auto' }}>
                    <Page history={history} homeUrl={config.appRoot}>
                        <div
                            style={{ width: '874px', margin: '0 auto', padding: '60px' }}
                            ref={pdfRef}
                        >
                            {loading || processLoading || pdfLoading ? <Loading /> : <Content />}
                        </div>
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
                                            dispatch(plCreditDebitNoteActions.clearErrorsForItem());
                                            dispatch(
                                                plCreditDebitNoteActions.update(id, {
                                                    itemNumber: id,
                                                    reasonCancelled: cancelReason
                                                })
                                            );
                                            setCancelDialogOpen(false);
                                        }}
                                    />
                                </Grid>
                            </div>
                        </div>
                    </Dialog>
                    <Grid item xs={8} />

                    <Grid item xs={4}>
                        <Button onClick={() => toPdf(false)} variant="outlined">
                            pdf
                        </Button>
                        <Button
                            onClick={() => {
                                setPdfLoading(true);
                                dispatch(plCreditDebitNoteActions.clearErrorsForItem());
                                dispatch(clearProcessData);
                                toPdf(true);
                            }}
                            variant="contained"
                        >
                            email
                        </Button>
                        <Button
                            onClick={() => setCancelDialogOpen(true)}
                            variant="contained"
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
