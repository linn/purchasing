import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Dropdown,
    Title,
    collectionSelectorHelpers,
    reportSelectorHelpers,
    Loading
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import vendorManagersActions from '../../actions/vendorManagersActions';

function SpendBySupplierReportOptions() {
    const vendorManagers = useSelector(state =>
        collectionSelectorHelpers.getItems(state.vendorManagers)
    );
    const vendorManagersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.vendorManagers)
    );

    const prevOptions = useSelector(state =>
        reportSelectorHelpers.getReportOptions(state.spendBySupplierReport)
    );

    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(vendorManagersActions.fetch());
    }, [dispatch]);

    const [vm, setVm] = useState(prevOptions?.vm ? prevOptions.vm : '');

    const handleClick = () =>
        history.push({
            pathname: `/purchasing/reports/spend-by-supplier/report`,
            search: `?vm=${vm}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot} title="Spend By Supplier">
            <Title text="Spend By Supplier" />
            <Grid container spacing={3}>
                {vendorManagersLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={4}>
                            <Dropdown
                                fullWidth
                                value={vm}
                                label="Vendor Manager"
                                propertyName="vendorManager"
                                items={[
                                    ...[{ id: '', displayText: 'All' }],
                                    ...vendorManagers
                                        ?.sort((a, b) => {
                                            if (a.vmId < b.vmId) {
                                                return -1;
                                            }
                                            if (a.vmId > b.vmId) {
                                                return 1;
                                            }
                                            return 0;
                                        })
                                        .map(v => ({
                                            id: v.vmId,
                                            displayText: `${v.vmId} ${v.name} (${v.userNumber})`
                                        }))
                                ]}
                                allowNoValue={false}
                                onChange={(propertyName, newValue) => setVm(newValue)}
                            />
                        </Grid>
                        <Grid item xs={8} />
                        <Grid item xs={12}>
                            <Button color="primary" variant="contained" onClick={handleClick}>
                                Run Report
                            </Button>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default SpendBySupplierReportOptions;
