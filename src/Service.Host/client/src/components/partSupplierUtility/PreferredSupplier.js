import React, { useCallback, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import {
    InputField,
    SaveBackCancelButtons,
    collectionSelectorHelpers,
    itemSelectorHelpers,
    Dropdown,
    userSelectors,
    Loading,
    getItemError,
    ErrorCard,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import preferredSupplierChangeActions from '../../actions/preferredSupplierChangeActions';
import currenciesActions from '../../actions/currenciesActions';
import partSuppliersActions from '../../actions/partSuppliersActions';
import priceChangeReasonsActions from '../../actions/priceChangeReasonsActions';
import partPriceConversionsActions from '../../actions/partPriceConversionsActions';
import { partPriceConversions } from '../../itemTypes';

function PreferredSupplier({
    partNumber,
    partDescription,
    oldSupplierId,
    oldSupplierName,
    oldPrice,
    baseOldPrice,
    oldCurrencyCode,
    close,
    refreshPart,
    partLoading,
    safetyCriticalPart,
    currentSupplier
}) {
    const dispatch = useDispatch();
    const postChange = body => dispatch(preferredSupplierChangeActions.add(body));
    useEffect(() => {
        dispatch(partSuppliersActions.searchWithOptions(null, `&partNumber=${partNumber}`));
        dispatch(currenciesActions.fetch());
        dispatch(priceChangeReasonsActions.fetch());
    }, [dispatch, partNumber]);

    const suppliers = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.partSuppliers)
    );

    const currencies = useSelector(reduxState =>
        collectionSelectorHelpers.getItems(reduxState.currencies)
    );

    const reasons = useSelector(reduxState =>
        collectionSelectorHelpers.getItems(reduxState.priceChangeReasons)
    );

    const currentUserNumber = useSelector(reduxState => userSelectors.getUserNumber(reduxState));

    const partPriceConversionsResult = useSelector(reduxState =>
        itemSelectorHelpers.getItem(reduxState.partPriceConversions)
    );

    const preferredSupplierChange = useSelector(reduxState =>
        itemSelectorHelpers.getItem(reduxState.preferredSupplierChange)
    );

    const preferredSupplierChangeLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.preferredSupplierChange)
    );

    const snackbarVisible = useSelector(reduxState =>
        itemSelectorHelpers.getSnackbarVisible(reduxState.preferredSupplierChange)
    );

    const itemError = useSelector(reduxState =>
        getItemError(reduxState, 'preferredSupplierChange')
    );

    const clearErrors = useCallback(
        () => dispatch(preferredSupplierChangeActions.clearErrorsForItem()),
        [dispatch]
    );

    const [formData, setFormData] = useState({
        newSupplierId: currentSupplier,
        changeReasonCode: 'NEW'
    });

    useEffect(() => {
        if (formData?.newSupplierId) {
            const selectedSupplier = suppliers?.find(
                x => x.supplierId === Number(formData.newSupplierId)
            );
            if (selectedSupplier) {
                dispatch(
                    partPriceConversionsActions.fetchByHref(
                        `${partPriceConversions.uri}?partNumber=${selectedSupplier.partNumber}&newPrice=${selectedSupplier.currencyUnitPrice}&newCurrency=${selectedSupplier.currencyCode}`
                    )
                );
                setFormData(d => ({
                    ...d,
                    newCurrency: selectedSupplier.currencyCode,
                    changeReasonCode: 'NEW'
                }));
            }
        }
    }, [formData?.newSupplierId, suppliers, dispatch]);

    const [saveDisabled, setSaveDisabled] = useState(true);

    useEffect(() => {
        if (partPriceConversionsResult) {
            setFormData(d => ({
                ...d,
                newPrice: partPriceConversionsResult.newPrice,
                baseNewPrice: partPriceConversionsResult.baseNewPrice
            }));
            setSaveDisabled(false);
        }
    }, [partPriceConversionsResult]);

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'newSupplierId') {
            setFormData(d => ({ ...d, [propertyName]: Number(newValue) }));
        } else {
            setFormData(d => ({ ...d, [propertyName]: newValue }));
        }
        setSaveDisabled(false);
    };

    useEffect(() => {
        if (preferredSupplierChange?.newSupplierId) {
            refreshPart();
            setFormData({});
        }
    }, [preferredSupplierChange, refreshPart]);

    useEffect(() => {
        clearErrors();
    }, [clearErrors]);

    useEffect(() => {
        setFormData({ newSupplierId: currentSupplier });
    }, [currentSupplier]);

    if (preferredSupplierChangeLoading || partLoading) {
        return (
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Loading />
                </Grid>
            </Grid>
        );
    }

    return (
        <Grid container spacing={3}>
            <SnackbarMessage
                visible={snackbarVisible}
                onClose={() => dispatch(preferredSupplierChangeActions.setSnackbarVisible(false))}
                message="Save Successful"
            />
            <Grid item xs={12}>
                <Typography variant="h6">Change Preferred Supplier</Typography>
            </Grid>
            {itemError && (
                <Grid item xs={12}>
                    <ErrorCard errorMessage={itemError.details} />
                </Grid>
            )}
            {safetyCriticalPart && (
                <Grid item xs={12}>
                    <Typography variant="subtitle" color="secondary">
                        WARNING: This is a safety critical part. Please ensure new part number has
                        been verified for this function.
                    </Typography>
                </Grid>
            )}
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={partNumber}
                    label="Part Number"
                    disabled
                    propertyName="partNumber"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={partDescription}
                    label="Description"
                    propertyName="partDescription"
                    onChange={() => {}}
                    disabled
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={oldSupplierId}
                    label="Old Supplier"
                    propertyName="oldSupplierId"
                    onChange={() => {}}
                    disabled
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={oldSupplierName}
                    label="Name"
                    propertyName="oldSupplierName"
                    onChange={() => {}}
                    disabled
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={oldPrice}
                    label="Old Price"
                    propertyName="oldPrice"
                    onChange={() => {}}
                    disabled
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={baseOldPrice}
                    label="Base Old Price"
                    propertyName="baseOldPrice"
                    onChange={() => {}}
                    disabled
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={oldCurrencyCode}
                    label="Old Currency"
                    propertyName="oldCurrencyCode"
                    onChange={() => {}}
                    disabled
                />
            </Grid>
            <Grid item xs={6}>
                <Dropdown
                    value={formData?.newSupplierId}
                    propertyName="newSupplierId"
                    label="Select a New Supplier"
                    items={suppliers.map(s => ({ id: s.supplierId, displayText: s.supplierName }))}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6} />
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={formData?.newPrice}
                    label="New Price"
                    propertyName="newPrice"
                    type="number"
                    disabled
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={formData?.baseNewPrice}
                    label="Base New Price"
                    propertyName="baseNewPrice"
                    type="number"
                    disabled
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <Dropdown
                    value={formData?.newCurrency}
                    propertyName="newCurrency"
                    label="New Currency"
                    items={currencies.map(s => ({
                        id: s.code,
                        displayText: s.name
                    }))}
                    onChange={handleFieldChange}
                    disabled
                />
            </Grid>
            <Grid item xs={6}>
                <Dropdown
                    value={formData?.changeReasonCode}
                    propertyName="changeReasonCode"
                    label="Reason"
                    items={reasons.map(s => ({
                        id: s.reasonCode,
                        displayText: s.description
                    }))}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6} />

            <Grid item xs={12}>
                <InputField
                    fullWidth
                    value={formData?.remarks}
                    label="Remarks"
                    propertyName="remarks"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={12}>
                <SaveBackCancelButtons
                    cancelClick={close}
                    backClick={close}
                    saveDisabled={saveDisabled}
                    saveClick={() => {
                        clearErrors();
                        setSaveDisabled(true);
                        postChange({
                            partNumber,
                            oldSupplierId,
                            oldPrice,
                            baseOldPrice,
                            oldCurrencyCode,
                            ...formData,
                            changedById: Number(currentUserNumber)
                        });
                    }}
                />
            </Grid>
        </Grid>
    );
}

PreferredSupplier.propTypes = {
    partNumber: PropTypes.string,
    partDescription: PropTypes.string,
    oldSupplierId: PropTypes.number,
    oldSupplierName: PropTypes.string,
    oldPrice: PropTypes.number,
    baseOldPrice: PropTypes.number,
    oldCurrencyCode: PropTypes.string,
    close: PropTypes.func.isRequired,
    refreshPart: PropTypes.func.isRequired,
    partLoading: PropTypes.bool,
    safetyCriticalPart: PropTypes.bool,
    currentSupplier: PropTypes.number.isRequired
};

PreferredSupplier.defaultProps = {
    partNumber: null,
    partDescription: null,
    oldPrice: null,
    baseOldPrice: null,
    oldCurrencyCode: null,
    partLoading: false,
    safetyCriticalPart: false,
    oldSupplierId: null,
    oldSupplierName: null
};

export default PreferredSupplier;
