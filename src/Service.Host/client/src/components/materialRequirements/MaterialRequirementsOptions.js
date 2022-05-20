import React, { useCallback, useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    itemSelectorHelpers,
    Loading,
    Typeahead,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Snackbar from '@mui/material/Snackbar';
import Typography from '@mui/material/Typography';
import { DataGrid } from '@mui/x-data-grid';
import { Button } from '@mui/material';
import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';

import mrMasterActions from '../../actions/mrMasterActions';
import { mrMaster as mrMasterItemType } from '../../itemTypes';
import partsActions from '../../actions/partsActions';
import partActions from '../../actions/partActions';

import history from '../../history';

function MaterialRequirementsOptions() {
    const [lastPart, setLastPart] = useState('');
    const [typeaheadPart, setTypeaheadPart] = useState(null);
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
    const selectectPartLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.part)
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

    const removeFromParts = partToRemove => {
        setParts(parts.filter(p => p.id !== partToRemove));
    };

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
            dispatch(
                partActions.fetchByHref(`/parts?searchTerm=${selectedPart.id}&exactOnly=true`)
            );
        }
        setTypeaheadPart(null);
    };

    const handlePartChange = selectedPart => {
        addToParts(selectedPart);
    };

    const clear = () => {
        dispatch(partsActions.clearSearch());
    };

    const handleDeleteRow = params => {
        if (params.id) {
            removeFromParts(params.id);
        }
    };

    const runReport = () => {
        const body = {
            typeOfReport: 'MR',
            partSelector: 'Select Parts',
            jobRef: mrMaster.jobRef,
            partNumbers: parts.map(p => p.id)
        };
        history.push('/purchasing/material-requirements/report', body);
    };

    const selectedPartsColumns = [
        { field: 'partNumber', headerName: 'Part', minWidth: 140 },
        { field: 'description', headerName: 'Description', minWidth: 300 },
        {
            field: 'delete',
            headerName: ' ',
            width: 50,
            renderCell: params => (
                <IconButton
                    aria-label="delete"
                    size="small"
                    onClick={() => handleDeleteRow(params)}
                >
                    <DeleteIcon fontSize="inherit" />
                </IconButton>
            )
        }
    ];

    const handleSetTypeaheadPart = (val, val3) => {
        setTypeaheadPart(val3);
    };

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={10}>
                    <Typography variant="h6">MR Options</Typography>
                </Grid>
                <Grid item xs={2}>
                    {mrMasterLoading ? (
                        <Loading />
                    ) : (
                        <Typography variant="subtitle1">Jobref: {mrMaster?.jobRef}</Typography>
                    )}
                </Grid>
                <Grid item xs={6}>
                    <Typeahead
                        label="Part"
                        title="Search for a part"
                        onSelect={handlePartChange}
                        items={partsSearchResults}
                        loading={partsSearchLoading}
                        fetchItems={searchTerm => dispatch(partsActions.search(searchTerm))}
                        clearSearch={() => clear()}
                        links={false}
                        value={typeaheadPart}
                        openModalOnClick={false}
                        debounce={1000}
                        minimumSearchTermLength={2}
                        modal
                        handleFieldChange={handleSetTypeaheadPart}
                        handleReturnPress={handleTextFieldChange}
                    />
                </Grid>
                <Grid item xs={6}>
                    <Typography variant="subtitle1">Selected Parts</Typography>
                    <DataGrid
                        rows={parts}
                        columns={selectedPartsColumns}
                        pageSize={5}
                        rowsPerPageOptions={[5]}
                        density="compact"
                        rowHeight={34}
                        headerHeight={34}
                        autoHeight
                        loading={selectectPartLoading}
                        hideFooter
                    />
                </Grid>
                <Grid item xs={12}>
                    <Button
                        variant="outlined"
                        onClick={runReport}
                        disabled={mrMasterLoading || selectectPartLoading}
                    >
                        Run Report
                    </Button>
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
