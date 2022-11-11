import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    collectionSelectorHelpers,
    Dropdown,
    getItemError,
    ErrorCard,
    InputField,
    itemSelectorHelpers,
    Loading,
    Page,
    SaveBackCancelButtons,
    SnackbarMessage,
    Typeahead,
    utilities
} from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import history from '../history';
import config from '../config';
import partsActions from '../actions/partsActions';
import bomTypeChangeActions from '../actions/bomTypeChangeActions';
import partSuppliersActions from '../actions/partSuppliersActions';

function BomTypeChange() {
    const dispatch = useDispatch();

    const partsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.parts)
    ).map?.(c => ({
        id: c.id,
        name: c.partNumber,
        partNumber: c.partNumber,
        description: c.description
    }));

    const applicationState = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.suppliers)
    );

    const emailSentResult = useSelector(state => itemSelectorHelpers.getItem(state.sendEdiEmail));

    const partsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.parts)
    );

    const [part, setPart] = useState({ partNumber: 'click to set part' });
    const [bomTypeChange, setBomTypeChange] = useState(null);

    const loading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.bomTypeChange)
    );

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.bomTypeChange));

    const suppliers = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.partSuppliers)
    );

    const handlePartChange = selectedPart => {
        setPart(selectedPart);
    };

    const handleFieldChange = (propertyName, newValue) => {
        setBomTypeChange(a => ({ ...a, [propertyName]: newValue }));
    };

    const changeBomTypeUri = utilities.getHref(item, 'change-bom-type');

    const snackbarMessage = () => {
        if (emailSentResult) {
            if (emailSentResult.success) {
                return 'Email will be sent by server';
            }

            return emailSentResult.message;
        }
        return '';
    };

    const lookupPart = () => {
        if (part?.id) {
            dispatch(bomTypeChangeActions.fetch(part.id));
        }
    };

    const handleSaveClick = () => {
        dispatch(bomTypeChangeActions.clearErrorsForItem());
        dispatch(bomTypeChangeActions.add(bomTypeChange));
    };

    const NewBomTypeError = () => {
        if (bomTypeChange?.oldBomType) {
            if (!bomTypeChange.newBomType) {
                return 'please pick a new bom type';
            }
            if (bomTypeChange.newBomType === bomTypeChange.oldBomType) {
                return 'new bom type cannot be the same as old bom type';
            }
        }
        return null;
    };

    const NewSupplierError = () => {
        if (bomTypeChange?.oldBomType && bomTypeChange?.newBomType) {
            if (bomTypeChange.newBomType === 'C' && !bomTypeChange.newSupplierId) {
                return 'please pick a supplier for components';
            }
        }
        return null;
    };

    useEffect(() => {
        setBomTypeChange(item);
    }, [item]);

    useEffect(() => {
        if (part?.id) {
            dispatch(bomTypeChangeActions.fetch(part.id));
            dispatch(
                partSuppliersActions.searchWithOptions(null, `&partNumber=${part.partNumber}`)
            );
        }
    }, [part, dispatch]);

    const itemError = useSelector(reduxState => getItemError(reduxState, 'bomTypeChange'));

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <SnackbarMessage visible={emailSentResult} message={snackbarMessage()} />
            <Grid container spacing={3}>
                <Grid item xs={11}>
                    <Typography variant="h3">Change BOM Type</Typography>
                </Grid>
                {itemError && (
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={itemError.details} />
                    </Grid>
                )}
                {bomTypeChange?.oldBomType && (
                    <Grid item xs={1}>
                        {changeBomTypeUri ? (
                            <Tooltip title="You have permission to change BOM type">
                                <ModeEditIcon fontSize="large" color="primary" />
                            </Tooltip>
                        ) : (
                            <Tooltip title="You do not have permission to change BOM type">
                                <EditOffIcon fontSize="large" color="secondary" />
                            </Tooltip>
                        )}
                    </Grid>
                )}

                <Grid item xs={12}>
                    <Typeahead
                        label="Part"
                        title="Search for a part"
                        onSelect={handlePartChange}
                        items={partsSearchResults}
                        loading={partsSearchLoading}
                        fetchItems={searchTerm => dispatch(partsActions.search(searchTerm))}
                        clearSearch={() => dispatch(partsActions.clearSearch)}
                        value={`${part?.partNumber}`}
                        modal
                        links={false}
                        debounce={1000}
                        minimumSearchTermLength={2}
                    />
                </Grid>
                {loading ? (
                    <Loading />
                ) : (
                    <Grid container spacing={2} justifyContent="center">
                        <Grid item xs={4}>
                            <InputField
                                value={item?.partNumber}
                                label="Part Number"
                                disabled
                                propertyName="partNumber"
                            />
                        </Grid>
                        <Grid item xs={8}>
                            <InputField
                                value={item?.partDescription}
                                fullWidth
                                label="Description"
                                disabled
                                propertyName="partDescription"
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Dropdown
                                fullWidth
                                value={item?.oldBomType}
                                label="Old BOM Type"
                                propertyName="oldBomType"
                                disabled
                                items={[
                                    { id: 'A', displayText: 'Assembly' },
                                    { id: 'C', displayText: 'Component' },
                                    { id: 'P', displayText: 'Phantom' }
                                ]}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Dropdown
                                fullWidth
                                value={bomTypeChange?.newBomType}
                                label="New BOM Type"
                                propertyName="newBomType"
                                error={NewBomTypeError()}
                                helperText={NewBomTypeError()}
                                onChange={handleFieldChange}
                                items={[
                                    { id: 'A', displayText: 'Assembly' },
                                    { id: 'C', displayText: 'Component' },
                                    { id: 'P', displayText: 'Phantom' }
                                ]}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                value={item?.preferredSuppliedId}
                                fullWidth
                                label="Old Supplier Id"
                                disabled
                                propertyName="preferredSuppliedId"
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                value={item?.preferredSupplierName}
                                fullWidth
                                label="Name"
                                disabled
                                propertyName="preferredSupplierName"
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                value={item?.partCurrency}
                                fullWidth
                                label="Currency"
                                disabled
                                propertyName="partCurrency"
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Dropdown
                                value={bomTypeChange?.newSupplierId}
                                fullWidth
                                propertyName="newSupplierId"
                                label="Select a New Supplier"
                                items={suppliers.map(s => ({
                                    id: s.supplierId,
                                    displayText: s.supplierName
                                }))}
                                onChange={handleFieldChange}
                                helperText={NewSupplierError()}
                                error={NewSupplierError()}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <SaveBackCancelButtons
                                cancelClick={() => {
                                    lookupPart();
                                }}
                                saveDisabled={
                                    !bomTypeChange ||
                                    NewBomTypeError() ||
                                    NewSupplierError() ||
                                    !changeBomTypeUri
                                }
                                saveClick={handleSaveClick}
                            />
                        </Grid>
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default BomTypeChange;
