import React, { useState } from 'react';
import { Dropdown, Page } from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
import LinearProgress from '@mui/material/LinearProgress';

import Grid from '@mui/material/Grid';
import { useLocation } from 'react-router';
import queryString from 'query-string';

import BomTree from '../BomTree';
import history from '../../history';
import config from '../../config';
import { changeRequests as changeRequestsItemType } from '../../itemTypes';

import changeRequestsActions from '../../actions/changeRequestsActions';
import useInitialise from '../../hooks/useInitialise';

function BomUtility() {
    const { search } = useLocation();
    const { bomName } = queryString.parse(search);
    const [crNumber, setCrNumber] = useState();
    const [changeRequests, changeRequestsLoading] = useInitialise(
        () => changeRequestsActions.search(bomName),
        changeRequestsItemType.item,
        'searchItems'
    );

    console.log(changeRequests);

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
                {changeRequestsLoading ? (
                    <Grid item xs={12}>
                        <LinearProgress />
                    </Grid>
                ) : (
                    <Grid item xs={12}>
                        <Dropdown
                            items={changeRequests?.map(c => ({
                                id: c.documnerNumber,
                                displayText: `${c.documentType}${c.documentNumber}`
                            }))}
                            allowNoValue
                            label="CRF Number"
                            helperText="Select a corresponding CRF to start editing"
                            value={crNumber}
                            onChange={(_, n) => {
                                setCrNumber(n);
                            }}
                        />
                    </Grid>
                )}
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
