import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    InputField,
    CheckboxWithLabel,
    Search,
    collectionSelectorHelpers,
    utilities
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import partsActions from '../../../actions/partsActions';
import history from '../../../history';

function ReplaceChange({ item, creating, handleFieldChange }) {
    const dispatch = useDispatch();

    const partsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.parts)
    ).map?.(c => ({
        id: c.id,
        name: c.partNumber,
        partNumber: c.partNumber,
        description: c.description
    }));

    const partsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.parts)
    );

    const handleOldPartChange = partNumber => {
        handleFieldChange('oldPartNumber', partNumber);
    };

    const handleNewPartChange = partNumber => {
        handleFieldChange('newPartNumber', partNumber);
    };

    const replaceUri = utilities.getHref(item, 'replace');

    return (
        <>
            {creating ? (
                <>
                    <Grid item xs={3}>
                        <Search
                            propertyName="oldPartNumber"
                            label="Old Part"
                            helperText="use Enter to search"
                            handleValueChange={(_, newVal) => handleOldPartChange(newVal)}
                            onResultSelect={newValue => {
                                handleOldPartChange(newValue.partNumber);
                            }}
                            clearSearch={() => dispatch(partsActions.clearSearch())}
                            searchResults={partsSearchResults}
                            loading={partsSearchLoading}
                            search={searchTerm => dispatch(partsActions.search(searchTerm))}
                            value={item?.oldPartNumber}
                            resultsInModal
                        />
                    </Grid>
                    <Grid item xs={1} />
                    <Grid item xs={3}>
                        <Search
                            propertyName="newPartNumber"
                            label="New Part"
                            helperText="use Enter to search"
                            handleValueChange={(_, newVal) => handleNewPartChange(newVal)}
                            onResultSelect={newValue => {
                                handleNewPartChange(newValue.partNumber);
                            }}
                            clearSearch={() => dispatch(partsActions.clearSearch())}
                            searchResults={partsSearchResults}
                            loading={partsSearchLoading}
                            search={searchTerm => dispatch(partsActions.search(searchTerm))}
                            value={item?.newPartNumber}
                            resultsInModal
                        />
                    </Grid>
                </>
            ) : (
                <>
                    <Grid item xs={12}>
                        <Typography>Replace Change</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            value={item?.oldPartNumber}
                            label="Old Part"
                            propertyName="oldPartNumber"
                            disabled
                        />
                        <Typography>{item?.oldPartDescription}</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            value={item?.newPartNumber}
                            label="New Part"
                            propertyName="newPartNumber"
                            disabled
                        />
                        <Typography>{item?.newPartDescription}</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <Button
                            disabled={!replaceUri}
                            onClick={() => {
                                history.push(
                                    `/purchasing/change-requests/replace?documentNumber=${item?.documentNumber}`
                                );
                            }}
                        >
                            Replace / Add
                        </Button>
                    </Grid>
                    <Grid item xs={6}>
                        <CheckboxWithLabel
                            label="Global Replace"
                            checked={item?.globalReplace}
                            disabled
                        />
                    </Grid>
                </>
            )}
        </>
    );
}

ReplaceChange.propTypes = {
    item: PropTypes.shape({
        documentNumber: PropTypes.number,
        oldPartNumber: PropTypes.string,
        oldPartDescription: PropTypes.string,
        newPartNumber: PropTypes.string,
        newPartDescription: PropTypes.string,
        globalReplace: PropTypes.bool
    }),
    creating: PropTypes.bool,
    handleFieldChange: PropTypes.func
};

ReplaceChange.defaultProps = {
    item: {
        documentNumber: null,
        oldPartNumber: null,
        oldPartDescription: null,
        newPartNumber: null,
        newPartDescription: null,
        globalReplace: false
    },
    creating: false,
    handleFieldChange: null
};

export default ReplaceChange;
