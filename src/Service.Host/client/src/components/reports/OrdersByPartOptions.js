import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    DatePicker,
    Dropdown,
    Title,
    Typeahead,
    collectionSelectorHelpers,
    reportSelectorHelpers
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import partsActions from '../../actions/partsActions';

function OrdersByPartReportOptions() {
    const partsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.parts)
    ).map?.(c => ({
        id: c.partNumber,
        name: c.partNumber,
        partNumber: c.partNumber,
        description: c.description
    }));
    const partsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.parts)
    );
    const prevOptions = useSelector(state =>
        reportSelectorHelpers.getReportOptions(state.ordersByPart)
    );

    const dispatch = useDispatch();

    const defaultStartDate = new Date();
    defaultStartDate.setMonth(defaultStartDate.getMonth() - 1);

    const [fromDate, setFromDate] = useState(
        prevOptions?.fromDate ? new Date(prevOptions?.fromDate) : defaultStartDate
    );
    const [toDate, setToDate] = useState(
        prevOptions?.toDate ? new Date(prevOptions?.toDate) : new Date()
    );

    const [part, setPart] = useState(
        prevOptions?.partNumber
            ? { partNumber: prevOptions.partNumber }
            : { partNumber: 'click to set part' }
    );
    const [cancelled, setCancelled] = useState(
        prevOptions?.cancelled ? prevOptions.cancelled : 'N'
    );

    const handlePartChange = selectedPart => {
        setPart(selectedPart);
    };

    const handleClick = () =>
        history.push({
            pathname: `/purchasing/reports/orders-by-part/report`,
            search:
                `?partNumber=${part.partNumber}` +
                `&fromDate=${fromDate.toISOString()}` +
                `&toDate=${toDate.toISOString()}` +
                `&cancelled=${cancelled}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot} width="s">
            <Title text="Orders By Part" />
            <Grid container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <Typeahead
                        label="Part"
                        title="Search for a part"
                        onSelect={handlePartChange}
                        items={partsSearchResults}
                        loading={partsSearchLoading}
                        fetchItems={searchTerm => dispatch(partsActions.search(searchTerm))}
                        clearSearch={() => dispatch(partsActions.clearSearch)}
                        value={`${part?.partNumber}`}
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
                <Grid item xs={8} />

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

export default OrdersByPartReportOptions;
