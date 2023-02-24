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
import Typography from '@mui/material/Typography';
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
        { field: 'changeId', headerName: 'CHG ID', width: 100, hide: true },
        {
            field: 'detailId',
            headerName: 'DET ID',
            width: 200,
            hide: true,
            renderCell: cellValues => (
                <div>
                    {cellValues.row.detailId.split('\r\n').map(v => (
                        <Typography>{v}</Typography>
                    ))}
                </div>
            )
        },
        { field: 'bomName', headerName: 'Name', width: 150 },
        { field: 'dateApplied', headerName: 'Date', width: 150 },
        { field: 'appliedBy', headerName: 'By', width: 200 },
        { field: 'documentType', headerName: 'Doc', width: 100 },
        { field: 'documentNumber', headerName: 'Number', width: 150 },
        {
            field: 'operation',
            headerName: 'Op',
            width: 150,
            renderCell: cellValues => (
                <div>
                    {cellValues.row.operation.split('\r\n').map(v => (
                        <Typography>{v}</Typography>
                    ))}
                </div>
            )
        },
        {
            field: 'partNumber',
            headerName: 'Part',
            width: 250,
            renderCell: cellValues => (
                <div>
                    {cellValues.row.partNumber.split('\r\n').map(v => (
                        <Typography>{v}</Typography>
                    ))}
                </div>
            )
        },
        {
            field: 'qty',
            headerName: 'Qty',
            width: 150,
            renderCell: cellValues => (
                <div>
                    {cellValues.row.qty.split('\r\n').map(v => (
                        <Typography>{v}</Typography>
                    ))}
                </div>
            )
        },
        {
            field: 'generateRequirement',
            headerName: 'Req',
            width: 150,
            renderCell: cellValues => (
                <div>
                    {cellValues.row.generateRequirement.split('\r\n').map(v => (
                        <Typography>{v}</Typography>
                    ))}
                </div>
            )
        }
    ];
    const defaultStartDate = new Date();
    defaultStartDate.setMonth(defaultStartDate.getMonth() - 1);
    const [options, setOptions] = useState({
        bomName: 'SK HUB',
        includeSubAssemblies: true,
        from: new Date('2022-11-01T00:00:00'),
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
                                id: `${d.bomName}/${d.partNumber}/${d.changeId}`
                            })) ?? []
                        }
                        loading={loading}
                        autoHeight
                        getRowHeight={() => 'auto'}
                        disableSelectionOnClick
                        columns={columns}
                    />
                </Grid>
            </Grid>
        </Page>
    );
};

export default BomHistoryReport;
