import React, { useState } from 'react';
import PropTypes from 'prop-types';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import { LinnWeekPicker } from '@linn-it/linn-form-components-library';
import ChangeState from '../ChangeState';
import BomChangeDetails from '../BomChangeDetails';

function BomChangesTab({ bomChanges, handleSelectChange, phaseInsUri, phaseIn }) {
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

    const [weekStartDate, setWeekStartDate] = useState(null);

    const validPhaseInWeek = weekStart => {
        if (!weekStart) {
            return false;
        }
        const today = new Date();
        const lastSaturday = new Date(
            new Date().setDate(
                today.getDate() - (today.getDay() + 1 === 0 ? 7 : today.getDay() + 2)
            )
        );
        return weekStartDate >= lastSaturday;
    };

    const handleWeekChange = (_, newValue) => {
        setWeekStartDate(newValue);
    };

    return (
        <Grid container spacing={3}>
            {bomChanges ? (
                <Grid item xs={12}>
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
                </Grid>
            ) : (
                <span>No Bom Changes</span>
            )}

            {phaseInsUri && (
                <Grid item xs={12}>
                    <LinnWeekPicker
                        label="From Week Starting"
                        selectedDate={weekStartDate?.toString()}
                        setWeekStartDate={handleWeekChange}
                        propertyName="weekStartDate"
                        required
                    />
                    <Button
                        variant="outlined"
                        onClick={() => phaseIn(weekStartDate)}
                        disabled={!validPhaseInWeek(weekStartDate)}
                    >
                        Phase In
                    </Button>
                </Grid>
            )}
        </Grid>
    );
}

BomChangesTab.propTypes = {
    bomChanges: PropTypes.arrayOf(PropTypes.shape({})),
    phaseInsUri: PropTypes.string,
    handleSelectChange: PropTypes.func,
    phaseIn: PropTypes.func
};

BomChangesTab.defaultProps = {
    bomChanges: [],
    phaseInsUri: null,
    handleSelectChange: null,
    phaseIn: null
};

export default BomChangesTab;
