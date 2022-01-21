import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import {
    InputField,
    SaveBackCancelButtons,
    collectionSelectorHelpers,
    Dropdown,
    userSelectors
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import preferredSupplierChangeActions from '../../actions/preferredSupplierChangeActions';
import partSuppliersActions from '../../actions/partSuppliersActions';

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
    useEffect(() => {
        dispatch(
            partSuppliersActions.searchWithOptions(null, `&partNumber=${partNumber}&supplierName=`)
        );
    }, [dispatch, partNumber]);

    const suppliers = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.partSuppliers)
    );

    const currentUserNumber = useSelector(reduxState => userSelectors.getUserNumber(reduxState));

    const [formData, setFormData] = useState({});

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'newSupplierId') {
            setFormData(d => ({ ...d, [propertyName]: Number(newValue) }));
        } else {
            setFormData(d => ({ ...d, [propertyName]: newValue }));
        }
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
            <Grid item xs={6}>
                <Dropdown
                    value={formData?.newSupplierId}
                    propertyName="newSupplierId"
                    label="newSupplier"
                    items={suppliers.map(s => ({ id: s.supplierId, displayText: s.supplierName }))}
                    allowNoValue
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6} />

            <Grid item xs={12}>
                <SaveBackCancelButtons
                    saveClick={() =>
                        postChange({
                            partNumber,
                            oldSupplierId,
                            oldPrice,
                            baseOldPrice,
                            oldCurrencyCode,
                            ...formData,
                            changedById: Number(currentUserNumber)
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
