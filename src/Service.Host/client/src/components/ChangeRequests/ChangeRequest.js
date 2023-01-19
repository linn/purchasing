import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import {
    Page,
    Loading,
    itemSelectorHelpers,
    utilities
} from '@linn-it/linn-form-components-library';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import changeRequestActions from '../../actions/changeRequestActions';
import changeRequestStatusChangeActions from '../../actions/changeRequestStatusChangeActions';
import changeRequestPhaseInsActions from '../../actions/changeRequestPhaseInsActions';
import MainTab from './Tabs/MainTab';
import BomChangesTab from './Tabs/BomChangesTab';
import PcasChangesTab from './Tabs/PcasChangesTab';
import history from '../../history';

function ChangeRequest() {
    const { id } = useParams();

    const reduxDispatch = useDispatch();

    const loading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.changeRequest)
    );

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.changeRequest));

    useEffect(() => {
        if (id && !item) {
            reduxDispatch(changeRequestActions.fetch(id));
        }
    }, [id, reduxDispatch, item]);

    const statusChange = useSelector(reduxState =>
        itemSelectorHelpers.getItem(reduxState.changeRequestStatusChange)
    );

    const changedState = (changereq, origreq) => {
        if (changereq?.changeState !== origreq?.changeState) {
            return true;
        }

        if (
            changereq.bomChanges.filter(c => c.changeState === 'CANCEL').length >
            origreq.bomChanges.filter(c => c.changeState === 'CANCEL').length
        ) {
            return true;
        }

        if (
            changereq.pcasChanges.filter(c => c.changeState === 'CANCEL').length >
            origreq.pcasChanges.filter(c => c.changeState === 'CANCEL').length
        ) {
            return true;
        }

        if (
            changereq.bomChanges.filter(c => c.changeState === 'LIVE').length >
            origreq.bomChanges.filter(c => c.changeState === 'LIVE').length
        ) {
            return true;
        }

        if (
            changereq.pcasChanges.filter(c => c.changeState === 'LIVE').length >
            origreq.pcasChanges.filter(c => c.changeState === 'LIVE').length
        ) {
            return true;
        }

        return false;
    };

    useEffect(() => {
        if (item && statusChange && changedState(statusChange, item)) {
            reduxDispatch(changeRequestActions.fetch(id));
        }
    }, [statusChange, reduxDispatch, item, id]);

    const [tab, setTab] = useState(0);

    const [selectedBomChanges, setSelectedBomChanges] = useState(null);
    const [selectedPcasChanges, setSelectedPcasChanges] = useState(null);

    const handleBomChangesSelectRow = selected => {
        setSelectedBomChanges(selected);
    };

    const handlePcasChangesSelectRow = selected => {
        setSelectedPcasChanges(selected);
    };

    const approve = request => {
        if (request?.changeState === 'PROPOS') {
            reduxDispatch(changeRequestStatusChangeActions.add({ id, status: 'ACCEPT' }));
        }
    };

    const cancelUri = utilities.getHref(item, 'cancel');
    const phaseInUri = utilities.getHref(item, 'phase-in');
    const makeLiveUri = utilities.getHref(item, 'make-live');

    const cancel = request => {
        if (request?.changeState === 'PROPOS' || request?.changeState === 'ACCEPT') {
            reduxDispatch(
                changeRequestStatusChangeActions.add({
                    id,
                    status: 'CANCEL',
                    selectedBomChangeIds: selectedBomChanges,
                    selectedPcasChangeIds: selectedPcasChanges
                })
            );
        }
    };

    const makeLive = request => {
        if (request?.changeState === 'ACCEPT') {
            reduxDispatch(
                changeRequestStatusChangeActions.add({
                    id,
                    status: 'LIVE',
                    selectedBomChangeIds: selectedBomChanges,
                    selectedPcasChangeIds: selectedPcasChanges
                })
            );
        }
    };

    const phaseIn = date => {
        reduxDispatch(
            changeRequestPhaseInsActions.add({
                documentNumber: id,
                phaseInWeekStart: date,
                selectedBomChangeIds: selectedBomChanges
            })
        );
    };

    return (
        <Page history={history}>
            {loading ? (
                <Loading />
            ) : (
                <Grid container spacing={2} justifyContent="center">
                    <Grid item xs={12}>
                        <Typography variant="h6">Change Request {id}</Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <Box sx={{ width: '100%' }}>
                            <Box sx={{ borderBottom: 0, borderColor: 'divider' }}>
                                <Tabs
                                    value={tab}
                                    onChange={(event, newValue) => {
                                        setTab(newValue);
                                    }}
                                >
                                    <Tab label="Main" />
                                    <Tab
                                        label={`Pcas Changes${
                                            item?.pcasChanges?.length
                                                ? ` (${item?.pcasChanges?.length})`
                                                : ''
                                        }`}
                                        disabled={!item?.pcasChanges?.length}
                                    />
                                    <Tab
                                        label={`Bom Changes${
                                            item?.bomChanges?.length
                                                ? ` (${item?.bomChanges?.length})`
                                                : ''
                                        }`}
                                        disabled={!item?.bomChanges?.length}
                                    />
                                </Tabs>
                            </Box>
                        </Box>
                        {tab === 0 && (
                            <Box sx={{ paddingTop: 3 }}>
                                <MainTab item={item} approve={approve} />
                            </Box>
                        )}
                        {tab === 1 && (
                            <Box sx={{ paddingTop: 3 }}>
                                <PcasChangesTab
                                    pcasChanges={item?.pcasChanges}
                                    handleSelectChange={handlePcasChangesSelectRow}
                                />
                            </Box>
                        )}
                        {tab === 2 && (
                            <Box sx={{ paddingTop: 3 }}>
                                <BomChangesTab
                                    bomChanges={item?.bomChanges}
                                    handleSelectChange={handleBomChangesSelectRow}
                                    phaseInsUri={phaseInUri}
                                    phaseIn={phaseIn}
                                />
                            </Box>
                        )}
                    </Grid>
                    <Grid item xs={12}>
                        <Button
                            variant="outlined"
                            disabled={!makeLiveUri}
                            onClick={() => makeLive(item)}
                            style={{ marginRight: '30px' }}
                        >
                            Make Live
                        </Button>
                        <Button
                            variant="outlined"
                            disabled={!cancelUri}
                            onClick={() => cancel(item)}
                        >
                            Cancel
                        </Button>
                    </Grid>
                </Grid>
            )}
        </Page>
    );
}

export default ChangeRequest;
