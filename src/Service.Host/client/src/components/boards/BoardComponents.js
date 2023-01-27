import React, { useState, useReducer, useEffect } from 'react';
import { useParams, Link as RouterLink } from 'react-router-dom';
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
    getRequestErrors,
    utilities,
    getItemError,
    InputField,
    SaveBackCancelButtons,
    ErrorCard
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
import Link from '@mui/material/Link';

import boardComponentsActions from '../../actions/boardComponentsActions';
import boardsActions from '../../actions/boardsActions';
import changeRequestsActions from '../../actions/changeRequestsActions';
import history from '../../history';
import config from '../../config';
import boardComponentsReducer from './boardComponentsReducer';
import partsActions from '../../actions/partsActions';
import { boardComponents } from '../../itemTypes';

function BoardComponents() {
    const reduxDispatch = useDispatch();
    const { id } = useParams();

    const [board, setBoard] = useState(null);
    const [crfNumber, setCrfNumber] = useState(null);
    const [crfRevisionCode, setCrfRevisionCode] = useState(null);
    const [showChanges, setShowChanges] = useState(true);
    const searchBoards = searchTerm => reduxDispatch(boardsActions.search(searchTerm));
    const clearSearchBoards = () => reduxDispatch(boardsActions.clearSearch());
    const [findDialogOpen, setFindDialogOpen] = useState(false);
    const [findField, setFindField] = useState('partNumber');
    const [findValue, setFindValue] = useState(null);
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
    const requestErrors = useSelector(reduxState =>
        getRequestErrors(reduxState)?.filter(error => error.type !== 'FETCH_ERROR')
    );

    const componentError = useSelector(reduxState =>
        getItemError(reduxState, boardComponents.item)
    );

    useEffect(() => {
        if (id && item?.boardCode !== id && !board) {
            reduxDispatch(boardComponentsActions.fetch(id));
            setBoard(id);
        }
    }, [id, item?.boardCode, reduxDispatch, board]);

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
            dispatch({ type: 'deleteProposedComponent', payload: { component: comp } });
        } else {
            dispatch({ type: 'deleteComponent', payload: { crfNumber, component: comp } });
        }
    };

    const handleReplaceRow = params => {
        const comp = params.row;
        if (comp.addChangeDocumentNumber?.toString() === crfNumber) {
            dispatch({ type: 'replaceProposedComponent', payload: { crfNumber, component: comp } });
        } else {
            dispatch({ type: 'replaceComponent', payload: { crfNumber, component: comp } });
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
                <IconButton
                    onClick={() => openPartLookUp(params.row)}
                    disabled={!crfNumber || !params.row.adding}
                >
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
                    <div>
                        <IconButton
                            aria-label="remove"
                            size="small"
                            disabled={!crfNumber || params.row.deleteChangeId > 0}
                            onClick={() => handleDeleteRow(params)}
                        >
                            <DeleteIcon fontSize="inherit" />
                        </IconButton>
                    </div>
                </Tooltip>
            )
        },
        {
            field: 'replace',
            headerName: ' ',
            width: 50,
            renderCell: params => (
                <Tooltip title="Replace">
                    <div>
                        <IconButton
                            aria-label="replace"
                            disabled={!crfNumber}
                            size="small"
                            onClick={() => handleReplaceRow(params)}
                        >
                            <UpgradeIcon fontSize="inherit" />
                        </IconButton>
                    </div>
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
                    (toRevision &&
                        state.selectedRevision.versionNumber > toRevision &&
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

    const matchesFindCriteria = (cRef, partNumber) => {
        if (findField && findValue) {
            if (findField === 'cref') {
                return findValue.toUpperCase() === cRef;
            }

            if (findField === 'partNumber') {
                return findValue.toUpperCase() === partNumber;
            }
        }

        return true;
    };

    const componentRows = state.board?.components
        ? utilities.sortEntityList(
              state.board.components
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
                          changesStateOk(f.changeState) &&
                          matchesFindCriteria(f.cRef, f.partNumber)
                  )
                  .map(c => ({ ...c, id: c.boardLine })),
              'cRef'
          )
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
        setCrfNumber(null);
        reduxDispatch(boardComponentsActions.fetch(selectedBoard.boardCode));
    };

    const goToBoard = () => {
        setCrfNumber(null);
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

    const changesDisplaying = () => showChanges || crfNumber > 0;

    const getDisplayClass = params => {
        if (params.row.changeState === 'LIVE' && params.row.deleteChangeId && changesDisplaying()) {
            return `removing-${params.row.deleteChangeState?.toLowerCase()}`;
        }

        if (params.row.removing) {
            return `removing-${params.row.changeState?.toLowerCase()}`;
        }

        return params.row.changeState?.toLowerCase();
    };

    const setCrfDetails = documentNumber => {
        setCrfNumber(documentNumber);
        if (documentNumber) {
            const crf = changeRequests.find(a => a.documentNumber.toString() === documentNumber);
            setCrfRevisionCode(crf.revisionCode);
            dispatch({ type: 'setSelectedRevisionToCrf', payload: crf.revisionCode });
        } else {
            setCrfRevisionCode(null);
        }
    };

    return (
        <Page
            history={history}
            style={{ paddingBottom: '20px' }}
            homeUrl={config.appRoot}
            requestErrors={requestErrors}
            showRequestErrors
        >
            <Typography variant="h5" gutterBottom>
                Search or select PCAS board
            </Typography>
            {renderPartLookUp()}
            <Grid container spacing={2}>
                <Dialog open={findDialogOpen} fullWidth maxWidth="md">
                    <DialogTitle>Find</DialogTitle>
                    <DialogContent dividers>
                        <Dropdown
                            items={[
                                { id: 'cref', displayText: 'Cref' },
                                { id: 'partNumber', displayText: 'Part Number' }
                            ]}
                            label="Find"
                            propertyName="findField"
                            value={findField}
                            onChange={(_, n) => {
                                setFindField(n);
                            }}
                        />
                        <InputField
                            value={findValue}
                            fullWidth
                            label="Value To Find"
                            onChange={(_, val) => setFindValue(val)}
                            propertyName="findValue"
                        />
                        <Button
                            variant="outlined"
                            onClick={() => setFindDialogOpen(false)}
                            size="small"
                            style={{ marginBottom: '25px' }}
                        >
                            Find
                        </Button>
                    </DialogContent>
                    <DialogActions>
                        <Button
                            onClick={() => {
                                setFindValue(null);
                                setFindDialogOpen(false);
                            }}
                        >
                            Clear Find
                        </Button>
                        <Button onClick={() => setFindDialogOpen(false)}>Close</Button>
                    </DialogActions>
                </Dialog>
                <Grid item xs={5}>
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
                <Grid item xs={3}>
                    <Stack direction="row" spacing={2}>
                        <Dropdown
                            items={changeRequests?.map(c => ({
                                id: c.documentNumber,
                                displayText: `${c.documentType}${c.documentNumber}`
                            }))}
                            allowNoValue
                            loading={changeRequestsLoading}
                            label="CRF"
                            propertyName="crNumber"
                            helperText="Select a corresponding CRF to start editing"
                            value={crfNumber}
                            onChange={(_, n) => {
                                setCrfDetails(n);
                            }}
                        />
                    </Stack>
                </Grid>
                <Grid item xs={4}>
                    <Stack direction="row" spacing={2}>
                        <Button
                            variant="outlined"
                            onClick={() => setFindDialogOpen(true)}
                            size="small"
                            style={{ marginBottom: '25px' }}
                        >
                            Find Items
                        </Button>
                        <Button
                            variant="outlined"
                            style={{ marginBottom: '25px' }}
                            onClick={() => {
                                setShowChanges(!showChanges);
                            }}
                        >
                            {showChanges ? 'hide' : 'show'} changes{' '}
                        </Button>
                        <Link
                            style={{ marginTop: '5px' }}
                            component={RouterLink}
                            variant="button"
                            to={`/purchasing/boms/boards/${state?.board?.boardCode}`}
                        >
                            Board Details
                        </Link>
                    </Stack>
                </Grid>
                {loading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
                {componentError && (
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={componentError.details} />
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
                                        if (!crfNumber > 0) {
                                            dispatch({
                                                type: 'setSelectedLayout',
                                                payload: newSelectionModel
                                            });
                                        }
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
                                        if (!crfNumber > 0) {
                                            dispatch({
                                                type: 'setSelectedRevision',
                                                payload: newSelectionModel
                                            });
                                        }
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
                                        '& .removing-propos': {
                                            textDecorationLine: 'line-through',
                                            bgcolor: 'yellow'
                                        },
                                        '& .removing-accept': {
                                            textDecorationLine: 'line-through',
                                            bgcolor: '#b0f7b9'
                                        }
                                    }}
                                    columnVisibilityModel={{
                                        addChangeDocumentNumber: changesDisplaying(),
                                        deleteChangeDocumentNumber: changesDisplaying()
                                    }}
                                    rows={componentRows}
                                    columns={componentColumns}
                                    pageSize={100}
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
                                        state.board.components.length <= 100
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
                        <div>
                            <Button
                                disabled={!crfNumber}
                                onClick={() => {
                                    dispatch({ type: 'newComponent', payload: { crfNumber } });
                                }}
                            >
                                New Component
                            </Button>
                        </div>
                    </Tooltip>
                </Grid>
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveDisabled={!crfNumber}
                        saveClick={() => {
                            reduxDispatch(boardComponentsActions.clearErrorsForItem());
                            reduxDispatch(
                                boardComponentsActions.update(state.board.boardCode, {
                                    ...state.board,
                                    changeRequestId: crfNumber,
                                    changeRequestRevisionCode: crfRevisionCode
                                })
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
