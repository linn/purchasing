import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';

function BomChangesTab({ bomChanges }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'changeId', headerName: 'Id', width: 100 },

        { field: 'bomName', headerName: 'Bom', width: 200 },
        { field: 'changeState', headerName: 'State', width: 200 },
        { field: 'phaseInWWYYYY', headerName: 'Phase In Week', width: 200 }
    ];

    return (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                {bomChanges ? (
                    <DataGrid
                        getRowId={row => row.changeId}
                        className={classes.gap}
                        rows={bomChanges}
                        columns={columns}
                        rowHeight={34}
                        autoHeight
                        loading={false}
                        hideFooter
                    />
                ) : (
                    <span>No Bom Changes</span>
                )}
            </Grid>
        </Grid>
    );
}

BomChangesTab.propTypes = {
    bomChanges: PropTypes.arrayOf(PropTypes.shape({}))
};

BomChangesTab.defaultProps = {
    bomChanges: []
};

export default BomChangesTab;