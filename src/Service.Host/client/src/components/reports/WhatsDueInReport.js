import React, { useEffect, useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import queryString from 'query-string';
import {
    Page,
    Title,
    Typeahead,
    collectionSelectorHelpers,
    ExportButton,
    Loading,
    Dropdown,
    ReportTable,
    DatePicker
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import suppliersActions from '../../actions/suppliersActions';
import { suppliers, vendorManagers } from '../../itemTypes';
import { whatsDueInReport } from '../../reportTypes';
import vendorManagersActions from '../../actions/vendorManagersActions';
import whatsDueInReportActions from '../../actions/whatsDueInReportActions';

function WhatsDueInReport() {
    const dispatch = useDispatch();
    const vendorManagersOptions = useSelector(state =>
        collectionSelectorHelpers.getItems(state[vendorManagers.item])
    );
    const vendorManagersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state[vendorManagers.item])
    );
    useEffect(() => {
        dispatch(vendorManagersActions.fetch());
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

    const defaultStartDate = new Date();
    defaultStartDate.setMonth(defaultStartDate.getMonth() - 1);

    const [options, setOptions] = useState({
        fromDate: defaultStartDate,
        toDate: new Date(),
        orderBy: 'ORDER NUMBER'
    });

    const handleOptionChange = (propertyName, newValue) => {
        setOptions(o => ({ ...o, [propertyName]: newValue }));
    };

    const loading = useSelector(state => state[whatsDueInReport.item]?.loading);

    const reportData = useSelector(state => state[whatsDueInReport.item]?.data);

    return (
        <Page history={history} homeUrl={config.appRoot} title="What's Due In">
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Whats Due In Report" />
                </Grid>
                {vendorManagersLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={3}>
                            <Dropdown
                                label="Vendor Manager  (leave blank for all)"
                                propertyName="vendorManager"
                                value={options.vendorManager}
                                onChange={handleOptionChange}
                                items={vendorManagersOptions.map(v => ({
                                    id: v.vmId,
                                    displayText: `${v.vmId} ${v.name} (${v.userNumber})`
                                }))}
                                allowNoValue
                            />
                        </Grid>
                        <Grid item xs={3}>
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
                        <Grid item xs={3}>
                            <Dropdown
                                label="Order By"
                                propertyName="orderBy"
                                value={options.orderBy}
                                onChange={handleOptionChange}
                                items={['EXPECTED DATE', 'ORDER NUMBER', 'VALUE', 'SUPPLIER']}
                            />
                        </Grid>
                        <Grid item xs={3} />

                        <Grid item xs={3}>
                            <DatePicker
                                label="From Date"
                                value={options.fromDate}
                                propertyName="fromDate"
                                maxDate={options.toDate?.toString() || null}
                                onChange={newVal => setOptions(o => ({ ...o, fromDate: newVal }))}
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <DatePicker
                                label="To Date"
                                propertyName="toDate"
                                value={options.toDate}
                                minDate={options.fromDate || null}
                                onChange={newVal => {
                                    setOptions(o => ({ ...o, toDate: newVal }));
                                }}
                            />
                        </Grid>
                        <Grid item xs={6} />
                        <Grid item xs={6} />

                        <Grid item xs={6}>
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={() =>
                                    dispatch(
                                        whatsDueInReportActions.fetchReport({
                                            ...options,
                                            fromDate: options.fromDate.toISOString(),
                                            toDate: options.toDate.toISOString()
                                        })
                                    )
                                }
                            >
                                Run Report
                            </Button>
                            <Button
                                color="primary"
                                variant="outlined"
                                onClick={() =>
                                    setOptions({
                                        fromDate: defaultStartDate,
                                        toDate: new Date(),
                                        orderBy: 'ORDER NUMBER'
                                    })
                                }
                            >
                                Reset Filters
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
                                        }/purchasing/reports/whats-due-in?${queryString.stringify({
                                            ...options,
                                            fromDate: options.fromDate.toISOString(),
                                            toDate: options.toDate.toISOString()
                                        })}`}
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

export default WhatsDueInReport;
