import React, { useState } from 'react';
import { Page } from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';

import Grid from '@mui/material/Grid';
import BomTree from '../BomTree';
import history from '../../history';
import config from '../../config';

function BomUtility() {
    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'type', headerName: 'Type', width: 100 },
        { field: 'name', headerName: 'Part', width: 100 },
        { field: 'description', headerName: 'Description', width: 500 },
        { field: 'qty', headerName: 'Qty', width: 100 }
    ];
    const [selected, setSelected] = useState(null);
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={4}>
                    <BomTree
                        renderDescriptions={false}
                        renderComponents={false}
                        renderQties={false}
                        onNodeSelect={id => setSelected(id)}
                    />
                </Grid>
                <Grid item xs={8}>
                    {selected && (
                        <DataGrid
                            columnBuffer={6}
                            rows={selected}
                            autoHeight
                            hideFooter
                            checkboxSelection
                            columns={columns}
                        />
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default BomUtility;
