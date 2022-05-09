import React, { useCallback, useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    itemSelectorHelpers,
    Loading,
    // Typeahead,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Snackbar from '@mui/material/Snackbar';
import Typography from '@mui/material/Typography';
import mrMasterActions from '../../actions/mrMasterActions';
import { mrMaster as mrMasterItemType } from '../../itemTypes';
import partsActions from '../../actions/partsActions';
import partActions from '../../actions/partActions';

import history from '../../history';
import Typeahead from './Typeahead';

function MaterialRequirementsOptions() {
    const [lastPart, setLastPart] = useState('');
    const [parts, setParts] = useState([]);
    const [showMessage, setShowMessage] = useState(false);
    const [message, setMessage] = useState(null);
    const mrMaster = useSelector(state => itemSelectorHelpers.getItem(state.mrMaster));
    const mrMasterLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrMaster)
    );

    const partsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.parts)
    ).map?.(c => ({
        id: c.partNumber,
        name: c.partNumber,
        partNumber: c.partNumber,
        description: c.description
    }));
    const partsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.parts)
    );

    const selectedPartDetails = useSelector(state => itemSelectorHelpers.getItem(state.part));

    const addToParts = useCallback(
        newPart => {
            if (!parts.some(p => p.id === newPart.partNumber)) {
                setParts([...parts, { ...newPart, id: newPart.partNumber }]);
            }
        },
        [parts]
    );

    const displayMessage = useCallback(
        newMessage => {
            setMessage(newMessage);
            setShowMessage(true);
        },
        [setShowMessage]
    );

    const handleClose = () => {
        setShowMessage(false);
        setMessage(null);
    };

    const dispatch = useDispatch();
    useEffect(() => dispatch(mrMasterActions.fetchByHref(mrMasterItemType.uri)), [dispatch]);

    useEffect(() => {
        if (selectedPartDetails) {
            if (selectedPartDetails.length > 0) {
                addToParts(selectedPartDetails[0]);
            } else {
                displayMessage(`Could not find part ${lastPart}`);
            }
        }
        dispatch(partActions.clearItem());
    }, [selectedPartDetails, addToParts, dispatch, displayMessage, lastPart]);

    const handleTextFieldChange = selectedPart => {
        setLastPart(selectedPart.id);
        if (selectedPart) {
            dispatch(partActions.fetchByHref(`/parts?searchTerm=${selectedPart.id}`));
        }
    };

    const handlePartChange = selectedPart => {
        addToParts(selectedPart);
    };

    const clear = () => {
        dispatch(partsActions.clearSearch());
    };

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={6}>
                    <Typography variant="h6">MR Options</Typography>
                </Grid>
                <Grid item xs={6}>
                    {mrMasterLoading ? (
                        <Loading />
                    ) : (
                        <Typography variant="subtitle1">Jobref: {mrMaster?.jobRef}</Typography>
                    )}
                </Grid>
                <Grid item xs={12}>
                    <Typeahead
                        label="Part"
                        title="Search for a part"
                        onSelect={handlePartChange}
                        items={partsSearchResults}
                        loading={partsSearchLoading}
                        fetchItems={searchTerm => dispatch(partsActions.search(searchTerm))}
                        clearSearch={() => clear()}
                        links={false}
                        openModalOnClick={false}
                        debounce={1000}
                        minimumSearchTermLength={2}
                        textFieldEntryAllowed
                        modal
                        searchButtonOnly
                        onTextFieldChange={handleTextFieldChange}
                    />
                </Grid>
                <Grid item xs={12}>
                    {parts?.map(p => (
                        <span key={p.id}>{p.id}</span>
                    ))}
                </Grid>
            </Grid>
            <Snackbar
                open={showMessage}
                autoHideDuration={3000}
                onClose={handleClose}
                message={message}
            />
        </Page>
    );
}

export default MaterialRequirementsOptions;
