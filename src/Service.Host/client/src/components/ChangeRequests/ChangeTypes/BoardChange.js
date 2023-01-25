import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    Dropdown,
    InputField,
    Loading,
    Typeahead,
    collectionSelectorHelpers,
    itemSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import boardsActions from '../../../actions/boardsActions';
import boardActions from '../../../actions/boardActions';
import history from '../../../history';

function BoardChange({ item, creating, handleFieldChange }) {
    const dispatch = useDispatch();

    const boardsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.boards)
    ).map?.(b => ({
        id: b.id,
        name: b.boardCode,
        boardCode: b.boardCode,
        description: b.description
    }));

    const boardsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.boards)
    );

    const board = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.board));
    const boardLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.board)
    );

    const handleBoardChange = selectedBoard => {
        handleFieldChange('boardCode', selectedBoard.boardCode);
        handleFieldChange('boardDescription', selectedBoard.description);
        dispatch(boardActions.fetch(selectedBoard.boardCode));
    };

    const revisionsList = () => {
        if (board?.layouts) {
            return board.layouts
                .map(l =>
                    l.revisions.map(r => ({ displayText: r.revisionCode, id: r.revisionCode }))
                )
                .flat();
        }
        return [];
    };

    return (
        <>
            {creating ? (
                <>
                    <Grid item xs={4}>
                        <Typeahead
                            label="Board"
                            title="Search for board"
                            onSelect={handleBoardChange}
                            items={boardsSearchResults}
                            loading={boardsSearchLoading}
                            fetchItems={searchTerm => dispatch(boardsActions.search(searchTerm))}
                            clearSearch={() => dispatch(boardsActions.clearSearch)}
                            value={item?.boardCode}
                            modal
                            links={false}
                            debounce={1000}
                            minimumSearchTermLength={2}
                        />
                    </Grid>
                    <Grid item xs={8}>
                        {boardLoading ? (
                            <Loading />
                        ) : (
                            <Dropdown
                                fullWidth
                                value={item?.revisionCode}
                                label="Revision"
                                items={revisionsList()}
                                propertyName="revisionCode"
                                onChange={handleFieldChange}
                            />
                        )}
                    </Grid>
                    <Grid item xs={12}>
                        <Typography>{item?.boardDescription}</Typography>
                    </Grid>
                </>
            ) : (
                <>
                    <Grid item xs={12}>
                        <Typography>Board Change</Typography>
                    </Grid>
                    <Grid item xs={4}>
                        <InputField
                            value={item?.boardCode}
                            label="Board"
                            propertyName="boardCode"
                            disabled
                        />
                    </Grid>
                    <Grid item xs={4}>
                        <InputField
                            value={item?.revisionCode}
                            label="RevisionCode"
                            propertyName="revisionCode"
                            disabled
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <Button
                            onClick={() => {
                                history.push(`/purchasing/boms/board/${item?.boardCode}`);
                            }}
                        >
                            {item?.boardDescription}
                        </Button>
                    </Grid>
                </>
            )}
        </>
    );
}

BoardChange.propTypes = {
    item: PropTypes.shape({
        boardCode: PropTypes.string,
        revisionCode: PropTypes.string,
        boardDescription: PropTypes.string
    }),
    creating: PropTypes.bool,
    handleFieldChange: PropTypes.func
};

BoardChange.defaultProps = {
    item: {
        boardCode: null,
        revisionCode: null,
        boardDescription: null
    },
    creating: false,
    handleFieldChange: null
};

export default BoardChange;
