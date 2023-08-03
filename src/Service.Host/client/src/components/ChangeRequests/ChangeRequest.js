import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import {
    Page,
    Loading,
    itemSelectorHelpers,
    utilities,
    getItemError,
    ErrorCard
} from '@linn-it/linn-form-components-library';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Tabs from '@mui/material/Tabs';
import { useLocation } from 'react-router';
import { CSVLink } from 'react-csv';
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
import { changeRequest, changeRequestStatusChange } from '../../itemTypes';
import usePreviousNextNavigation from '../../hooks/usePreviousNextNavigation';
import PrevNextButtons from '../PrevNextButtons';

function ChangeRequest() {
    const { id } = useParams();
    const reduxDispatch = useDispatch();

    const item = useSelector(state => state[changeRequest.item].item);
    const loading = useSelector(state => state[changeRequest.item].loading);
    const itemError = useSelector(reduxState =>
        getItemError(reduxState, changeRequestStatusChange.item)
    );

    useEffect(() => {
        if (!item || (item?.documentNumber && id.toString() !== item.documentNumber.toString())) {
            reduxDispatch(changeRequestActions.fetch(id));
        }
    }, [id, reduxDispatch, item]);

    const statusChangeLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState[changeRequestStatusChange.item])
    );

    const statusChange = useSelector(reduxState =>
        itemSelectorHelpers.getItem(reduxState.changeRequestStatusChange)
    );

    const phaseInChange = useSelector(reduxState =>
        itemSelectorHelpers.getItem(reduxState.changeRequestPhaseIns)
    );

    const changedState = (changereq, origreq) => {
        if (changereq?.documentNumber !== origreq?.documentNumber) {
            return false;
        }
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

    if (item && statusChange && changedState(statusChange, item)) {
        reduxDispatch(changeRequestActions.fetch(id));
    }

    if (item && phaseInChange) {
        reduxDispatch(changeRequestPhaseInsActions.clearItem());
        reduxDispatch(changeRequestActions.fetch(id));
    }

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
        reduxDispatch(changeRequestStatusChangeActions.clearItem());
        if (request?.changeState === 'PROPOS') {
            reduxDispatch(changeRequestStatusChangeActions.add({ id, status: 'ACCEPT' }));
        }
    };

    const cancelUri = utilities.getHref(item, 'cancel');
    const phaseInUri = utilities.getHref(item, 'phase-in');
    const makeLiveUri = utilities.getHref(item, 'make-live');
    const undoUri = utilities.getHref(item, 'undo');

    const cancel = request => {
        reduxDispatch(changeRequestStatusChangeActions.clearErrorsForItem());
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
        reduxDispatch(changeRequestStatusChangeActions.clearErrorsForItem());
        reduxDispatch(changeRequestStatusChangeActions.clearItem());
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

    const undo = request => {
        reduxDispatch(changeRequestStatusChangeActions.clearErrorsForItem());

        if (request?.changeState === 'ACCEPT' || request?.changeState === 'LIVE') {
            reduxDispatch(
                changeRequestStatusChangeActions.add({
                    id,
                    status: 'UNDO',
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

    const formatDateTime = isoString => {
        if (!isoString) {
            return '';
        }

        const dateTime = new Date(isoString);
        const formattedDate = dateTime
            .toLocaleDateString('en-GB', {
                day: 'numeric',
                month: 'short',
                year: 'numeric'
            })
            .replace(/ /g, '-');
        return `${formattedDate} ${dateTime.toLocaleTimeString()}`;
    };

    const csvData = () => {
        let prev;
        return item?.bomChanges
            ? [
                  [`${item.documentType}${item.documentNumber}`],
                  [`${item.newPartNumber} - ${item.newPartDescription}`],
                  [formatDateTime(item.dateEntered)],
                  [item.enteredBy.fullName],
                  [item.descriptionOfChange],
                  [item.reasonForChange],
                  [],
                  [],
                  ['BOM CHANGES', 'DELETE', 'ADD', 'OLD QTY', 'REQ', 'NEW QTY', 'REQ', 'STATUS'],
                  ...item.bomChanges
                      .map(c =>
                          c.bomChangeDetails.map(d => ({
                              ...d,
                              bomName: c.bomName,
                              dateApplied: c.dateApplied,
                              changeState: c.changeState
                          }))
                      )
                      .flat()
                      .map(f => {
                          const line = [
                              prev !== f.bomName ? f.bomName : '',
                              f.deletePartNumber,
                              f.addPartNumber,
                              f.deleteQty,
                              f.deleteGenerateRequirement,
                              f.addQty,
                              f.addGenerateRequirement,
                              prev !== f.bomName
                                  ? `${f.changeState} ${formatDateTime(f.dateApplied)}`
                                  : ''
                          ];
                          prev = f.bomName;
                          return line;
                      })
              ]
            : [];
    };
    const { state } = useLocation();

    const [goPrev, goNext, prevResult, nextResult] = usePreviousNextNavigation(
        docNo => `/purchasing/change-requests/${docNo}`,
        state?.searchResults
    );

    return (
        <Page history={history}>
            {loading || statusChangeLoading ? (
                <Loading />
            ) : (
                <Grid container spacing={2} justifyContent="center">
                    {itemError && (
                        <Grid item xs={12} style={{ maxHeight: '200px' }} overflow="scroll">
                            <ErrorCard
                                errorMessage={itemError.details?.error || itemError.details}
                            />
                        </Grid>
                    )}
                    {item?.bomChanges && (
                        <>
                            <Grid item xs={10} />
                            <Grid item xs={2}>
                                <CSVLink
                                    data={csvData()}
                                    filename={`${item.documentType}${item.documentNumber}`}
                                >
                                    <Button variant="outlined">Export</Button>
                                </CSVLink>
                            </Grid>
                        </>
                    )}
                    <PrevNextButtons
                        goPrev={goPrev}
                        goNext={goNext}
                        nextResult={nextResult}
                        prevResult={prevResult}
                    />
                    <Grid item xs={12}>
                        <Typography variant="h6">Change Request {id}</Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <Box sx={{ width: '100%' }}>
                            <Box sx={{ borderBottom: 0, borderColor: 'divider' }}>
                                <Tabs
                                    value={tab}
                                    onChange={(_, newValue) => {
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
                            style={{ marginRight: '30px' }}
                        >
                            Cancel
                        </Button>
                        <Button variant="outlined" disabled={!undoUri} onClick={() => undo(item)}>
                            Undo
                        </Button>
                    </Grid>
                </Grid>
            )}
        </Page>
    );
}

export default ChangeRequest;
