import React from 'react';
import PropTypes from 'prop-types';
import Button from '@mui/material/Button';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid';
import { InputField, Dropdown } from '@linn-it/linn-form-components-library';

function RevisionTab({
    layouts,
    selectedLayout,
    selectedRevision,
    dispatch,
    setEditStatus,
    okToSave
}) {
    const columns = [{ field: 'revisionCode', headerName: 'Revision', width: 140 }];
    const rows =
        layouts &&
        selectedLayout?.length &&
        layouts.find(a => a.layoutCode === selectedLayout[0]).revisions
            ? layouts
                  .find(a => a.layoutCode === selectedLayout[0])
                  .revisions.map(l => ({ ...l, id: l.revisionCode }))
            : [];

    const layout =
        layouts && selectedLayout?.length
            ? layouts.find(a => a.layoutCode === selectedLayout[0])
            : null;
    const revision =
        layout && selectedRevision?.length && layout.revisions && layout.revisions.length
            ? layout.revisions.find(a => a.revisionCode === selectedRevision[0])
            : null;
    const handleRevisionChange = (propertyName, newValue) => {
        setEditStatus('edit');
        dispatch({ type: 'updateRevision', fieldName: propertyName, payload: newValue });
    };

    return (
        <Grid container spacing={2} style={{ paddingTop: '30px' }}>
            <Grid item xs={2}>
                <div style={{ width: '150px' }}>
                    {revision && (
                        <>
                            <DataGrid
                                rows={rows}
                                columns={columns}
                                pageSize={10}
                                selectionModel={selectedRevision}
                                density="compact"
                                autoHeight
                                onSelectionModelChange={newSelectionModel => {
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
                                    value={revision.splitBom}
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
                                <InputField
                                    fullWidth
                                    value={revision.pcasPartNumber}
                                    label="Pcas Part Number"
                                    propertyName="pcasPartNumber"
                                    onChange={handleRevisionChange}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <InputField
                                    fullWidth
                                    value={revision.pcsmPartNumber}
                                    label="Pcsm Part Number"
                                    propertyName="pcsmPartNumber"
                                    onChange={handleRevisionChange}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <InputField
                                    fullWidth
                                    value={revision.pcbPartNumber}
                                    label="PCB Part Number"
                                    propertyName="pcbPartNumber"
                                    onChange={handleRevisionChange}
                                />
                            </Grid>
                            <Grid item xs={3} />
                        </>
                    )}
                    <Grid item xs={2}>
                        <Button
                            disabled={!okToSave()}
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
    okToSave: PropTypes.func.isRequired
};

RevisionTab.defaultProps = {
    selectedLayout: [],
    selectedRevision: []
};

export default RevisionTab;
