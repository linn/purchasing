import React, { useState } from 'react';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import { useSelector, useDispatch } from 'react-redux';
import { Page, collectionSelectorHelpers, Search } from '@linn-it/linn-form-components-library';
import boardsActions from '../../actions/boardsActions';
import history from '../../history';
import config from '../../config';
import { board as boardItemType } from '../../itemTypes';

function BoardSearch() {
    const dispatch = useDispatch();

    const [board, setBoard] = useState(null);
    const searchBoards = searchTerm => dispatch(boardsActions.search(searchTerm));
    const clearSearchBoards = () => dispatch(boardsActions.clearSearch());
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
        history.push(`${boardItemType.uri}/create`);
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
                <Grid item xs={3}>
                    <Button
                        variant="outlined"
                        onClick={goToCreate}
                        size="small"
                        style={{ marginBottom: '25px' }}
                    >
                        Create New Board
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default BoardSearch;