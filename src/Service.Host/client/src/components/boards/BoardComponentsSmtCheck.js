import React, { useState, useEffect } from 'react';
import { useParams, Link as RouterLink } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    Loading,
    Dropdown,
    collectionSelectorHelpers,
    itemSelectorHelpers,
    processSelectorHelpers,
    Search,
    getRequestErrors,
    getItemError,
    InputField,
    FileUploader
} from '@linn-it/linn-form-components-library';
import IconButton from '@mui/material/IconButton';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Link from '@mui/material/Link';
import Close from '@mui/icons-material/Close';
import { makeStyles } from '@mui/styles';

import boardComponentsActions from '../../actions/boardComponentsActions';
import boardsActions from '../../actions/boardsActions';
import history from '../../history';
import config from '../../config';
import uploadSmtBoardFileActions from '../../actions/uploadSmtBoardFileActions';
import { uploadSmtBoardFile } from '../../processTypes';

function BoardComponentsSmtCheck() {
    const reduxDispatch = useDispatch();
    const { id } = useParams();

    const [boardSearch, setBoardSearch] = useState(null);
    const [revisionSearch, setRevisionSearch] = useState(null);
    const [selectedRevision, setSelectedRevision] = useState(null);
    const [revisions, setRevisions] = useState([]);

    const searchBoards = searchTerm => reduxDispatch(boardsActions.search(searchTerm));
    const clearSearchBoards = () => reduxDispatch(boardsActions.clearSearch());

    const [loadDialogOpen, setLoadDialogOpen] = useState({ open: false, makeChanges: false });
    const [resultsDialogOpen, setResultsDialogOpen] = useState(false);

    const searchBoardsResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.boards)
    );
    const searchBoardsLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.boards)
    );

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.boardComponents));
    const loading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.boardComponents)
    );
    const requestErrors = useSelector(reduxState =>
        getRequestErrors(reduxState)?.filter(error => error.type !== 'FETCH_ERROR')
    );

    useEffect(() => {
        if (id && item?.boardCode !== id && !boardSearch) {
            reduxDispatch(boardComponentsActions.fetch(id));
            setBoardSearch(id);
        }
    }, [id, item?.boardCode, reduxDispatch, boardSearch]);

    useEffect(() => {
        if (item) {
            const allRevisions = item.layouts.flatMap(a => a.revisions);
            setRevisions(allRevisions);
            if (revisionSearch && allRevisions.length > 0) {
                const rev = allRevisions.find(a => a.revisionCode === revisionSearch.toUpperCase());
                if (rev) {
                    setSelectedRevision(rev);
                } else {
                    setSelectedRevision(allRevisions.at(0));
                }
            } else if (allRevisions.length > 0) {
                setSelectedRevision(allRevisions.at(0));
            } else {
                setSelectedRevision(null);
            }
        } else {
            setSelectedRevision(null);
            setRevisions([]);
        }
    }, [item, revisionSearch]);

    const uploadLoading = useSelector(reduxState =>
        processSelectorHelpers.getWorking(reduxState[uploadSmtBoardFile.item])
    );

    const uploadResult = useSelector(reduxState =>
        processSelectorHelpers.getData(reduxState[uploadSmtBoardFile.item])
    );

    const uploadError = useSelector(reduxState =>
        getItemError(reduxState, uploadSmtBoardFile.item)
    );

    useEffect(() => {
        if (uploadResult?.message) {
            if (loadDialogOpen?.makeChanges) {
                reduxDispatch(boardComponentsActions.fetch(boardSearch));
            }

            setLoadDialogOpen({ open: false, makeChanges: false });
            setResultsDialogOpen(true);
        }
    }, [boardSearch, loadDialogOpen?.makeChanges, reduxDispatch, uploadResult?.message]);

    const useStyles = makeStyles(() => ({
        pullRight: {
            float: 'right'
        }
    }));

    const classes = useStyles();

    const goToSelectedBoard = selectedBoard => {
        setBoardSearch(selectedBoard.boardCode);
        reduxDispatch(boardComponentsActions.fetch(selectedBoard.boardCode));
    };

    const goToBoard = () => {
        if (boardSearch) {
            reduxDispatch(boardComponentsActions.fetch(boardSearch.toUpperCase()));
        }
    };

    const handleSelectRevision = revisionCode => {
        setSelectedRevision(revisions.find(a => a.revisionCode === revisionCode));
    };

    return (
        <Page
            history={history}
            style={{ paddingBottom: '20px' }}
            homeUrl={config.appRoot}
            requestErrors={requestErrors}
            showRequestErrors
            title="Board File Check"
        >
            <Typography variant="h5" gutterBottom>
                Search or select PCAS board
            </Typography>

            <Grid container spacing={2}>
                <Dialog open={resultsDialogOpen} fullWidth maxWidth="lg">
                    <DialogTitle>
                        File Processing Results
                        <IconButton
                            className={classes.pullRight}
                            aria-label="Close"
                            onClick={() => setResultsDialogOpen(false)}
                        >
                            <Close />
                        </IconButton>
                    </DialogTitle>
                    <DialogContent dividers>
                        <Typography variant="body1" style={{ whiteSpace: 'pre' }}>
                            {uploadResult?.message}
                        </Typography>
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={() => setResultsDialogOpen(false)}>Close</Button>
                    </DialogActions>
                </Dialog>
                <Grid item xs={7}>
                    <Stack direction="row" spacing={2}>
                        <Search
                            propertyName="boardCode"
                            label="Select Board"
                            resultsInModal
                            resultLimit={100}
                            value={boardSearch}
                            handleValueChange={(_, b) => setBoardSearch(b)}
                            search={searchBoards}
                            helperText="Press <ENTER> to search or GO button to go directly"
                            searchResults={searchBoardsResults.map(s => ({
                                ...s,
                                id: `${s.boardCode}`,
                                name: `${s.boardCode}`
                            }))}
                            loading={searchBoardsLoading}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={newValue => {
                                goToSelectedBoard(newValue);
                            }}
                            clearSearch={clearSearchBoards}
                        />
                        <InputField
                            value={revisionSearch}
                            label="Revision"
                            propertyName="selected rev"
                            onChange={(_, val) => setRevisionSearch(val)}
                        />
                        <Button
                            variant="outlined"
                            onClick={goToBoard}
                            size="medium"
                            style={{ marginBottom: '45px' }}
                        >
                            Go
                        </Button>
                    </Stack>
                </Grid>
                <Grid item xs={3}>
                    <Stack direction="row" spacing={2}>
                        <Dropdown
                            items={revisions?.map(c => ({
                                id: c.revisionCode,
                                displayText: c.revisionCode
                            }))}
                            allowNoValue
                            loading={loading}
                            label="Revs"
                            propertyName="revisionSelector"
                            helperText="Select a revision"
                            value={selectedRevision?.revisionCode}
                            onChange={(_, n) => {
                                handleSelectRevision(n);
                            }}
                        />
                    </Stack>
                </Grid>
                <Grid item xs={2}>
                    <div style={{ paddingTop: '30px', float: 'right' }}>
                        <Link
                            component={RouterLink}
                            variant="button"
                            to={`/purchasing/boms/board-components/${item?.boardCode}`}
                        >
                            Board Components
                        </Link>
                    </div>
                </Grid>
                {loading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
                <Grid item xs={4}>
                    <Typography variant="h6" gutterBottom>
                        {`Selected Board: ${selectedRevision?.boardCode}`}
                    </Typography>
                </Grid>
                <Grid item xs={4}>
                    <Typography variant="h6" gutterBottom>
                        {`Selected Revision: ${selectedRevision?.revisionCode}`}
                    </Typography>
                </Grid>
                <Grid item xs={4}>
                    <Typography variant="h6" gutterBottom>
                        {`PCSM: ${selectedRevision?.pcsmPartNumber}`}
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <FileUploader
                        doUpload={data => {
                            reduxDispatch(uploadSmtBoardFileActions.clearErrorsForItem());
                            reduxDispatch(uploadSmtBoardFileActions.clearProcessData());
                            reduxDispatch(
                                uploadSmtBoardFileActions.requestProcessStart(data, {
                                    boardCode: item?.boardCode,
                                    revisionCode: selectedRevision?.revisionCode,
                                    changeRequestId: 0
                                })
                            );
                        }}
                        loading={uploadLoading}
                        result={uploadResult}
                        error={uploadError}
                        snackbarVisible={false}
                        setSnackbarVisible={() => {}}
                        initiallyExpanded
                        helperText="Check an SMT board file against board details"
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default BoardComponentsSmtCheck;
