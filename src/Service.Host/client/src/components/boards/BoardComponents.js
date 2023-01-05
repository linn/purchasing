import React, { useState, useReducer, useEffect } from 'react';
import { useParams } from 'react-router-dom';
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
    Search,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';

import boardComponentsActions from '../../actions/boardComponentsActions';
import boardsActions from '../../actions/boardsActions';
import changeRequestsActions from '../../actions/changeRequestsActions';
import history from '../../history';
import config from '../../config';
import boardComponentsReducer from './boardComponentsReducer';

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
    const componentColumns = [
        { field: 'cRef', headerName: 'CRef', width: 140 },
        { field: 'partNumber', headerName: 'Part Number', width: 140 },
        { field: 'assemblyTechnology', headerName: 'Ass Tech', width: 140 },
        { field: 'quantity', headerName: 'Qty', width: 120, editable: crfNumber },
        { field: 'addChangeDocumentNumber', headerName: 'Add Crf', width: 140 },
        { field: 'deleteChangeDocumentNumber', headerName: 'Del Crf', width: 140 }
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

    return (
        <Page history={history} style={{ paddingBottom: '20px' }} homeUrl={config.appRoot}>
            <Typography variant="h5" gutterBottom>
                Search or select PCAS board
            </Typography>
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
                    <div style={{ width: '850px' }}>
                        {state.board?.components && (
                            <>
                                <DataGrid
                                    sx={{
                                        '& .propos': {
                                            bgcolor: 'yellow'
                                        },
                                        '& .accept': {
                                            bgcolor: '#b0f7b9'
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
                                    getRowClassName={params =>
                                        params.row.changeState?.toLowerCase()
                                    }
                                />
                            </>
                        )}
                    </div>
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
