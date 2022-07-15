import React, { useEffect } from 'react';
import Grid from '@mui/material/Grid';
import { Page, Title, itemSelectorHelpers, Loading } from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import Typography from '@mui/material/Typography';
import Link from '@mui/material/Link';
import Stack from '@mui/material/Stack';
import moment from 'moment';
import { useParams } from 'react-router-dom';
import history from '../history';
import config from '../config';
import automaticPurchaseOrderActions from '../actions/automaticPurchaseOrderActions';

function AutomaticPurchaseOrders() {
    const { id } = useParams();
    const dispatch = useDispatch();

    useEffect(() => {
        if (id) {
            dispatch(automaticPurchaseOrderActions.fetch(id));
        }
    }, [dispatch, id]);

    const automaticPurchaseOrder = useSelector(state =>
        itemSelectorHelpers.getItem(state.automaticPurchaseOrder)
    );
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.automaticPurchaseOrder)
    );

    const columns = [
        { field: 'partNumber', headerName: 'Part Number', minWidth: 140 },
        { field: 'supplierId', headerName: 'Supplier', minWidth: 100 },
        { field: 'supplierName', headerName: 'Name', minWidth: 300 },
        { field: 'quantity', headerName: 'Qty', minWidth: 100 },
        {
            field: 'requestedDate',
            headerName: 'Date Requested',
            minWidth: 190,
            valueGetter: ({ value }) => value && moment(value).format('DD MMM YYYY')
        },
        { field: 'orderNumber', headerName: 'Order Number', minWidth: 150 },
        { field: 'issuePartsToSupplier', headerName: 'Issue Parts', minWidth: 150 },
        { field: 'orderMethod', headerName: 'Method', minWidth: 100 }
    ];

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Orders Raised Automatically" />
            <Grid container>
                <Grid item xs={12}>
                    {loading && <Loading />}
                </Grid>
                {!loading && automaticPurchaseOrder && (
                    <>
                        <Grid item xs={4}>
                            <Stack direction="row" spacing={2}>
                                <Typography variant="body2">Transaction Id:</Typography>
                                <Typography variant="body2" style={{ fontWeight: 'bold' }}>
                                    {automaticPurchaseOrder.id}
                                </Typography>
                            </Stack>
                        </Grid>
                        <Grid item xs={4}>
                            <Stack direction="row" spacing={2}>
                                <Typography variant="body2">JobRef:</Typography>
                                <Typography variant="body2">
                                    {automaticPurchaseOrder.jobRef}
                                </Typography>
                            </Stack>
                        </Grid>
                        <Grid item xs={4}>
                            <Link
                                href="/purchasing/automatic-purchase-order-suggestions"
                                color="inherit"
                                variant="body2"
                            >
                                Raise more automatic orders
                            </Link>
                        </Grid>
                        <Grid item xs={12} style={{ paddingTop: '40px' }}>
                            <div>
                                <DataGrid
                                    rows={automaticPurchaseOrder.details.map(d => ({
                                        ...d,
                                        id: d.sequence
                                    }))}
                                    columns={columns}
                                    density="compact"
                                    rowHeight={34}
                                    autoHeight
                                    loading={loading}
                                    hideFooter
                                />
                            </div>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default AutomaticPurchaseOrders;
