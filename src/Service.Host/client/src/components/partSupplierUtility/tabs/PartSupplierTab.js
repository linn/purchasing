import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';

import {
    InputField,
    LinkButton,
    Typeahead,
    utilities
} from '@linn-it/linn-form-components-library';
import config from '../../../config';

function PartSupplierTab({
    partNumber,
    partDescription,
    partsSearchResults,
    partsSearchLoading,
    searchParts,
    designation,
    supplierId,
    supplierName,
    handleFieldChange,
    searchSuppliers,
    suppliersSearchResults,
    suppliersSearchLoading,
    editStatus,
    part,
    setPreferredSupplierDialogOpen
}) {
    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('partNumber', newValue.id);
                        handleFieldChange('partDescription', newValue.description);
                    }}
                    label="Part"
                    modal
                    propertyName="partNumber"
                    items={partsSearchResults}
                    value={partNumber}
                    loading={partsSearchLoading}
                    fetchItems={searchParts}
                    links={false}
                    text
                    disabled={editStatus !== 'create'}
                    clearSearch={() => {}}
                    placeholder="Search Parts"
                    minimumSearchTermLength={3}
                />
            </Grid>
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={partDescription}
                    label="Description"
                    propertyName="partDescription"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={4}>
                <Typeahead
                    onSelect={newValue => {
                        handleFieldChange('supplierId', newValue.id);
                        handleFieldChange('supplierName', newValue.name);
                    }}
                    label="Supplier"
                    modal
                    propertyName="supplierId"
                    items={suppliersSearchResults}
                    value={supplierId?.toString()}
                    loading={suppliersSearchLoading}
                    fetchItems={searchSuppliers}
                    disabled={editStatus !== 'create'}
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
                    value={supplierName}
                    label="Name"
                    propertyName="supplierName"
                    onChange={() => {}}
                />
            </Grid>
            <Grid item xs={2}>
                <Button onClick={() => setPreferredSupplierDialogOpen(true)}>
                    Preferred Supplier
                </Button>
            </Grid>
            <Grid item xs={10} />
            <Grid item xs={8}>
                <InputField
                    fullWidth
                    value={designation}
                    label="Designation"
                    rows={4}
                    propertyName="designation"
                    onChange={handleFieldChange}
                />
            </Grid>
            {part?.manufacturers?.length > 0 && (
                <>
                    <Grid item xs={8}>
                        <InputField
                            fullWidth
                            value={part?.manufacturers
                                ?.map(x => `${x.manufacturerDescription} - ${x.partNumber}`)
                                .join('\n')}
                            label="Manufacturers"
                            rows={4}
                            propertyName="manufacturers"
                            onChange={() => {}}
                        />
                    </Grid>
                    <Grid item xs={4}>
                        {!!utilities.getHref(part, 'mechanical-sourcing-sheet') && (
                            <LinkButton
                                external
                                newTab
                                to={`${config.proxyRoot}${utilities.getHref(
                                    part,
                                    'mechanical-sourcing-sheet'
                                )}?tab=manufacturers`}
                                text="Edit Manufacturers"
                            />
                        )}
                    </Grid>
                </>
            )}
        </Grid>
    );
}

PartSupplierTab.propTypes = {
    partNumber: PropTypes.string,
    partDescription: PropTypes.string,
    designation: PropTypes.string,
    supplierId: PropTypes.number,
    supplierName: PropTypes.string,
    handleFieldChange: PropTypes.func.isRequired,
    partsSearchResults: PropTypes.arrayOf(PropTypes.shape({})),
    partsSearchLoading: PropTypes.bool,
    searchParts: PropTypes.func.isRequired,
    suppliersSearchResults: PropTypes.arrayOf(PropTypes.shape({})),
    suppliersSearchLoading: PropTypes.bool,
    searchSuppliers: PropTypes.func.isRequired,
    editStatus: PropTypes.string,
    part: PropTypes.shape({
        manufacturers: PropTypes.arrayOf(
            PropTypes.shape({
                manufacturerDescription: PropTypes.string,
                partNumber: PropTypes.string
            })
        )
    }),
    setPreferredSupplierDialogOpen: PropTypes.func.isRequired
};

PartSupplierTab.defaultProps = {
    partNumber: null,
    partDescription: null,
    designation: null,
    supplierId: null,
    supplierName: null,
    partsSearchResults: [],
    partsSearchLoading: false,
    suppliersSearchResults: [],
    suppliersSearchLoading: false,
    editStatus: 'view',
    part: null
};

export default PartSupplierTab;
