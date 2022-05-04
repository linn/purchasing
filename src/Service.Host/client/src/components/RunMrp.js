import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    itemSelectorHelpers,
    processSelectorHelpers,
    Loading,
    utilities
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { Button } from '@mui/material';
import Typography from '@mui/material/Typography';
import LinearProgress from '@mui/material/LinearProgress';
import moment from 'moment';

import history from '../history';
import useInterval from '../helpers/useInterval';
import mrMasterActions from '../actions/mrMasterActions';
import mrpRunLogActions from '../actions/mrpRunLogActions';
import runMrpActions from '../actions/runMrpActions';
import { mrMaster as mrMasterItemType } from '../itemTypes';

function RunMrp() {
    const [runMrpAllowed, setRunMrpAllowed] = useState(false);
    const [currentRunLogId, setCurrentRunLogId] = useState(null);
    const [message, setMessage] = useState(null);
    const mrMaster = useSelector(state => itemSelectorHelpers.getItem(state.mrMaster));
    const mrMasterLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrMaster)
    );
    const mrpRunLog = useSelector(state => itemSelectorHelpers.getItem(state.mrpRunLog));
    const mrpRunLogLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrpRunLog)
    );
    const runMrpResult = useSelector(state => processSelectorHelpers.getData(state.runMrp));

    const mrpRunHref = utilities.getHref(mrMaster, 'run-mrp');
    const lastRunLogHref = utilities.getHref(mrMaster, 'last-run-log');

    const dispatch = useDispatch();
    useEffect(() => dispatch(mrMasterActions.fetchByHref(mrMasterItemType.uri)), [dispatch]);

    useEffect(() => {
        if (mrMaster && !mrMasterLoading) {
            if (mrpRunHref) {
                setRunMrpAllowed(true);
            } else {
                setRunMrpAllowed(false);
            }

            if (mrMaster.runLogIdCurrentlyInProgress) {
                setCurrentRunLogId(mrMaster.runLogIdCurrentlyInProgress);
                dispatch(mrpRunLogActions.fetch(mrMaster.runLogIdCurrentlyInProgress));
            } else {
                dispatch(mrpRunLogActions.fetchByHref(lastRunLogHref));
            }
        }
    }, [mrMaster, mrMasterLoading, dispatch, mrpRunHref, lastRunLogHref]);

    useEffect(() => {
        if (runMrpResult) {
            if (runMrpResult.success && runMrpResult.processHref) {
                setRunMrpAllowed(false);
                dispatch(mrMasterActions.fetchByHref(mrMasterItemType.uri));
                dispatch(mrpRunLogActions.fetchByHref(runMrpResult.processHref));
            } else {
                setMessage(runMrpResult.message);
            }
        }
    }, [runMrpResult, dispatch]);

    useEffect(() => {
        if (mrpRunLog) {
            setCurrentRunLogId(mrpRunLog.mrRunLogId);
        }
    }, [mrpRunLog]);

    useInterval(
        () => {
            dispatch(mrpRunLogActions.fetch(currentRunLogId));
            if (mrpRunLog.jobRef) {
                dispatch(mrMasterActions.fetchByHref(mrMasterItemType.uri));
            }
        },
        mrMaster?.runLogIdCurrentlyInProgress ? 20000 : null
    );

    const runMrp = () => {
        setRunMrpAllowed(false);
        dispatch(runMrpActions.requestProcessStart());
    };

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={12}>
                    <Typography variant="h6">Last MRP Run</Typography>
                </Grid>
                {mrMasterLoading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
                {mrMaster && !mrMasterLoading ? (
                    <>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">JobRef: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1">{mrMaster.jobRef}</Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">Run Completed At: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1" gutterBottom>
                                {moment(mrMaster.runDate).format('DD MMM YYYY HH:mm')}
                            </Typography>
                        </Grid>
                        {mrMaster?.runLogIdCurrentlyInProgress && (
                            <>
                                <Grid item xs={6}>
                                    <Typography variant="subtitle1">
                                        MRP Run in progress with runlog id {currentRunLogId}...
                                    </Typography>
                                    <LinearProgress
                                        style={{ marginTop: '20px', marginBottom: '20px' }}
                                    />
                                </Grid>
                                <Grid item xs={6} />
                            </>
                        )}
                        {message && (
                            <Grid item xs={12}>
                                <Typography variant="subtitle1">{message}</Typography>
                            </Grid>
                        )}
                    </>
                ) : (
                    <></>
                )}
                <Grid item xs={12} style={{ paddingBottom: '20px' }}>
                    <Button variant="outlined" onClick={() => runMrp()} disabled={!runMrpAllowed}>
                        Run New MRP
                    </Button>
                </Grid>
                {mrpRunLogLoading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
                {mrpRunLog && !mrpRunLogLoading ? (
                    <>
                        <Grid item xs={12}>
                            <Typography variant="h6">
                                {mrMaster?.runLogIdCurrentlyInProgress ? 'Current ' : 'Latest '}
                                MRP Run Log
                            </Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">Run Log Id: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1">{mrpRunLog.mrRunLogId}</Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">Run Started: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1">
                                {moment(mrpRunLog.runDate).format('DD MMM YYYY HH:mm')}
                            </Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">Selected Options: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1" style={{ whiteSpace: 'pre-line' }}>
                                {mrpRunLog.runDetails}
                            </Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">
                                {mrMaster?.runLogIdCurrentlyInProgress ? 'Current ' : ''} Status:
                            </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography
                                variant="body1"
                                style={{ whiteSpace: 'pre-line' }}
                                gutterBottom
                            >
                                {mrpRunLog.loadMessage}
                            </Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">JobRef: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1">{mrpRunLog.jobRef}</Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">Finished Message: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1" style={{ whiteSpace: 'pre-line' }}>
                                {mrpRunLog.mrMessage}
                            </Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">Success: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1">{mrpRunLog.success}</Typography>
                        </Grid>
                    </>
                ) : (
                    <></>
                )}
            </Grid>
        </Page>
    );
}

export default RunMrp;
