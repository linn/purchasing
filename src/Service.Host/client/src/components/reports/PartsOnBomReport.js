import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    Loading,
    ReportTable,
    ExportButton,
    InputField
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import { partsOnBomReport } from '../../reportTypes';
import partsOnBomReportActions from '../../actions/partsOnBomReportActions';

function PartsOnBomReport() {
    const dispatch = useDispatch();
    const [bomName, setBomName] = useState();

    const loading = useSelector(state => state[partsOnBomReport.item]?.loading);

    const reportData = useSelector(state => state[partsOnBomReport.item]?.data);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Bom Print" />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={bomName}
                        propertyName="bomName"
                        label="Bom Name"
                        onChange={(_, newVal) => setBomName(newVal)}
                    />
                </Grid>
                <Grid item xs={3}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={() =>
                            dispatch(
                                partsOnBomReportActions.fetchReport({
                                    bomName
                                })
                            )
                        }
                    >
                        Run
                    </Button>
                </Grid>

                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        {reportData && (
                            <>
                                <Grid item xs={12}>
                                    <ExportButton
                                        href={`${config.appRoot}/purchasing/boms/reports/list/export?bomName=${bomName}`}
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <ReportTable
                                        reportData={reportData}
                                        title={reportData.title}
                                        showTitle
                                        placeholderRows={4}
                                        placeholderColumns={4}
                                    />
                                </Grid>
                            </>
                        )}
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default PartsOnBomReport;
