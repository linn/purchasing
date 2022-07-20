import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    InputField,
    Page,
    Dropdown,
    Title,
    collectionSelectorHelpers,
    Loading
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import { vendorManagers } from '../../itemTypes';
import vendorManagersActions from '../../actions/vendorManagersActions';

function ShortagesReportOptions() {
    const dispatch = useDispatch();
    const vendorManagersOptions = useSelector(state => [
        { vmId: 'ALL', name: '', userNumber: '' },
        ...collectionSelectorHelpers.getItems(state[vendorManagers.item])
    ]);
    const vendorManagersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state[vendorManagers.item])
    );
    useEffect(() => {
        dispatch(vendorManagersActions.fetch());
    }, [dispatch]);

    const [options, setOptions] = useState({
        purchaseLevel: 3,
        vendorManager: 'ALL'
    });

    const handleOptionChange = (propertyName, newValue) => {
        setOptions(o => ({ ...o, [propertyName]: newValue }));
    };

    const handleClick = () =>
        history.push({
            pathname: `/purchasing/reports/shortages/report`,
            search:
                `?purchaseLevel=${options.purchaseLevel}` +
                `&vendorManager=${options.vendorManager}`
        });

    return (
        <Page history={history} homeUrl={config.appRoot} width="s">
            <Title text="Shortages Report" />
            <Grid container spacing={3} justifyContent="center">
                {vendorManagersLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={5}>
                            <Dropdown
                                label="Vendor Manager"
                                propertyName="vendorManager"
                                value={options.vendorManager}
                                onChange={handleOptionChange}
                                items={vendorManagersOptions.map(v => ({
                                    id: v.vmId,
                                    displayText: `${v.vmId} ${v.name} (${v.userNumber})`
                                }))}
                                allowNoValue={false}
                            />
                        </Grid>
                        <Grid item xs={5}>
                            <InputField
                                propertyName="purchaseLevel"
                                label="Purchase Level"
                                type="number"
                                fullWidth
                                value={options.purchaseLevel}
                                onChange={handleOptionChange}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Button color="primary" variant="contained" onClick={handleClick}>
                                Run Report
                            </Button>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default ShortagesReportOptions;
