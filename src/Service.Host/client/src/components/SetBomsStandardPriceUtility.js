import { InputField, Loading, Page } from '@linn-it/linn-form-components-library';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import queryString from 'query-string';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import { useLocation } from 'react-router';
import history from '../history';
import config from '../config';
import useInitialise from '../hooks/useInitialise';
import bomStandardPricesActions from '../actions/bomStandardPricesActions';
import { bomStandardPrices } from '../itemTypes';

const SetBomStandardPriceUtility = () => {
    const reduxDispatch = useDispatch();
    const { search } = useLocation();
    const { bomName } = queryString.parse(search);
    const [requestBody, setRequestBody] = useState({});

    const [result, loading] = useInitialise(
        () => bomStandardPricesActions.fetchByQueryString('searchTerm', bomName),
        bomStandardPrices.item,
        'item',
        bomStandardPricesActions.clearErrorsForItem
    );
    const columns = [
        { field: 'depth', headerName: 'Depth', width: 100 },
        { field: 'bomName', headerName: 'bom', width: 100 },
        { field: 'materialPrice', headerName: 'Mat Price', width: 100 },
        { field: 'standardPrice', headerName: 'Std Price', width: 100 },
        { field: 'stockMaterialVariance', headerName: 'Stock Mat Var', width: 100 },
        { field: 'loanMaterialVariance', headerName: 'Loan Mat Var', width: 100 },
        { field: 'allocLines', headerName: 'Alloc Lines', width: 100 }
    ];

    const handleSelectRow = selected => {
        setRequestBody(b => ({ ...b, lines: result.lines.filter(x => selected.includes(x.id)) }));
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                {loading && <Loading />}
                {result && (
                    <>
                        <Grid item xs={12}>
                            <DataGrid
                                columns={columns}
                                loading={loading}
                                autoHeight
                                rows={result?.lines.map(l => ({ ...l, id: l.bomName }))}
                                checkboxSelection
                                hideFooter
                                onSelectionModelChange={handleSelectRow}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                propertyName="remarks"
                                label="Remarks"
                                onChange={(_, newVal) =>
                                    setRequestBody(b => ({ ...b, remarks: newVal }))
                                }
                                value={requestBody.remarks}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Button
                                variant="contained"
                                disabled={!requestBody?.lines}
                                onClick={() =>
                                    reduxDispatch(bomStandardPricesActions.add(requestBody))
                                }
                            >
                                MAKE CHANGES
                            </Button>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
};

export default SetBomStandardPriceUtility;
