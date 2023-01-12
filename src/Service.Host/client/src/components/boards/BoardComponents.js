import React, { useState, useReducer, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import Tooltip from '@mui/material/Tooltip';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    Loading,
    Dropdown,
    collectionSelectorHelpers,
    itemSelectorHelpers,
    Search,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';
import UpgradeIcon from '@mui/icons-material/Upgrade';
import ManageSearchIcon from '@mui/icons-material/ManageSearch';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';

import boardComponentsActions from '../../actions/boardComponentsActions';
import boardsActions from '../../actions/boardsActions';
import changeRequestsActions from '../../actions/changeRequestsActions';
import history from '../../history';
import config from '../../config';
import boardComponentsReducer from './boardComponentsReducer';
import partsActions from '../../actions/partsActions';

function BoardComponents() {
    const reduxDispatch = useDispatch();
    const { id } = useParams();

    const [board, setBoard] = useState(null);
    const [crfNumber, setCrfNumber] = useState();
    const [showChanges, setShowChanges] = useState(true);
    const searchBoards = searchTerm => reduxDispatch(boardsActions.search(searchTerm));
    const clearSearchBoards = () => reduxDispatch(boardsActions.clearSearch());
    const searchBoardsResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.boards)
    );
    const searchBoardsLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.boards)
    );

    const changeRequests = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.changeRequests)
    );
    const changeRequestsLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.changeRequests)
    );

    const [state, dispatch] = useReducer(boardComponentsReducer, { board: null });
    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.boardComponents));
    const loading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.boardComponents)
    );

    useEffect(() => {
        if (id) {
            reduxDispatch(boardComponentsActions.fetch(id));
            setBoard(id);
        }
    }, [id, reduxDispatch]);

    useEffect(() => {
        if (item) {
            dispatch({ type: 'populate', payload: item });
            reduxDispatch(
                changeRequestsActions.searchWithOptions(item.boardCode, '&includeForBoard=true')
            );
        } else {
            dispatch({ type: 'initialise' });
        }
    }, [item, reduxDispatch]);

    const layoutColumns = [{ field: 'layoutCode', headerName: 'Layout', width: 165 }];
    const layoutRows = state.board?.layouts
        ? state.board.layouts.map(l => ({ ...l, id: l.layoutCode }))
        : [];
    const revisionColumns = [{ field: 'revisionCode', headerName: 'Revision', width: 165 }];
    const revisionRows =
        state.board?.layouts &&
        state.layoutSelectionModel?.length &&
        state.board.layouts.find(a => a.layoutCode === state.layoutSelectionModel[0]).revisions
            ? state.board.layouts
                  .find(a => a.layoutCode === state.layoutSelectionModel[0])
                  .revisions.map(l => ({ ...l, id: l.revisionCode }))
            : [];

    const handleDeleteRow = params => {
        const comp = params.row;
        if (comp.addChangeDocumentNumber?.toString() === crfNumber) {
            dispatch({ type: 'deleteProposedComponent', payload: comp });
        } else {
            dispatch({ type: 'deleteComponent', payload: { crfNumber, component: comp } });
        }
    };

    const handleReplaceRow = params => {
        const comp = params.row;
        if (comp.addChangeDocumentNumber?.toString() === crfNumber) {
            dispatch({ type: 'deleteProposedComponent', payload: comp });
            dispatch({ type: 'newComponent', payload: { crfNumber, component: comp } });
        } else {
            dispatch({ type: 'deleteComponent', payload: crfNumber, component: comp });
            dispatch({ type: 'newComponent', payload: { crfNumber, component: comp } });
        }
    };

    const [partSearchTerm, setPartSearchTerm] = useState();
    const [partLookUp, setPartLookUp] = useState({ open: false, forRow: null });
    const openPartLookUp = forRow => {
        setPartLookUp({ open: true, forRow });
        setPartSearchTerm(null);
    };
    const searchParts = searchTerm => reduxDispatch(partsActions.search(searchTerm));
    const partsSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(
            reduxState.parts,
            100,
            'id',
            'partNumber',
            'description'
        )
    );
    const partsSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.parts)
    );

    const handlePartSelect = newValue => {
        dispatch({
            type: 'setComponentPart',
            payload: { part: newValue, boardLine: partLookUp.forRow.boardLine }
        });
        setPartLookUp(p => ({ ...p, selectedPart: newValue, open: false }));
    };

    const setPartWithoutSearch = () => {
        dispatch({
            type: 'setComponentPart',
            payload: {
                part: { partNumber: partSearchTerm?.toUpperCase() },
                boardLine: partLookUp.forRow.boardLine
            }
        });
        setPartLookUp(p => ({
            ...p,
            selectedPart: { partNumber: partSearchTerm?.toUpperCase() },
            open: false
        }));
    };

    function renderPartLookUp() {
        return (
            <Dialog open={partLookUp.open}>
                <DialogTitle>Search For A Part</DialogTitle>
                <DialogContent dividers>
                    <Search
                        propertyName="partNumber"
                        label="Part Number"
                        resultsInModal
                        resultLimit={100}
                        value={partSearchTerm}
                        handleValueChange={(_, newVal) => setPartSearchTerm(newVal)}
                        search={searchParts}
                        searchResults={partsSearchResults}
                        helperText="Press ENTER to search or TAB to proceed"
                        onKeyPressFunctions={[{ keyCode: 9, action: setPartWithoutSearch }]}
                        loading={partsSearchLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={handlePartSelect}
                        clearSearch={() => {}}
                    />
                </DialogContent>
                <DialogActions>
                    <Button
                        onClick={() =>
                            setPartLookUp({ open: false, forRow: null, selectedPart: null })
                        }
                    >
                        Cancel
                    </Button>
                </DialogActions>
            </Dialog>
        );
    }

    const partLookUpCell = params => (
        <>
            <span style={{ float: 'left' }}>
                {params.row.partNumber}
                <IconButton onClick={() => openPartLookUp(params.row)} disabled={!crfNumber}>
                    <ManageSearchIcon />
                </IconButton>
            </span>
        </>
    );

    const componentColumns = [
        { field: 'cRef', headerName: 'CRef', width: 140, editable: true },
        {
            field: 'partNumber',
            headerName: 'Part',
            width: 140,
            editable: false,
            renderCell: partLookUpCell
        },
        { field: 'assemblyTechnology', headerName: 'Ass Tech', width: 140 },
        { field: 'quantity', headerName: 'Qty', type: 'number', width: 120, editable: crfNumber },
        { field: 'addChangeDocumentNumber', headerName: 'Add Crf', width: 140 },
        { field: 'deleteChangeDocumentNumber', headerName: 'Del Crf', width: 140 },
        {
            field: 'delete',
            headerName: ' ',
            width: 50,
            renderCell: params => (
                <Tooltip title="Remove">
                    <IconButton
                        aria-label="remove"
                        size="small"
                        disabled={!crfNumber}
                        onClick={() => handleDeleteRow(params)}
                    >
                        <DeleteIcon fontSize="inherit" />
                    </IconButton>
                </Tooltip>
            )
        },
        {
            field: 'replace',
            headerName: ' ',
            width: 50,
            renderCell: params => (
                <Tooltip title="Replace">
                    <IconButton
                        aria-label="replace"
                        disabled={!crfNumber}
                        size="small"
                        onClick={() => handleReplaceRow(params)}
                    >
                        <UpgradeIcon fontSize="inherit" />
                    </IconButton>
                </Tooltip>
            )
        }
    ];

    const versionsAreCorrect = (fromLayout, toLayout, fromRevision, toRevision) => {
        if (state.selectedLayout && state.selectedRevision) {
            if (
                state.selectedLayout.layoutSequence >= fromLayout &&
                (!toLayout || state.selectedLayout.layoutSequence <= toLayout)
            ) {
                if (
                    (state.selectedRevision.versionNumber < fromRevision &&
                        state.selectedLayout.layoutSequence === fromLayout) ||
                    (state.selectedRevision.versionNumber > toRevision &&
                        state.selectedLayout.layoutSequence === toLayout)
                ) {
                    return false;
                }

                return true;
            }
        }

        return false;
    };

    const changesStateOk = changeState => {
        if (!crfNumber && !showChanges && (changeState === 'ACCEPT' || changeState === 'PROPOS')) {
            return false;
        }

        return true;
    };

    const componentRows = state.board?.components
        ? state.board.components
              .filter(
                  f =>
                      f.changeState !== 'CANCEL' &&
                      f.changeState !== 'HIST' &&
                      versionsAreCorrect(
                          f.fromLayoutVersion,
                          f.toLayoutVersion,
                          f.fromRevisionVersion,
                          f.toRevisionVersion
                      ) &&
                      changesStateOk(f.changeState)
              )
              .map(c => ({ ...c, id: c.boardLine }))
        : [];

    const layout =
        state.board?.layouts && state.layoutSelectionModel?.length
            ? state.board.layouts.find(a => a.layoutCode === state.layoutSelectionModel[0])
            : null;
    const revision =
        layout &&
        state.revisionSelectionModel?.length &&
        layout.revisions &&
        layout.revisions.length
            ? layout.revisions.find(a => a.revisionCode === state.revisionSelectionModel[0])
            : null;

    const goToSelectedBoard = selectedBoard => {
        setBoard(selectedBoard.boardCode);
        reduxDispatch(boardComponentsActions.fetch(selectedBoard.boardCode));
    };

    const goToBoard = () => {
        if (board) {
            reduxDispatch(boardComponentsActions.fetch(board.toUpperCase()));
        }
    };

    const processRowUpdate = newRow => {
        dispatch({ type: 'updateComponent', payload: newRow });

        return newRow;
    };

    const handleCancel = () => {
        reduxDispatch(boardComponentsActions.clearErrorsForItem());
        dispatch({ type: 'populate', payload: item });
    };

    const getDisplayClass = params => {
        if (params.row.removing) {
            return 'removing';
        }

        return params.row.changeState?.toLowerCase();
    };

    return (
        <Page history={history} style={{ paddingBottom: '20px' }} homeUrl={config.appRoot}>
            <Typography variant="h5" gutterBottom>
                Search or select PCAS board
            </Typography>
            {renderPartLookUp()}
            <Grid container spacing={2}>
                <Grid item xs={6}>
                    <Stack direction="row" spacing={2}>
                        <Search
                            propertyName="boardCode"
                            label="Select Board"
                            resultsInModal
                            resultLimit={100}
                            value={board}
                            handleValueChange={(_, b) => setBoard(b)}
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
                        <Button
                            variant="outlined"
                            onClick={goToBoard}
                            size="small"
                            style={{ marginBottom: '25px' }}
                        >
                            Go
                        </Button>
                    </Stack>
                </Grid>
                <Grid item xs={4}>
                    <Stack direction="row" spacing={2}>
                        <Dropdown
                            items={changeRequests?.map(c => ({
                                id: c.documentNumber,
                                displayText: `${c.documentType}${c.documentNumber}`
                            }))}
                            allowNoValue
                            loading={changeRequestsLoading}
                            label="CRF Number"
                            propertyName="crNumber"
                            helperText="Select a corresponding CRF to start editing"
                            value={crfNumber}
                            onChange={(_, n) => {
                                setCrfNumber(n);
                            }}
                        />
                    </Stack>
                </Grid>
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        onClick={() => {
                            setShowChanges(!showChanges);
                        }}
                    >
                        {showChanges ? 'hide' : 'show'} changes{' '}
                    </Button>
                </Grid>
                {loading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
                <Grid item xs={2}>
                    <div style={{ width: '170px' }}>
                        {layout && (
                            <>
                                <DataGrid
                                    rows={layoutRows}
                                    columns={layoutColumns}
                                    pageSize={40}
                                    selectionModel={state.layoutSelectionModel}
                                    density="compact"
                                    autoHeight
                                    onSelectionModelChange={newSelectionModel => {
                                        dispatch({
                                            type: 'setSelectedLayout',
                                            payload: newSelectionModel
                                        });
                                    }}
                                    loading={loading}
                                    hideFooterSelectedRowCount
                                    hideFooter={
                                        !state.board?.layouts || state.board.layouts.length <= 40
                                    }
                                />
                            </>
                        )}
                    </div>
                </Grid>
                <Grid item xs={2}>
                    <div style={{ width: '170px' }}>
                        {revision && (
                            <>
                                <DataGrid
                                    rows={revisionRows}
                                    columns={revisionColumns}
                                    pageSize={40}
                                    selectionModel={state.revisionSelectionModel}
                                    density="compact"
                                    hideFooterSelectedRowCount
                                    autoHeight
                                    onSelectionModelChange={newSelectionModel => {
                                        dispatch({
                                            type: 'setSelectedRevision',
                                            payload: newSelectionModel
                                        });
                                    }}
                                    hideFooter={revisionRows.length <= 40}
                                />
                            </>
                        )}
                    </div>
                </Grid>
                <Grid item xs={8}>
                    <div style={{ width: '940px' }}>
                        {state.board?.components && (
                            <>
                                <DataGrid
                                    sx={{
                                        '& .propos': {
                                            bgcolor: 'yellow'
                                        },
                                        '& .accept': {
                                            bgcolor: '#b0f7b9'
                                        },
                                        '& .removing': {
                                            bgcolor: 'indianred',
                                            textDecorationLine: 'line-through'
                                        }
                                    }}
                                    rows={componentRows}
                                    columns={componentColumns}
                                    pageSize={40}
                                    selectionModel={state.componentSelectionModel}
                                    density="compact"
                                    autoHeight
                                    onSelectionModelChange={newSelectionModel => {
                                        dispatch({
                                            type: 'setSelectedComponent',
                                            payload: newSelectionModel
                                        });
                                    }}
                                    experimentalFeatures={{ newEditingApi: true }}
                                    processRowUpdate={processRowUpdate}
                                    loading={loading}
                                    hideFooterSelectedRowCount
                                    hideFooter={
                                        !state.board?.components ||
                                        state.board.components.length <= 40
                                    }
                                    getRowClassName={params => getDisplayClass(params)}
                                    isCellEditable={params => params.row.adding && crfNumber}
                                />
                            </>
                        )}
                    </div>
                </Grid>
                <Grid item xs={4} />
                <Grid item xs={8}>
                    <Tooltip title="Remove">
                        <Button
                            disabled={!crfNumber}
                            onClick={() => {
                                dispatch({ type: 'newComponent', payload: { crfNumber } });
                            }}
                        >
                            New Component
                        </Button>
                    </Tooltip>
                </Grid>
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveDisabled={!crfNumber}
                        saveClick={() => {
                            reduxDispatch(boardComponentsActions.clearErrorsForItem());
                            reduxDispatch(
                                boardComponentsActions.update(state.board.boardCode, state.board)
                            );
                        }}
                        cancelClick={handleCancel}
                        backClick={() => {
                            history.push('/purchasing/boms/board-components');
                        }}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default BoardComponents;
