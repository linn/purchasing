import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import { Page, Loading, itemSelectorHelpers } from '@linn-it/linn-form-components-library';
import Box from '@mui/material/Box';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import changeRequestActions from '../../actions/changeRequestActions';
import changeRequestStatusChangeActions from '../../actions/changeRequestStatusChangeActions';
import MainTab from './Tabs/MainTab';
import BomChangesTab from './Tabs/BomChangesTab';
import PcasChangesTab from './Tabs/PcasChangesTab';

import history from '../../history';

function ChangeRequest() {
    const { id } = useParams();

    const reduxDispatch = useDispatch();
    useEffect(() => {
        if (id) {
            reduxDispatch(changeRequestActions.fetch(id));
        }
    }, [id, reduxDispatch]);

    const loading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.changeRequest)
    );

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.changeRequest));

    const statusChange = useSelector(reduxState =>
        itemSelectorHelpers.getItem(reduxState.changeRequestStatusChange)
    );

    useEffect(() => {
        if (item && statusChange && statusChange?.changeState !== item?.changeState) {
            reduxDispatch(changeRequestActions.fetch(id));
        }
    }, [statusChange, reduxDispatch, item, id]);

    const [tab, setTab] = useState(0);

    const approve = request => {
        if (request?.changeState === 'PROPOS') {
            reduxDispatch(changeRequestStatusChangeActions.add({ id, status: 'ACCEPT' }));
        }
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
                                    <Tab label="Pcas Changes" />
                                    <Tab label="BOM Changes" />
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
                                <PcasChangesTab pcasChanges={item?.pcasChanges} />
                            </Box>
                        )}
                        {tab === 2 && (
                            <Box sx={{ paddingTop: 3 }}>
                                <BomChangesTab bomChanges={item?.bomChanges} />
                            </Box>
                        )}
                    </Grid>
                </Grid>
            )}
        </Page>
    );
}

export default ChangeRequest;
