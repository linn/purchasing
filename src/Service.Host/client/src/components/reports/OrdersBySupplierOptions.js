import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import { Page, DatePicker, Title, Typeahead } from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import { useSelector, useDispatch } from 'react-redux';
import { getSearchItems, getSearchLoading } from '../../selectors/CollectionSelectorHelpers';
import { getReportLoading, getReportOptions } from '../../selectors/ReportSelectorHelpers';
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

    const [fromDate, setFromDate] = useState('12/12/21');
    const [toDate, setToDate] = useState('12/12/21');
    const [supplier, setSupplier] = useState();

    const handleSupplierChange = selectedsupplier => {
        setSupplier(selectedsupplier);
    };

    const handleClick = () =>
        history.push({
            pathname: `/purchasing/reports/orders-by-supplier/${supplier.Id}`,
            search: `?from=${fromDate.toISOString()}&to=${toDate.toISOString()}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Orders By Supplier" />
            <Grid style={{ marginTop: 40 }} container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <Typography variant="h6" gutterBottom>
                        Choose a date range:
                    </Typography>
                </Grid>
                <Grid item xs={3}>
                    <DatePicker
                        label="From Date"
                        value={fromDate.toString()}
                        onChange={setFromDate}
                    />
                </Grid>
                <Grid item xs={3}>
                    <DatePicker
                        label="To Date"
                        value={toDate.toString()}
                        minDate={fromDate.toString()}
                        onChange={setToDate}
                    />
                </Grid>
                <Grid item xs={6}>
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

OrdersBySupplierReportOptions.propTypes = {
    history: PropTypes.shape({ push: PropTypes.func }).isRequired
};

export default OrdersBySupplierReportOptions;
