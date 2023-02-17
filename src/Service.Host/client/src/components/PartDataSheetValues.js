import {
    Page,
    SaveBackCancelButtons,
    Title,
    SnackbarMessage,
    ErrorCard,
    getItemError,
    itemSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import React, { useState } from 'react';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import { useDispatch, useSelector } from 'react-redux';
import config from '../config';
import partDataSheetValuesActions from '../actions/partDataSheetValuesActions';
import partDataSheetValuesListActions from '../actions/partDataSheetValuesListActions';
import useInitialise from '../hooks/useInitialise';
import { partDataSheetValuesList, partDataSheetValues } from '../itemTypes';
import history from '../history';

function PartDataSheetValues() {
    const dispatch = useDispatch();
    const [items, loading] = useInitialise(
        () => partDataSheetValuesListActions.fetch(),
        partDataSheetValuesList.item,
        'items'
    );

    const [rows, setRows] = useState();

    if (!rows && items.length) {
        setRows(
            items.map(i => ({
                ...i,
                id: `${i.attributeSet}/${i.field}/${i.value}`
            }))
        );
    }
    const itemError = useSelector(state => getItemError(state, partDataSheetValues.item));
    const snackbarVisible = useSelector(state =>
        itemSelectorHelpers.getSnackbarVisible(state[partDataSheetValues.item])
    );
    const updateLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[partDataSheetValues.item])
    );
    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'attributeSet', headerName: 'Att. Set', editable: true, width: 200 },
        {
            field: 'field',
            headerName: 'Field',
            width: 200,
            editable: true,
            type: 'singleSelect',
            valueOptions: ['DIELECTRIC', 'PACKAGE', 'POLARITY', 'CONSTRUCTION']
        },
        { field: 'value', headerName: 'Value', width: 250, editable: true },
        { field: 'description', headerName: 'Desc', width: 300, editable: true },
        {
            field: 'assemblyTechnology',
            headerName: 'Assembly Tech',
            width: 150,
            editable: true,
            type: 'singleSelect',
            valueOptions: ['', 'SM', 'TH']
        },

        { field: 'imdsNumber', headerName: 'IMDS', width: 100, editable: true },
        { field: 'imdsWeight', headerName: 'Weight(g)', width: 100, editable: true }
    ];

    const [changesMade, setChangesMade] = useState(false);
    const setSnackbarVisible = () => dispatch(partDataSheetValuesActions.setSnackbarVisible(false));

    const processRowUpdate = newRow => {
        setChangesMade(true);
        setRows(r => r.map(x => (x.id === newRow.id ? { ...newRow, hasChanged: true } : x)));
        return { ...newRow, hasChanged: true };
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <SnackbarMessage
                visible={snackbarVisible}
                onClose={() => setSnackbarVisible(false)}
                message="Save Successful"
            />
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Part Data Sheet Values" />
                </Grid>
                {itemError && (
                    <Grid style={{ paddingTop: '100px' }} item xs={12}>
                        <ErrorCard errorMessage={itemError.statusText} />
                    </Grid>
                )}
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveDisabled={
                            !changesMade ||
                            rows.some(row => !row.attributeSet || !row.field || !row.value)
                        }
                        saveClick={() => {
                            setChangesMade(false);
                            dispatch(partDataSheetValuesActions.clearErrorsForItem());
                            rows.filter(x => x.isAddition).forEach(addition => {
                                dispatch(partDataSheetValuesActions.add(addition));
                            });
                            rows.filter(x => x.hasChanged && !x.isAddition).forEach(i => {
                                dispatch(
                                    partDataSheetValuesActions.update(
                                        `${i.attributeSet}/${i.field}/${i.value}`,
                                        i
                                    )
                                );
                            });
                        }}
                        cancelClick={() => {
                            setChangesMade(false);
                            setRows(
                                items.map(i => ({
                                    ...i,
                                    id: `${i.attributeSet}/${i.field}/${i.value}`
                                }))
                            );
                        }}
                        backClick={() => history.push('/purchasing')}
                    />
                </Grid>
                <Grid item xs={12}>
                    <Button
                        variant="outlined"
                        disabled={!rows || rows?.some(r => r.isAddition)}
                        onClick={() => setRows(r => [{ isAddition: true, id: 'newRow' }, ...r])}
                    >
                        +
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    <DataGrid
                        columnBuffer={6}
                        rows={rows ?? []}
                        loading={loading || updateLoading}
                        processRowUpdate={processRowUpdate}
                        onProcessRowUpdateError={() => {}}
                        autoHeight
                        isCellEditable={params => params.row.isAddition || params.colDef.editable}
                        experimentalFeatures={{ newEditingApi: true }}
                        disableSelectionOnClick
                        columns={columns}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default PartDataSheetValues;
