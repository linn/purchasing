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
import Link from '@mui/material/Link';
import { DataGrid } from '@mui/x-data-grid';
import makeStyles from '@mui/styles/makeStyles';

import { useLocation } from 'react-router';
import queryString from 'query-string';

import { mrReport as mrReportItem } from '../../itemTypes';
import mrReportActions from '../../actions/mrReportActions';

import history from '../../history';
import config from '../../config';

function MaterialRequirementsReport() {
    const [selectedItem, setSelectedItem] = useState(null);
    const [selectedIndex, setSelectedIndex] = useState(null);
    const [selectedSegment, setSelectedSegment] = useState(0);
    const [nextPart, setNextPart] = useState(null);
    const [previousPart, setPreviousPart] = useState(null);

    const options = useLocation();

    const mrReport = useSelector(state => itemSelectorHelpers.getItem(state.mrReport));
    const mrReportLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrReport)
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
            dispatch(
                mrReportActions.postByHref(mrReportItem.uri, {
                    partNumber: query?.partNumber,
                    jobRef: query?.jobRef,
                    typeOfReport: 'MR',
                    partSelector: 'Select Parts'
                })
            );
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
        } else {
            setSelectedItem(null);
            setNextPart(null);
            setPreviousPart(null);
        }
    }, [mrReport]);

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
            setNextPart(mrReport.results[selectedIndex + 1].partNumber);
        }

        setSelectedItem(mrReport.results[selectedIndex + 1]);
        setSelectedIndex(selectedIndex + 1);
    };

    const getCellClass = params => {
        switch (params.field) {
            case 'week1':
                if (params.row.week1Item.tag) {
                    return classes[params.row.week1Item.tag];
                }

                return null;
            default:
                return null;
        }
    };

    const detailsColumns = [
        { field: 'title', headerName: '', width: 120 },
        { field: 'immediate', headerName: '', width: 80 },
        { field: 'week0', headerName: '', width: 80 },
        {
            field: 'week1',
            headerName: '',
            width: 80,
            cellClassName: params => getCellClass(params)
        },
        { field: 'week2', headerName: '', width: 80 },
        { field: 'week3', headerName: '', width: 80 },
        { field: 'week4', headerName: '', width: 80 },
        { field: 'week5', headerName: '', width: 80 },
        { field: 'week6', headerName: '', width: 80 },
        { field: 'week7', headerName: '', width: 80 },
        { field: 'week8', headerName: '', width: 80 },
        { field: 'week9', headerName: '', width: 80 },
        { field: 'week10', headerName: '', width: 80 },
        { field: 'week11', headerName: '', width: 80 },
        { field: 'week12', headerName: '', width: 80 }
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
            immediate: b.immediateItem.textValue || b.immediateItem.value,
            week0: b.week0Item.textValue || b.week0Item.value,
            week1: b.week1Item.textValue || b.week1Item.value,
            week2: b.week2Item.textValue || b.week2Item.value,
            week3: b.week3Item.textValue || b.week3Item.value,
            week4: b.week4Item.textValue || b.week4Item.value,
            week5: b.week5Item.textValue || b.week5Item.value,
            week6: b.week6Item.textValue || b.week6Item.value,
            week7: b.week7Item.textValue || b.week7Item.value,
            week8: b.week8Item.textValue || b.week8Item.value,
            week9: b.week9Item.textValue || b.week9Item.value,
            week10: b.week10Item.textValue || b.week10Item.value,
            week11: b.week11Item.textValue || b.week11Item.value,
            week12: b.week12Item.textValue || b.week12Item.value
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

    return (
        <div className="print-landscape" onKeyDown={onKeyPressed} tabIndex={-1} role="textbox">
            <Page history={history} width="xl">
                <ThemeProvider theme={theme}>
                    <div style={{ width: 1300, paddingLeft: '20px' }}>
                        {mrReportLoading && <Loading />}
                        {!selectedItem && (
                            <>
                                <Typography variant="body2" style={{ fontWeight: 'bold' }}>
                                    No results found for selected options
                                </Typography>
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
                                                        `${config.proxyRoot}${
                                                            selectedItem.links.find(
                                                                l => l.rel === 'part-used-on'
                                                            )?.href
                                                        }`,
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
                                <Grid item xs={4}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2">
                                            Jobref: {selectedItem.jobRef}
                                        </Typography>
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
                                <Grid item xs={12}>
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
                                <Grid item xs={12}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2">
                                            Stock: {selectedItem.quantityInStock}
                                        </Typography>
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
                                <Grid item xs={12}>
                                    <Stack direction="row" spacing={2}>
                                        <Typography variant="body2">
                                            Annual Usage: {selectedItem.annualUsage}
                                        </Typography>
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
                            </Grid>
                        )}
                    </div>
                </ThemeProvider>
            </Page>
        </div>
    );
}

export default MaterialRequirementsReport;
