import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import Link from '@mui/material/Link';

function BomChangeReplace({ wused, handleSelectChange, documentNumber }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'qty', headerName: 'Qty', width: 100 },
        {
            field: 'name',
            headerName: 'Name',
            width: 150,
            renderCell: params => (
                <Link
                    className={classes.a}
                    component={RouterLink}
                    to={`/purchasing/boms/bom-utility?bomName=${params.row.name}&changeRequest=${documentNumber}`}
                >
                    {params.row.name}
                </Link>
            )
        },
        { field: 'description', headerName: 'Description', width: 450 },
        { field: 'pcasLine', headerName: 'Pcas', width: 80 },
        {
            field: 'deleteChangeDocumentNumber',
            headerName: 'Delete Chg',
            width: 100,
            renderCell: params => (
                <Link
                    className={classes.a}
                    component={RouterLink}
                    to={`/purchasing/change-requests/${params.row.deleteChangeDocumentNumber}`}
                >
                    {params.row.deleteChangeDocumentNumber}
                </Link>
            )
        }
    ];

    return (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                {wused ? (
                    <DataGrid
                        getRowId={row => row.id}
                        className={classes.gap}
                        rows={wused}
                        columns={columns}
                        getRowHeight={() => 'auto'}
                        autoHeight
                        loading={false}
                        checkboxSelection
                        isRowSelectable={params => !params.row.deleteChangeDocumentNumber}
                        onSelectionModelChange={handleSelectChange}
                        hideFooter
                    />
                ) : (
                    <span>Old Part Not Used Anywhere</span>
                )}
            </Grid>
        </Grid>
    );
}

BomChangeReplace.propTypes = {
    wused: PropTypes.arrayOf(PropTypes.shape({})),
    handleSelectChange: PropTypes.func,
    documentNumber: PropTypes.number
};

BomChangeReplace.defaultProps = {
    wused: [],
    handleSelectChange: null,
    documentNumber: 0
};

export default BomChangeReplace;