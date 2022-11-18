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
    InputField,
    getRequestErrors
} from '@linn-it/linn-form-components-library';
import boardActions from '../../actions/boardActions';
import history from '../../history';
import config from '../../config';
import BoardTab from './BoardTab';
import LayoutTab from './LayoutTab';
import boardReducer from './boardReducer';

function Board({ creating }) {
    const reduxDispatch = useDispatch();
    const { id } = useParams();
    const [selectedLayout, setSelectedLayout] = useState(null);

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
            if (item.layouts && item.layouts.length > 0) {
                setSelectedLayout([item.layouts[item.layouts.length - 1].layoutCode]);
            } else {
                setSelectedLayout([]);
            }
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

    const okToSave = () =>
        state.board &&
        state.board.clusterBoard &&
        state.board.idBoard &&
        state.board.coreBoard &&
        state.board.description &&
        state.board.splitBom &&
        state.board.boardCode;

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
                                selectedLayout={selectedLayout}
                                setSelectedLayout={setSelectedLayout}
                            />
                        )}
                    </Grid>
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
