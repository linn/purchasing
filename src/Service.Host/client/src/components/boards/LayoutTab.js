import React from 'react';
import PropTypes from 'prop-types';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid';
import { InputField, Dropdown, Search } from '@linn-it/linn-form-components-library';

function LayoutTab({
    layouts,
    selectedLayout,
    dispatch,
    setEditStatus,
    okToSave,
    searchParts,
    partsSearchResults,
    partsSearchLoading
}) {
    const columns = [{ field: 'layoutCode', headerName: 'Layout', width: 140 }];
    const rows = layouts ? layouts.map(l => ({ ...l, id: l.layoutCode })) : [];

    const layout =
        layouts && selectedLayout?.length
            ? layouts.find(a => a.layoutCode === selectedLayout[0])
            : null;
    const handleLayoutChange = (propertyName, newValue) => {
        setEditStatus('edit');
        dispatch({ type: 'updateLayout', fieldName: propertyName, payload: newValue });
    };

    return (
        <Grid container spacing={2} style={{ paddingTop: '30px' }}>
            <Grid item xs={2}>
                <div style={{ width: '150px' }}>
                    {layout && (
                        <>
                            <DataGrid
                                rows={rows}
                                columns={columns}
                                pageSize={10}
                                selectionModel={selectedLayout}
                                density="compact"
                                autoHeight
                                onSelectionModelChange={newSelectionModel => {
                                    dispatch({
                                        type: 'setSelectedLayout',
                                        payload: newSelectionModel
                                    });
                                }}
                                hideFooter={!layouts || layouts.length <= 10}
                            />
                        </>
                    )}
                </div>
            </Grid>
            <Grid item xs={10}>
                <Grid container spacing={2}>
                    {layout && (
                        <>
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
                                    onChange={handleLayoutChange}
                                />
                            </Grid>
                            <Grid item xs={2}>
                                <InputField
                                    fullWidth
                                    value={layout.layoutNumber}
                                    label="Layout Number"
                                    disabled={!layout.creating}
                                    propertyName="layoutNumber"
                                    onChange={handleLayoutChange}
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
                                    onChange={handleLayoutChange}
                                />
                            </Grid>
                            <Grid item xs={10} />
                            <Grid item xs={3}>
                                <Search
                                    propertyName="pcbPartNumber"
                                    label="PCB Part Number"
                                    resultsInModal
                                    resultLimit={100}
                                    value={layout.pcbPartNumber}
                                    handleValueChange={handleLayoutChange}
                                    search={searchParts}
                                    searchResults={partsSearchResults}
                                    loading={partsSearchLoading}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={newValue => {
                                        handleLayoutChange('pcbPartNumber', newValue.partNumber);
                                    }}
                                    clearSearch={() => {}}
                                />
                            </Grid>
                            <Grid item xs={9} />
                        </>
                    )}
                    <Grid item xs={2}>
                        <Button
                            disabled={!okToSave()}
                            onClick={() => {
                                setEditStatus('edit');
                                dispatch({ type: 'newLayout', payload: null });
                            }}
                        >
                            New Layout
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
    dispatch: PropTypes.func.isRequired,
    okToSave: PropTypes.func.isRequired,
    partsSearchResults: PropTypes.arrayOf(PropTypes.shape({})),
    partsSearchLoading: PropTypes.bool,
    searchParts: PropTypes.func.isRequired
};

LayoutTab.defaultProps = {
    selectedLayout: [],
    partsSearchResults: [],
    partsSearchLoading: false
};

export default LayoutTab;
