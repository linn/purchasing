import React, { useEffect, useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import queryString from 'query-string';
import {
    Page,
    Title,
    Typeahead,
    collectionSelectorHelpers,
    CheckboxWithLabel,
    Loading,
    Dropdown,
    ReportTable,
    ExportButton
} from '@linn-it/linn-form-components-library';
import moment from 'moment';
import { DatePicker } from '@mui/x-date-pickers';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import suppliersActions from '../../actions/suppliersActions';
import tqmsJobrefsActions from '../../actions/tqmsJobrefsActions';
import { suppliers, tqmsJobrefs } from '../../itemTypes';
import { partsReceivedReport } from '../../reportTypes';
import partsReceivedReportActions from '../../actions/partsReceivedReportActions';

function PartsReceivedReport() {
    const dispatch = useDispatch();
    const jobrefOptions = useSelector(state =>
        collectionSelectorHelpers.getItems(state[tqmsJobrefs.item])
    );
    const tqmsJobrefsLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state[tqmsJobrefs.item])
    );

    useEffect(() => {
        dispatch(tqmsJobrefsActions.fetch());
    }, [dispatch]);

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state[suppliers.item])
    )?.map(c => ({
        id: c.id,
        name: c.id.toString(),
        description: c.name
    }));
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );

    const defaultStartDate = moment().subtract(1, 'months');

    const [options, setOptions] = useState({
        jobref: '',
        fromDate: defaultStartDate,
        toDate: moment(),
        orderBy: 'DATE BOOKED',
        includeNegativeValues: true
    });

    const handleOptionChange = (propertyName, newValue) => {
        setOptions(o => ({ ...o, [propertyName]: newValue }));
    };

    useEffect(() => {
        if (jobrefOptions?.length) {
            setOptions(o => ({ ...o, jobref: jobrefOptions[0].jobref }));
        }
    }, [jobrefOptions]);

    const loading = useSelector(state => state[partsReceivedReport.item]?.loading);

    const reportData = useSelector(state => state[partsReceivedReport.item]?.data);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Parts Received Report" />
                </Grid>
                {tqmsJobrefsLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={4}>
                            <Dropdown
                                label="Jobref"
                                propertyName="jobref"
                                value={options.jobref}
                                onChange={handleOptionChange}
                                items={jobrefOptions.map(x => ({
                                    id: x.jobref,
                                    displayText: `${x.jobref} - ${x.date}`
                                }))}
                                allowNoValue
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <Typeahead
                                label="Supplier (leave blank for all)"
                                title="Search for a supplier"
                                onSelect={newValue => handleOptionChange('supplier', newValue.id)}
                                items={suppliersSearchResults}
                                loading={suppliersSearchLoading}
                                fetchItems={searchTerm =>
                                    dispatch(suppliersActions.search(searchTerm))
                                }
                                clearSearch={() => dispatch(suppliersActions.clearSearch)}
                                value={options.supplier}
                                modal
                                links={false}
                                debounce={1000}
                                minimumSearchTermLength={2}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <Dropdown
                                label="Order By"
                                propertyName="orderBy"
                                value={options.orderBy}
                                onChange={handleOptionChange}
                                items={[
                                    'DATE BOOKED',
                                    'PART',
                                    'SUPPLIER',
                                    'OVERSTOCK',
                                    'MATERIAL PRICE'
                                ]}
                            />
                        </Grid>

                        <Grid item xs={3}>
                            <DatePicker
                                label="From Date"
                                value={options.fromDate}
                                format="DD/MM/YYYY"
                                maxDate={options.toDate}
                                onChange={newVal => setOptions(o => ({ ...o, fromDate: newVal }))}
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <DatePicker
                                label="To Date"
                                value={options.toDate}
                                format="DD/MM/YYYY"
                                maxDate={moment()}
                                minDate={options.fromDate}
                                onChange={newVal => setOptions(o => ({ ...o, toDate: newVal }))}
                            />
                        </Grid>
                        <Grid item xs={6} />
                        <Grid item xs={3}>
                            <CheckboxWithLabel
                                label="Include Negative Values"
                                checked={options.includeNegativeValues}
                                onChange={() =>
                                    setOptions(o => ({
                                        ...o,
                                        includeNegativeValues: !o.includeNegativeValues
                                    }))
                                }
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={() =>
                                    dispatch(
                                        partsReceivedReportActions.fetchReport({
                                            ...options,
                                            fromDate: options.fromDate.toISOString(),
                                            toDate: options.toDate.toISOString()
                                        })
                                    )
                                }
                            >
                                Run
                            </Button>
                        </Grid>
                    </>
                )}
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        {reportData && (
                            <>
                                <Grid item xs={12}>
                                    <ExportButton
                                        href={`${
                                            config.appRoot
                                        }/purchasing/reports/parts-received?${queryString.stringify(
                                            {
                                                ...options,
                                                fromDate: options.fromDate.toISOString(),
                                                toDate: options.toDate.toISOString()
                                            }
                                        )}`}
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <ReportTable
                                        reportData={reportData}
                                        title={reportData.title}
                                        showTitle
                                        showTotals
                                        placeholderRows={4}
                                        placeholderColumns={4}
                                    />
                                </Grid>
                            </>
                        )}
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default PartsReceivedReport;
