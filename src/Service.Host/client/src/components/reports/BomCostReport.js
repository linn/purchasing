import React, { Fragment, useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { Page, Title, Loading, ReportTable } from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation } from 'react-router-dom';
import queryString from 'query-string';
import Typography from '@mui/material/Typography';
import history from '../../history';
import config from '../../config';
import { bomCostReport } from '../../reportTypes';
import bomCostReportActions from '../../actions/bomCostReportActions';

function BomCostReport() {
    const dispatch = useDispatch();

    const loading = useSelector(state => state[bomCostReport.item]?.loading);
    const { search } = useLocation();
    const { bomName } = queryString.parse(search);

    const [prevBomName, setPrevBomName] = useState();
    if (bomName && bomName !== prevBomName) {
        setPrevBomName(bomName);
        dispatch(
            bomCostReportActions.fetchReport({
                bomName,
                splitBySubAssembly: true,
                levels: 999,
                labourHourlyRate: 15
            })
        );
    }

    const reportData = useSelector(state => state[bomCostReport.item].data);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Bom Cost Report" />
                </Grid>

                <Grid item xs={3}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={() => history.push('/purchasing/boms/reports/cost/options')}
                    >
                        Back
                    </Button>
                </Grid>
                <Grid item xs={9} />
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        {reportData &&
                            reportData.map(d => (
                                <Fragment key={d.subAssembly}>
                                    <Grid item xs={12}>
                                        <Typography variant="h6">{d.subAssembly}</Typography>
                                    </Grid>
                                    <Grid item xs={12}>
                                        <ReportTable
                                            reportData={d.breakdown.reportResults[0]}
                                            title={d.subAssembly}
                                            showTitle={false}
                                            placeholderRows={4}
                                            placeholderColumns={4}
                                        />
                                    </Grid>
                                    <Grid item xs={6} />
                                    <Grid item xs={3}>
                                        <Typography variant="h6">
                                            Std Total: {d.standardTotal}
                                        </Typography>
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Typography variant="h6">
                                            Mat Total: {d.materialTotal}
                                        </Typography>
                                    </Grid>
                                </Fragment>
                            ))}
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default BomCostReport;
