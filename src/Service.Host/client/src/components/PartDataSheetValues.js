import { Loading, Page, Title } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import React from 'react';
import history from '../history';
import { useDispatch, useSelector } from 'react-redux';
import config from '../config';
import partDataSheetValuesActions from '../actions/partDataSheetValuesActions';
import partDataSheetValuesListActions from '../actions/partDataSheetValuesListActions';
import useInitialise from '../hooks/useInitialise';
import { partDataSheetValues, partDataSheetValuesList } from '../itemTypes';
import { DataGrid } from '@mui/x-data-grid';

function PartDataSheetValues() {
    const [items, loading] = useInitialise(
        () => partDataSheetValuesListActions.fetch(),
        partDataSheetValuesList.item,
        'items'
    );

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'attributeSet', headerName: 'Att. Set', width: 200 },
        { field: 'field', headerName: 'Field', width: 200 },
        { field: 'value', headerName: 'Value', width: 200 },
        { field: 'value', headerName: 'Value', width: 200 }
    ];

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Part Data Sheet Values" />
                </Grid>
                {loading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
                <Grid item xs={12}>
                    {items && (
                        <DataGrid
                            columnBuffer={6}
                            rows={items.map(i => ({
                                ...i,
                                id: `${i.attributeSet}/${i.field}/${i.value}`
                            }))}
                            loading={loading}
                            // processRowUpdate={processRowUpdate}
                            // onProcessRowUpdateError={err => console.log(err)}
                            hideFooter
                            autoHeight
                            experimentalFeatures={{ newEditingApi: true }}
                            disableSelectionOnClick
                            columns={columns}
                        />
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default PartDataSheetValues;
