import React from 'react';
import PropTypes from 'prop-types';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid';
import { InputField, Dropdown } from '@linn-it/linn-form-components-library';

function LayoutTab({ layouts, selectedLayout, dispatch, setEditStatus }) {
    const columns = [{ field: 'layoutCode', headerName: 'Layout', width: 140 }];
    const rows = layouts.map(l => ({ ...l, id: l.layoutCode }));

    const layout =
        layouts && selectedLayout.length > 0
            ? layouts.find(a => a.layoutCode === selectedLayout[0])
            : {};
    const handleLayoutChange = (propertyName, newValue) => {
        setEditStatus('edit');
        dispatch({ type: 'updateLayout', fieldName: propertyName, payload: newValue });
    };

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
                            dispatch({ type: 'setSelectedLayout', payload: newSelectionModel });
                        }}
                        hideFooter={!layouts || layouts.length <= 9}
                    />
                </div>
            </Grid>
            <Grid item xs={10}>
                <Grid container spacing={2}>
                    <Grid item xs={2}>
                        <Dropdown
                            value={layout.layoutType}
                            label="Layout Type"
                            disabled={!layout.creating}
                            propertyName="layoutType"
                            items={[
                                { id: 'L', displayText: 'L - Production' },
                                { id: 'P', displayText: 'P - Prototype' }
                            ]}
                            allowNoValue={false}
                            onChange={() => {}}
                        />
                    </Grid>
                    <Grid item xs={2}>
                        <InputField
                            fullWidth
                            value={layout.layoutNumber}
                            label="Layout Number"
                            disabled={!layout.creating}
                            propertyName="layoutNumber"
                            onChange={() => {}}
                        />
                    </Grid>
                    <Grid item xs={8} />
                    <Grid item xs={2}>
                        <InputField
                            fullWidth
                            value={layout.layoutCode}
                            label="Layout Code"
                            disabled={!layout.creating}
                            propertyName="layoutCode"
                            onChange={() => {}}
                        />
                    </Grid>
                    <Grid item xs={10} />
                    <Grid item xs={3}>
                        <InputField
                            fullWidth
                            value={layout.pcbPartNumber}
                            label="PCB Part Number"
                            propertyName="pcbPartNumber"
                            onChange={handleLayoutChange}
                        />
                    </Grid>
                    <Grid item xs={9} />
                    <Grid item xs={2}>
                        <Button
                            onClick={() => {
                                dispatch({ type: 'newLayout', payload: null });
                            }}
                        >
                            {!layout.creating ? 'New Layout' : 'Done'}
                        </Button>
                    </Grid>
                    <Grid item xs={10} />
                </Grid>
            </Grid>
        </Grid>
    );
}

LayoutTab.propTypes = {
    layouts: PropTypes.arrayOf(PropTypes.shape({})).isRequired,
    selectedLayout: PropTypes.arrayOf(PropTypes.string),
    setEditStatus: PropTypes.func.isRequired,
    dispatch: PropTypes.func.isRequired
};

LayoutTab.defaultProps = {
    selectedLayout: []
};

export default LayoutTab;
