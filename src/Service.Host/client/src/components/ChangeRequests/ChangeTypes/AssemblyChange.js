import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    InputField,
    Typeahead,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import partsActions from '../../../actions/partsActions';

function AssemblyChange({ item, creating, handleFieldChange }) {
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

    const handlePartChange = selectedPart => {
        handleFieldChange('newPartNumber', selectedPart.partNumber);
    };

    return (
        <>
            {creating ? (
                <Grid item xs={12}>
                    <Typeahead
                        label="Part"
                        title="Search for a part"
                        onSelect={handlePartChange}
                        items={partsSearchResults}
                        loading={partsSearchLoading}
                        fetchItems={searchTerm => dispatch(partsActions.search(searchTerm))}
                        clearSearch={() => dispatch(partsActions.clearSearch)}
                        value={item?.newPartNumber}
                        modal
                        links={false}
                        debounce={1000}
                        minimumSearchTermLength={2}
                    />
                </Grid>
            ) : (
                <>
                    <Grid item xs={4}>
                        <Typography>Assembly Change</Typography>
                    </Grid>
                    <Grid item xs={8}>
                        <InputField
                            value={item?.newPartNumber}
                            label="Part Number"
                            propertyName="proposedBy"
                            disabled
                        />
                        <Typography>{item?.newPartDescription}</Typography>
                    </Grid>
                </>
            )}
        </>
    );
}

AssemblyChange.propTypes = {
    item: PropTypes.shape({
        newPartNumber: PropTypes.string,
        newPartDescription: PropTypes.string
    }),
    creating: PropTypes.bool,
    handleFieldChange: PropTypes.func
};

AssemblyChange.defaultProps = {
    item: {
        newPartNumber: null,
        newPartDescription: null
    },
    creating: false,
    handleFieldChange: null
};

export default AssemblyChange;
