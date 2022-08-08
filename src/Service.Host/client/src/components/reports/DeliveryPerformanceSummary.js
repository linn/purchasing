import React, { useEffect, useState } from 'react';
import {
    Title,
    Page,
    collectionSelectorHelpers,
    Dropdown,
    Loading
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import moment from 'moment';
import history from '../../history';
import config from '../../config';
import ledgerPeriodsActions from '../../actions/ledgerPeriodsActions';

function DeliveryPerformanceSummary() {
    const [endPeriod, setEndPeriod] = useState(null);
    const [startPeriod, setStartPeriod] = useState(null);

    const dispatch = useDispatch();
    const ledgerPeriods = useSelector(state =>
        collectionSelectorHelpers.getItems(state.ledgerPeriods)
    );
    const ledgerPeriodsLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.ledgerPeriods)
    );

    useEffect(() => {
        dispatch(ledgerPeriodsActions.fetch());
    }, [dispatch]);

    useEffect(() => {
        if (ledgerPeriods && ledgerPeriods.length > 0) {
            const start = ledgerPeriods.find(
                a =>
                    a.monthName ===
                    moment(new Date()).subtract(6, 'months').format('MMMYYYY').toUpperCase()
            );
            const current = ledgerPeriods.find(
                a => a.monthName === moment(new Date()).format('MMMYYYY').toUpperCase()
            );
            setStartPeriod(start.periodNumber);
            setEndPeriod(current.periodNumber);
        }
    }, [ledgerPeriods]);

    const handleStartChange = (_, period) => {
        setStartPeriod(period);
    };

    const handleEndChange = (_, period) => {
        setEndPeriod(period);
    };

    const runReport = () => {
        const body = {
            startPeriod,
            endPeriod
        };
        history.push('/purchasing/reports/delivery-performance-summary/report', body);
    };
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Delivery Performance" />
            <Grid container>
                {ledgerPeriodsLoading && <Loading />}
                {!ledgerPeriodsLoading && startPeriod && (
                    <>
                        <Grid item xs={6}>
                            <Dropdown
                                items={ledgerPeriods.map(e => ({
                                    displayText: `${e.monthName}`,
                                    id: e.periodNumber
                                }))}
                                propertyName="startPeriod"
                                label="Start Month"
                                value={startPeriod}
                                onChange={handleStartChange}
                                type="number"
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Dropdown
                                items={ledgerPeriods.map(e => ({
                                    displayText: `${e.monthName}`,
                                    id: e.periodNumber
                                }))}
                                propertyName="endPeriod"
                                label="End Month"
                                value={endPeriod}
                                onChange={handleEndChange}
                                type="number"
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Button
                                variant="outlined"
                                onClick={runReport}
                                style={{ marginTop: '40px' }}
                            >
                                Run Report
                            </Button>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default DeliveryPerformanceSummary;
