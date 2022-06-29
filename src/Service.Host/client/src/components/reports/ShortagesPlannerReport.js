import React, { useEffect, useMemo } from 'react';
import { Loading, MultiReportTable, BackButton } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import shortagesPlannerReportActions from '../../actions/shortagesPlannerReportActions';
import { shortagesPlannerReport } from '../../reportTypes';

function ShortagesPlannerReport() {
    const options = useMemo(() => queryString.parse(window.location.search) || {}, []);

    const loading = useSelector(state => state[shortagesPlannerReport.item]?.results.loading);

    const reportData = useSelector(state => state[shortagesPlannerReport.item]?.results.data);
    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/reports/shortages/`;
        history.push(uri);
    };

    useEffect(() => {
        if (options) {
            dispatch(shortagesPlannerReportActions.fetchReport(options));
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
                                showRowTitles={false}
                                showTotals={false}
                                placeholderRows={10}
                                placeholderColumns={4}
                            />
                        )
                    )}
                </Grid>
            </Grid>
        </>
    );
}

ShortagesPlannerReport.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default ShortagesPlannerReport;
