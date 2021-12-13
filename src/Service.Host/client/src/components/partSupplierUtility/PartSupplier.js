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
import {
    Page,
    Loading,
    SaveBackCancelButtons,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';
import getQuery from '../../selectors/routerSelelctors';
import partSupplierActions from '../../actions/partSupplierActions';
import history from '../../history';
import config from '../../config';
import partSupplierReducer from './partSupplierReducer';
import { partSupplier } from '../../itemTypes';
import PartSupplierTab from './tabs/PartSupplierTab';
import partsActions from '../../actions/partsActions';
import { getSearchItems, getSearchLoading } from '../../selectors/CollectionSelectorHelpers';
import { getSnackbarVisible, getItem } from '../../selectors/ItemSelectors';

function PartSupplier() {
    const reduxDispatch = useDispatch();

    const searchParts = searchTerm => reduxDispatch(partsActions.search(searchTerm));
    const updatePartSupplier = body => reduxDispatch(partSupplierActions.update(null, body));

    const creating = () => false;
    const [state, dispatch] = useReducer(partSupplierReducer, {
        partSupplier: creating() ? {} : {},
        prevPart: {}
    });

    const partKey = useSelector(reduxState => getQuery(reduxState));
    const loading = useSelector(reduxState => reduxState.partSupplier.loading);
    const snackbarVisible = useSelector(reduxState => getSnackbarVisible(reduxState.partSupplier));

    const item = useSelector(reduxState => getItem(reduxState.partSupplier));

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
            <SnackbarMessage
                visible={snackbarVisible}
                onClose={() => reduxDispatch(partSupplierActions.setSnackbarVisible(false))}
                message="Save Successful"
            />
            <Grid container spacing={3}>
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={3}>
                            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                <Typography variant="h6">
                                    {state.partSupplier?.partNumber}
                                </Typography>
                            </Box>
                        </Grid>
                        <Grid item xs={7}>
                            <Box
                                sx={{
                                    borderBottom: 1,
                                    borderColor: 'divider',
                                    marginBottom: '20px'
                                }}
                            >
                                <Typography variant="h6">
                                    {state.partSupplier?.supplierName}
                                </Typography>
                            </Box>
                        </Grid>
                        <Grid item xs={1} />
                        <Grid item xs={1}>
                            {canEdit() ? (
                                <Tooltip title="You have write access to Part Suppliers">
                                    <ModeEditIcon fontSize="large" color="primary" />
                                </Tooltip>
                            ) : (
                                <Tooltip title="You do not have write access to Part Suppliers">
                                    <EditOffIcon fontSize="large" color="secondary" />
                                </Tooltip>
                            )}
                        </Grid>
                        <Grid item xs={12}>
                            <Box sx={{ width: '100%' }}>
                                <Box sx={{ borderBottom: 0, borderColor: 'divider' }}>
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
                                            designation={state.partSupplier?.designation}
                                            partsSearchResults={partsSearchResults}
                                            partsSearchLoading={partsSearchLoading}
                                            searchParts={searchParts}
                                        />
                                    </Box>
                                )}
                            </Box>
                        </Grid>
                    </>
                )}
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        //saveDisabled={!canEdit() || true}
                        saveClick={() => (false ? null : updatePartSupplier(state.partSupplier))}
                        cancelClick={() => {}}
                        backClick={() => history.push('/purchasing/part-suppliers')}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default PartSupplier;
