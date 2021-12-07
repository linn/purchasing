import React, { useEffect, useState, useCallback } from 'react';
import Typography from '@material-ui/core/Typography';
import { useSelector, useDispatch } from 'react-redux';
import { Page, SaveBackCancelButtons, Dropdown } from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@material-ui/core/Grid';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import { getItems, getLoading } from '../selectors/CollectionSelectorHelpers';
import history from '../history';

import signingLimitsActions from '../actions/signingLimitsActions';
import employeesActions from '../actions/employeesActions';

function SigningLimits() {
    const [rows, setRows] = useState([]);
    const [editing, setEditing] = useState(false);

    const signingLimits = useSelector(state => getItems(state.signingLimits));
    const signingLimitsLoading = useSelector(state => getLoading(state.signingLimits));
    const employees = useSelector(state => getItems(state.employees));

    const dispatch = useDispatch();
    useEffect(() => dispatch(signingLimitsActions.fetch()), [dispatch]);
    useEffect(() => dispatch(employeesActions.fetch()), [dispatch]);
    useEffect(() => {
        setRows(
            !signingLimits
                ? []
                : signingLimits.map(s => ({ ...s, id: s.userNumber, name: s.user?.fullName }))
        );
    }, [signingLimits]);

    const handleEditRowsModelChange = useCallback(
        model => {
            if (model && Object.keys(model)[0]) {
                setEditing(true);
                const key = parseInt(Object.keys(model)[0], 10);
                const key2 = Object.keys(model[key])[0];
                if (model && model[key] && model[key][key2] && model[key][key2].value) {
                    const newRows = rows.map(r =>
                        r.userNumber === key
                            ? {
                                  ...r,
                                  [key2]: model[key][key2].value,
                                  updated: true
                              }
                            : r
                    );
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
        setRows(
            !signingLimits
                ? []
                : signingLimits.map(s => ({ ...s, id: s.userNumber, name: s.user?.fullName }))
        );
        setEditing(false);
    };

    const handleDropdownChange = (id, field, value) => {
        const newRows = rows.map(r =>
            r.userNumber === id
                ? {
                      ...r,
                      [field]: value,
                      updated: true
                  }
                : r
        );
        setRows(newRows);
    };

    const addNewSigningLimit = (_, newValue) => {
        const newUserNumber = parseInt(newValue, 10);
        if (!rows.find(a => a.id === newUserNumber)) {
            setEditing(true);
            const name = employees.find(a => a.id === newUserNumber).fullName;
            setRows([
                ...rows,
                {
                    id: newUserNumber,
                    userNumber: newUserNumber,
                    name,
                    unlimited: 'N',
                    returnsAuthorisation: 'N',
                    inserting: true
                }
            ]);
        }
    };

    const columns = [
        { field: 'userNumber', headerName: 'User Id', width: 140 },
        { field: 'name', headerName: 'Name', width: 300 },
        { field: 'productionLimit', headerName: 'Production Limit', width: 200, editable: true },
        { field: 'sundryLimit', headerName: 'Sundry Limit', width: 200, editable: true },
        {
            field: 'returnsAuthorisation',
            headerName: 'Returns',
            width: 140,
            editable: true,
            renderEditCell: p => (
                <Card style={{ height: 500, width: '100%' }} variant="outlined">
                    <CardContent>
                        <div style={{ height: 500, width: '100%' }}>
                            <Dropdown
                                label="Labels"
                                propertyName="printLabels"
                                items={[
                                    { id: 'Y', displayText: 'Yes' },
                                    { id: 'N', displayText: 'No' }
                                ]}
                                value={p.row.returnsAuthorisation}
                                onChange={(a, b) => handleDropdownChange(p.id, a, b)}
                                allowNoValue={false}
                            />
                        </div>
                    </CardContent>
                </Card>
            )
        },
        { field: 'unlimited', headerName: 'Unlimited', width: 140, editable: true }
    ];

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={12}>
                    <Typography variant="h6">Signing Limits</Typography>
                    <div style={{ height: 500, width: '100%' }}>
                        <DataGrid
                            rows={rows}
                            columns={columns}
                            density="compact"
                            autoHeight
                            rowHeight={34}
                            loading={signingLimitsLoading}
                            hideFooter
                            onEditRowsModelChange={handleEditRowsModelChange}
                            isCellEditable={() => true}
                        />
                    </div>
                </Grid>
                <Grid item xs={12}>
                    <Dropdown
                        items={employees.map(e => ({
                            displayText: `${e.fullName} (${e.id})`,
                            id: parseInt(e.id, 10)
                        }))}
                        propertyName="add"
                        label="Add new Signing Limit For"
                        onChange={addNewSigningLimit}
                        type="number"
                    />
                </Grid>
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveDisabled={!editing}
                        saveClick={handleSave}
                        cancelClick={handleCancel}
                        backClick={handleCancel}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default SigningLimits;
