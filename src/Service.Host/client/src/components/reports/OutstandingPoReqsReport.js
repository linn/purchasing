import React, { useEffect, useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    collectionSelectorHelpers,
    Loading,
    Dropdown,
    ReportTable
} from '@linn-it/linn-form-components-library';
import { useLocation } from 'react-router';
import { useSelector, useDispatch } from 'react-redux';
import queryString from 'query-string';
import history from '../../history';
import config from '../../config';
import { purchaseOrderReqStates } from '../../itemTypes';
import { outstandingPoReqsReport } from '../../reportTypes';
import purchaseOrderReqStatesActions from '../../actions/purchaseOrderReqStatesActions';
import outstandingPoReqsReportActions from '../../actions/outstandingPoReqsReportActions';

function OutstandingPoReqsReport() {
    const dispatch = useDispatch();
    const purchaseOrderReqStatesOptions = useSelector(state =>
        collectionSelectorHelpers.getItems(state[purchaseOrderReqStates.item])
    );
    const purchaseOrderReqStatesLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state[purchaseOrderReqStates.item])
    );
    useEffect(() => {
        dispatch(purchaseOrderReqStatesActions.fetch());
    }, [dispatch]);

    const { search } = useLocation();

    const urlStateParam = search.length ? queryString.parse(search).state : null;
    const [reqState, setReqState] = useState(urlStateParam);

    useEffect(() => {
        if (urlStateParam) {
            dispatch(
                outstandingPoReqsReportActions.fetchReport({
                    state: urlStateParam
                })
            );
        }
    }, [dispatch, urlStateParam]);

    const loading = useSelector(reduxState => reduxState[outstandingPoReqsReport.item]?.loading);

    const reportData = useSelector(reduxState => reduxState[outstandingPoReqsReport.item]?.data);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Outstanding PO Reqs Report" />
                </Grid>
                {purchaseOrderReqStatesLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={12}>
                            <Dropdown
                                label="State  (leave blank for all Outstanding)"
                                propertyName="purchaseOrderReqState"
                                value={reqState}
                                onChange={(_, newValue) => setReqState(newValue)}
                                items={purchaseOrderReqStatesOptions.map(v => ({
                                    id: v.state,
                                    displayText: `${v.state} - ${v.description}`
                                }))}
                                allowNoValue
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={() =>
                                    dispatch(
                                        outstandingPoReqsReportActions.fetchReport({
                                            state: reqState
                                        })
                                    )
                                }
                            >
                                Run Report
                            </Button>
                        </Grid>
                    </>
                )}
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        {reportData && (
                            <Grid item xs={12}>
                                <ReportTable
                                    reportData={reportData}
                                    title={reportData.title}
                                    showTitle
                                    showRowTitles
                                    showTotals
                                    placeholderRows={4}
                                    placeholderColumns={4}
                                />
                            </Grid>
                        )}
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default OutstandingPoReqsReport;
