import React, { useEffect, useState } from 'react';
import {
    Title,
    Page,
    collectionSelectorHelpers,
    Dropdown,
    InputField,
    Typeahead,
    utilities,
    Loading,
    reportSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import moment from 'moment';
import history from '../../history';
import config from '../../config';
import ledgerPeriodsActions from '../../actions/ledgerPeriodsActions';
import vendorManagersActions from '../../actions/vendorManagersActions';
import suppliersActions from '../../actions/suppliersActions';

function DeliveryPerformanceSummary() {
    const [endPeriod, setEndPeriod] = useState(null);
    const [startPeriod, setStartPeriod] = useState(null);
    const [vendorManager, setVendorManager] = useState('');
    const [supplier, setSupplier] = useState(null);

    const dispatch = useDispatch();
    const ledgerPeriods = useSelector(state =>
        collectionSelectorHelpers.getItems(state.ledgerPeriods)
    );
    const ledgerPeriodsLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.ledgerPeriods)
    );
    const vendorManagers = useSelector(state =>
        collectionSelectorHelpers.getItems(state.vendorManagers)
    );
    const vendorManagersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.vendorManagers)
    );

    const prevOptions = useSelector(state =>
        reportSelectorHelpers.getReportOptions(state.deliveryPerformanceSummaryReport)
    );

    useEffect(() => {
        dispatch(ledgerPeriodsActions.fetch());
    }, [dispatch]);

    useEffect(() => {
        dispatch(vendorManagersActions.fetch());
    }, [dispatch]);

    useEffect(() => {
        if (prevOptions) {
            setStartPeriod(prevOptions.startPeriod);
            setEndPeriod(prevOptions.endPeriod);
            setVendorManager(prevOptions.vendorManager);
            setSupplier({ id: prevOptions.supplierId });
        }
    }, [prevOptions]);

    useEffect(() => {
        if (!prevOptions?.startPeriod && ledgerPeriods && ledgerPeriods.length > 0) {
            const start = ledgerPeriods.find(
                a =>
                    a.monthName ===
                    moment(new Date()).subtract(6, 'months').format('MMMYYYY').toUpperCase()
            );
            const current = ledgerPeriods.find(
                a => a.monthName === moment(new Date()).format('MMMYYYY').toUpperCase()
            );
            setStartPeriod(start.periodNumber);
            setEndPeriod(current.periodNumber);
        }
    }, [ledgerPeriods, prevOptions]);

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers)
    )?.map(c => ({
        id: c.id,
        name: c.id.toString(),
        description: c.name
    }));
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );

    const handleStartChange = (_, period) => {
        setStartPeriod(period);
    };

    const handleEndChange = (_, period) => {
        setEndPeriod(period);
    };

    const handleSetSupplier = (_, supp) => {
        setSupplier({ id: supp });
    };

    const handleSupplierReturn = () => {};

    const handleSupplierChange = selectedsupplier => {
        setSupplier(selectedsupplier);
    };

    const runReport = () => {
        let body = {
            startPeriod,
            endPeriod,
            vendorManager
        };

        if (supplier?.id) {
            body = { ...body, supplierId: supplier.id };
        }

        history.push('/purchasing/reports/delivery-performance-summary/report', body);
    };
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Delivery Performance" />
            <Grid container>
                {ledgerPeriodsLoading && <Loading />}
                {!ledgerPeriodsLoading && startPeriod && (
                    <>
                        <Grid item xs={6}>
                            <Dropdown
                                items={utilities.sortEntityList(ledgerPeriods, 'periodNumber').map(e => ({
                                    displayText: `${e.monthName}`,
                                    id: e.periodNumber
                                }))}
                                propertyName="startPeriod"
                                label="Start Month"
                                value={startPeriod}
                                onChange={handleStartChange}
                                type="number"
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Dropdown
                                items={utilities.sortEntityList(ledgerPeriods, 'periodNumber').map(e => ({
                                    displayText: `${e.monthName}`,
                                    id: e.periodNumber
                                }))}
                                propertyName="endPeriod"
                                label="End Month"
                                value={endPeriod}
                                onChange={handleEndChange}
                                type="number"
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <Dropdown
                                fullWidth
                                value={vendorManager}
                                label="Vendor Manager"
                                propertyName="vendorManager"
                                optionsLoading={vendorManagersLoading}
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
                                onChange={(_, newValue) => setVendorManager(newValue)}
                            />
                        </Grid>
                        <Grid item xs={8} />
                        <Grid item xs={4}>
                            <Typeahead
                                label="Supplier"
                                title="Search for a supplier"
                                onSelect={handleSupplierChange}
                                items={suppliersSearchResults}
                                loading={suppliersSearchLoading}
                                fetchItems={searchTerm =>
                                    dispatch(suppliersActions.search(searchTerm))
                                }
                                clearSearch={() => dispatch(suppliersActions.clearSearch)}
                                value={supplier?.id}
                                openModalOnClick={false}
                                modal
                                links={false}
                                debounce={1000}
                                handleFieldChange={handleSetSupplier}
                                handleReturnPress={handleSupplierReturn}
                                minimumSearchTermLength={2}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                disabled
                                value={supplier?.description}
                                fullWidth
                                label="Supplier Name"
                            />
                        </Grid>
                        <Grid item xs={2} />
                        <Grid item xs={12}>
                            <Button
                                variant="outlined"
                                onClick={runReport}
                                style={{ marginTop: '40px' }}
                            >
                                Run Report
                            </Button>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default DeliveryPerformanceSummary;
