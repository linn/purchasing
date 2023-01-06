import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import ChangeState from '../ChangeState';
import BomChangeDetails from '../BomChangeDetails';

function BomChangesTab({ bomChanges, handleSelectChange }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'bomName', headerName: 'Bom', width: 200 },
        {
            field: 'changeState',
            headerName: 'State',
            width: 150,
            renderCell: params => (
                <ChangeState changeState={params.row.changeState} showLabel={false} />
            )
        },
        { field: 'phaseInWWYYYY', headerName: 'Phase In Week', width: 200 },
        {
            field: 'bomChangesDetails',
            headerName: 'Chgs',
            width: 40,
            renderCell: params => (
                <BomChangeDetails bomChangeDetails={params.row.bomChangeDetails} />
            )
        },
        {
            field: 'lifecycleText',
            headerName: 'Lifecycle',
            width: 400
        },
        { field: 'changeId', headerName: 'Id', width: 100 }
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
                        getRowHeight={() => 'auto'}
                        autoHeight
                        loading={false}
                        checkboxSelection
                        onSelectionModelChange={handleSelectChange}
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
    bomChanges: PropTypes.arrayOf(PropTypes.shape({})),
    handleSelectChange: PropTypes.func
};

BomChangesTab.defaultProps = {
    bomChanges: [],
    handleSelectChange: null
};

export default BomChangesTab;
