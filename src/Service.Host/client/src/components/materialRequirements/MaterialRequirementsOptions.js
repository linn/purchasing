import React, { useCallback, useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    itemSelectorHelpers,
    Loading,
    Typeahead,
    Dropdown,
    collectionSelectorHelpers,
    InputField
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Snackbar from '@mui/material/Snackbar';
import Typography from '@mui/material/Typography';
import { DataGrid } from '@mui/x-data-grid';
import { Button } from '@mui/material';
import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';

import mrMasterActions from '../../actions/mrMasterActions';
import {
    mrMaster as mrMasterItemType,
    mrReportOptions as mrReportOptionsItemType
} from '../../itemTypes';
import partsActions from '../../actions/partsActions';
import partActions from '../../actions/partActions';
import mrReportOptionsActions from '../../actions/mrReportOptionsActions';
import mrReportActions from '../../actions/mrReportActions';
import suppliersActions from '../../actions/suppliersActions';

import history from '../../history';

function MaterialRequirementsOptions() {
    const [lastPart, setLastPart] = useState('');
    const [typeaheadPart, setTypeaheadPart] = useState(null);
    const [parts, setParts] = useState([]);
    const [showMessage, setShowMessage] = useState(false);
    const [supplier, setSupplier] = useState(null);
    const [message, setMessage] = useState(null);
    const [partSelector, setPartSelector] = useState('Select Parts');
    const [stockLevelSelector, setStockLevelSelector] = useState('All');
    const [orderBySelector, setOrderBySelector] = useState('supplier/part');
    const mrMaster = useSelector(state => itemSelectorHelpers.getItem(state.mrMaster));
    const mrMasterLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrMaster)
    );
    const mrReportOptions = useSelector(state =>
        itemSelectorHelpers.getItem(state.mrReportOptions)
    );
    const mrReportOptionsLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrReportOptions)
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

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers)
    )?.map(c => ({
        id: c.id,
        name: c.id.toString(),
        description: c.name
    }));
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );

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
    useEffect(
        () => dispatch(mrReportOptionsActions.fetchByHref(mrReportOptionsItemType.uri)),
        [dispatch]
    );

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

    const handleSupplierChange = selectedsupplier => {
        setSupplier(selectedsupplier);
    };

    const handleSupplierReturn = () => {};

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

    const getOptionTag = (options, option) => options?.find(a => a.option === option)?.dataTag;

    const runReport = () => {
        dispatch(mrReportActions.clearItem());
        const body = {
            typeOfReport: 'MR',
            partSelector,
            jobRef: mrMaster.jobRef,
            partNumbers: parts.map(p => p.id),
            supplierId: supplier?.id,
            stockLevelSelector,
            orderBySelector
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

    const handleSetTypeaheadPart = (_, part) => {
        setTypeaheadPart(part);
    };

    const handleSetSupplier = (_, supp) => {
        setSupplier({ id: supp });
    };

    const notReadyToRun = () => {
        const tag = getOptionTag(mrReportOptions?.partSelectorOptions, partSelector);
        if (!tag || (tag === 'parts' && parts.length === 0)) {
            return true;
        }

        if (tag === 'supplier' && !supplier?.id) {
            return true;
        }

        return false;
    };

    const handlePartOptionChange = value => {
        setPartSelector(value);

        const tag = getOptionTag(mrReportOptions?.partSelectorOptions, value);
        if (tag === 'parts') {
            setStockLevelSelector('All');
        } else if (tag === 'planner') {
            setStockLevelSelector('0-4');
        }
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
                <Grid item xs={12}>
                    <Dropdown
                        propertyName="Parts Options"
                        label="Parts Options"
                        value={partSelector}
                        items={mrReportOptions?.partSelectorOptions
                            ?.sort((a, b) => a.displaySequence - b.displaySequence)
                            .map(e => ({
                                displayText: e.displayText,
                                id: e.option
                            }))}
                        optionsLoading={mrReportOptionsLoading}
                        onChange={(_, value) => handlePartOptionChange(value)}
                    />
                </Grid>
                {getOptionTag(mrReportOptions?.partSelectorOptions, partSelector) === 'parts' && (
                    <>
                        <Grid item xs={6}>
                            <Typeahead
                                label="Select Part (with <Return> or search using icon)"
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
                    </>
                )}
                <Grid item xs={12}>
                    <Dropdown
                        propertyName="Stock Level Options"
                        label="Stock Level Options"
                        value={stockLevelSelector}
                        items={mrReportOptions?.stockLevelOptions
                            ?.sort((a, b) => a.displaySequence - b.displaySequence)
                            .map(e => ({
                                displayText: e.displayText,
                                id: e.option
                            }))}
                        optionsLoading={mrReportOptionsLoading}
                        onChange={(_, value) => setStockLevelSelector(value)}
                    />
                </Grid>
                <Grid item xs={12}>
                    <Dropdown
                        propertyName="Order By"
                        label="Order By"
                        value={orderBySelector}
                        items={mrReportOptions?.orderByOptions
                            ?.sort((a, b) => a.displaySequence - b.displaySequence)
                            .map(e => ({
                                displayText: e.displayText,
                                id: e.option
                            }))}
                        optionsLoading={mrReportOptionsLoading}
                        onChange={(_, value) => setOrderBySelector(value)}
                    />
                </Grid>
                <Grid item xs={4}>
                    <Typeahead
                        label="Supplier"
                        title="Search for a supplier"
                        onSelect={handleSupplierChange}
                        items={suppliersSearchResults}
                        loading={suppliersSearchLoading}
                        fetchItems={searchTerm => dispatch(suppliersActions.search(searchTerm))}
                        clearSearch={() => dispatch(suppliersActions.clearSearch)}
                        value={supplier?.id}
                        openModalOnClick={false}
                        modal
                        links={false}
                        debounce={1000}
                        handleFieldChange={handleSetSupplier}
                        handleReturnPress={handleSupplierReturn}
                        minimumSearchTermLength={2}
                    />
                </Grid>
                <Grid item xs={6}>
                    <InputField
                        disabled
                        value={supplier?.description}
                        fullWidth
                        label="Supplier Name"
                    />
                </Grid>
                <Grid item xs={2} />
                <Grid item xs={12}>
                    <Button
                        variant="outlined"
                        onClick={runReport}
                        disabled={
                            mrMasterLoading ||
                            selectectPartLoading ||
                            mrReportOptionsLoading ||
                            notReadyToRun()
                        }
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
