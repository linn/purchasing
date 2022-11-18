import React from 'react';
import PropTypes from 'prop-types';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid';
import { InputField } from '@linn-it/linn-form-components-library';

function LayoutTab({ layouts, selectedLayout, setSelectedLayout, handleFieldChange }) {
    const columns = [{ field: 'layoutCode', headerName: 'Layout', width: 140 }];
    const rows = layouts.map(l => ({ ...l, id: l.layoutCode }));

    return (
        <Grid container spacing={2} style={{ paddingTop: '30px' }}>
            <Grid item xs={2}>
                <div style={{ width: '150px' }}>
                    <DataGrid
                        rows={rows}
                        columns={columns}
                        pageSize={9}
                        selectionModel={selectedLayout}
                        density="compact"
                        autoHeight
                        onSelectionModelChange={newSelectionModel => {
                            setSelectedLayout(newSelectionModel);
                        }}
                        hideFooter={!layouts || layouts.length <= 9}
                    />
                </div>
            </Grid>
            <Grid item xs={10}>
                <Grid container spacing={2}>
                    hello
                </Grid>
            </Grid>
        </Grid>
    );
}

LayoutTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    layouts: PropTypes.arrayOf(PropTypes.shape({})).isRequired,
    selectedLayout: PropTypes.arrayOf(PropTypes.string),
    setSelectedLayout: PropTypes.func.isRequired
};

LayoutTab.defaultProps = {
    selectedLayout: []
};

export default LayoutTab;
