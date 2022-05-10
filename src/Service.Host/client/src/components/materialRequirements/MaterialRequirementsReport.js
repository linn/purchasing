import React, { useCallback, useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Page, itemSelectorHelpers, Loading } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Snackbar from '@mui/material/Snackbar';
import Typography from '@mui/material/Typography';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import Stack from '@mui/material/Stack';

import { useLocation } from 'react-router';
import queryString from 'query-string';

import { mrReport as mrReportItem } from '../../itemTypes';
import mrReportActions from '../../actions/mrReportActions';

import history from '../../history';

function MaterialRequirementsReport() {
    const [showMessage, setShowMessage] = useState(false);
    const [message, setMessage] = useState(null);
    const [selectedItem, setSelectedItem] = useState(null);
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
        }
    });

    const displayMessage = useCallback(
        newMessage => {
            setMessage(newMessage);
            setShowMessage(true);
        },
        [setShowMessage]
    );

    const handleClose = () => {
        setShowMessage(false);
        setMessage(null);
    };

    const dispatch = useDispatch();

    useEffect(() => {
        if (options && options.state) {
            dispatch(mrReportActions.postByHref(mrReportItem.uri, options.state));
        } else if (options && options.search) {
            const query = queryString.parse(options.search);
            dispatch(
                mrReportActions.postByHref(mrReportItem.uri, {
                    partNumber: query?.partNumber,
                    jobRef: query?.jobRef
                })
            );
        } else {
            history.push('/purchasing/material-requirements');
        }
    }, [dispatch, options]);

    useEffect(() => {
        if (mrReport && mrReport.results && mrReport.results.length > 0) {
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

    return (
        <Page history={history}>
            <ThemeProvider theme={theme}>
                <>
                    {mrReportLoading && <Loading />}
                    {selectedItem && (
                        <Grid container spacing={1}>
                            <Grid item xs={10}>
                                <Typography variant="body2" style={{ fontWeight: 'bold' }}>
                                    {selectedItem.partNumber}: {selectedItem.partDescription}
                                </Typography>
                            </Grid>
                            <Grid item xs={2}>
                                <Typography variant="body2">
                                    Jobref: {selectedItem.jobRef}
                                </Typography>
                            </Grid>
                            <Grid item xs={12}>
                                <Stack direction="row" spacing={2}>
                                    <Typography variant="body2">
                                        Supplier: {selectedItem.preferredSupplierId} -{' '}
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
                        </Grid>
                    )}
                </>
                <Snackbar
                    open={showMessage}
                    autoHideDuration={3000}
                    onClose={handleClose}
                    message={message}
                />
            </ThemeProvider>
        </Page>
    );
}

export default MaterialRequirementsReport;
