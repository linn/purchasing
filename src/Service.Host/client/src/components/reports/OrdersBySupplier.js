import React, { useEffect, useMemo } from 'react';
import {
    Loading,
    ReportTable,
    BackButton,
    ExportButton,
    reportSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import config from '../../config';
import ordersBySupplierActions from '../../actions/ordersBySupplierActions';

function OrderBySupplierReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.ordersBySupplier)
    );
    const reportData = useSelector(state =>
        reportSelectorHelpers.getReportData(state.ordersBySupplier)
    );

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri =
            `/purchasing/reports/orders-by-supplier/` +
            `?id=${options.id}` +
            `&fromDate=${options.fromDate}` +
            `&toDate=${options.toDate}` +
            `&outstanding=${options.outstanding}` +
            `&returns=${options.returns}` +
            `&stockControlled=${options.stockControlled}` +
            `&credits=${options.credits}` +
            `&cancelled=${options.cancelled}`;
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(ordersBySupplierActions.fetchReport(options));
        }
    }, [options, dispatch]);

    return (
        <>
            <Grid style={{ marginTop: 40 }} container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
                <Grid item xs={12}>
                    {!loading && reportData ? (
                        <ExportButton
                            href={
                                `${config.appRoot}/purchasing/reports/orders-by-supplier/export?` +
                                `?id=${options.id}` +
                                `&fromDate=${options.fromDate}` +
                                `&toDate=${options.toDate}` +
                                `&outstanding=${options.outstandingOnly}` +
                                `&returns=${options.returns}` +
                                `&stockControlled=${options.stockControlled}` +
                                `&credits=${options.credits}` +
                                `&cancelled=${options.cancelled}`
                            }
                        />
                    ) : (
                        ''
                    )}
                </Grid>
                <Grid item xs={12}>
                    {loading || !reportData ? (
                        <Loading />
                    ) : (
                        <ReportTable
                            reportData={reportData}
                            title={reportData.title}
                            showTitle
                            showTotals={false}
                            placeholderRows={4}
                            placeholderColumns={4}
                        />
                    )}
                </Grid>
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
            </Grid>
        </>
    );
}

OrderBySupplierReport.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default OrderBySupplierReport;
