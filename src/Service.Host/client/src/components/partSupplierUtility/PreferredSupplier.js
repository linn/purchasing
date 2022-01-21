import React, { useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import {
    InputField,
    SaveBackCancelButtons,
    Typeahead,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import preferredSupplierChangeActions from '../../actions/preferredSupplierChangeActions';
import suppliersActions from '../../actions/suppliersActions';

function PreferredSupplier({
    partNumber,
    partDescription,
    oldSupplierId,
    oldSupplierName,
    oldPrice,
    baseOldPrice,
    oldCurrencyCode
}) {
    const dispatch = useDispatch();
    const postChange = body => dispatch(preferredSupplierChangeActions.add(body));
    const searchSuppliers = searchTerm => dispatch(suppliersActions.search(searchTerm));
    const suppliersSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.suppliers, 100, 'id', 'name', 'name')
    );
    const suppliersSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.suppliers)
    );

    const [formData, setFormData] = useState({});

    const handleFieldChange = (propertyName, newValue) => {
        setFormData(d => ({ ...d, [propertyName]: newValue }));
    };

    return (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                <Typography variant="h6">Change Preferred Supplier</Typography>
            </Grid>
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
            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('newSupplierId', newValue.id);
                        handleFieldChange('newSupplierName', newValue.name);
                    }}
                    label="New Supplier"
                    modal
                    propertyName="newSupplierId"
                    items={suppliersSearchResults}
                    value={formData?.newSupplierId?.toString()}
                    loading={suppliersSearchLoading}
                    fetchItems={searchSuppliers}
                    links={false}
                    text
                    clearSearch={() => {}}
                    placeholder="Search Suppliers"
                    minimumSearchTermLength={3}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={formData?.newSupplierName}
                    label="Name"
                    propertyName="newSupplierName"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={12}>
                <SaveBackCancelButtons
                    saveClick={() =>
                        postChange({
                            partNumber,
                            oldSupplierId,
                            oldPrice,
                            baseOldPrice,
                            oldCurrencyCode,
                            ...formData
                        })
                    }
                />
            </Grid>
        </Grid>
    );
}

PreferredSupplier.propTypes = {
    partNumber: PropTypes.string.isRequired,
    partDescription: PropTypes.string.isRequired,
    oldSupplierId: PropTypes.number.isRequired,
    oldSupplierName: PropTypes.string.isRequired,
    oldPrice: PropTypes.number,
    baseOldPrice: PropTypes.number,
    oldCurrencyCode: PropTypes.number
};

PreferredSupplier.defaultProps = {
    oldPrice: null,
    baseOldPrice: null,
    oldCurrencyCode: null
};

export default PreferredSupplier;
