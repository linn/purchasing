import React, { useEffect, useReducer, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    itemSelectorHelpers,
    Page,
    Loading,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { useParams } from 'react-router-dom';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Box from '@mui/material/Box';
import supplierActions from '../../actions/supplierActions';
import history from '../../history';
import config from '../../config';
import supplierReducer from './supplierReducer';
import GeneralTab from './tabs/GeneralTab';

function Supplier() {
    const reduxDispatch = useDispatch();

    const [state, dispatch] = useReducer(supplierReducer, {
        supplier: {},
        prevPart: {}
    });

    const { id } = useParams();
    const supplier = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.supplier));
    const supplierLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.supplier)
    );

    const clearErrors = () => reduxDispatch(supplierActions.clearErrorsForItem());
    const updateSupplier = body => reduxDispatch(supplierActions.update(id, body));

    useEffect(() => {
        if (supplier) {
            dispatch({ type: 'initialise', payload: supplier });
        }
    }, [supplier]);

    const canEdit = () => supplier?.links.some(l => l.rel === 'edit');

    useEffect(() => {
        if (id) {
            reduxDispatch(supplierActions.fetch(id));
        }
    }, [id, reduxDispatch]);
    const [tab, setTab] = useState(0);

    const handleFieldChange = (propertyName, newValue) => {
        dispatch({ type: 'fieldChange', fieldName: propertyName, payload: newValue });
    };

    const setEditStatus = status => reduxDispatch(supplierActions.setEditStatus(status));

    const editStatus = useSelector(reduxState =>
        itemSelectorHelpers.getItemEditStatus(reduxState.supplier)
    );

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                {supplierLoading ? (
                    <>
                        <Grid item xs={12}>
                            <Loading />
                        </Grid>
                    </>
                ) : (
                    state.supplier && (
                        <>
                            <Grid item xs={3}>
                                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                    <Typography variant="h6">{state.supplier.id}</Typography>
                                </Box>
                            </Grid>
                            <Grid item xs={7}>
                                <Box
                                    sx={{
                                        borderBottom: 1,
                                        borderColor: 'divider'
                                    }}
                                >
                                    <Typography variant="h6">{state.supplier.name}</Typography>
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
                                            value={tab}
                                            onChange={(event, newValue) => {
                                                setTab(newValue);
                                            }}
                                        >
                                            <Tab label="General" />
                                            <Tab label="Finanace" />
                                            <Tab label="Purch" />
                                            <Tab label="Where" />
                                            <Tab label="Whose" />
                                            <Tab label="Lifecycle" />
                                            <Tab label="Notes" />
                                        </Tabs>
                                    </Box>
                                    {tab === 0 && (
                                        <Box sx={{ paddingTop: 3 }}>
                                            <GeneralTab
                                                name={state.supplier.name}
                                                phoneNumber={state.supplier.phoneNumber}
                                                webAddress={state.supplier.webAddress}
                                                orderContactMethod={
                                                    state.supplier.orderContactMethod
                                                }
                                                invoiceContactMethod={
                                                    state.supplier.invoiceContactMethod
                                                }
                                                suppliersReference={
                                                    state.supplier.suppliersReference
                                                }
                                                liveOnOracle={state.supplier.liveOnOracle}
                                                handleFieldChange={handleFieldChange}
                                            />
                                        </Box>
                                    )}
                                </Box>
                            </Grid>
                            <Grid item xs={12}>
                                <SaveBackCancelButtons
                                    saveDisabled={!canEdit() || editStatus === 'view'}
                                    saveClick={() => {
                                        clearErrors();
                                        updateSupplier(state.partSupplier);
                                    }}
                                    cancelClick={() => {
                                        dispatch({ type: 'initialise', payload: supplier });
                                        setEditStatus('view');
                                    }}
                                    backClick={() => history.push('/purchasing/part-suppliers')}
                                />
                            </Grid>
                        </>
                    )
                )}
            </Grid>
        </Page>
    );
}

export default Supplier;
