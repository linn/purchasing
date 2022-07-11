import React, { useState, useEffect } from 'react';
import Grid from '@mui/material/Grid';
import {
    Page,
    InputField,
    Typeahead,
    Title,
    collectionSelectorHelpers,
    Loading
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import DeleteIcon from '@mui/icons-material/Delete';
import history from '../history';
import config from '../config';
import automaticPurchaseOrderSuggestionsActions from '../actions/automaticPurchaseOrderSuggestionsActions';
import automaticPurchaseOrderActions from '../actions/automaticPurchaseOrderActions';
import suppliersActions from '../actions/suppliersActions';

function AutomaticPurchaseOrders() {
    const [supplier, setSupplier] = useState(null);
    const [planner, setPlanner] = useState(null);
    const [rows, setRows] = useState([]);

    const suggestions = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.automaticPurchaseOrderSuggestions)
    );
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

    useEffect(() => {
        if (suggestions && suggestions.length > 0) {
            setRows(!suggestions ? [] : suggestions.map((s, i) => ({ ...s, id: i, s })));
        } else {
            setRows([]);
        }
    }, [suggestions]);

    const dispatch = useDispatch();

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
        if (supplier) {
            options += `&supplierId=${supplier.id}`;
        }

        if (planner) {
            options += `&planner=${planner}`;
        }

        dispatch(automaticPurchaseOrderSuggestionsActions.searchWithOptions(null, options));
    };

    const handleDeleteRow = () => {};
    const createOrders = () => {};
    const handleEditRowsModelChange = () => {};
    const getBackgroundColourClass = () => {};

    const columns = [
        { field: 'partNumber', headerName: 'Part Number', minWidth: 100 },
        { field: 'preferredSupplierId', headerName: 'Supplier', width: 100 },
        {
            field: 'delete',
            headerName: ' ',
            width: 130,
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
            <Title text="Automatic Orders" />
            <Grid container>
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
                <Grid item xs={12}>
                    <Button variant="outlined" onClick={getSuggestedOrders}>
                        Fetch Suggested Orders
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
                            hideFooter
                            onEditRowsModelChange={handleEditRowsModelChange}
                            getRowClassName={getBackgroundColourClass}
                        />
                    </div>
                </Grid>
                <Grid item xs={12}>
                    <Button variant="outlined" onClick={createOrders}>
                        Create Orders
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default AutomaticPurchaseOrders;
