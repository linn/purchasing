import React, { useEffect } from 'react';
import {
    Loading,
    BackButton,
    reportSelectorHelpers,
    ReportTable
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';
import history from '../../history';
import outstandingChangesReportActions from '../../actions/outstandingChangesReportActions';

function OutstandingChangesReport() {
    const { months } = useParams();
    const loading = useSelector(state =>
        reportSelectorHelpers.getReportLoading(state.outstandingChangesReport)
    );
    const reportData = useSelector(state =>
        reportSelectorHelpers.getReportData(state.outstandingChangesReport)
    );

    const dispatch = useDispatch();

    const handleBackClick = () => {
        const uri = `/purchasing/reports/change-status/`;
        history.push(uri);
    };

    useEffect(() => {
        if (months) {
            dispatch(outstandingChangesReportActions.fetchReport(months));
        }
    }, [months, dispatch]);

    return (
        <Grid style={{ marginTop: 40 }} container spacing={3} justifyContent="center">
            <Grid item xs={12}>
                <BackButton backClick={() => handleBackClick()} />
            </Grid>
            <Grid item xs={6}>
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
            <Grid item xs={6} />

            <Grid item xs={12}>
                <BackButton backClick={() => handleBackClick()} />
            </Grid>
        </Grid>
    );
}
export default OutstandingChangesReport;
