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
    collectionSelectorHelpers,
    itemSelectorHelpers,
    Search
} from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';

import boardComponentsActions from '../../actions/boardComponentsActions';
import boardsActions from '../../actions/boardsActions';
import history from '../../history';
import config from '../../config';
import boardComponentsReducer from './boardComponentsReducer';

function BoardComponents() {
    const reduxDispatch = useDispatch();
    const { id } = useParams();

    const [board, setBoard] = useState(null);
    const searchBoards = searchTerm => reduxDispatch(boardsActions.search(searchTerm));
    const clearSearchBoards = () => reduxDispatch(boardsActions.clearSearch());
    const searchBoardsResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.boards)
    );
    const searchBoardsLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.boards)
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
        } else {
            dispatch({ type: 'initialise' });
        }
    }, [item]);

    const layoutColumns = [{ field: 'layoutCode', headerName: 'Layout', width: 175 }];
    const layoutRows = state.board?.layouts
        ? state.board.layouts.map(l => ({ ...l, id: l.layoutCode }))
        : [];
    const revisionColumns = [{ field: 'revisionCode', headerName: 'Revision', width: 175 }];
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
        { field: 'assemblyTechnology', headerName: 'Ass Tech', width: 140 }
    ];
    const componentRows = state.board?.components
        ? state.board.components.map(c => ({ ...c, id: c.boardLine }))
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

    return (
        <Page history={history} style={{ paddingBottom: '20px' }} homeUrl={config.appRoot}>
            <Typography variant="h5" gutterBottom>
                Search or select PCAS board
            </Typography>
            <Grid container spacing={2}>
                <Grid item xs={9}>
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
                <Grid item xs={3} />
                {loading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
                <Grid item xs={2}>
                    <div style={{ width: '180px' }}>
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
                    <div style={{ width: '180px' }}>
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
                    <div style={{ width: '400px' }}>
                        {state.board?.components && (
                            <>
                                <DataGrid
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
                                    loading={loading}
                                    hideFooterSelectedRowCount
                                    hideFooter={
                                        !state.board?.components ||
                                        state.board.components.length <= 40
                                    }
                                />
                            </>
                        )}
                    </div>
                </Grid>
            </Grid>
        </Page>
    );
}

export default BoardComponents;
