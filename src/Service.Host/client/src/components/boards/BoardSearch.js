import React, { useState, useEffect } from 'react';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Tooltip from '@mui/material/Tooltip';
import Stack from '@mui/material/Stack';
import EditOffIcon from '@mui/icons-material/EditOff';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    collectionSelectorHelpers,
    Search,
    utilities
} from '@linn-it/linn-form-components-library';
import boardsActions from '../../actions/boardsActions';
import history from '../../history';
import config from '../../config';
import { board as boardItemType } from '../../itemTypes';

function BoardSearch() {
    const dispatch = useDispatch();

    const [board, setBoard] = useState(null);
    const searchBoards = searchTerm => dispatch(boardsActions.search(searchTerm));
    const clearSearchBoards = () => dispatch(boardsActions.clearSearch());

    useEffect(() => {
        dispatch(boardsActions.fetchState());
    }, [dispatch]);

    const applicationState = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.boards)
    );

    const createUri = utilities.getHref(applicationState, 'create');

    const searchBoardsResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.boards)
    );
    const searchBoardsLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.boards)
    );

    const goToSelectedBoard = selectedBoard => {
        history.push(`${boardItemType.uri}/${selectedBoard.boardCode}`);
    };

    const goToBoard = () => {
        history.push(`${boardItemType.uri}/${board}`);
    };

    const goToCreate = () => {
        history.push(createUri);
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
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        disabled={!createUri}
                        onClick={goToCreate}
                        size="small"
                        style={{ marginBottom: '25px' }}
                    >
                        Create New Board
                    </Button>
                </Grid>
                <Grid item xs={1}>
                    {createUri ? (
                        <Tooltip title="You can create and edit boards">
                            <ModeEditIcon color="primary" />
                        </Tooltip>
                    ) : (
                        <Tooltip title="You do not have permission to create or edit boards">
                            <EditOffIcon color="secondary" />
                        </Tooltip>
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default BoardSearch;
