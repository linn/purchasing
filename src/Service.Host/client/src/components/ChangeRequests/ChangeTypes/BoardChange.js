import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    Dropdown,
    InputField,
    Loading,
    Search,
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

    const setBoardWithoutSearch = () => {
        dispatch(boardActions.fetch(item.boardCode));
    };

    const revisionsList = () => {
        if (board?.layouts) {
            const list = board.layouts
                .map(l =>
                    l.revisions.map(r => ({
                        displayText: r.revisionCode,
                        id: r.revisionCode,
                        layoutSequence: r.layoutSequence,
                        versionNumber: r.versionNumber,
                        pcasPartNumber: r.pcasPartNumber
                    }))
                )
                .flat()
                .sort((a, b) => {
                    if (a.layoutSequence < b.layoutSequence) {
                        return -1;
                    }
                    if (a.layoutSequence > b.layoutSequence) {
                        return 1;
                    }
                    if (a.versionNumber < b.versionNumber) {
                        return -1;
                    }
                    if (a.versionNumber > b.versionNumber) {
                        return 1;
                    }
                    return 0;
                });
            return list;
        }
        return [];
    };

    const handleRevisionChange = (propertyName, newValue) => {
        if (newValue) {
            const revisions = revisionsList();
            const revision = revisions.find(r => r.id === newValue);

            handleFieldChange('revisionCode', newValue);
            if (revision?.pcasPartNumber) {
                handleFieldChange('oldPartNumber', revision.pcasPartNumber);
                handleFieldChange('newPartNumber', revision.pcasPartNumber);
            }
        }
    };

    return (
        <>
            {creating ? (
                <>
                    <Grid item xs={4}>
                        <Search
                            propertyName="boardCode"
                            label="Board"
                            helperText="use Enter to search or Tab to proceed"
                            handleValueChange={(_, newVal) =>
                                handleFieldChange('boardCode', newVal)
                            }
                            onResultSelect={newValue => {
                                handleBoardChange(newValue);
                            }}
                            clearSearch={() => {}}
                            onKeyPressFunctions={[{ keyCode: 9, action: setBoardWithoutSearch }]}
                            searchResults={boardsSearchResults}
                            loading={boardsSearchLoading}
                            search={searchTerm => dispatch(boardsActions.search(searchTerm))}
                            value={item?.boardCode}
                            resultsInModal
                        />
                    </Grid>
                    <Grid item xs={8}>
                        {item?.boardCode ? (
                            <>
                                {boardLoading ? (
                                    <Loading />
                                ) : (
                                    <>
                                        <Dropdown
                                            fullWidth
                                            value={item?.revisionCode}
                                            label="Revision"
                                            items={revisionsList()}
                                            propertyName="revisionsList()"
                                            onChange={handleRevisionChange}
                                        />

                                        <Button
                                            onClick={() => {
                                                history.push(
                                                    `/purchasing/boms/boards/${item?.boardCode}`
                                                );
                                            }}
                                        >
                                            New Layout / Revision
                                        </Button>
                                    </>
                                )}
                            </>
                        ) : (
                            <Button
                                onClick={() => {
                                    history.push('/purchasing/boms/boards/create');
                                }}
                                style={{ marginTop: '30px' }}
                            >
                                Create New Board
                            </Button>
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
                    <Grid item xs={4}>
                        <InputField
                            value={item?.newPartNumber}
                            label="PCAS Part#"
                            propertyName="newPartNumber"
                            disabled
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <Button
                            onClick={() => {
                                history.push(`/purchasing/boms/boards/${item?.boardCode}`);
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
        boardDescription: PropTypes.string,
        newPartNumber: PropTypes.string
    }),
    creating: PropTypes.bool,
    handleFieldChange: PropTypes.func
};

BoardChange.defaultProps = {
    item: {
        boardCode: null,
        revisionCode: null,
        boardDescription: null,
        newPartNumber: null
    },
    creating: false,
    handleFieldChange: null
};

export default BoardChange;
