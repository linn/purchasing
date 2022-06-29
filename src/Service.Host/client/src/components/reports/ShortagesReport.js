import React, { useEffect, useMemo } from 'react';
import { Loading, MultiReportTable, BackButton } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import { shortagesReport } from '../../reportTypes';
import shortagesReportActions from '../../actions/shortagesReportActions';

function ShortagesReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state => state[shortagesReport.item]?.loading);

    const reportData = useSelector(state => state[shortagesReport.item]?.data);

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/reports/shortages/`;
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(shortagesReportActions.fetchReport(options));
        }
    }, [options, dispatch]);

    return (
        <>
            <Grid style={{ marginTop: 40 }} container spacing={3} justifyContent="center">
                <Grid item xs={12}>
                    <BackButton backClick={() => handleBackClick(history, options)} />
                </Grid>
                <Grid item xs={12}>
                    {loading || !reportData ? (
                        <Loading />
                    ) : (
                        reportData && (
                            <MultiReportTable
                                reportData={reportData}
                                showTitle
                                showTotals
                                placeholderRows={4}
                                placeholderColumns={4}
                            />
                        )
                    )}
                </Grid>
            </Grid>
        </>
    );
}

ShortagesReport.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default ShortagesReport;
