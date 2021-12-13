import React, { useEffect, useState, useReducer } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Box from '@mui/material/Box';
import { useSelector, useDispatch } from 'react-redux';
import { Page, Loading, Typeahead } from '@linn-it/linn-form-components-library';
import getQuery from '../../selectors/routerSelelctors';
import partSupplierActions from '../../actions/partSupplierActions';
import history from '../../history';
import config from '../../config';
import partSupplierReducer from './partSupplierReducer';
import { partSupplier } from '../../itemTypes';
import PartSupplierTab from './tabs/PartSupplierTab';
import partsActions from '../../actions/partsActions';
import { getSearchItems, getSearchLoading } from '../../selectors/CollectionSelectorHelpers';

function PartSupplier() {
    const reduxDispatch = useDispatch();

    const searchParts = searchTerm => reduxDispatch(partsActions.search(searchTerm));

    const creating = () => false;
    const [state, dispatch] = useReducer(partSupplierReducer, {
        partSupplier: creating() ? {} : {},
        prevPart: {}
    });

    const partKey = useSelector(reduxState => getQuery(reduxState));
    const loading = useSelector(reduxState => reduxState.partSupplier.loading);
    const item = useSelector(reduxState => reduxState.partSupplier.item);

    const setEditStatus = status => reduxDispatch(partSupplierActions.setEditStatus(status));

    const partsSearchResults = useSelector(reduxState =>
        getSearchItems(reduxState.parts, 100, 'partNumber', 'partNumber', 'description')
    );
    const partsSearchLoading = useSelector(reduxState => getSearchLoading(reduxState.parts));

    useEffect(() => {
        if (partKey) {
            reduxDispatch(
                partSupplierActions.fetchByHref(
                    `${partSupplier.uri}?partId=${partKey.partId}&supplierId=${partKey.supplierId}`
                )
            );
        }
    }, [partKey, reduxDispatch]);

    useEffect(() => {
        if (item) {
            dispatch({ type: 'initialise', payload: item });
        }
    }, [item]);

    const handleFieldChange = (propertyName, newValue) => {
        setEditStatus('edit');
        dispatch({ type: 'fieldChange', fieldName: propertyName, payload: newValue });
    };

    const canEdit = () => item?.links.some(l => l.rel === 'edit' || l.rel === 'create');

    const [value, setValue] = useState(0);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={11}>
                    <Typography variant="h3">Part Supplier Record</Typography>
                </Grid>

                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={1}>
                            {canEdit() ? (
                                <Tooltip title="You have write access to Part Suppliers">
                                    <ModeEditIcon color="primary" />
                                </Tooltip>
                            ) : (
                                <Tooltip title="You do not have write access to Part Suppliers">
                                    <EditOffIcon color="secondary" />
                                </Tooltip>
                            )}
                        </Grid>
                        <Grid item xs={12}>
                            <Typeahead
                                onSelect={() => {}}
                                label="Part"
                                modal
                                openModalOnClick={false}
                                handleFieldChange={(_, newValue) => {
                                    handleFieldChange('partNumber', newValue);
                                }}
                                propertyName="partNumber"
                                items={partsSearchResults}
                                value={state.partSupplier?.ontoLocation}
                                loading={partsSearchLoading}
                                fetchItems={searchParts}
                                links={false}
                                text
                                clearSearch={() => {}}
                                placeholder="Search Locations"
                                minimumSearchTermLength={3}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Box sx={{ width: '100%' }}>
                                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                    <Tabs
                                        value={value}
                                        onChange={(event, newValue) => {
                                            setValue(newValue);
                                        }}
                                    >
                                        <Tab label="Part and Supplier" />
                                        <Tab label="Order Details" disabled />
                                        <Tab label="Other Details" disabled />
                                    </Tabs>
                                </Box>

                                {value === 0 && (
                                    <Box sx={{ p: 3 }}>
                                        <PartSupplierTab
                                            handleFieldChange={handleFieldChange}
                                            partNumber={state.partSupplier?.partNumber}
                                            partDescription={state.partSupplier?.partDescription}
                                            supplierId={state.partSupplier?.supplierId}
                                            supplierName={state.partSupplier?.supplierName}
                                            supplierDesignation={
                                                state.partSupplier?.supplierDesignation
                                            }
                                        />
                                    </Box>
                                )}
                            </Box>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default PartSupplier;
