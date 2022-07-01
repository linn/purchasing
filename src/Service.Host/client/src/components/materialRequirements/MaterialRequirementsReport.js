import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    itemSelectorHelpers,
    Loading,
    utilities
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import Stack from '@mui/material/Stack';
import Tooltip from '@mui/material/Tooltip';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import Button from '@mui/material/Button';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import NotesIcon from '@mui/icons-material/Notes';
import ShopIcon from '@mui/icons-material/Shop';
import OpenInFullIcon from '@mui/icons-material/OpenInFull';
import CloseFullscreenIcon from '@mui/icons-material/CloseFullscreen';
import ModeEditOutlineOutlinedIcon from '@mui/icons-material/ModeEditOutlineOutlined';
import Link from '@mui/material/Link';
import { DataGrid } from '@mui/x-data-grid';
import makeStyles from '@mui/styles/makeStyles';

import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';

import { useLocation } from 'react-router';
import queryString from 'query-string';

import moment from 'moment';
import { mrReport as mrReportItem, mrReportOrders as mrReportOrdersItem } from '../../itemTypes';
import mrReportActions from '../../actions/mrReportActions';
import mrReportOrdersActions from '../../actions/mrReportOrdersActions';

import history from '../../history';
import config from '../../config';

function MaterialRequirementsReport() {
    const [selectedItem, setSelectedItem] = useState(null);
    const [selectedIndex, setSelectedIndex] = useState(null);
    const [selectedSegment, setSelectedSegment] = useState(0);
    const [nextPart, setNextPart] = useState(null);
    const [previousPart, setPreviousPart] = useState(null);
    const [selectedPurchaseOrders, setSelectedPurchaseOrders] = useState([]);

    const options = useLocation();

    const mrReport = useSelector(state => itemSelectorHelpers.getItem(state.mrReport));
    const mrReportLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrReport)
    );

    const mrReportOrders = useSelector(state => itemSelectorHelpers.getItem(state.mrReportOrders));
    const mrReportOrdersLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrReportOrders)
    );

    const theme = createTheme({
        typography: {
            fontSize: 12
        },
        palette: {
            navBut: {
                main: '#616161'
            }
        }
    });

    const dispatch = useDispatch();

    useEffect(() => {
        if (options && options.state) {
            dispatch(mrReportActions.postByHref(mrReportItem.uri, options.state));
        } else if (options && options.search) {
            const query = queryString.parse(options.search);
            if (query.partNumber) {
                dispatch(
                    mrReportActions.postByHref(mrReportItem.uri, {
                        partNumber: query.partNumber,
                        jobRef: query.jobRef,
                        typeOfReport: 'MR',
                        partSelector: 'Select Parts',
                        supplierId: query.supplierId,
                        stockCategoryName: query.stockCategoryName
                    })
                );
            } else if (query.partNumberList) {
                dispatch(
                    mrReportActions.postByHref(mrReportItem.uri, {
                        partNumberList: query.partNumberList,
                        jobRef: query.jobRef,
                        typeOfReport: 'MR',
                        partSelector: 'Part Number List',
                        supplierId: query.supplierId,
                        stockCategoryName: query.stockCategoryName
                    })
                );
            }
        } else {
            history.push('/purchasing/material-requirements');
        }
    }, [dispatch, options]);

    useEffect(() => {
        if (mrReport && mrReport.results && mrReport.results.length > 0) {
            setSelectedIndex(0);
            setSelectedItem(mrReport.results[0]);
            if (mrReport.results.length > 1) {
                setNextPart(mrReport.results[1].partNumber);
            } else {
                setNextPart(null);
            }
            setPreviousPart(null);

            dispatch(
                mrReportOrdersActions.postByHref(mrReportOrdersItem.uri, {
                    partNumbers: mrReport.results.map(r => r.partNumber),
                    jobRef: mrReport.results[0].jobRef
                })
            );
        } else {
            setSelectedItem(null);
            setNextPart(null);
            setPreviousPart(null);
            dispatch(mrReportOrdersActions.clearItem());
        }
    }, [mrReport, dispatch]);

    useEffect(() => {
        if (mrReportOrders?.orders && selectedItem) {
            setSelectedPurchaseOrders(
                mrReportOrders.orders.filter(a => a.partNumber === selectedItem.partNumber)
            );
        } else {
            setSelectedPurchaseOrders([]);
        }
    }, [mrReportOrders, selectedItem]);

    const useStyles = makeStyles(() => ({
        headerText: {
            fontWeight: 500
        },
        boldText: {
            fontWeight: 550
        },
        newQuarter: {
            paddingTop: '50px',
            fontWeight: 500
        },
        redBoxOutline: {
            borderStyle: 'solid',
            borderColor: 'red !important',
            borderWidth: 'thin'
        },
        blueBoxOutline: {
            borderStyle: 'solid',
            borderColor: 'blue !important',
            borderWidth: 'thin'
        },
        greenBoxOutline: {
            borderStyle: 'solid',
            borderColor: 'green !important',
            borderWidth: 'thin'
        }
    }));
    const classes = useStyles();

    const backToOptions = () => {
        history.push('/purchasing/material-requirements');
    };

    const goToPreviousPart = () => {
        if (selectedIndex === 0) {
            return;
        }

        setNextPart(selectedItem.partNumber);
        if (selectedIndex === 1) {
            setPreviousPart(null);
        } else {
            setPreviousPart(mrReport.results[selectedIndex - 2].partNumber);
        }

        setSelectedItem(mrReport.results[selectedIndex - 1]);
        setSelectedIndex(selectedIndex - 1);
    };

    const goToNextPart = () => {
        if (selectedIndex === mrReport.results.length - 1) {
            return;
        }

        setPreviousPart(selectedItem.partNumber);
        if (selectedIndex === mrReport.results.length - 2) {
            setNextPart(null);
        } else {
            setNextPart(mrReport.results[selectedIndex + 2].partNumber);
        }

        setSelectedItem(mrReport.results[selectedIndex + 1]);
        setSelectedIndex(selectedIndex + 1);
    };

    const getCellClass = params => {
        switch (params.field) {
            case 'immediate':
                if (params.row.immediateItem?.tag) {
                    return classes[params.row.immediateItem.tag];
                }
                return null;
            case 'week0':
                if (params.row.week0Item?.tag) {
                    return classes[params.row.week0Item.tag];
                }
                return null;
            case 'week1':
                if (params.row.week1Item?.tag) {
                    return classes[params.row.week1Item.tag];
                }
                return null;
            case 'week2':
                if (params.row.week2Item?.tag) {
                    return classes[params.row.week2Item.tag];
                }
                return null;
            case 'week3':
                if (params.row.week3Item?.tag) {
                    return classes[params.row.week3Item.tag];
                }
                return null;
            case 'week4':
                if (params.row.week4Item?.tag) {
                    return classes[params.row.week4Item.tag];
                }
                return null;
            case 'week5':
                if (params.row.week5Item?.tag) {
                    return classes[params.row.week5Item.tag];
                }
                return null;
            case 'week6':
                if (params.row.week6Item?.tag) {
                    return classes[params.row.week6Item.tag];
                }
                return null;
            case 'week7':
                if (params.row.week7Item?.tag) {
                    return classes[params.row.week7Item.tag];
                }
                return null;
            case 'week8':
                if (params.row.week8Item?.tag) {
                    return classes[params.row.week8Item.tag];
                }
                return null;
            case 'week9':
                if (params.row.week9Item?.tag) {
                    return classes[params.row.week9Item.tag];
                }
                return null;
            case 'week10':
                if (params.row.week10Item?.tag) {
                    return classes[params.row.week10Item.tag];
                }
                return null;
            case 'week11':
                if (params.row.week11Item?.tag) {
                    return classes[params.row.week11Item.tag];
                }
                return null;
            case 'week12':
                if (params.row.week12Item?.tag) {
                    return classes[params.row.week12Item.tag];
                }
                return null;
            default:
                return null;
        }
    };

    const detailsColumns = [
        { field: 'title', headerName: '', width: 120 },
        {
            field: 'immediate',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week0',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week1',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week2',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week3',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week4',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week5',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week6',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week7',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week8',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week9',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week10',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week11',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        {
            field: 'week12',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        }
    ];

    const getRowClass = params => {
        const title = params?.row?.title;
        const segment = params?.row?.segment;
        if (title === 'Stock') {
            return classes.boldText;
        }

        if (title === 'Ending') {
            return classes.headerText;
        }

        if (title === 'Week') {
            if (segment === 0 || selectedSegment > -1) {
                return classes.headerText;
            }

            return classes.newQuarter;
        }

        return null;
    };

    const mapItems = items =>
        items.map((b, i) => ({
            ...b,
            id: i,
            immediate: b.immediateItem?.textValue || b.immediateItem?.value,
            week0: b.week0Item?.textValue || b.week0Item?.value,
            week1: b.week1Item?.textValue || b.week1Item?.value,
            week2: b.week2Item?.textValue || b.week2Item?.value,
            week3: b.week3Item?.textValue || b.week3Item?.value,
            week4: b.week4Item?.textValue || b.week4Item?.value,
            week5: b.week5Item?.textValue || b.week5Item?.value,
            week6: b.week6Item?.textValue || b.week6Item?.value,
            week7: b.week7Item?.textValue || b.week7Item?.value,
            week8: b.week8Item?.textValue || b.week8Item?.value,
            week9: b.week9Item?.textValue || b.week9Item?.value,
            week10: b.week10Item?.textValue || b.week10Item?.value,
            week11: b.week11Item?.textValue || b.week11Item?.value,
            week12: b.week12Item?.textValue || b.week12Item?.value
        }));

    const getRows = (item, segment) => {
        if (segment === -1) {
            return mapItems(item.details);
        }

        return mapItems(item.details.filter(a => a.segment === segment));
    };
    const nextSegment = () => {
        if (selectedSegment < 5) {
            setSelectedSegment(selectedSegment + 1);
        }
    };

    const previousSegment = () => {
        if (selectedSegment > 0) {
            setSelectedSegment(selectedSegment - 1);
        }
    };

    const toggleShowAllSegments = () => {
        if (selectedSegment === -1) {
            setSelectedSegment(0);
        } else {
            setSelectedSegment(-1);
        }
    };

    const onKeyPressed = data => {
        if (data.keyCode === 37) {
            goToPreviousPart();
        } else if (data.keyCode === 39) {
            goToNextPart();
        }
    };

    const showRow = row => (
        <>
            <TableRow key={`${row.orderNumber}/${row.orderLine}/h1`}>
                <TableCell component="th" scope="row">
                    <Link
                        href={utilities.getHref(row, 'view-order')}
                        underline="hover"
                        color="inherit"
                        variant="body2"
                    >
                        {row.orderNumber}
                    </Link>
                </TableCell>
                <TableCell>{row.orderLine}</TableCell>
                <TableCell>{moment(row.dateOfOrder).format('DD MMM YYYY')}</TableCell>
                <TableCell>{row.supplierId}</TableCell>
                <TableCell>{row.supplierName}</TableCell>
                <TableCell />
                <TableCell align="right">{row.quantity}</TableCell>
                <TableCell align="right">{row.quantityReceived}</TableCell>
                <TableCell align="right">{row.quantityInvoiced}</TableCell>
                <TableCell />
                <TableCell />
                <TableCell>{row.unauthorisedWarning}</TableCell>
            </TableRow>
            {row.deliveries.map(a => (
                <TableRow key={`${row.orderNumber}/${row.orderLine}/${a.deliverySequence}`}>
                    <TableCell />
                    <TableCell />
                    <TableCell />
                    <TableCell />
                    <TableCell />
                    <TableCell>{a.deliverySequence}</TableCell>
                    <TableCell align="right">{a.deliveryQuantity}</TableCell>
                    <TableCell align="right">{a.quantityReceived}</TableCell>
                    <TableCell />
                    <TableCell>{moment(a.requestedDeliveryDate).format('DD MMM YYYY')}</TableCell>
                    <TableCell>
                        {a.advisedDeliveryDate &&
                            moment(a.advisedDeliveryDate).format('DD MMM YYYY')}
                    </TableCell>
                    <TableCell>{a.reference}</TableCell>
                </TableRow>
            ))}
            <TableRow key={`${row.orderNumber}/${row.orderLine}/h2`}>
                <TableCell />
                <TableCell colSpan={2}>
                    <Button
                        color="navBut"
                        size="small"
                        onClick={() => {
                            window.open(
                                `${config.proxyRoot}${utilities.getHref(
                                    row,
                                    'acknowledge-deliveries'
                                )}`,
                                '_blank'
                            );
                        }}
                        startIcon={<ModeEditOutlineOutlinedIcon />}
                    >
                        Ack/Update Deliveries
                    </Button>
                </TableCell>
                <TableCell colSpan={4}>Contact: {row.supplierContact}</TableCell>
                <TableCell colSpan={5}>{row.remarks}</TableCell>
            </TableRow>
        </>
    );

    return (
        <div className="print-landscape" onKeyDown={onKeyPressed} tabIndex={-1} role="textbox">
            <Page history={history} width="xl">
                <ThemeProvider theme={theme}>
                    <div style={{ width: 1300, paddingLeft: '20px' }}>
                        {mrReportLoading && <Loading />}
                        {!selectedItem && (
                            <>
                                {!mrReportLoading && (
                                    <Typography variant="body2" style={{ fontWeight: 'bold' }}>
                                        No results found for selected options
                                    </Typography>
                                )}
                                <Tooltip title="Back To Options">
                                    <Button
                                        color="navBut"
                                        size="small"
                                        endIcon={<NotesIcon />}
                                        onClick={backToOptions}
                                    >
                                        Back To Options
                                    </Button>
                                </Tooltip>
                            </>
                        )}
                        {selectedItem && (
                            <Grid container spacing={1}>
                                <Grid
                                    item
                                    xs={6}
                                    style={{ paddingBottom: '10px' }}
                                    className="hide-when-printing"
                                >
                                    <Stack direction="row" spacing={2}>
                                        <Tooltip title="Previous part" placement="top-start">
                                            <div>
                                                <Button
                                                    style={{ float: 'left', marginRight: '60px' }}
                                                    color="navBut"
                                                    size="small"
                                                    onClick={goToPreviousPart}
                                                    startIcon={<ArrowBackIcon />}
                                                    disabled={!previousPart}
                                                >
                                                    {previousPart || 'At first'}
                                                </Button>
                                            </div>
                                        </Tooltip>
                                        <Tooltip title="Back To Options">
                                            <Button
                                                color="navBut"
                                                size="small"
                                                endIcon={<NotesIcon />}
                                                onClick={backToOptions}
                                            >
                                                Options
                                            </Button>
                                        </Tooltip>
                                    </Stack>
                                </Grid>
                                <Grid
                                    item
                                    xs={4}
                                    style={{ paddingBottom: '10px' }}
                                    className="hide-when-printing"
                                >
                                    <Stack direction="row" spacing={4}>
                                        <Tooltip title="Order (not yet implemented)">
                                            <div>
                                                <Button
                                                    color="navBut"
                                                    size="small"
                                                    endIcon={<ShopIcon />}
                                                    disabled
                                                >
                                                    Order
                                                </Button>
                                            </div>
                                        </Tooltip>
                                        <Tooltip title="Used On" className="hide-when-printing">
                                            <Button
                                                style={{ float: 'right' }}
                                                color="navBut"
                                                size="small"
                                                onClick={() => {
                                                    window.open(
                                                        `${config.proxyRoot}${utilities.getHref(
                                                            selectedItem,
                                                            'part-used-on'
                                                        )}`,
                                                        '_blank'
                                                    );
                                                }}
                                                endIcon={<ArrowUpwardIcon />}
                                            >
                                                Used On
                                            </Button>
                                        </Tooltip>
                                    </Stack>
                                </Grid>
                                <Grid
                                    item
                                    xs={2}
                                    style={{ paddingBottom: '10px' }}
                                    className="hide-when-printing"
                                >
                                    <Tooltip title="Next part" placement="top-end">
                                        <div>
                                            <Button
                                                style={{ float: 'right' }}
                                                color="navBut"
                                                size="small"
                                                disabled={!nextPart}
                                                onClick={goToNextPart}
                                                endIcon={<ArrowForwardIcon />}
                                            >
                                                {nextPart || 'At last'}
                                            </Button>
                                        </div>
                                    </Tooltip>
                                </Grid>
                                <Grid item xs={8}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2" style={{ fontWeight: 'bold' }}>
                                            {selectedItem.partNumber}
                                        </Typography>
                                        <Typography variant="body2" style={{ fontWeight: 'bold' }}>
                                            {selectedItem.partDescription}
                                        </Typography>
                                    </Stack>
                                </Grid>
                                <Grid item xs={2}>
                                    <Stack direction="row" spacing={2}>
                                        <Link
                                            href={utilities.getHref(selectedItem, 'part')}
                                            underline="hover"
                                            color="inherit"
                                            variant="body2"
                                        >
                                            View Part
                                        </Link>
                                    </Stack>
                                </Grid>
                                <Grid item xs={2}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2">
                                            Jobref: {selectedItem.jobRef}
                                        </Typography>
                                    </Stack>
                                </Grid>
                                <Grid item xs={8}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2">
                                            Supplier: {selectedItem.preferredSupplierId}{' '}
                                            {selectedItem.preferredSupplierName}
                                        </Typography>
                                        <Typography variant="body2">
                                            Currency: {selectedItem.currencyCode}
                                        </Typography>
                                        <Typography variant="body2">
                                            Vendor Manager: {selectedItem.vendorManagerInitials}
                                        </Typography>
                                        <Typography variant="body2">
                                            Order Units: {selectedItem.orderUnits}
                                        </Typography>
                                    </Stack>
                                </Grid>
                                <Grid item xs={4}>
                                    <Stack direction="row" spacing={2}>
                                        <Link
                                            href={utilities.getHref(selectedItem, 'part-supplier')}
                                            underline="hover"
                                            color="inherit"
                                            variant="body2"
                                        >
                                            View Part Supplier
                                        </Link>
                                    </Stack>
                                </Grid>
                                <Grid item xs={12}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2">Stock:</Typography>
                                        <Link
                                            href={utilities.getHref(selectedItem, 'view-stock')}
                                            underline="hover"
                                            color="inherit"
                                            variant="body2"
                                        >
                                            {selectedItem.quantityInStock}
                                        </Link>
                                        <Typography variant="body2">
                                            For Spares: {selectedItem.quantityForSpares}
                                        </Typography>
                                        <Typography variant="body2">
                                            Inspection: {selectedItem.quantityInInspection}
                                        </Typography>
                                        <Typography variant="body2">
                                            Faulty: {selectedItem.quantityFaulty}
                                        </Typography>
                                        <Typography variant="body2">
                                            Supplier: {selectedItem.quantityAtSupplier}
                                        </Typography>
                                    </Stack>
                                </Grid>
                                <Grid item xs={12}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2">
                                            GBP Price: {selectedItem.baseUnitPrice}
                                        </Typography>
                                        <Typography variant="body2">
                                            Currency Price: {selectedItem.currencyUnitPrice}
                                        </Typography>
                                    </Stack>
                                </Grid>
                                <Grid item xs={12}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2">
                                            Lead Time: {selectedItem.leadTimeWeeks}
                                        </Typography>
                                        <Typography variant="body2">
                                            MOQ: {selectedItem.minimumOrderQuantity}
                                        </Typography>
                                        <Typography variant="body2">
                                            MDQ: {selectedItem.minimumDeliveryQuantity}
                                        </Typography>
                                        <Typography variant="body2">
                                            Order Increment: {selectedItem.orderIncrement}
                                        </Typography>
                                        <Typography variant="body2">
                                            Our Units: {selectedItem.ourUnits}
                                        </Typography>
                                    </Stack>
                                </Grid>
                                <Grid item xs={3}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2">
                                            Annual Usage: {selectedItem.annualUsage}
                                        </Typography>
                                    </Stack>
                                </Grid>
                                <Grid item xs={9}>
                                    <Stack direction="row" spacing={2}>
                                        {selectedItem.mrComments && (
                                            <Typography variant="body2">
                                                Comments: {selectedItem.mrComments}
                                            </Typography>
                                        )}
                                    </Stack>
                                </Grid>
                                <Grid
                                    item
                                    xs={5}
                                    style={{ padding: '10px' }}
                                    className="hide-when-printing"
                                >
                                    <Tooltip title="Earlier Weeks" placement="top-start">
                                        <div>
                                            <Button
                                                style={{ float: 'left' }}
                                                color="navBut"
                                                size="small"
                                                onClick={previousSegment}
                                                startIcon={<ArrowBackIcon />}
                                                disabled={selectedSegment < 1}
                                            >
                                                Earlier Weeks
                                            </Button>
                                        </div>
                                    </Tooltip>
                                </Grid>
                                <Grid
                                    item
                                    xs={3}
                                    style={{ padding: '10px' }}
                                    className="hide-when-printing"
                                >
                                    <Tooltip
                                        title={
                                            selectedSegment === -1
                                                ? 'Show Quarter'
                                                : 'Show All Weeks'
                                        }
                                    >
                                        <Button
                                            color="navBut"
                                            size="small"
                                            onClick={toggleShowAllSegments}
                                            startIcon={
                                                selectedSegment === -1 ? (
                                                    <CloseFullscreenIcon />
                                                ) : (
                                                    <OpenInFullIcon />
                                                )
                                            }
                                        >
                                            {selectedSegment === -1
                                                ? 'Show Quarter'
                                                : 'Show All Weeks'}
                                        </Button>
                                    </Tooltip>
                                </Grid>
                                <Grid
                                    item
                                    xs={4}
                                    style={{ padding: '10px' }}
                                    className="hide-when-printing"
                                >
                                    <Tooltip title="Later Weeks" placement="top-end">
                                        <div>
                                            <Button
                                                style={{ float: 'right' }}
                                                color="navBut"
                                                size="small"
                                                disabled={
                                                    selectedSegment === 5 || selectedSegment === -1
                                                }
                                                onClick={nextSegment}
                                                endIcon={<ArrowForwardIcon />}
                                            >
                                                Later Weeks
                                            </Button>
                                        </div>
                                    </Tooltip>
                                </Grid>
                                <Grid item xs={12}>
                                    <div style={{ width: 1280 }}>
                                        <DataGrid
                                            rows={getRows(selectedItem, selectedSegment)}
                                            columns={detailsColumns}
                                            density="compact"
                                            rowHeight={34}
                                            headerHeight={1}
                                            autoHeight
                                            loading={mrReportLoading}
                                            hideFooter
                                            columnBuffer={15}
                                            getRowClassName={params => getRowClass(params)}
                                        />
                                    </div>
                                </Grid>
                                {selectedPurchaseOrders.length > 0 && (
                                    <Grid item xs={12}>
                                        <div style={{ paddingTop: '50px' }}>
                                            <Table sx={{ maxWidth: 1280 }} size="small">
                                                <TableHead>
                                                    <TableRow>
                                                        <TableCell>Order</TableCell>
                                                        <TableCell>Line</TableCell>
                                                        <TableCell>Date</TableCell>
                                                        <TableCell>Supplier</TableCell>
                                                        <TableCell>Supplier Name</TableCell>
                                                        <TableCell>Delivery</TableCell>
                                                        <TableCell align="right">
                                                            Qty Ordered
                                                        </TableCell>
                                                        <TableCell align="right">
                                                            Qty Received
                                                        </TableCell>
                                                        <TableCell align="right">
                                                            Qty Invoiced
                                                        </TableCell>
                                                        <TableCell>Requested</TableCell>
                                                        <TableCell>Advised</TableCell>
                                                        <TableCell>Ref</TableCell>
                                                    </TableRow>
                                                </TableHead>
                                                <TableBody>
                                                    {selectedPurchaseOrders.map(row =>
                                                        showRow(row)
                                                    )}
                                                </TableBody>
                                            </Table>
                                        </div>
                                    </Grid>
                                )}
                                {mrReportOrdersLoading && (
                                    <Grid item xs={12}>
                                        <Typography variant="body2" style={{ fontWeight: 'bold' }}>
                                            Just checking for orders...
                                        </Typography>
                                    </Grid>
                                )}
                            </Grid>
                        )}
                    </div>
                </ThemeProvider>
            </Page>
        </div>
    );
}

export default MaterialRequirementsReport;
