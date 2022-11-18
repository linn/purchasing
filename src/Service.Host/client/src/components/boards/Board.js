import React, { useEffect, useState } from 'react';
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

    const initialCreateState = { coreBoard: 'N', clusterBoard: 'N', idBoard: 'N', splitBom: 'N' };
    const [selectedTab, setSelectedTab] = useState(0);
    const [board, setBoard] = useState(initialCreateState);

    const requestErrors = useSelector(state =>
        getRequestErrors(state)?.filter(error => error.type !== 'FETCH_ERROR')
    );

    useEffect(() => {
        if (!creating && id) {
            reduxDispatch(boardActions.fetch(id));
        }
    }, [id, reduxDispatch, creating]);

    useEffect(() => {
        if (item) {
            setBoard(item);
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
        setBoard({ ...board, [propertyName]: newValue });
    };

    const saveBoard = () => {
        clearErrors();
        if (creating) {
            createBoard(board);
        } else {
            updateBoard(board);
        }
    };

    const handleCancel = () => {
        clearErrors();
        if (creating) {
            setBoard(initialCreateState);
        } else {
            setEditStatus('view');
            setBoard(item);
        }
    };

    const okToSave = () =>
        board &&
        board.clusterBoard &&
        board.idBoard &&
        board.coreBoard &&
        board.description &&
        board.splitBom &&
        board.boardCode;

    return (
        <Page
            history={history}
            homeUrl={config.appRoot}
            requestErrors={requestErrors}
            showRequestErrors
        >
            {loading && <Loading />}
            {board && (
                <Grid container spacing={2}>
                    <Grid item xs={2}>
                        <InputField
                            fullWidth
                            value={board.boardCode}
                            label="Board Code"
                            disabled
                            propertyName="boardCodeDisplay"
                            onChange={() => {}}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            fullWidth
                            value={board.description}
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
                                boardCode={board.boardCode}
                                description={board.description}
                                coreBoard={board.coreBoard}
                                clusterBoard={board.clusterBoard}
                                idBoard={board.idBoard}
                                defaultPcbNumber={board.defaultPcbNumber}
                                variantOfBoardCode={board.variantOfBoardCode}
                                splitBom={board.splitBom}
                                creating={creating}
                                style={{ paddingTop: '40px' }}
                            />
                        )}
                        {selectedTab === 1 && (
                            <LayoutTab
                                layouts={board.layouts}
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
