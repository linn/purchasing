import React, { useState, useEffect } from 'react';
import Grid from '@mui/material/Grid';
import {
    Page,
    InputField,
    Typeahead,
    Title,
    userSelectors,
    collectionSelectorHelpers,
    Dropdown
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import DeleteIcon from '@mui/icons-material/Delete';
import moment from 'moment';
import history from '../history';
import config from '../config';

import plannersActions from '../actions/plannersActions';
import automaticPurchaseOrderSuggestionsActions from '../actions/automaticPurchaseOrderSuggestionsActions';
import automaticPurchaseOrderActions from '../actions/automaticPurchaseOrderActions';
import suppliersActions from '../actions/suppliersActions';

function AutomaticPurchaseOrderSuggestions() {
    const [supplier, setSupplier] = useState(null);
    const [planner, setPlanner] = useState(null);
    const [rows, setRows] = useState([]);

    const suggestions = useSelector(state => state.automaticPurchaseOrderSuggestions.searchItems);
    const suggestionsLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.automaticPurchaseOrderSuggestions)
    );

    const suppliersSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.suppliers)
    )?.map(c => ({
        id: c.id,
        name: c.id.toString(),
        description: c.name
    }));
    const suppliersSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.suppliers)
    );
    const planners = useSelector(state => collectionSelectorHelpers.getItems(state.planners));
    const plannersLoading = useSelector(state =>
        collectionSelectorHelpers.getLoading(state.planners)
    );
    const dispatch = useDispatch();
    const userNumber = useSelector(state => userSelectors.getUserNumber(state));

    useEffect(() => {
        if (!planners || planners.length === 0) {
            dispatch(plannersActions.fetch());
        }
    }, [dispatch, planners]);

    useEffect(() => {
        if (suggestions && suggestions.length > 0) {
            setRows(suggestions.map((s, i) => ({ ...s, id: i, s })));
        } else {
            setRows([]);
        }
    }, [suggestions, setRows]);

    const useStyles = makeStyles(theme => ({
        formControl: {
            margin: theme.spacing(1),
            minWidth: 120
        },
        selectEmpty: {
            marginTop: theme.spacing(2)
        },
        editing: {
            backgroundColor: 'linen'
        },
        inserting: {
            backgroundColor: 'whiteSmoke'
        },
        deleting: {
            backgroundColor: 'indianred',
            textDecorationLine: 'line-through'
        },
        gap: {
            marginBottom: '20px'
        }
    }));
    const classes = useStyles();

    const handleSetSupplier = (_, supp) => {
        setSupplier({ id: supp });
    };

    const handleSupplierReturn = selected => {
        setSupplier(selected);
    };

    const handleSupplierChange = selectedsupplier => {
        setSupplier(selectedsupplier);
    };

    const getSuggestedOrders = () => {
        let options = '';
        if (supplier && supplier.id) {
            options += `&supplierId=${supplier.id}`;
        }

        if (planner) {
            options += `&planner=${planner}`;
        }

        dispatch(automaticPurchaseOrderSuggestionsActions.searchWithOptions(null, options));
    };

    const handleDeleteRow = row => {
        setRows(rows.filter(a => a.id !== row.id));
    };

    const showMr = () => {
        let href = '/purchasing/material-requirements/report?';
        rows.forEach(a => {
            href = `${href}partNumbers=${a.partNumber}&`;
        });

        window.open(href, '_blank');
    };

    const createOrders = () => {
        if (rows.length > 0) {
            const { jobRef } = rows[0];
            const details = rows.map(r => ({
                partNumber: r.partNumber,
                supplierId: r.preferredSupplierId,
                supplierName: r.supplierName,
                quantity: r.recommendedQuantity,
                recommendationCode: r.recommendationCode,
                currencyCode: r.currencyCode,
                currencyPrice: r.recommendedQuantity * r.ourPrice,
                requestedDate: r.recommendedDate,
                orderMethod: r.orderMethod
            }));
            const proposedAutoOrder = {
                startedBy: userNumber,
                jobRef,
                details
            };

            dispatch(automaticPurchaseOrderActions.add(proposedAutoOrder));
        }
    };
    const handleEditRowsModelChange = () => {};

    const columns = [
        { field: 'partNumber', headerName: 'Part Number', minWidth: 140 },
        { field: 'preferredSupplierId', headerName: 'Supplier', minWidth: 100 },
        { field: 'supplierName', headerName: 'Name', minWidth: 300 },
        { field: 'recommendedQuantity', headerName: 'Qty', minWidth: 100 },
        {
            field: 'recommendedDate',
            headerName: 'Date',
            minWidth: 160,
            valueGetter: ({ value }) => value && moment(value).format('DD MMM YYYY')
        },
        { field: 'orderMethod', headerName: 'Method', minWidth: 100 },
        {
            field: 'delete',
            headerName: ' ',
            width: 90,
            renderCell: params => (
                <IconButton
                    aria-label="delete"
                    size="small"
                    onClick={() => handleDeleteRow(params)}
                >
                    <DeleteIcon fontSize="inherit" />
                </IconButton>
            )
        }
    ];

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Raise Automatic Orders" />
            <Grid container>
                <Grid item xs={8}>
                    <Dropdown
                        items={planners.map(v => ({ id: v.id, displayText: v.employeeName }))}
                        value={planner}
                        allowNoValue
                        propertyName="planner"
                        label="Planner"
                        onChange={(_, value) => setPlanner(value)}
                        loading={plannersLoading}
                    />
                </Grid>
                <Grid item xs={4} />
                <Grid item xs={4}>
                    <Typeahead
                        label="Supplier"
                        title="Search for a supplier"
                        onSelect={handleSupplierChange}
                        items={suppliersSearchResults}
                        loading={suppliersSearchLoading}
                        fetchItems={searchTerm => dispatch(suppliersActions.search(searchTerm))}
                        clearSearch={() => dispatch(suppliersActions.clearSearch)}
                        value={supplier?.id}
                        openModalOnClick={false}
                        modal
                        links={false}
                        debounce={1000}
                        handleFieldChange={handleSetSupplier}
                        handleReturnPress={handleSupplierReturn}
                        minimumSearchTermLength={2}
                    />
                </Grid>
                <Grid item xs={6}>
                    <InputField
                        disabled
                        propertyName="supplierName"
                        value={supplier?.description}
                        fullWidth
                        label="Supplier Name"
                    />
                </Grid>
                <Grid item xs={2} />
                <Grid item xs={4}>
                    <Button variant="outlined" onClick={getSuggestedOrders}>
                        Fetch Suggested Orders
                    </Button>
                </Grid>
                <Grid item xs={8}>
                    <Button onClick={showMr} disabled={!rows || rows.length <= 0}>
                        Show MR For Suggested Parts
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    <Typography variant="h6">Automatic Order Suggestions</Typography>
                    <div>
                        <DataGrid
                            className={classes.gap}
                            rows={rows}
                            columns={columns}
                            density="compact"
                            rowHeight={34}
                            autoHeight
                            loading={suggestionsLoading}
                            onEditRowsModelChange={handleEditRowsModelChange}
                        />
                    </div>
                </Grid>
                <Grid item xs={12}>
                    <Button
                        variant="outlined"
                        onClick={createOrders}
                        disabled={!rows || rows.length <= 0}
                    >
                        Create Orders
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default AutomaticPurchaseOrderSuggestions;
