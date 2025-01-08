/* eslint-disable indent */
import React, { useEffect, useState } from 'react';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import { useParams } from 'react-router-dom';
import {
    Page,
    InputField,
    collectionSelectorHelpers,
    Dropdown,
    Loading,
    getItemError,
    itemSelectorHelpers
} from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';
import Button from '@mui/material/Button';
import currentEmployeesActions from '../actions/currentEmployeesActions';
import history from '../history';
import config from '../config';
import {
    vendorManager as vendorManagerItemTypes,
    currentEmployees as currentEmployeesItemTypes
} from '../itemTypes';
import vendorManagerActions from '../actions/vendorManagerActions';

function VendorManager({ creating }) {
    const { id } = useParams();
    const reduxDispatch = useDispatch();
    const [vendorManager, setVendorManager] = useState();

    const error = useSelector(state => getItemError(state, vendorManagerItemTypes.item));

    const storeItems = useSelector(state => state[vendorManagerItemTypes.item]);
    const vendorManagerLoading = itemSelectorHelpers.getItemLoading(storeItems);
    const vendorManagerInfo = itemSelectorHelpers.getItem(storeItems);

    const currentEmployeesStoreItem = useSelector(
        reduxState => reduxState[currentEmployeesItemTypes.item]
    );
    const currentEmployees = collectionSelectorHelpers.getItems(currentEmployeesStoreItem);
    const currentEmployeesLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getLoading(reduxState[currentEmployeesItemTypes.item])
    );

    console.log(currentEmployees);

    useEffect(() => {
        reduxDispatch(vendorManagerActions.fetch(id));
    }, [id, reduxDispatch]);

    useEffect(() => {
        reduxDispatch(currentEmployeesActions.fetch());
    }, [reduxDispatch]);

    useEffect(() => {
        setVendorManager(vendorManagerInfo);
    }, [vendorManagerInfo]);

    const handleFieldChange = (propertyName, newValue) => {
        setVendorManager(vm => ({ ...vm, [propertyName]: newValue }));
    };

    const handleEmployeeChange = (propertyName, newValue) => {
        console.log(newValue);
        const newEmployee = currentEmployees.find(e => e.id === newValue);

        setVendorManager(vm => ({
            ...vm,
            name: newEmployee?.fullName,
            userNumber: newEmployee?.id
        }));
    };

    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <Typography color="primary" variant="h4">
                        Vendor Managers
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    {(vendorManagerLoading || currentEmployeesLoading) && <Loading />}
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        propertyName="vmId"
                        disabled={!creating}
                        label="Id"
                        onChange={handleFieldChange}
                        value={vendorManager?.vmId}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Dropdown
                        propertyName="pmMeasured"
                        items={['Y', 'N']}
                        required
                        label="PM Measured"
                        onChange={handleFieldChange}
                        value={vendorManager?.pmMeasured}
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        propertyName="userNumber"
                        disabled
                        label="User Number"
                        value={vendorManager?.userNumber}
                    />
                </Grid>
                {/* <Grid item xs={2}>
                    <Dropdown
                        propertyName="name"
                        items={employeesInfo?.map(employee => ({
                            id: employee.id,
                            displayText: employee?.fullName
                        }))}
                        required
                        label="Employee"
                        onChange={handleEmployeeChange}
                        value={vendorManager?.name}
                    />
                </Grid> */}
                <Grid item xs={10}>
                    <Button
                        variant="contained"
                        onClick={() => {
                            if (creating) {
                                reduxDispatch(vendorManagerActions.clearErrorsForItem());
                                reduxDispatch(vendorManagerActions.add(vendorManager));
                            } else {
                                reduxDispatch(vendorManagerActions.clearErrorsForItem());
                                reduxDispatch(vendorManagerActions.update(id, vendorManager));
                            }
                        }}
                    >
                        Save
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

VendorManager.propTypes = { creating: PropTypes.bool };
VendorManager.defaultProps = { creating: false };

export default VendorManager;
