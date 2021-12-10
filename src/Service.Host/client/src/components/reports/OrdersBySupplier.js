import React, { useEffect } from 'react';
import {
    Loading,
    ReportTable,
    BackButton,
    ExportButton
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import PropTypes from 'prop-types';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import config from '../../config';
import { getReportLoading, getReportData } from '../../selectors/ReportSelectorHelpers';
import ordersBySupplierActions from '../../actions/ordersBySupplierActions';

function OrderBySupplierReport() {
    const options = queryString.parse(window.location.search) || {};

    const loading = useSelector(state => getReportLoading(state.ordersBySupplier));
    const reportData = useSelector(state => getReportData(state.ordersBySupplier));

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/reports/orders-by-supplier/report?
        id=${options.id}
        &fromDate=${encodeURIComponent(options.fromDate)}
        &toDate=${encodeURIComponent(options.toDate)}`;
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
                            href={`${config.appRoot}/purchasing/reports/orders-by-supplier/${options.id}/export?from=${options.fromDate}&to=${options.toDate}`}
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
                            // showRowTitles
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

OrderBySupplierReport.propTypes = {
    reportData: PropTypes.shape({ title: PropTypes.string }),
    history: PropTypes.shape({ push: PropTypes.func }).isRequired,
    loading: PropTypes.bool,
    options: PropTypes.shape({
        fromDate: PropTypes.instanceOf(Date),
        toDate: PropTypes.instanceOf(Date),
        id: PropTypes.number
    }),
    config: PropTypes.shape({ appRoot: PropTypes.string }).isRequired
};

OrderBySupplierReport.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default OrderBySupplierReport;
