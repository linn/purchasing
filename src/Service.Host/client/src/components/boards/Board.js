import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    itemSelectorHelpers,
    SaveBackCancelButtons,
    Loading,
    InputField
} from '@linn-it/linn-form-components-library';
import boardActions from '../../actions/boardActions';
import history from '../../history';
import config from '../../config';
import BoardTab from './BoardTab';

function Board() {
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
    const [board, setBoard] = useState(null);

    useEffect(() => {
        if (id) {
            reduxDispatch(boardActions.fetch(id));
        }
    }, [id, reduxDispatch]);

    useEffect(() => {
        reduxDispatch(boardActions.fetchState());
    }, [reduxDispatch]);

    useEffect(() => {
        setBoard(item);
    }, [item]);

    const creating = () => false;

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        setBoard({ ...board, [propertyName]: newValue });
    };

    const saveBoard = () => {
        clearErrors();
        if (creating()) {
            createBoard(board);
        } else {
            updateBoard(board);
        }
    };

    const handleCancel = () => {
        setBoard(item);
        setEditStatus('view');
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            {loading && <Loading />}
            {board && (
                <Grid container spacing={2}>
                    <Grid item xs={2}>
                        <InputField
                            fullWidth
                            value={board.boardCode}
                            label="Board Code"
                            disabled={!creating()}
                            propertyName="boardCode"
                            onChange={() => {}}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            fullWidth
                            value={board.description}
                            label="Description"
                            propertyName="description"
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
                                style={{ paddingTop: '40px' }}
                            />
                        )}
                    </Grid>
                    <Grid item xs={12}>
                        <SaveBackCancelButtons
                            saveDisabled={editStatus === 'view'}
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

export default Board;
