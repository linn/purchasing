import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Page, itemSelectorHelpers } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import { Button } from '@mui/material';
import Typography from '@mui/material/Typography';
import moment from 'moment';

import history from '../history';

import mrMasterActions from '../actions/mrMasterActions';
import { mrMaster as mrMasterItemType } from '../itemTypes';

function RunMrp() {
    const [runMrpAllowed, setRunMrpAllowed] = useState(false);
    const mrMaster = useSelector(state => itemSelectorHelpers.getItem(state.mrMaster));
    const mrMasterLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrMaster)
    );

    const dispatch = useDispatch();
    useEffect(() => dispatch(mrMasterActions.fetchByHref(mrMasterItemType.uri)), [dispatch]);

    useEffect(() => {
        if (mrMaster && !mrMasterLoading) {
            if (mrMaster.links?.find(a => a.rel === 'run-mrp')) {
                setRunMrpAllowed(true);
            } else {
                setRunMrpAllowed(false);
            }
        }
    }, [mrMaster, mrMasterLoading]);

    const runMrp = () => {
        dispatch(mrMasterActions.fetchByHref(mrMasterItemType.uri));
    };

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={12}>
                    <Typography variant="h6">Last MRP Run</Typography>
                </Grid>
                {mrMaster && !mrMasterLoading ? (
                    <>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">JobRef: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1">{mrMaster.jobRef}</Typography>
                        </Grid>
                        <Grid item xs={2}>
                            <Typography variant="subtitle2">Run Date: </Typography>
                        </Grid>
                        <Grid item xs={10}>
                            <Typography variant="body1">
                                {moment(mrMaster.runDate).format('DD MMM YYYY')}
                            </Typography>
                        </Grid>
                    </>
                ) : (
                    <></>
                )}
                <Grid item xs={12}>
                    <Button variant="outlined" onClick={() => runMrp()} disabled={!runMrpAllowed}>
                        Run New MRP
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default RunMrp;
