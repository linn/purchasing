import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import IconButton from '@mui/material/IconButton';
import Close from '@mui/icons-material/Close';
import Dialog from '@mui/material/Dialog';
import { useSelector, useDispatch } from 'react-redux';
import { itemSelectorHelpers, Page } from '@linn-it/linn-form-components-library';
import { makeStyles } from '@mui/styles';
import { DataGrid } from '@mui/x-data-grid';
import purchaseOrderActions from '../../actions/purchaseOrderActions';
import { purchaseOrder } from '../../itemTypes';
import history from '../../history';
import config from '../../config';

function PlInvRecDialog({ open, setOpen, ledgerEntries, orderNumber, inDialog }) {
    const useStyles = makeStyles(theme => ({
        pullRight: {
            float: 'right'
        },
        pad: { padding: theme.spacing(4) }
    }));

    const classes = useStyles();

    const columns = [
        { field: 'tref', headerName: 'tref', width: 100, hide: true },
        { field: 'invoiceDate', headerName: 'Inv Date', width: 100 },
        { field: 'plDeliveryRef', headerName: 'Del Ref', width: 100 },
        { field: 'qty', headerName: 'Qty', width: 100 },
        { field: 'netTotal', headerName: 'Net', width: 100 },
        { field: 'vatTotal', headerName: 'Vat', width: 100 },
        { field: 'invoiceRef', headerName: 'Inv Ref', width: 200 },
        { field: 'baseVat', headerName: 'Base Vat', width: 100 }
    ];

    const dispatch = useDispatch();

    useEffect(() => {
        if (!ledgerEntries) {
            dispatch(purchaseOrderActions.fetch(orderNumber));
        }
    }, [ledgerEntries, orderNumber, dispatch]);

    const order = useSelector(state => itemSelectorHelpers.getItem(state[purchaseOrder.item]));
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[purchaseOrder.item])
    );

    const content = () => (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                <DataGrid
                    rows={
                        ledgerEntries?.map(e => ({ ...e, id: e.tref })) ??
                        order?.ledgerEntries ??
                        []
                    }
                    columns={columns}
                    rowHeight={34}
                    autoHeight
                    columnBuffer={6}
                    loading={loading}
                    hideFooter
                    checkboxSelection={false}
                />
            </Grid>
        </Grid>
    );
    if (inDialog) {
        return (
            <Dialog open={open} fullWidth maxWidth="md">
                <div className={classes.pad}>
                    <IconButton
                        className={classes.pullRight}
                        aria-label="Close"
                        onClick={() => setOpen(false)}
                    >
                        <Close />
                    </IconButton>
                    {content()}
                </div>
            </Dialog>
        );
    }
    return (
        <Page history={history} homeUrl={config.appRoot}>
            {content()}
        </Page>
    );
}

PlInvRecDialog.propTypes = {
    open: PropTypes.bool.isRequired,
    setOpen: PropTypes.func.isRequired,
    ledgerEntries: PropTypes.arrayOf(PropTypes.shape({})),
    orderNumber: PropTypes.number,
    inDialog: PropTypes.bool
};

PlInvRecDialog.defaultProps = {
    ledgerEntries: null,
    orderNumber: null,
    inDialog: false
};

export default PlInvRecDialog;
