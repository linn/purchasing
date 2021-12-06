import React, { useEffect } from 'react';
import Typography from '@material-ui/core/Typography';
import { useSelector, useDispatch } from 'react-redux';
import { Page, SaveBackCancelButtons } from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@material-ui/core/Grid';

import { getItems, getLoading } from '../selectors/CollectionSelectorHelpers';
import signingLimitsActions from '../actions/signingLimitsActions';
import history from '../history';

function SigningLimits() {
    const [rows, setRows] = React.useState([]);
    const [editing, setEditing] = React.useState(false);

    const signingLimits = useSelector(state => getItems(state.signingLimits));
    const signingLimitsLoading = useSelector(state => getLoading(state.signingLimits));
    const dispatch = useDispatch();
    useEffect(() => dispatch(signingLimitsActions.fetch()), [dispatch]);
    useEffect(() => {
        setRows(
            !signingLimits
                ? []
                : signingLimits.map(s => ({ ...s, id: s.userNumber, name: s.user?.fullName }))
        );
    }, [signingLimits]);

    const handleEditRowsModelChange = React.useCallback(
        model => {
            // console.log(JSON.stringify(model));
            // console.log(Object.keys(model)[0]);
            // console.log(JSON.stringify(model[key]));
            // console.log(Object.keys(model[key])[0]);
            // console.log(JSON.stringify(model[key][key2]));
            // console.log(JSON.stringify(model[key][key2].value));
            if (model && Object.keys(model)[0]) {
                setEditing(true);
                const key = parseInt(Object.keys(model)[0], 10);
                const key2 = Object.keys(model[key])[0];
                if (model && model[key] && model[key][key2] && model[key][key2].value) {
                    // console.log(key2);
                    // console.log(model[key][key2].value);
                    const newRows = rows.map(r =>
                        r.userNumber === key
                            ? {
                                  ...r,
                                  [key2]: model[key][key2].value,
                                  updated: true
                              }
                            : r
                    );
                    // const newRows2 = rows.find(a => a.userNumber === key);
                    // const newRows3 = { ...newRows2, updated: true };
                    setRows(newRows);
                }
            }
        },
        [rows, setRows]
    );

    const handleSave = () => {
        rows.forEach(a => {
            setEditing(a === {});
        });

        setEditing(false);
    };

    const handleCancel = () => {
        setEditing(false);
    };

    const handleBack = () => {};

    const columns = [
        { field: 'userNumber', headerName: 'User Id', width: 140 },
        { field: 'name', headerName: 'Name', width: 300 },
        { field: 'productionLimit', headerName: 'Production Limit', width: 200, editable: true },
        { field: 'sundryLimit', headerName: 'Sundry Limit', width: 200, editable: true },
        { field: 'returnsAuthorisation', headerName: 'Returns', width: 140, editable: true },
        { field: 'unlimited', headerName: 'Unlimited', width: 140, editable: true }
    ];

    return (
        <Page history={history}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h6">Signing Limits</Typography>
                    {/* <Typography variant="h6">{JSON.stringify(editRowsModel)}</Typography> */}
                    <div style={{ height: 500, width: '100%' }}>
                        <DataGrid
                            rows={rows}
                            columns={columns}
                            density="compact"
                            autoHeight
                            rowHeight={34}
                            loading={signingLimitsLoading}
                            hideFooter
                            // editRowsModel={editRowsModel}
                            onEditRowsModelChange={handleEditRowsModelChange}
                        />
                    </div>
                </Grid>
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveDisabled={!editing}
                        saveClick={handleSave}
                        cancelClick={handleCancel}
                        backClick={handleBack}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default SigningLimits;
