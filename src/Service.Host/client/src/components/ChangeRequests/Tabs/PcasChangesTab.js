import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import ChangeState from '../ChangeState';

function PcasChangesTab({ pcasChanges, handleSelectChange }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'boardCode', headerName: 'Board Code', width: 140 },
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
                        onSelectionModelChange={handleSelectChange}
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
    pcasChanges: PropTypes.arrayOf(PropTypes.shape({})),
    handleSelectChange: PropTypes.func
};

PcasChangesTab.defaultProps = {
    pcasChanges: [],
    handleSelectChange: null
};

export default PcasChangesTab;
