import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import Link from '@mui/material/Link';
import ChangeState from './ChangeState';

function PcasChangeReplace({ wused, handleSelectChange, documentNumber }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        },
        a: {
            textDecoration: 'none'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'quantity', headerName: 'Qty', width: 100 },
        {
            field: 'boardCode',
            headerName: 'Board',
            width: 150,
            renderCell: params => (
                <Link
                    className={classes.a}
                    component={RouterLink}
                    to={`/purchasing/boms/board-components/${params.row.boardCode}?changeRequest=${documentNumber}`}
                >
                    {params.row.boardCode}
                </Link>
            )
        },
        { field: 'revisionCode', headerName: 'Revision', width: 150 },
        { field: 'cref', headerName: 'Cref', width: 100 },
        { field: 'assemblyTechnology', headerName: 'AssT', width: 100 },
        {
            field: 'changeState',
            headerName: 'State',
            width: 150,
            renderCell: params => (
                <>
                    <ChangeState changeState={params.row.changeState} showLabel={false} />
                </>
            )
        },
        { field: 'deleteChangeRequest', headerName: 'Delete Change Request', width: 150 }
    ];

    return (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                {wused ? (
                    <DataGrid
                        getRowId={row =>
                            `${row.boardCode}/${row.revisionCode}/${row.cref}/${row.quantity}/${row.assemblyTechnology}/${row.boardLine}`
                        }
                        className={classes.gap}
                        rows={wused}
                        columns={columns}
                        getRowHeight={() => 'auto'}
                        autoHeight
                        loading={false}
                        checkboxSelection
                        isRowSelectable={params => !params.row.deleteChangeId}
                        onSelectionModelChange={handleSelectChange}
                        pageSize={100}
                        hideFooter={!wused || wused.length <= 100}
                    />
                ) : (
                    <span>Old Part Not Used Anywhere</span>
                )}
            </Grid>
        </Grid>
    );
}

PcasChangeReplace.propTypes = {
    wused: PropTypes.arrayOf(PropTypes.shape({})),
    handleSelectChange: PropTypes.func,
    documentNumber: PropTypes.number
};

PcasChangeReplace.defaultProps = {
    wused: [],
    handleSelectChange: null,
    documentNumber: 0
};

export default PcasChangeReplace;
