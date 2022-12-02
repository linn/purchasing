import React, { useEffect, useState, useReducer } from 'react';
import { useParams } from 'react-router-dom';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    itemSelectorHelpers,
    SaveBackCancelButtons,
    Loading,
    ErrorCard,
    InputField,
    getItemError,
    getRequestErrors,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import boardActions from '../../actions/boardActions';
import history from '../../history';
import config from '../../config';
import BoardTab from './BoardTab';
import LayoutTab from './LayoutTab';
import RevisionTab from './RevisionTab';
import boardReducer from './boardReducer';
import partsActions from '../../actions/partsActions';

function Board({ creating }) {
    const reduxDispatch = useDispatch();
    const { id } = useParams();

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.board));
    const loading = useSelector(reduxState => itemSelectorHelpers.getItemLoading(reduxState.board));
    const setEditStatus = status => reduxDispatch(boardActions.setEditStatus(status));
    const editStatus = useSelector(reduxState =>
        itemSelectorHelpers.getItemEditStatus(reduxState.board)
    );
    const clearErrors = () => reduxDispatch(boardActions.clearErrorsForItem());

    const updateBoard = board => reduxDispatch(boardActions.update(board.boardCode, board));
    const createBoard = board => reduxDispatch(boardActions.add(board));

    const [selectedTab, setSelectedTab] = useState(0);

    const requestErrors = useSelector(state =>
        getRequestErrors(state)?.filter(error => error.type !== 'FETCH_ERROR')
    );
    const [state, dispatch] = useReducer(boardReducer, { board: null });
    const itemError = useSelector(reduxState => getItemError(reduxState, 'board'));

    useEffect(() => {
        if (creating) {
            dispatch({ type: 'initialise' });
        } else if (id) {
            reduxDispatch(boardActions.fetch(id));
        }
    }, [id, reduxDispatch, creating]);

    useEffect(() => {
        if (item) {
            dispatch({ type: 'populate', payload: item });
            setSelectedTab(0);
        }
    }, [item]);

    useEffect(() => {
        reduxDispatch(boardActions.fetchState());
    }, [reduxDispatch]);

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        dispatch({ type: 'fieldChange', fieldName: propertyName, payload: newValue });
    };

    const saveBoard = () => {
        clearErrors();
        if (creating) {
            createBoard(state.board);
        } else {
            updateBoard(state.board);
        }
    };

    const handleCancel = () => {
        clearErrors();
        if (creating) {
            dispatch({ type: 'initialise', payload: null });
        } else {
            setEditStatus('view');
            dispatch({ type: 'populate', payload: item });
        }
    };

    const layoutsAreOk = layouts => {
        if (!layouts || !layouts.length) {
            return true;
        }

        if (
            layouts.some(a => a.layoutCode === 'new') ||
            layouts.some(a => a.layoutCode === 'duplicate')
        ) {
            return false;
        }

        const codes = layouts.map(a => a.layoutCode);

        return codes.every((a, i) => codes.indexOf(a) === i);
    };

    const revisionsAreOk = layouts => {
        if (!layouts || !layouts.length) {
            return true;
        }

        const allRevisions = layouts.flatMap(a => a.revisions);

        if (allRevisions.some(a => a.revisionCode === 'duplicate')) {
            return false;
        }

        const revisionCodes = allRevisions.map(a => a.revisionCode);

        return revisionCodes.every((a, i) => revisionCodes.indexOf(a) === i);
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

    const okToSave = () =>
        state.board &&
        state.board.clusterBoard &&
        state.board.idBoard &&
        state.board.coreBoard &&
        state.board.description &&
        state.board.splitBom &&
        state.board.boardCode &&
        layoutsAreOk(state.board.layouts) &&
        revisionsAreOk(state.board.layouts);

    return (
        <Page
            history={history}
            homeUrl={config.appRoot}
            requestErrors={requestErrors}
            showRequestErrors
        >
            {loading && <Loading />}
            {state.board && (
                <Grid container spacing={2}>
                    <Grid item xs={2}>
                        <InputField
                            fullWidth
                            value={state.board.boardCode}
                            label="Board Code"
                            disabled
                            propertyName="boardCodeDisplay"
                            onChange={() => {}}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            fullWidth
                            value={state.board.description}
                            label="Board Description"
                            disabled
                            propertyName="boardDescriptionDisplay"
                            onChange={() => {}}
                        />
                    </Grid>
                    <Grid item xs={4} />
                    <Grid item xs={12}>
                        <Tabs
                            value={selectedTab}
                            onChange={(_, newValue) => {
                                setSelectedTab(newValue);
                            }}
                        >
                            <Tab label="Board Details" />
                            <Tab label="Layouts" />
                            <Tab label="Revisions" />
                        </Tabs>
                        {selectedTab === 0 && (
                            <BoardTab
                                handleFieldChange={handleFieldChange}
                                boardCode={state.board.boardCode}
                                description={state.board.description}
                                coreBoard={state.board.coreBoard}
                                clusterBoard={state.board.clusterBoard}
                                idBoard={state.board.idBoard}
                                defaultPcbNumber={state.board.defaultPcbNumber}
                                variantOfBoardCode={state.board.variantOfBoardCode}
                                splitBom={state.board.splitBom}
                                creating={creating}
                                style={{ paddingTop: '40px' }}
                            />
                        )}
                        {selectedTab === 1 && (
                            <LayoutTab
                                layouts={state.board.layouts}
                                style={{ paddingTop: '40px' }}
                                dispatch={dispatch}
                                selectedLayout={state.selectedLayout}
                                setEditStatus={setEditStatus}
                                okToSave={okToSave}
                                searchParts={searchParts}
                                partsSearchResults={partsSearchResults}
                                partsSearchLoading={partsSearchLoading}
                            />
                        )}
                        {selectedTab === 2 && (
                            <RevisionTab
                                layouts={state.board.layouts}
                                style={{ paddingTop: '40px' }}
                                dispatch={dispatch}
                                selectedLayout={state.selectedLayout}
                                selectedRevision={state.selectedRevision}
                                setEditStatus={setEditStatus}
                                okToSave={okToSave}
                                searchParts={searchParts}
                                partsSearchResults={partsSearchResults}
                                partsSearchLoading={partsSearchLoading}
                            />
                        )}
                    </Grid>
                    {itemError && (
                        <Grid item xs={12}>
                            <ErrorCard
                                errorMessage={itemError.details?.error || itemError.details}
                            />
                        </Grid>
                    )}
                    <Grid item xs={12}>
                        <SaveBackCancelButtons
                            saveDisabled={!okToSave() || editStatus === 'view'}
                            saveClick={saveBoard}
                            cancelClick={handleCancel}
                            backClick={() => history.push('/purchasing/boms/boards')}
                        />
                    </Grid>
                </Grid>
            )}
        </Page>
    );
}

Board.propTypes = {
    creating: PropTypes.bool
};

Board.defaultProps = {
    creating: false
};

export default Board;
