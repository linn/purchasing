import React, { useEffect, useMemo } from 'react';
import {
    Loading,
    ReportTable,
    reportSelectorHelpers,
    BackButton
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import spendByPartByDateActions from '../../actions/spendByPartByDateActions';
import history from '../../history';

function SpendByPartByDate() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.spendByPartByDateReport)
    );
    const reportData = useSelector(state =>
        reportSelectorHelpers.getReportData(state.spendByPartByDateReport)
    );

    const handleBackClick = () => {
        const uri = `/purchasing/reports/spend-by-supplier-by-date-range/report?${queryString.stringify(
            options
        )}`;
        history.push(uri);
    };

    const dispatch = useDispatch();

    useEffect(() => {
        if (options) {
            dispatch(spendByPartByDateActions.fetchReport(options));
        }
    }, [options, dispatch]);

    return (
        <>
            <Grid style={{ marginTop: 40 }} container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick()} />
                </Grid>
                <Grid item xs={12}>
                    {loading || !reportData ? (
                        <Loading />
                    ) : (
                        <>
                            <ReportTable
                                reportData={reportData}
                                title={reportData.title}
                                showTitle
                                showTotals
                                placeholderRows={4}
                                placeholderColumns={4}
                                showRowCount
                            />
                        </>
                    )}
                </Grid>
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick()} />
                </Grid>
            </Grid>
        </>
    );
}

SpendByPartByDate.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default SpendByPartByDate;
