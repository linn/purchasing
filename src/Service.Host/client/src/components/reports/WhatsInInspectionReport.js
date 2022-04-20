import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Typography from '@mui/material/Typography';
import {
    Page,
    Title,
    CheckboxWithLabel,
    Loading,
    ReportTable
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import { whatsInInspectionReport } from '../../reportTypes';
import whatsInInspectionReportActions from '../../actions/whatsInInspectionReportActions';

function WhatsInInspectionReport() {
    const dispatch = useDispatch();

    const [options, setOptions] = useState({
        includePartsWithNoOrderNumber: false,
        showStockLocations: true,
        includeFailedStock: false,
        includeFinishedGoods: true,
        showBackOrdered: true
    });

    const loading = useSelector(state => state[whatsInInspectionReport.item]?.loading);

    const reportData = useSelector(state => state[whatsInInspectionReport.item]?.data);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Whats In Inspection Report" />
                </Grid>

                <>
                    <Grid item xs={12}>
                        <CheckboxWithLabel
                            label="Include Parts With No OrderNumber"
                            checked={options.includePartsWithNoOrderNumber}
                            onChange={() =>
                                setOptions(o => ({
                                    ...o,
                                    includePartsWithNoOrderNumber: !o.includePartsWithNoOrderNumber
                                }))
                            }
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <CheckboxWithLabel
                            label="Show Stock Locations"
                            checked={options.showStockLocations}
                            onChange={() =>
                                setOptions(o => ({
                                    ...o,
                                    showStockLocations: !o.showStockLocations
                                }))
                            }
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <CheckboxWithLabel
                            label="Include Failed Stock"
                            checked={options.includeFailedStock}
                            onChange={() =>
                                setOptions(o => ({
                                    ...o,
                                    includeFailedStock: !o.includeFailedStock
                                }))
                            }
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <CheckboxWithLabel
                            label="Include Finished Goods"
                            checked={options.includeFinishedGoods}
                            onChange={() =>
                                setOptions(o => ({
                                    ...o,
                                    includeFinishedGoods: !o.includeFinishedGoods
                                }))
                            }
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <CheckboxWithLabel
                            label="Show Back Ordered"
                            checked={options.showBackOrdered}
                            onChange={() =>
                                setOptions(o => ({
                                    ...o,
                                    showBackOrdered: !o.showBackOrdered
                                }))
                            }
                        />
                    </Grid>
                    <Grid item xs={3}>
                        <Button
                            variant="contained"
                            color="primary"
                            onClick={() =>
                                dispatch(whatsInInspectionReportActions.fetchReport(options))
                            }
                        >
                            Run
                        </Button>
                    </Grid>
                </>

                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        {reportData && (
                            <>
                                <Grid item xs={12}>
                                    <TableContainer
                                        sx={{ overflowX: 'scroll', transform: 'rotateX(180deg)' }}
                                    >
                                        <Table
                                            sx={{ transform: 'rotateX(180deg)' }}
                                            aria-label="simple table"
                                        >
                                            <TableHead>
                                                <TableRow>
                                                    <TableCell>
                                                        <Typography variant="h5">
                                                            Part Number
                                                        </Typography>
                                                    </TableCell>
                                                    <TableCell>
                                                        <Typography variant="h5">
                                                            Description
                                                        </Typography>
                                                    </TableCell>

                                                    <TableCell align="right">
                                                        <Typography variant="h5">Units</Typography>
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        <Typography variant="h5">
                                                            Qty In Stock
                                                        </Typography>
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        <Typography variant="h5">
                                                            Qty In Inspection
                                                        </Typography>
                                                    </TableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {reportData.map(row => (
                                                    <>
                                                        <TableRow
                                                            key={row.partNumber}
                                                            sx={{
                                                                '&:last-child td, &:last-child th':
                                                                    {
                                                                        border: 0
                                                                    }
                                                            }}
                                                        >
                                                            <TableCell component="th" scope="row">
                                                                <Typography variant="h6">
                                                                    {row.partNumber}
                                                                </Typography>
                                                            </TableCell>
                                                            <TableCell component="th" scope="row">
                                                                <Typography variant="h6">
                                                                    {row.description}
                                                                </Typography>
                                                            </TableCell>
                                                            <TableCell align="right">
                                                                <Typography variant="h6">
                                                                    {row.ourUnitOfMeasure}
                                                                </Typography>
                                                            </TableCell>
                                                            <TableCell align="right">
                                                                <Typography variant="h6">
                                                                    {row.qtyInStock}
                                                                </Typography>
                                                            </TableCell>
                                                            <TableCell align="right">
                                                                <Typography variant="h6">
                                                                    {row.qtyInInspection}
                                                                </Typography>
                                                            </TableCell>
                                                        </TableRow>
                                                        <TableRow key={row.partNumber}>
                                                            {row.locationsBreakdown && (
                                                                <TableCell
                                                                    align="right"
                                                                    colSpan={2}
                                                                >
                                                                    <ReportTable
                                                                        reportData={
                                                                            row.locationsBreakdown
                                                                                .reportResults[0]
                                                                        }
                                                                        title={row.title}
                                                                        showTitle={false}
                                                                        showTotals
                                                                        placeholderRows={4}
                                                                        placeholderColumns={4}
                                                                    />
                                                                </TableCell>
                                                            )}
                                                            <TableCell align="right" colSpan={3}>
                                                                <ReportTable
                                                                    reportData={
                                                                        row.ordersBreakdown
                                                                            .reportResults[0]
                                                                    }
                                                                    title={row.title}
                                                                    showTitle={false}
                                                                    showTotals
                                                                    placeholderRows={4}
                                                                    placeholderColumns={4}
                                                                />
                                                            </TableCell>
                                                        </TableRow>
                                                    </>
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                </Grid>
                            </>
                        )}
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default WhatsInInspectionReport;
