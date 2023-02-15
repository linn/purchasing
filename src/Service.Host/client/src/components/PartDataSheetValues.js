import { Page, SaveBackCancelButtons, Title } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import React, { useState } from 'react';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import { useDispatch } from 'react-redux';
import config from '../config';
import partDataSheetValuesActions from '../actions/partDataSheetValuesActions';
import partDataSheetValuesListActions from '../actions/partDataSheetValuesListActions';
import useInitialise from '../hooks/useInitialise';
import { partDataSheetValuesList } from '../itemTypes';
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

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'attributeSet', headerName: 'Attribute Set', editable: true, width: 250 },
        {
            field: 'field',
            headerName: 'Field',
            width: 250,
            editable: true,
            type: 'singleSelect',
            valueOptions: ['DIELECTRIC', 'PACKAGE', 'POLARITY', 'CONSTRUCTION']
        },
        { field: 'value', headerName: 'Value', width: 250, editable: true },
        { field: 'description', headerName: 'Desc', width: 200, editable: true },
        {
            field: 'assemblyTechnology',
            headerName: 'Assembly Tech',
            width: 200,
            editable: true,
            type: 'singleSelect',
            valueOptions: ['', 'SM', 'TH']
        },

        { field: 'imdsNumber', headerName: 'IMDS', width: 200, editable: true },
        { field: 'imdsWeight', headerName: 'Weight (g', width: 200, editable: true }
    ];

    const [changesMade, setChangesMade] = useState(false);

    const processRowUpdate = newRow => {
        setChangesMade(true);
        setRows(r => r.map(x => (x.id === newRow.id ? { ...newRow, hasChanged: true } : x)));
        return { ...newRow, hasChanged: true };
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Part Data Sheet Values" />
                </Grid>
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveDisabled={
                            !changesMade ||
                            rows.some(row => !row.attributeSet || !row.field || !row.value)
                        }
                        saveClick={() => {
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
                        loading={loading}
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
