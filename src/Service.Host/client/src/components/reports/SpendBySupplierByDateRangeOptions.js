import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    DatePicker,
    Dropdown,
    Typeahead,
    Title,
    collectionSelectorHelpers,
    reportSelectorHelpers,
    Loading
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import vendorManagersActions from '../../actions/vendorManagersActions';
import suppliersActions from '../../actions/suppliersActions';

function SpendBySupplierByDateRangeReportOptions() {
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
    const vendorManagers = useSelector(state =>
        collectionSelectorHelpers.getItems(state.vendorManagers)
    );
    const vendorManagersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.vendorManagers)
    );

    const prevOptions = useSelector(state =>
        reportSelectorHelpers.getReportOptions(state.spendBySupplierByDateRangeReport)
    );

    const dispatch = useDispatch();

    const [supplier, setSupplier] = useState(
        prevOptions?.id ? { id: prevOptions.id, name: '' } : null
    );
    const defaultStartDate = new Date();
    defaultStartDate.setMonth(defaultStartDate.getMonth() - 1);

    const [fromDate, setFromDate] = useState(
        prevOptions?.fromDate ? new Date(prevOptions?.fromDate) : defaultStartDate
    );
    const [toDate, setToDate] = useState(
        prevOptions?.toDate ? new Date(prevOptions?.toDate) : new Date()
    );

    const handleSupplierChange = selectedsupplier => {
        setSupplier(selectedsupplier);
    };

    useEffect(() => {
        dispatch(vendorManagersActions.fetch());
    }, [dispatch]);

    const [vm, setVm] = useState(prevOptions?.vm ? prevOptions.vm : '');

    const handleClick = () => {
        let search = `?vm=${vm}&fromDate=${fromDate.toISOString()}&toDate=${toDate.toISOString()}`;
        if (supplier?.id) {
            search = `${search}&supplierId=${supplier.id}`;
        }
        history.push({
            pathname: `/purchasing/reports/spend-by-supplier-by-date-range/report`,
            search
        });
    };

    return (
        <Page history={history} homeUrl={config.appRoot} width="s">
            <Title text="Spend By Supplier By Date Range" />
            <Grid container spacing={3} justifyContent="center">
                {vendorManagersLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={6}>
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
                                onChange={(_, newValue) => setVm(newValue)}
                            />
                        </Grid>
                        <Grid item xs={6} />
                        <Grid item xs={6}>
                            <DatePicker
                                label="From Date"
                                value={fromDate.toString()}
                                minDate="01/01/2000"
                                maxDate={toDate.toString()}
                                onChange={newValue => {
                                    setFromDate(newValue);
                                }}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <DatePicker
                                label="To Date"
                                value={toDate.toString()}
                                minDate={fromDate.toString()}
                                onChange={newValue => {
                                    setToDate(newValue);
                                }}
                            />
                        </Grid>
                        <Grid item xs={12} data-testid="supplierSearch">
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
                                value={`${supplier?.id ?? ''} - ${supplier?.description ?? ''}`}
                                modal
                                links={false}
                                debounce={1000}
                                minimumSearchTermLength={2}
                            />
                        </Grid>
                        <Grid item xs={12} />

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

export default SpendBySupplierByDateRangeReportOptions;
