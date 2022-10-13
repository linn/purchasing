import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import IconButton from '@mui/material/IconButton';
import Close from '@mui/icons-material/Close';
import Dialog from '@mui/material/Dialog';
import { makeStyles } from '@mui/styles';
import { DataGrid } from '@mui/x-data-grid';

function PlInvRecDialog({ open, setOpen, ledgerEntries }) {
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
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <DataGrid
                            rows={ledgerEntries?.map(e => ({ ...e, id: e.tref })) ?? []}
                            columns={columns}
                            rowHeight={34}
                            autoHeight
                            columnBuffer={6}
                            loading={false}
                            hideFooter
                            checkboxSelection={false}
                        />
                    </Grid>
                </Grid>
            </div>
        </Dialog>
    );
}

PlInvRecDialog.propTypes = {
    open: PropTypes.bool.isRequired,
    setOpen: PropTypes.func.isRequired,
    ledgerEntries: PropTypes.arrayOf(PropTypes.shape({})).isRequired
};

export default PlInvRecDialog;
