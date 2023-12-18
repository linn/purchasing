import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import PropTypes from 'prop-types';
import { ExportButton } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Link from '@mui/material/Link';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import ChangeState from '../ChangeState';
import config from '../../../config';

function PcasChangesTab({ pcasChanges, handleSelectChange }) {
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
        {
            field: 'boardCode',
            headerName: 'Board Code',
            width: 140,
            renderCell: params => (
                <Link
                    className={classes.a}
                    component={RouterLink}
                    to={`/purchasing/boms/board-components/${params.row.boardCode}`}
                >
                    {params.row.boardCode}
                </Link>
            )
        },
        { field: 'revisionCode', headerName: 'Revision Code', width: 140 },
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
        { field: 'changeId', headerName: 'Id', width: 100 },
        { field: 'lifecycleText', headerName: 'Lifecycle', width: 350 }
    ];

    return (
        <Grid container spacing={3}>
            <Grid item xs={9} />
            <Grid item xs={3}>
                <ExportButton
                    href={`${config.appRoot}/purchasing/change-requests/pcas-component-changes?documentNumber=${pcasChanges?.[0]?.documentNumber}`}
                    buttonText="Export Component Changes"
                />
            </Grid>

            <Grid item xs={12}>
                {pcasChanges ? (
                    <DataGrid
                        getRowId={row => row.changeId}
                        className={classes.gap}
                        rows={pcasChanges}
                        columns={columns}
                        rowHeight={34}
                        autoHeight
                        loading={false}
                        checkboxSelection
                        onRowSelectionModelChange={handleSelectChange}
                        pageSize={100}
                        hideFooter={!pcasChanges || pcasChanges.length <= 100}
                    />
                ) : (
                    <span>No Bom Changes</span>
                )}
            </Grid>
        </Grid>
    );
}

PcasChangesTab.propTypes = {
    pcasChanges: PropTypes.arrayOf(PropTypes.shape({ documentNumber: PropTypes.number })),
    handleSelectChange: PropTypes.func
};

PcasChangesTab.defaultProps = {
    pcasChanges: [],
    handleSelectChange: null
};

export default PcasChangesTab;
