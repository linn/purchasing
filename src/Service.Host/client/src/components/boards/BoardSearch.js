import React, { useEffect, useState } from 'react';
import Grid from '@mui/material/Grid';

import { useSelector, useDispatch } from 'react-redux';
import { Page, collectionSelectorHelpers, Search } from '@linn-it/linn-form-components-library';
import boardsActions from '../../actions/boardsActions';
import history from '../../history';
import config from '../../config';
import { board as boardItemType } from '../../itemTypes';

function BoardSearch() {
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(boardsActions.fetchState());
    }, [dispatch]);

    const [board, setBoard] = useState(null);
    const searchBoards = searchTerm => dispatch(boardsActions.search(searchTerm));
    const clearSearchBoards = () => dispatch(boardsActions.clearSearch());
    const searchBoardsResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.boards)
    );
    const searchBoardsLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.boards)
    );

    const goToBoard = selectedBoard => {
        var a = selectedBoard;
        history.push(`${boardItemType.uri}/424`);
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <Search
                        propertyName="boardCode"
                        label="Select Board"
                        resultsInModal
                        resultLimit={100}
                        value={board}
                        handleValueChange={(_, b) => setBoard(b)}
                        search={searchBoards}
                        searchResults={searchBoardsResults.map(s => ({
                            ...s,
                            id: `${s.boardCode}`
                        }))}
                        loading={searchBoardsLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={newValue => {
                            goToBoard(newValue);
                        }}
                        clearSearch={clearSearchBoards}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default BoardSearch;
