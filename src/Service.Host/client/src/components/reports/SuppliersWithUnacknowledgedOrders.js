import React, { useState, useEffect } from 'react';
import Grid from '@mui/material/Grid';
import {
    Page,
    Dropdown,
    Title,
    collectionSelectorHelpers,
    reportSelectorHelpers,
    Loading,
    ReportTable
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import vendorManagersActions from '../../actions/vendorManagersActions';
import suppliersWithUnacknowledgedOrdersActions from '../../actions/suppliersWithUnacknowledgedOrdersActions';

function SuppliersWithUnacknowledgedOrders() {
    const [options, setOptions] = useState({});

    const vendorManagers = useSelector(state =>
        collectionSelectorHelpers.getItems(state.vendorManagers)
    );
    const vendorManagersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.vendorManagers)
    );

    const prevOptions = useSelector(state =>
        reportSelectorHelpers.getReportOptions(state.suppliersWithUnacknowledgedOrders)
    );
    const reportLoading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.suppliersWithUnacknowledgedOrders)
    );
    const reportData = useSelector(state =>
        reportSelectorHelpers.getReportData(state.suppliersWithUnacknowledgedOrders)
    );

    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(vendorManagersActions.fetch());
    }, [dispatch]);

    useEffect(() => {
        dispatch(suppliersWithUnacknowledgedOrdersActions.fetchReport(options));
    }, [dispatch, options]);

    const [vm, setVm] = useState(prevOptions?.vm ? prevOptions.vm : '');

    const handleVmChange = selectedVm => {
        setVm(selectedVm);
        setOptions({ ...options, vendorManager: selectedVm });
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Suppliers with unacknowledged orders" />
            <Grid container justifyContent="center">
                {vendorManagersLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={4} style={{ marginBottom: '30px' }}>
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
                                            displayText: `${v.vmId} - ${v.name} (${v.userNumber})`
                                        }))
                                ]}
                                allowNoValue={false}
                                onChange={(propertyName, newValue) => handleVmChange(newValue)}
                            />
                        </Grid>

                        <Grid item xs={8} />
                        <Grid item xs={12}>
                            {reportLoading || !reportData ? (
                                <Loading />
                            ) : (
                                <ReportTable
                                    reportData={reportData}
                                    title={reportData?.title}
                                    showTitle={false}
                                    showTotals={false}
                                    placeholderRows={4}
                                    placeholderColumns={2}
                                />
                            )}
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default SuppliersWithUnacknowledgedOrders;
