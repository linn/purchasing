import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import {
    Page,
    Dropdown,
    Title,
    collectionSelectorHelpers,
    Loading,
    Typeahead
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';
import { suppliers, vendorManagers } from '../../itemTypes';
import suppliersActions from '../../actions/suppliersActions';
import vendorManagersActions from '../../actions/vendorManagersActions';

function ShortagesReportOptions() {
    const dispatch = useDispatch();
    const vendorManagersOptions = useSelector(state =>
        collectionSelectorHelpers.getItems(state[vendorManagers.item])
    );
    const vendorManagersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state[vendorManagers.item])
    );
    useEffect(() => {
        dispatch(vendorManagersActions.fetch());
    }, [dispatch]);

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state[suppliers.item])
    )?.map(c => ({
        id: c.id,
        name: c.id.toString(),
        description: c.name
    }));
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );

    const [options, setOptions] = useState({
        purchaseLevel: 3,
        supplier: 0,
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
                `&supplier=${options.supplier}` +
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
                        <Grid item xs={3}>
                            <Dropdown
                                label="Vendor Manager  (leave blank for all)"
                                propertyName="vendorManager"
                                value={options.vendorManager}
                                onChange={handleOptionChange}
                                items={vendorManagersOptions.map(v => ({
                                    id: v.vmId,
                                    displayText: `${v.vmId} ${v.name} (${v.userNumber})`
                                }))}
                                allowNoValue
                            />
                        </Grid>
                        <Grid item xs={3}>
                            <Typeahead
                                label="Supplier (leave blank for all)"
                                title="Search for a supplier"
                                onSelect={newValue => handleOptionChange('supplier', newValue.id)}
                                items={suppliersSearchResults}
                                loading={suppliersSearchLoading}
                                fetchItems={searchTerm =>
                                    dispatch(suppliersActions.search(searchTerm))
                                }
                                clearSearch={() => dispatch(suppliersActions.clearSearch)}
                                value={options.supplier}
                                modal
                                links={false}
                                debounce={1000}
                                minimumSearchTermLength={2}
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
