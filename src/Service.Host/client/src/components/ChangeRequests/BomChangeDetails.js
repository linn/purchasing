import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { DataGrid } from '@mui/x-data-grid';
import makeStyles from '@mui/styles/makeStyles';
import Popover from '@mui/material/Popover';
import Typography from '@mui/material/Typography';

function BomChangeDetails({ bomChangeDetails }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const [anchorEl, setAnchorEl] = useState(null);

    const handlePopoverOpen = event => {
        setAnchorEl(event.currentTarget);
    };

    const handlePopoverClose = () => {
        setAnchorEl(null);
    };

    const open = Boolean(anchorEl);

    const columns = [
        { field: 'deleteQty', headerName: 'Delete Qty', width: 100 },
        { field: 'deletePartNumber', headerName: 'Delete Part#', width: 160 },
        { field: 'addQty', headerName: 'Add Qty', width: 100 },
        { field: 'addPartNumber', headerName: 'Add Part#', width: 160 }
    ];

    return (
        <>
            {bomChangeDetails && (
                <>
                    <Typography
                        aria-owns={open ? 'mouse-over-popover' : undefined}
                        aria-haspopup="true"
                        onMouseEnter={handlePopoverOpen}
                        onMouseLeave={handlePopoverClose}
                    >
                        {bomChangeDetails.length}
                    </Typography>
                    <Popover
                        id="mouse-over-popover"
                        sx={{
                            pointerEvents: 'none'
                        }}
                        open={open}
                        anchorEl={anchorEl}
                        anchorOrigin={{
                            vertical: 'bottom',
                            horizontal: 'left'
                        }}
                        transformOrigin={{
                            vertical: 'top',
                            horizontal: 'left'
                        }}
                        onClose={handlePopoverClose}
                        disableRestoreFocus
                        PaperProps={{
                            style: { width: '80%' }
                        }}
                    >
                        <DataGrid
                            getRowId={row => row.detailId}
                            className={classes.gap}
                            rows={bomChangeDetails}
                            columns={columns}
                            rowHeight={28}
                            autoHeight
                            loading={false}
                            hideFooter
                        />
                    </Popover>
                </>
            )}
        </>
    );
}

BomChangeDetails.propTypes = {
    bomChangeDetails: PropTypes.arrayOf(PropTypes.shape({}))
};

BomChangeDetails.defaultProps = {
    bomChangeDetails: []
};

export default BomChangeDetails;
