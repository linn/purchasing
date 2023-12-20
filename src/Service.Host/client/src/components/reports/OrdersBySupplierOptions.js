import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Dropdown,
    Title,
    Typeahead,
    collectionSelectorHelpers,
    reportSelectorHelpers,
    DatePicker
} from '@linn-it/linn-form-components-library';
import moment from 'moment';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import suppliersActions from '../../actions/suppliersActions';

function OrdersBySupplierReportOptions() {
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
    const prevOptions = useSelector(state =>
        reportSelectorHelpers.getReportOptions(state.ordersBySupplier)
    );

    const dispatch = useDispatch();

    const defaultStartDate = moment().subtract(1, 'months');

    const [fromDate, setFromDate] = useState(
        prevOptions?.fromDate ? moment(prevOptions?.fromDate) : defaultStartDate
    );
    const [toDate, setToDate] = useState(
        prevOptions?.toDate ? moment(prevOptions?.toDate) : moment()
    );

    const [supplier, setSupplier] = useState(
        prevOptions?.id
            ? { id: prevOptions.id, name: '' }
            : { id: '', name: 'click to set supplier' }
    );
    const [outstandingOnly, setOutstandingOnly] = useState(
        prevOptions?.outstanding ? prevOptions.outstanding : 'N'
    );
    const [returns, setReturns] = useState(prevOptions?.returns ? prevOptions.returns : 'N');
    const [stockControlled, setStockControlled] = useState(
        prevOptions?.stockControlled ? prevOptions.stockControlled : 'A'
    );
    const [credits, setCredits] = useState(prevOptions?.credits ? prevOptions.credits : 'N');
    const [cancelled, setCancelled] = useState(
        prevOptions?.cancelled ? prevOptions.cancelled : 'N'
    );

    const handleSupplierChange = selectedsupplier => {
        setSupplier(selectedsupplier);
    };

    const handleClick = () =>
        history.push({
            pathname: `/purchasing/reports/orders-by-supplier/report`,
            search:
                `?id=${supplier.id}` +
                `&fromDate=${fromDate.toISOString()}` +
                `&toDate=${toDate.toISOString()}` +
                `&outstanding=${outstandingOnly}` +
                `&returns=${returns}` +
                `&stockControlled=${stockControlled}` +
                `&credits=${credits}` +
                `&cancelled=${cancelled}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot} width="s">
            <Title text="Orders By Supplier" />
            <Grid container spacing={3} justifyContent="center">
                <Grid item xs={12} data-testid="supplierSearch">
                    <Typeahead
                        label="Supplier"
                        title="Search for a supplier"
                        onSelect={handleSupplierChange}
                        items={suppliersSearchResults}
                        loading={suppliersSearchLoading}
                        fetchItems={searchTerm => dispatch(suppliersActions.search(searchTerm))}
                        clearSearch={() => dispatch(suppliersActions.clearSearch)}
                        value={`${supplier?.id} - ${supplier?.description}`}
                        modal
                        links={false}
                        debounce={1000}
                        minimumSearchTermLength={2}
                    />
                </Grid>
                <Grid item xs={6}>
                    <DatePicker
                        label="From Date"
                        value={fromDate}
                        maxDate={toDate}
                        onChange={newValue => {
                            setFromDate(newValue);
                        }}
                    />
                </Grid>
                <Grid item xs={6}>
                    <DatePicker
                        label="To Date"
                        value={toDate}
                        minDate={fromDate}
                        onChange={newValue => {
                            setToDate(newValue);
                        }}
                    />
                </Grid>
                <Grid item xs={12} />
                <Grid item xs={4}>
                    <Dropdown
                        fullWidth
                        value={outstandingOnly}
                        label="All or Outstanding"
                        propertyName="outstanding"
                        items={[
                            { id: 'N', displayText: 'All' },
                            { id: 'Y', displayText: 'Outstanding' }
                        ]}
                        allowNoValue={false}
                        onChange={(propertyName, newValue) => setOutstandingOnly(newValue)}
                    />
                </Grid>

                <Grid item xs={4}>
                    <Dropdown
                        fullWidth
                        value={returns}
                        label="Include Returns"
                        propertyName="returns"
                        items={[
                            { id: 'N', displayText: 'No' },
                            { id: 'Y', displayText: 'Yes' }
                        ]}
                        allowNoValue={false}
                        onChange={(propertyName, newValue) => setReturns(newValue)}
                    />
                </Grid>

                <Grid item xs={4}>
                    <Dropdown
                        fullWidth
                        value={stockControlled}
                        label="Stock Controlled"
                        propertyName="stockControlled"
                        items={[
                            { id: 'A', displayText: 'All' },
                            { id: 'O', displayText: 'Stock Controlled Only' },
                            { id: 'N', displayText: 'Non Stock Controlled' }
                        ]}
                        allowNoValue={false}
                        onChange={(propertyName, newValue) => setStockControlled(newValue)}
                    />
                </Grid>

                <Grid item xs={4}>
                    <Dropdown
                        fullWidth
                        value={credits}
                        label="Include Credits"
                        propertyName="credits"
                        items={[
                            { id: 'Y', displayText: 'Yes' },
                            { id: 'N', displayText: 'No' },
                            { id: 'O', displayText: 'Only' }
                        ]}
                        allowNoValue={false}
                        onChange={(propertyName, newValue) => setCredits(newValue)}
                    />
                </Grid>

                <Grid item xs={4}>
                    <Dropdown
                        fullWidth
                        value={cancelled}
                        label="Include Cancelled"
                        propertyName="cancelled"
                        items={[
                            { id: 'N', displayText: 'No' },
                            { id: 'Y', displayText: 'Yes' }
                        ]}
                        allowNoValue={false}
                        onChange={(propertyName, newValue) => setCancelled(newValue)}
                    />
                </Grid>

                <Grid item xs={12}>
                    <Button
                        color="primary"
                        variant="contained"
                        disabled={!fromDate && !toDate}
                        onClick={handleClick}
                    >
                        Run Report
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default OrdersBySupplierReportOptions;
