import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Title,
    Loading,
    SnackbarMessage,
    ErrorCard,
    InputField,
    Page,
    collectionSelectorHelpers,
    itemSelectorHelpers,
    getItemError
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Dialog from '@mui/material/Dialog';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import Button from '@mui/material/Button';
import CloseIcon from '@mui/icons-material/Close';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import plCreditDebitNoteActions from '../../actions/plCreditDebitNoteActions';
import openDebitNotesActions from '../../actions/openDebitNotesActions';
import history from '../../history';
import config from '../../config';

function OpenDebitNotes() {
    const dispatch = useDispatch();
    const [rows, setRows] = useState([]);
    const [selectedRows, setSelectedRows] = useState([]);
    const [dialogOpen, setDialogOpen] = useState(false);
    const [commentsDialogOpen, setCommentsDialogOpen] = useState(false);

    const [closeReason, setCloseReason] = useState('');
    const [comments, setComments] = useState('');

    const items = useSelector(state => collectionSelectorHelpers.getItems(state.openDebitNotes));
    const itemsLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.openDebitNotes)
    );

    const snackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state.plCreditDebitNote)
    );
    const updateError = useSelector(state => getItemError(state, 'plCreditDebitNote'));
    const updateLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.plCreditDebitNote)
    );
    const updatedItem = useSelector(state => itemSelectorHelpers.getItem(state.plCreditDebitNote));

    useEffect(() => {
        if (updatedItem) {
            dispatch(openDebitNotesActions.fetch());
        }
    }, [updatedItem, updateLoading, dispatch]);

    const useStyles = makeStyles(theme => ({
        dialog: {
            margin: theme.spacing(6),
            minWidth: theme.spacing(62)
        },
        total: {
            float: 'right'
        }
    }));

    useEffect(() => dispatch(openDebitNotesActions.fetch()), [dispatch]);

    const classes = useStyles();

    useEffect(() => {
        setRows(
            items.map(s => ({
                ...s,
                id: s.noteNumber
            }))
        );
    }, [items]);

    const handleSelectRow = selected => {
        setSelectedRows(rows.filter(r => selected.includes(r.id)));
    };

    const columns = [
        {
            headerName: '#',
            field: 'noteNumber',
            width: 100
        },
        {
            headerName: 'Part',
            field: 'partNumber',
            width: 150
        },
        {
            headerName: 'Created',
            field: 'dateCreated',
            width: 150
        },
        {
            headerName: 'Supplier',
            field: 'supplierName',
            width: 250
        },
        {
            headerName: 'Qty',
            field: 'orderQty',
            width: 100
        },
        {
            headerName: 'Order No',
            field: 'originalOrderNumber',
            width: 100
        },
        {
            headerName: 'Returns Order',
            field: 'returnsOrderNumber',
            width: 200
        },
        {
            headerName: 'Net Total',
            field: 'netTotal',
            width: 200
        },
        {
            headerName: 'Comments',
            field: 'notes',
            width: 400
        }
    ];
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <SnackbarMessage
                visible={snackbarVisible}
                onClose={() => dispatch(plCreditDebitNoteActions.setSnackbarVisible(false))}
                message="Save Successful"
            />
            <Grid container spacing={3}>
                <Dialog open={dialogOpen} fullWidth maxWidth="md">
                    <div>
                        <IconButton
                            className={classes.pullRight}
                            aria-label="Close"
                            onClick={() => setDialogOpen(false)}
                        >
                            <CloseIcon />
                        </IconButton>
                        <div className={classes.dialog}>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Typography variant="h5" gutterBottom>
                                        Mark Selected as Closed
                                    </Typography>
                                </Grid>

                                <Grid item xs={12}>
                                    <InputField
                                        fullWidth
                                        value={closeReason}
                                        onChange={(_, newValue) => setCloseReason(newValue)}
                                        label="Reason? (optional)"
                                        propertyName="closeReason"
                                    />
                                </Grid>
                                <Grid item xs={2}>
                                    <Button
                                        style={{ marginTop: '22px' }}
                                        variant="contained"
                                        color="primary"
                                        onClick={() => {
                                            selectedRows.forEach(r =>
                                                dispatch(
                                                    plCreditDebitNoteActions.update(r.noteNumber, {
                                                        ...r,
                                                        close: true,
                                                        reasonClosed: closeReason
                                                    })
                                                )
                                            );
                                            setDialogOpen(false);
                                        }}
                                    >
                                        Confirm
                                    </Button>
                                </Grid>
                            </Grid>
                        </div>
                    </div>
                </Dialog>

                <Dialog open={commentsDialogOpen} fullWidth maxWidth="md">
                    <div>
                        <IconButton
                            className={classes.pullRight}
                            aria-label="Close"
                            onClick={() => setCommentsDialogOpen(false)}
                        >
                            <CloseIcon />
                        </IconButton>
                        <div className={classes.dialog}>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Typography variant="h5" gutterBottom>
                                        Edit Comments
                                    </Typography>
                                </Grid>

                                <Grid item xs={12}>
                                    <InputField
                                        fullWidth
                                        value={comments}
                                        onChange={(_, newValue) => setComments(newValue)}
                                        label="Comments"
                                        propertyName="comments"
                                    />
                                </Grid>
                                <Grid item xs={2}>
                                    <Button
                                        style={{ marginTop: '22px' }}
                                        variant="contained"
                                        color="primary"
                                        onClick={() => {
                                            dispatch(
                                                plCreditDebitNoteActions.update(
                                                    selectedRows[0].noteNumber,
                                                    {
                                                        ...selectedRows[0],
                                                        notes: comments
                                                    }
                                                )
                                            );
                                            setSelectedRows([]);
                                            setCommentsDialogOpen(false);
                                        }}
                                    >
                                        Save
                                    </Button>
                                </Grid>
                            </Grid>
                        </div>
                    </div>
                </Dialog>

                <Grid item xs={12}>
                    <Title text="Open Debit Notes" />
                </Grid>
                {updateError && (
                    <Grid item xs={12}>
                        <ErrorCard
                            errorMessage={
                                updateError?.details?.errors?.[0] || updateError.statusText
                            }
                        />
                    </Grid>
                )}
                {itemsLoading || updateLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        {rows && (
                            <>
                                <Grid item xs={12}>
                                    <div style={{ height: 500, width: '100%' }}>
                                        <DataGrid
                                            rows={rows}
                                            columns={columns}
                                            density="standard"
                                            rowHeight={34}
                                            checkboxSelection
                                            onSelectionModelChange={handleSelectRow}
                                            loading={itemsLoading}
                                            hideFooter
                                            filterModel={{
                                                items: [
                                                    {
                                                        columnField: 'supplierName',
                                                        operatorValue: 'contains',
                                                        value: ''
                                                    }
                                                ]
                                            }}
                                        />
                                    </div>
                                </Grid>
                                <Grid item xs={2}>
                                    <Button
                                        style={{ marginTop: '22px' }}
                                        colour="primary"
                                        variant="outlined"
                                        onClick={() => {
                                            setDialogOpen(true);
                                        }}
                                    >
                                        Close Selected
                                    </Button>
                                </Grid>
                                <Grid item xs={10} />
                                <Grid item xs={4}>
                                    <Button
                                        style={{ marginTop: '22px' }}
                                        colour="primary"
                                        variant="outlined"
                                        disabled={selectedRows.length !== 1}
                                        onClick={() => {
                                            setComments(selectedRows[0].notes);
                                            setCommentsDialogOpen(true);
                                        }}
                                    >
                                        Edit Comments of Selected
                                    </Button>
                                </Grid>
                                <Grid item xs={4} />
                                <Grid item xs={4}>
                                    <Typography
                                        className={classes.dialog}
                                        variant="h5"
                                        gutterBottom
                                    >
                                        Total Outstanding: £
                                        {rows.length > 0
                                            ? rows
                                                  .map(r => r.netTotal)
                                                  .reduce((a, b) => a + b, 0)
                                                  .toFixed(2)
                                            : ''}
                                    </Typography>
                                </Grid>
                            </>
                        )}
                    </>
                )}
            </Grid>
        </Page>
    );
}

OpenDebitNotes.propTypes = {};

OpenDebitNotes.defaultProps = {};

export default OpenDebitNotes;
