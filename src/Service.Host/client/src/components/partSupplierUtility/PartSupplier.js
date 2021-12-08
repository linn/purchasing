import React, { Fragment, useEffect, useReducer } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
// import { makeStyles } from '@mui/styles';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import EditOffIcon from '@mui/icons-material/EditOff';
import Tooltip from '@mui/material/Tooltip';
import { useSelector, useDispatch } from 'react-redux';
import { Page, InputField, Loading } from '@linn-it/linn-form-components-library';
import getQuery from '../../selectors/routerSelelctors';
import partSupplierActions from '../../actions/partSupplierActions';
import history from '../../history';
import config from '../../config';
import partSupplierReducer from './partSupplierReducer';
import { partSupplier } from '../../itemTypes';

function PartSupplierSearch() {
    const creating = () => false;
    const [state, dispatch] = useReducer(partSupplierReducer, {
        partSupplier: creating() ? {} : {},
        prevPart: {}
    });

    const reduxDispatch = useDispatch();

    // const useStyles = makeStyles(theme => ({
    //     button: {
    //         marginLeft: theme.spacing(1),
    //         marginTop: theme.spacing(4)
    //     },
    //     a: {
    //         textDecoration: 'none'
    //     }
    // }));

    // const classes = useStyles();

    const partKey = useSelector(reduxState => getQuery(reduxState));
    const loading = useSelector(reduxState => reduxState.partSupplier.loading);
    const item = useSelector(reduxState => reduxState.partSupplier.item);

    const setEditStatus = status => dispatch(partSupplierActions.setEditStatus(status));

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
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={state.partSupplier?.partNumber}
                                label="Part Number"
                                propertyName="partNumber"
                                textFieldProps={{ size: 'small' }}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={state.partSupplier?.partDescription}
                                label="Description"
                                propertyName="partDescription"
                                textFieldProps={{ size: 'small' }}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={state.partSupplier?.supplierId}
                                label="Supplier"
                                propertyName="supplierId"
                                textFieldProps={{ size: 'small' }}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                fullWidth
                                value={state.partSupplier?.supplierName}
                                label="Name"
                                propertyName="supplierName"
                                textFieldProps={{ size: 'small' }}
                                onChange={handleFieldChange}
                            />
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default PartSupplierSearch;
