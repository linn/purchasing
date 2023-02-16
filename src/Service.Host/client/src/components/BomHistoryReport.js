import React, { useState } from 'react';
import {
    Page,
    DatePicker,
    InputField,
    Title,
    collectionSelectorHelpers,
    OnOffSwitch
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import queryString from 'query-string';
import history from '../history';
import config from '../config';
import bomHistoryReportActions from '../actions/bomHistoryReportActions';
import { bomHistoryReport } from '../itemTypes';

const BomHistoryReport = () => {
    const loading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state[bomHistoryReport.item])
    );
    const data = useSelector(state =>
        collectionSelectorHelpers.getItems(state[bomHistoryReport.item])
    );
    const columns = [
        { field: 'changeId', headerName: 'CHG ID', width: 100 },
        { field: 'detailId', headerName: 'DET ID', width: 100 },
        { field: 'bomName', headerName: 'Name', width: 150 },
        { field: 'dateApplied', headerName: 'Date', width: 150 },
        { field: 'documentType', headerName: 'Doc', width: 100 },
        { field: 'documentNumber', headerName: 'Number', width: 150 },
        { field: 'operation', headerName: 'Op', width: 150 },
        { field: 'partNumber', headerName: 'Part', width: 150 },
        { field: 'qty', headerName: 'Qty', width: 150 },
        { field: 'generateRequirement', headerName: 'Req', width: 150 }
    ];
    const defaultStartDate = new Date();
    defaultStartDate.setMonth(defaultStartDate.getMonth() - 1);
    const [options, setOptions] = useState({
        includeSubAssemblies: true,
        from: defaultStartDate,
        to: new Date()
    });
    const handleOptionChange = (propertyName, newValue) => {
        setOptions(o => ({ ...o, [propertyName]: newValue }));
    };

    const dispatch = useDispatch();

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="BOM History Report" />
                </Grid>
                <Grid item xs={2}>
                    <InputField
                        propertyName="bomName"
                        value={options.bomName}
                        label="BOM"
                        fullWidth
                        onChange={handleOptionChange}
                    />
                </Grid>
                <Grid item xs={2}>
                    <DatePicker
                        label="From Date"
                        value={options.from.toString()}
                        minDate="01/01/2000"
                        maxDate={options.to.toString()}
                        onChange={newVal => setOptions(o => ({ ...o, from: newVal }))}
                    />
                </Grid>
                <Grid item xs={2}>
                    <DatePicker
                        label="To Date"
                        value={options.to.toString()}
                        minDate={options.from.toString()}
                        onChange={newVal => setOptions(o => ({ ...o, to: newVal }))}
                    />
                </Grid>
                <Grid item xs={2}>
                    <OnOffSwitch
                        label="Incl Sub Assemblies?"
                        value={options.includeSubAssemblies}
                        onChange={handleOptionChange}
                        propertyName="includeSubAssemblies"
                    />
                </Grid>
                <Grid item xs={3}>
                    <Button
                        variant="contained"
                        disabled={!options.bomName}
                        onClick={() => {
                            dispatch(
                                bomHistoryReportActions.fetchByHref(
                                    `/purchasing/reports/bom-history?${queryString.stringify({
                                        ...options,
                                        bomName: options.bomName.toUpperCase(),
                                        from: options.from.toISOString(),
                                        to: options.to.toISOString()
                                    })}`
                                )
                            );
                        }}
                    >
                        RUN REPORT
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    <DataGrid
                        columnBuffer={6}
                        rows={
                            data?.map(d => ({
                                ...d,
                                id: `${d.bomName}/${d.detailId}/${d.changeId}`
                            })) ?? []
                        }
                        loading={loading}
                        autoHeight
                        disableSelectionOnClick
                        columns={columns}
                    />
                </Grid>
            </Grid>
        </Page>
    );
};

export default BomHistoryReport;
