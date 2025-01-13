/* eslint-disable indent */
import React, { useEffect } from 'react';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import {
    Page,
    Loading,
    utilities,
    CreateButton,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useDispatch, useSelector } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import vendorManagersActions from '../actions/vendorManagersActions';
import employeesActions from '../actions/employeesActions';
import history from '../history';
import config from '../config';
import { vendorManagers as vendorManagerItemTypes } from '../itemTypes';

function VendorManagers() {
    const reduxDispatch = useDispatch();

    const storeItems = useSelector(state => state[vendorManagerItemTypes.item]);
    const vendorManagersLoading = collectionSelectorHelpers.getLoading(storeItems);
    const vendorManagersInfo = collectionSelectorHelpers.getItems(storeItems);

    useEffect(() => {
        reduxDispatch(vendorManagersActions.fetch());
        reduxDispatch(employeesActions.fetch());
    }, [reduxDispatch]);

    const columns = [
        {
            field: 'vmId',
            headerName: 'Id',
            width: 200
        },
        { field: 'userNumber', headerName: 'User Number', width: 200 },
        {
            field: 'name',
            headerName: 'Name',
            width: 200
        },
        {
            field: 'pmMeasured',
            headerName: 'Include In Purchase Measure',
            width: 300
        }
    ];

    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Grid container spacing={2}>
                <Grid item xs={7}>
                    <Typography color="primary" variant="h4">
                        Vendor Managers
                    </Typography>
                </Grid>
                <Grid item xs={4}>
                    <CreateButton createUrl="/purchasing/vendor-managers/create" />
                </Grid>
                <Grid item xs={12}>
                    {vendorManagersLoading && <Loading />}
                </Grid>
                <Grid item xs={12}>
                    <DataGrid
                        rows={
                            vendorManagersInfo?.map(vm => ({
                                ...vm,
                                id: vm.vmId
                            })) || []
                        }
                        columns={columns}
                        editMode="cell"
                        onRowClick={clicked => {
                            history.push(utilities.getSelfHref(clicked.row));
                        }}
                        autoHeight
                        columnBuffer={8}
                        density="comfortable"
                        rowHeight={34}
                        loading={false}
                        disableMultipleSelection
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default VendorManagers;
