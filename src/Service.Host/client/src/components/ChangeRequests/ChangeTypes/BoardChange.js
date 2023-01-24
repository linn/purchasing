import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    InputField,
    Typeahead,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import boardsActions from '../../../actions/boardsActions';
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

    const handleBoardChange = selectedBoard => {
        handleFieldChange('boardCode', selectedBoard.boardCode);
        handleFieldChange('boardDescription', selectedBoard.description);
    };

    return (
        <>
            {creating ? (
                <Grid item xs={12}>
                    <Typeahead
                        label="Board"
                        title="Search for a board"
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
                    <Typography>{item?.boardDescription}</Typography>
                </Grid>
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
