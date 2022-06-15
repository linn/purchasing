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
import queryString from 'query-string';
import {
    Page,
    Title,
    CheckboxWithLabel,
    ExportButton,
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
        showStockLocations: false,
        includeFailedStock: false,
        includeFinishedGoods: true,
        showBackOrdered: false,
        showOrders: false,
        showGoodStockQty: false
    });

    const loading = useSelector(state => state[whatsInInspectionReport.item]?.loading);

    const reportData = useSelector(state => state[whatsInInspectionReport.item]?.data);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <div className="hide-when-printing">
                    <Grid item xs={12}>
                        <Title text="Whats In Inspection Report" />
                    </Grid>
                    <Grid item xs={12}>
                        <CheckboxWithLabel
                            label="Show good stock qty"
                            checked={options.showGoodStockQty}
                            onChange={() =>
                                setOptions(o => ({
                                    ...o,
                                    showGoodStockQty: !o.showGoodStockQty
                                }))
                            }
                        />
                    </Grid>
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
                    <Grid item xs={12}>
                        <CheckboxWithLabel
                            label="Show Orders"
                            checked={options.showOrders}
                            onChange={() =>
                                setOptions(o => ({
                                    ...o,
                                    showOrders: !o.showOrders
                                }))
                            }
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <Typography variant="subtitle2">
                            Click run to run the report or export to download a csv of the top level
                            data (i.e. the report without the orders and locations breakdowns)
                        </Typography>
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
                        <ExportButton
                            href={`${
                                config.appRoot
                            }/purchasing/reports/whats-in-inspection/export?${queryString.stringify(
                                options
                            )}`}
                        />
                    </Grid>
                </div>

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
                                                        <Typography variant="subtitle2">
                                                            Part Number
                                                        </Typography>
                                                    </TableCell>
                                                    <TableCell>
                                                        <Typography variant="subtitle2">
                                                            Description
                                                        </Typography>
                                                    </TableCell>
                                                    <TableCell>
                                                        <Typography variant="subtitle2">
                                                            Oldest Batch
                                                        </Typography>
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        <Typography variant="subtitle2">
                                                            Units
                                                        </Typography>
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        <Typography variant="subtitle2">
                                                            Qty In Stock
                                                        </Typography>
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        <Typography variant="subtitle2">
                                                            Qty In Inspection
                                                        </Typography>
                                                    </TableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {reportData.partsInInspection.map(row => (
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
                                                                <Typography variant="subtitle2">
                                                                    {row.partNumber}
                                                                </Typography>
                                                            </TableCell>
                                                            <TableCell component="th" scope="row">
                                                                <Typography variant="subtitle2">
                                                                    {row.description}
                                                                </Typography>
                                                            </TableCell>
                                                            <TableCell component="th" scope="row">
                                                                <Typography variant="subtitle2">
                                                                    {row.batch}
                                                                </Typography>
                                                            </TableCell>
                                                            <TableCell align="right">
                                                                <Typography variant="subtitle2">
                                                                    {row.ourUnitOfMeasure}
                                                                </Typography>
                                                            </TableCell>
                                                            <TableCell align="right">
                                                                <Typography variant="subtitle2">
                                                                    {row.qtyInStock}
                                                                </Typography>
                                                            </TableCell>
                                                            <TableCell align="right">
                                                                <Typography variant="subtitle2">
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
                                                                        showTitle={false}
                                                                        showTotals
                                                                        placeholderRows={4}
                                                                        placeholderColumns={4}
                                                                    />
                                                                </TableCell>
                                                            )}
                                                            {row.ordersBreakdown && (
                                                                <TableCell
                                                                    align="right"
                                                                    colSpan={3}
                                                                >
                                                                    <ReportTable
                                                                        reportData={
                                                                            row.ordersBreakdown
                                                                                .reportResults[0]
                                                                        }
                                                                        showTitle={false}
                                                                        showTotals
                                                                        placeholderRows={4}
                                                                        placeholderColumns={4}
                                                                    />
                                                                </TableCell>
                                                            )}
                                                        </TableRow>
                                                    </>
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                    {reportData.backOrderData && (
                                        <Grid item xs={12}>
                                            <ReportTable
                                                reportData={
                                                    reportData.backOrderData.reportResults[0]
                                                }
                                                showTitle
                                                title="Parts In Inspection On Purchasing Back Order"
                                                placeholderRows={4}
                                                placeholderColumns={4}
                                            />
                                        </Grid>
                                    )}
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
