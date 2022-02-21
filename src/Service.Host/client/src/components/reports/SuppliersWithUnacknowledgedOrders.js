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
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
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
        if (!vendorManagers.length > 0) {
            dispatch(vendorManagersActions.fetch());
        }
    }, [dispatch, vendorManagers]);

    const [vm, setVm] = useState(prevOptions?.vendorManager ? prevOptions.vendorManager : '');

    const handleVmChange = selectedVm => {
        setVm(selectedVm);
        setOptions({ ...options, vendorManager: selectedVm });
        dispatch(
            suppliersWithUnacknowledgedOrdersActions.fetchReport({
                ...options,
                vendorManager: selectedVm
            })
        );
    };

    const handleRun = () => {
        dispatch(suppliersWithUnacknowledgedOrdersActions.fetchReport(options));
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

                        <Grid item xs={8}>
                            <Button
                                color="primary"
                                variant="contained"
                                onClick={handleRun}
                                style={{ marginTop: '30px' }}
                            >
                                Run Report
                            </Button>
                        </Grid>
                        <Grid item xs={12}>
                            {!reportData && !reportLoading ? (
                                <Typography variant="subtitle1" gutterBottom>
                                    Please select a Vendor Manager or click Run Report
                                </Typography>
                            ) : (
                                ''
                            )}
                        </Grid>
                        <Grid item xs={12}>
                            {reportLoading ? (
                                <Loading />
                            ) : (
                                <ReportTable
                                    reportData={reportData}
                                    showTitle={false}
                                    showTotals={false}
                                    placeholderRows={0}
                                    placeholderColumns={0}
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
