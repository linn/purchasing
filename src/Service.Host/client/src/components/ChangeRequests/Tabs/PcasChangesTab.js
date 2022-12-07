import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import ChangeState from '../ChangeState';

function PcasChangesTab({ pcasChanges }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'changeId', headerName: 'Id', width: 100 },

        { field: 'boardCode', headerName: 'Board Code', width: 140 },
        { field: 'revisionCode', headerName: 'Revision Code', width: 140 },
        {
            field: 'changeState',
            headerName: 'State',
            width: 200,
            renderCell: params => (
                <>
                    <ChangeState changeState={params.row.changeState} showLabel={false} />
                </>
            )
        },
    ];

    return (
        <Grid container spacing={3}>
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
                        hideFooter
                    />
                ) : (
                    <span>No Bom Changes</span>
                )}
            </Grid>
        </Grid>
    );
}

PcasChangesTab.propTypes = {
    pcasChanges: PropTypes.arrayOf(PropTypes.shape({}))
};

PcasChangesTab.defaultProps = {
    pcasChanges: []
};

export default PcasChangesTab;
