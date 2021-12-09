import React, { useEffect, useState, useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Page, SaveBackCancelButtons, Dropdown } from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
import Grid from '@mui/material/Grid';
import Paper from '@mui/material/Paper';
import Popper from '@mui/material/Popper';
import PropTypes from 'prop-types';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import Typography from '@mui/material/Typography';
import { makeStyles } from '@mui/styles';

import { getItems, getLoading } from '../selectors/CollectionSelectorHelpers';
import history from '../history';

import signingLimitActions from '../actions/signingLimitActions';
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

    const useStyles = makeStyles(theme => ({
        formControl: {
            margin: theme.spacing(1),
            minWidth: 120
        },
        selectEmpty: {
            marginTop: theme.spacing(2)
        },
        editing: {
            backgroundColor: 'linen'
        },
        inserting: {
            backgroundColor: 'whiteSmoke'
        },
        gap: {
            marginBottom: '20px'
        }
    }));

    const classes = useStyles();
    const updateRow = useCallback(
        (rowId, fieldName, newValue) => {
            let saveValue = newValue;
            if (fieldName === 'productionLimit' || fieldName === 'sundryLimit') {
                saveValue = parseInt(newValue, 10);
            }
            const newRows = rows.map(r =>
                r.userNumber === rowId
                    ? {
                          ...r,
                          [fieldName]: saveValue,
                          updating: true
                      }
                    : r
            );
            setRows(newRows);
        },
        [rows]
    );

    const EditTextarea = props => {
        const { id, value, field, colDef, api } = props;
        const [anchorEl, setAnchorEl] = useState(null);

        const handleRef = el => {
            setAnchorEl(el);
        };
        const onClose = () => {
            api.setCellMode(id, field, 'view');
        };

        const handleDropdownChange = (rowId, fieldName, newValue) => {
            const saveValue = newValue === 'Yes' ? 'Y' : 'N';
            updateRow(rowId, fieldName, saveValue);
            api.setEditCellValue({ id: rowId, field: fieldName, value: saveValue });
            api.setCellMode(rowId, fieldName, 'view');
        };

        return (
            <div>
                <div
                    ref={handleRef}
                    style={{
                        height: 1,
                        width: colDef.computedWidth,
                        display: 'block',
                        position: 'absolute'
                    }}
                />
                {anchorEl && (
                    <Popper open anchorEl={anchorEl} placement="top-start">
                        <Paper elevation={1} sx={{ p: 1, minWidth: colDef.computedWidth }}>
                            <FormControl variant="filled" className={classes.formControl}>
                                <InputLabel id="demo-simple-select-filled-label">
                                    Select Values
                                </InputLabel>
                                <Select
                                    id={field}
                                    value={value}
                                    onChange={e => handleDropdownChange(id, field, e.target.value)}
                                    onClose={onClose}
                                >
                                    <MenuItem value="Yes">Yes</MenuItem>
                                    <MenuItem value="No">No</MenuItem>
                                </Select>
                            </FormControl>
                        </Paper>
                    </Popper>
                )}
            </div>
        );
    };

    EditTextarea.propTypes = {
        id: PropTypes.number.isRequired,
        value: PropTypes.string.isRequired,
        field: PropTypes.string.isRequired,
        colDef: PropTypes.shape({ computedWidth: PropTypes.number }).isRequired,
        api: PropTypes.shape({ setEditCellValue: PropTypes.func, setCellMode: PropTypes.func })
            .isRequired
    };

    const renderEditTextarea = params => (
        <EditTextarea
            id={params.id}
            field={params.field}
            value={params.value}
            colDef={params.colDef}
            api={params.api}
        />
    );

    const handleEditRowsModelChange = useCallback(
        model => {
            if (model && Object.keys(model)[0]) {
                setEditing(true);
                const key = parseInt(Object.keys(model)[0], 10);
                const key2 = Object.keys(model[key])[0];
                if (model && model[key] && model[key][key2] && model[key][key2].value) {
                    updateRow(key, key2, model[key][key2].value);
                }
            }
        },
        [updateRow]
    );

    const handleSave = () => {
        rows.forEach(a => {
            if (a.inserting) {
                dispatch(signingLimitActions.add(a));
            } else if (a.updating) {
                dispatch(signingLimitActions.update(a.id, a));
            }
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
                    inserting: true,
                    productionLimit: 0,
                    sundryLimit: 0
                }
            ]);
        }
    };

    const getYesNo = params => (params.row[params.field] === 'Y' ? 'Yes' : 'No');
    const getBackgroundColourClass = params => {
        if (params.row.inserting) {
            return classes.inserting;
        }

        if (params.row.updating) {
            return classes.editing;
        }

        return '';
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
            renderEditCell: renderEditTextarea,
            valueGetter: params => getYesNo(params)
        },
        {
            field: 'unlimited',
            headerName: 'Unlimited',
            width: 140,
            editable: true,
            renderEditCell: renderEditTextarea,
            valueGetter: params => getYesNo(params)
        }
    ];

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={8} />
                <Grid item xs={4}>
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
                    <Typography variant="h6">Signing Limits</Typography>
                    <div style={{ height: 500, width: '100%' }}>
                        <DataGrid
                            className={classes.gap}
                            rows={rows}
                            columns={columns}
                            density="compact"
                            rowHeight={34}
                            autoHeight
                            loading={signingLimitsLoading}
                            hideFooter
                            onEditRowsModelChange={handleEditRowsModelChange}
                            getRowClassName={getBackgroundColourClass}
                        />
                        <SaveBackCancelButtons
                            saveDisabled={!editing}
                            saveClick={handleSave}
                            cancelClick={handleCancel}
                            backClick={handleCancel}
                        />
                    </div>
                </Grid>
            </Grid>
        </Page>
    );
}

export default SigningLimits;
