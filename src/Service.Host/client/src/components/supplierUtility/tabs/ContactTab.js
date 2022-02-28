import React, { useState, useCallback } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import { Button } from '@mui/material';

function ContactTab({ contacts, updateContact, addContact }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'id', headerName: 'Id', width: 100 },
        { field: 'supplierId', headerName: 'Id', width: 100, hide: true },

        { field: 'firstName', headerName: 'First Name', width: 200, editable: true },
        { field: 'lastName', headerName: 'Last Name', width: 200, editable: true },
        { field: 'jobTitle', headerName: 'Job Title', width: 200, editable: true },
        { field: 'phoneNumber', headerName: 'Phone', width: 150, editable: true },
        { field: 'emailAddress', headerName: 'Email', width: 150, editable: true },
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
        },
        { field: 'comments', headerName: 'Comments', width: 250, editable: true },

        { field: 'personId', headerName: 'Id', width: 100 }
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
            <Grid item xs={12}>
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
            </Grid>
            <Grid item xs={3}>
                <Button onClick={addContact}>Add</Button>
            </Grid>
        </Grid>
    );
}

ContactTab.propTypes = {
    contacts: PropTypes.arrayOf(PropTypes.shape({})),
    updateContact: PropTypes.func.isRequired,
    addContact: PropTypes.func.isRequired
};

ContactTab.defaultProps = {
    contacts: null
};

export default ContactTab;
