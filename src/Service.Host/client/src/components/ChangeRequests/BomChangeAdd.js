import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import DeleteIcon from '@mui/icons-material/Delete';
import IconButton from '@mui/material/IconButton';
import { makeStyles } from '@mui/styles';
import { Search, collectionSelectorHelpers } from '@linn-it/linn-form-components-library';
import partsActions from '../../actions/partsActions';

function BomChangeAdd({ addBoms, addAddBomsItem, deleteAddBomsItem, defaultQty, setAddBoms }) {
    const useStyles = makeStyles(() => ({
        buttonMarginTop: {
            marginTop: '12px'
        }
    }));
    const classes = useStyles();

    const dispatch = useDispatch();

    const handleEditRowsModelChange = model => {
        /* you can only edit qty */
        if (model && Object.keys(model)[0]) {
            const id = Object.keys(model)[0];
            if (model && model[id] && model[id].qty) {
                const newValue = parseInt(model[id].qty.value, 10);
                const newAddBoms = addBoms.map(x => (x.name === id ? { ...x, qty: newValue } : x));
                setAddBoms(newAddBoms);
            }
        }
    };

    const [bomName, setBomName] = useState('');
    const [selectedBom, setSelectedBom] = useState('');

    const partsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.parts)
    ).map?.(c => ({
        id: c.id,
        name: c.partNumber,
        partNumber: c.partNumber,
        description: c.description,
        qty: defaultQty
    }));

    const partsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.parts)
    );

    const bomsColumns = [
        { field: 'partNumber', headerName: 'Bom', minWidth: 140 },
        { field: 'description', headerName: 'Description', minWidth: 300 },
        {
            field: 'qty',
            headerName: 'Qty',
            width: 150,
            editable: true
        },
        {
            field: 'delete',
            headerName: ' ',
            width: 50,
            renderCell: params => (
                <IconButton
                    aria-label="delete"
                    size="small"
                    onClick={() => deleteAddBomsItem(params)}
                >
                    <DeleteIcon fontSize="inherit" />
                </IconButton>
            )
        }
    ];

    const addToList = () => {
        if (selectedBom) {
            addAddBomsItem(selectedBom);
            setSelectedBom(null);
        }
    };

    return (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                {addBoms ? (
                    <DataGrid
                        rows={addBoms}
                        columns={bomsColumns}
                        getRowId={row => row.partNumber}
                        pageSize={5}
                        rowsPerPageOptions={[5]}
                        density="compact"
                        rowHeight={34}
                        headerHeight={34}
                        onEditRowsModelChange={handleEditRowsModelChange}
                        autoHeight
                        hideFooter
                    />
                ) : (
                    <span>Not Added to any BOMs</span>
                )}
                <Search
                    propertyName="bomToAdd"
                    helperText="use Enter to search for bom to add"
                    handleValueChange={(_, newVal) => setBomName(newVal)}
                    onResultSelect={newValue => {
                        setSelectedBom(newValue);
                        setBomName(newValue.partNumber);
                    }}
                    clearSearch={() => dispatch(partsActions.clearSearch())}
                    searchResults={partsSearchResults}
                    loading={partsSearchLoading}
                    search={searchTerm => dispatch(partsActions.search(searchTerm))}
                    value={bomName}
                    resultsInModal
                />
                <Button
                    variant="outlined"
                    className={classes.buttonMarginTop}
                    color="primary"
                    onClick={addToList}
                    disabled={!selectedBom}
                >
                    Add
                </Button>
            </Grid>
        </Grid>
    );
}

BomChangeAdd.propTypes = {
    addBoms: PropTypes.arrayOf(PropTypes.shape({})),
    addAddBomsItem: PropTypes.func,
    deleteAddBomsItem: PropTypes.func,
    defaultQty: PropTypes.number,
    setAddBoms: PropTypes.func
};

BomChangeAdd.defaultProps = {
    addBoms: [],
    addAddBomsItem: null,
    deleteAddBomsItem: null,
    defaultQty: 1,
    setAddBoms: null
};

export default BomChangeAdd;
