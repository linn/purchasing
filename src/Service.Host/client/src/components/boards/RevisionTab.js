import React from 'react';
import PropTypes from 'prop-types';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid';
import { InputField, Dropdown, Search } from '@linn-it/linn-form-components-library';

function RevisionTab({
    layouts,
    selectedLayout,
    selectedRevision,
    dispatch,
    setEditStatus,
    okToSave,
    searchParts,
    partsSearchResults,
    partsSearchLoading,
    editingAllowed,
    revision
}) {
    const columns = [{ field: 'revisionCode', headerName: 'Revision', width: 175 }];
    const rows =
        layouts &&
        selectedLayout?.length &&
        layouts.find(a => a.layoutCode === selectedLayout[0]).revisions
            ? layouts
                  .find(a => a.layoutCode === selectedLayout[0])
                  .revisions.map(l => ({ ...l, id: l.revisionCode }))
            : [];
    const handleRevisionChange = (propertyName, newValue) => {
        if (editingAllowed) {
            setEditStatus('edit');
            dispatch({ type: 'updateRevision', fieldName: propertyName, payload: newValue });
        }
    };

    return (
        <Grid container spacing={2} style={{ paddingTop: '30px' }}>
            <Grid item xs={2}>
                <div style={{ width: '180px' }}>
                    {revision && (
                        <>
                            <DataGrid
                                rows={rows}
                                columns={columns}
                                pageSize={10}
                                selectionModel={selectedRevision}
                                density="compact"
                                hideFooterSelectedRowCount
                                autoHeight
                                onRowSelectionModelChange={newSelectionModel => {
                                    dispatch({
                                        type: 'setSelectedRevision',
                                        payload: newSelectionModel
                                    });
                                }}
                                hideFooter={rows.length <= 10}
                            />
                        </>
                    )}
                </div>
            </Grid>
            <Grid item xs={10}>
                <Grid container spacing={2}>
                    {revision && (
                        <>
                            <Grid item xs={2}>
                                <InputField
                                    fullWidth
                                    value={revision.revisionNumber}
                                    label="Revision Number"
                                    disabled={!revision.creating}
                                    propertyName="revisionNumber"
                                    onChange={handleRevisionChange}
                                />
                            </Grid>
                            <Grid item xs={2}>
                                <InputField
                                    fullWidth
                                    value={revision.revisionCode}
                                    label="Revision Code"
                                    disabled={!revision.creating}
                                    propertyName="revisionCode"
                                    onChange={handleRevisionChange}
                                />
                            </Grid>
                            <Grid item xs={8} />
                            <Grid item xs={3}>
                                <Dropdown
                                    value={revision.splitBom ?? 'Y'}
                                    label="Split Bom"
                                    propertyName="splitBom"
                                    items={[
                                        { id: 'N', displayText: 'No' },
                                        { id: 'Y', displayText: 'Yes' }
                                    ]}
                                    allowNoValue={false}
                                    onChange={handleRevisionChange}
                                />
                            </Grid>
                            <Grid item xs={9} />
                            <Grid item xs={3}>
                                <Search
                                    propertyName="pcasPartNumber"
                                    label="Pcas Part Number"
                                    resultsInModal
                                    resultLimit={100}
                                    value={revision.pcasPartNumber}
                                    handleValueChange={handleRevisionChange}
                                    search={searchParts}
                                    searchResults={partsSearchResults}
                                    loading={partsSearchLoading}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={newValue => {
                                        handleRevisionChange('pcasPartNumber', newValue.partNumber);
                                    }}
                                    clearSearch={() => {}}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <Search
                                    propertyName="pcsmPartNumber"
                                    label="Pcsm Part Number"
                                    resultsInModal
                                    resultLimit={100}
                                    value={revision.pcsmPartNumber}
                                    handleValueChange={handleRevisionChange}
                                    search={searchParts}
                                    searchResults={partsSearchResults}
                                    loading={partsSearchLoading}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={newValue => {
                                        handleRevisionChange('pcsmPartNumber', newValue.partNumber);
                                    }}
                                    clearSearch={() => {}}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <Search
                                    propertyName="pcbPartNumber"
                                    label="PCB Part Number"
                                    resultsInModal
                                    resultLimit={100}
                                    value={revision.pcbPartNumber}
                                    handleValueChange={handleRevisionChange}
                                    search={searchParts}
                                    searchResults={partsSearchResults}
                                    loading={partsSearchLoading}
                                    priorityFunction="closestMatchesFirst"
                                    onResultSelect={newValue => {
                                        handleRevisionChange('pcbPartNumber', newValue.partNumber);
                                    }}
                                    clearSearch={() => {}}
                                />
                            </Grid>
                            <Grid item xs={3} />
                        </>
                    )}
                    <Grid item xs={2}>
                        <Button
                            disabled={!okToSave() || !editingAllowed}
                            onClick={() => {
                                setEditStatus('edit');
                                dispatch({ type: 'newRevision', payload: null });
                            }}
                        >
                            New Revision
                        </Button>
                    </Grid>
                    <Grid item xs={10} />
                </Grid>
            </Grid>
        </Grid>
    );
}

RevisionTab.propTypes = {
    layouts: PropTypes.arrayOf(PropTypes.shape({})).isRequired,
    selectedLayout: PropTypes.arrayOf(PropTypes.string),
    selectedRevision: PropTypes.arrayOf(PropTypes.string),
    setEditStatus: PropTypes.func.isRequired,
    dispatch: PropTypes.func.isRequired,
    okToSave: PropTypes.func.isRequired,
    partsSearchResults: PropTypes.arrayOf(PropTypes.shape({})),
    partsSearchLoading: PropTypes.bool,
    searchParts: PropTypes.func.isRequired,
    editingAllowed: PropTypes.string,
    revision: PropTypes.shape({
        pcbPartNumber: PropTypes.string,
        pcasPartNumber: PropTypes.string,
        creating: PropTypes.bool,
        pcsmPartNumber: PropTypes.string,
        revisionCode: PropTypes.string,
        splitBom: PropTypes.string,
        revisionNumber: PropTypes.string
    })
};

RevisionTab.defaultProps = {
    selectedLayout: [],
    selectedRevision: [],
    partsSearchResults: [],
    partsSearchLoading: false,
    editingAllowed: null,
    revision: null
};

export default RevisionTab;
