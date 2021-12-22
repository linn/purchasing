import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    DatePicker,
    Dropdown,
    Title,
    Typeahead
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import moment from 'moment';
import { getSearchItems, getSearchLoading } from '../../selectors/CollectionSelectorHelpers';
import { getReportOptions } from '../../selectors/ReportSelectorHelpers';
import history from '../../history';
import config from '../../config';
import suppliersActions from '../../actions/suppliersActions';

function OrdersBySupplierReportOptions() {
    const suppliersSearchResults = useSelector(state => getSearchItems(state.suppliers)).map?.(
        c => ({
            id: c.id,
            name: c.name.toString(),
            description: c.name
        })
    );
    const suppliersSearchLoading = useSelector(state => getSearchLoading(state.suppliers));
    const prevOptions = useSelector(state => getReportOptions(state.suppliers));

    const dispatch = useDispatch();

    const defaultStartDate = new Date();
    defaultStartDate.setMonth(defaultStartDate.getMonth() - 1);

    const [fromDate, setFromDate] = useState(
        prevOptions?.fromDate ? new Date(prevOptions?.fromDate) : defaultStartDate
    );
    const [toDate, setToDate] = useState(
        prevOptions?.toDate ? new Date(prevOptions?.toDate) : new Date()
    );

    const [supplier, setSupplier] = useState({ name: 'please enter a value' });
    const [outstandingOnly, setOutstandingOnly] = useState('N');
    const [returns, setReturns] = useState('N');
    const [stockControlled, setStockControlled] = useState('A');
    const [credits, setCredits] = useState('N');
    const [cancelled, setCancelled] = useState('N');

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
                <Grid item xs={12}>
                    <Typeahead
                        label="Supplier"
                        title="Search for a supplier"
                        onSelect={handleSupplierChange}
                        items={suppliersSearchResults}
                        loading={suppliersSearchLoading}
                        fetchItems={searchTerm => dispatch(suppliersActions.search(searchTerm))}
                        clearSearch={() => dispatch(suppliersActions.clearSearch)}
                        value={`${supplier?.id} - ${supplier?.name}`}
                        modal
                        links={false}
                        debounce={1000}
                        minimumSearchTermLength={2}
                    />
                </Grid>
                <Grid item xs={6}>
                    <DatePicker
                        label="From Date"
                        value={fromDate.toString()}
                        minDate="01/01/2000"
                        maxDate={toDate}
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
                            { id: 'Y', displayText: 'Stock Controlled Only' },
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
