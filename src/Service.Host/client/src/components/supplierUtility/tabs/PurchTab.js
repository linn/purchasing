import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';

import { useSelector, useDispatch } from 'react-redux';

import {
    InputField,
    collectionSelectorHelpers,
    Dropdown,
    Typeahead
} from '@linn-it/linn-form-components-library';

import suppliersActions from '../../../actions/suppliersActions';
import currenciesActions from '../../../actions/currenciesActions';
import partCategoriesActions from '../../../actions/partCategoriesActions';

function PurchTab({
    handleFieldChange,
    partCategory,
    partCategoryDescription,
    orderHold,
    notesForBuyer,
    deliveryDay,
    refersToFcId,
    refersToFcName,
    pmDeliveryDaysGrace,
    holdLink,
    openHoldDialog
}) {
    const reduxDispatch = useDispatch();
    useEffect(() => {
        reduxDispatch(currenciesActions.fetch());
    }, [reduxDispatch]);

    const searchSuppliers = searchTerm => reduxDispatch(suppliersActions.search(searchTerm));
    const suppliersSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(reduxState.suppliers, 100, 'id', 'name', 'name')
    );
    const suppliersSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.suppliers)
    );

    const searchPartCategories = searchTerm =>
        reduxDispatch(partCategoriesActions.search(searchTerm));
    const partCategoriesSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(
            reduxState.partCategories,
            100,
            'category',
            'description',
            'description'
        )
    );
    const partCategoriesSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.partCategories)
    );
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('partCategory', newValue.id);
                        handleFieldChange('partCategoryDescription', newValue.description);
                    }}
                    label="Part Category"
                    modal
                    propertyName="partCategory"
                    items={partCategoriesSearchResults}
                    value={partCategory}
                    loading={partCategoriesSearchLoading}
                    fetchItems={searchPartCategories}
                    links={false}
                    text
                    clearSearch={() => {}}
                    placeholder="Search Part Categories"
                    minimumSearchTermLength={3}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={partCategoryDescription}
                    label="Desc"
                    propertyName="partCategoryDescription"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('refersToFcId', newValue.id);
                        handleFieldChange('refersToFcName', newValue.description);
                    }}
                    label="Refers to Fc Supplier"
                    modal
                    propertyName="refersToFcId"
                    items={suppliersSearchResults}
                    value={refersToFcId}
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
                    value={refersToFcName}
                    label="Name"
                    propertyName="refersToFcName"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={orderHold}
                    label="On Hold?"
                    propertyName="orderHold"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={8} />

            <Grid item xs={3}>
                {holdLink && (
                    <Button onClick={() => openHoldDialog()} variant="outlined">
                        {orderHold === 'Y' ? 'TAKE OFF HOLD' : 'PUT ON HOLD'}
                    </Button>
                )}
            </Grid>
            <Grid item xs={10}>
                <InputField
                    fullWidth
                    value={notesForBuyer}
                    label="Notes For Buyer"
                    rows={3}
                    propertyName="notesForBuyer"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={2} />
            <Grid item xs={4}>
                <Dropdown
                    fullWidth
                    value={deliveryDay}
                    label="Delivery Day"
                    items={[
                        'MONDAY',
                        'TUESDAY',
                        'WEDNESDAY',
                        'THURSDAY',
                        'FRIDAY',
                        'SATURDAY',
                        'SUNDAY'
                    ]}
                    propertyName="deliveryDay"
                    onChange={handleFieldChange}
                    allowNoValue
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={3}>
                <InputField
                    fullWidth
                    value={pmDeliveryDaysGrace}
                    label="PM Delivery Days Grance"
                    type="number"
                    propertyName="pmDeliveryDaysGrace"
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={9} />
        </Grid>
    );
}

PurchTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    partCategory: PropTypes.string,
    partCategoryDescription: PropTypes.string,
    orderHold: PropTypes.string,
    notesForBuyer: PropTypes.string,
    deliveryDay: PropTypes.string,
    refersToFcId: PropTypes.number,
    refersToFcName: PropTypes.string,
    pmDeliveryDaysGrace: PropTypes.number,
    holdLink: PropTypes.string,
    openHoldDialog: PropTypes.func.isRequired
};

PurchTab.defaultProps = {
    partCategory: null,
    partCategoryDescription: null,
    orderHold: null,
    notesForBuyer: null,
    deliveryDay: null,
    refersToFcId: null,
    refersToFcName: null,
    pmDeliveryDaysGrace: null,
    holdLink: null
};

export default PurchTab;
