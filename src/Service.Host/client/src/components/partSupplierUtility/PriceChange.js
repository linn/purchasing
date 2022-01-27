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

function PriceChange({
    partNumber,
    partDescription,
    supplierId,
    supplierName,
    oldPrice,
    baseOldPrice,
    oldCurrencyCode,
    close
}) {
    const dispatch = useDispatch();
    const postChange = body => dispatch(preferredSupplierChangeActions.add(body));
    useEffect(() => {
        dispatch(
            partSuppliersActions.searchWithOptions(null, `&partNumber=${partNumber}&supplierName=`)
        );
        dispatch(currenciesActions.fetch());
        dispatch(priceChangeReasonsActions.fetch());
    }, [dispatch, partNumber]);

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

    // const preferredSupplierChange = useSelector(reduxState =>
    //     itemSelectorHelpers.getItem(reduxState.preferredSupplierChange)
    // );

    // const preferredSupplierChangeLoading = useSelector(reduxState =>
    //     itemSelectorHelpers.getItemLoading(reduxState.preferredSupplierChange)
    // );

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

    const [formData, setFormData] = useState({ newCurrency: oldCurrencyCode });

    // useEffect(() => {
    //     if (formData?.newPrice && formData?.newCurrency) {
    //         dispatch(
    //             partPriceConversionsActions.fetchByHref(
    //                 `${partPriceConversions.uri}?partNumber=${partNumber}&newPrice=${formData?.newPrice}&newCurrency=${formData?.newCurrency}`
    //             )
    //         );
    //     }
    // }, [formData?.newPrice, formData?.newCurrency, partNumber, supplierId, dispatch]);

    useEffect(() => {
        if (partPriceConversionsResult) {
            setFormData(d => ({
                ...d,
                baseNewPrice: partPriceConversionsResult.baseNewPrice
            }));
        }
    }, [partPriceConversionsResult]);

    const [saveDisabled, setSaveDisabled] = useState(true);

    const tryFetchPriceConversions = newCurrency => {
        if (formData?.newPrice && formData?.newCurrency) {
            dispatch(
                partPriceConversionsActions.fetchByHref(
                    `${partPriceConversions.uri}?partNumber=${partNumber}&newPrice=${
                        formData?.newPrice
                    }&newCurrency=${newCurrency ?? formData?.newCurrency}&ledger=SL&round=TRUE`
                )
            );
        }
    };

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'newCurrency') {
            tryFetchPriceConversions(newValue);
        }
        if (propertyName === 'newSupplierId') {
            setFormData(d => ({ ...d, [propertyName]: Number(newValue) }));
        } else {
            setFormData(d => ({ ...d, [propertyName]: newValue }));
        }
        setSaveDisabled(false);
    };

    useEffect(() => {
        clearErrors();
    }, [clearErrors]);

    // useEffect(() => {
    //     setFormData({});
    // }, []);

    // if (preferredSupplierChangeLoading) {
    //     return (
    //         <Grid container spacing={3}>
    //             <Grid item xs={12}>
    //                 <Loading />
    //             </Grid>
    //         </Grid>
    //     );
    // }

    return (
        <Grid container spacing={3}>
            <SnackbarMessage
                visible={snackbarVisible}
                onClose={() => dispatch(preferredSupplierChangeActions.setSnackbarVisible(false))}
                message="Save Successful"
            />
            <Grid item xs={12}>
                <Typography variant="h6">Change Prices</Typography>
            </Grid>
            {itemError && (
                <Grid item xs={12}>
                    <ErrorCard errorMessage={itemError.details} />
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
                    value={supplierId}
                    label="Supplier"
                    propertyName="supplierId"
                    onChange={() => {}}
                    disabled
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={supplierName}
                    label="Name"
                    propertyName="supplierName"
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
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={formData?.newPrice}
                    textFieldProps={{
                        onBlur: () => tryFetchPriceConversions()
                    }}
                    label="New Price"
                    propertyName="newPrice"
                    type="number"
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
                            supplierId,
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

PriceChange.propTypes = {
    partNumber: PropTypes.string,
    partDescription: PropTypes.string,
    supplierId: PropTypes.number,
    supplierName: PropTypes.string,
    oldPrice: PropTypes.number,
    baseOldPrice: PropTypes.number,
    oldCurrencyCode: PropTypes.string,
    close: PropTypes.func.isRequired
};

PriceChange.defaultProps = {
    partNumber: null,
    partDescription: null,
    oldPrice: null,
    baseOldPrice: null,
    oldCurrencyCode: null,
    supplierId: null,
    supplierName: null
};

export default PriceChange;
