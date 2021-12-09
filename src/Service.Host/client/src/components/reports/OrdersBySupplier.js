import React from 'react';
import {
    Loading,
    ReportTable,
    BackButton,
    ExportButton
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import PropTypes from 'prop-types';

const handleBackClick = (history, options) => {
    const uri = `/purchasing/reports/orders-by-supplier?
    id=${options.id}
    from=${encodeURIComponent(options.fromDate)}
    &to=${encodeURIComponent(options.toDate)}`;

    history.push(uri);
};

const ImpbookEuReport = ({ reportData, loading, history, options, config }) => (
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
                        showRowTitles
                    />
                )}
            </Grid>
            <Grid item xs={12}>
                <BackButton backClick={() => handleBackClick(history, options)} />
            </Grid>
        </Grid>
    </>
);

ImpbookEuReport.propTypes = {
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

ImpbookEuReport.defaultProps = {
    reportData: {},
    options: {},
    loading: false
};

export default ImpbookEuReport;
