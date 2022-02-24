import React, { useState, useCallback } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';

function ContactTab({ contacts, updateContact }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'id', headerName: 'Id', width: 100 },
        { field: 'firstName', headerName: 'First Name', width: 200 },
        { field: 'lastName', headerName: 'Last Name', width: 200 },
        { field: 'phoneNumber', headerName: 'Phone', width: 150 },
        { field: 'emailAddress', headerName: 'Email', width: 150 },
        {
            field: 'isMainOrderContact',
            headerName: 'Main Order Contact',
            width: 150,
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true
        },
        {
            field: 'isMainInvoiceContact',
            headerName: 'Main Invoice Contact',
            width: 150,
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true
        }
    ];
    const [editRowsModel, setEditRowsModel] = useState({});

    const handleEditRowsModelChange = useCallback(
        model => {
            setEditRowsModel(model);

            if (model && Object.keys(model)[0]) {
                const id = parseInt(Object.keys(model)[0], 10);
                const propertyName = Object.keys(model[id])[0];
                if (
                    model &&
                    model[id] &&
                    model[id][propertyName] &&
                    model[id][propertyName].value
                ) {
                    const newValue = model[id][propertyName].value;
                    updateContact(id, propertyName, newValue);
                }
            }
        },
        [updateContact]
    );
    return (
        <Grid container spacing={3}>
            <div style={{ height: 500, width: '100%' }}>
                <DataGrid
                    className={classes.gap}
                    rows={contacts}
                    columns={columns}
                    rowHeight={34}
                    autoHeight
                    loading={false}
                    hideFooter
                    editRowsModel={editRowsModel}
                    onEditRowsModelChange={handleEditRowsModelChange}
                />
            </div>
        </Grid>
    );
}

ContactTab.propTypes = {
    contacts: PropTypes.arrayOf(PropTypes.shape({})),
    updateContact: PropTypes.func.isRequired
};

ContactTab.defaultProps = {
    contacts: null
};

export default ContactTab;
