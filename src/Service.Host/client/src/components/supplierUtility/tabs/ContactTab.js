import React, { useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { makeStyles } from '@mui/styles';
import { Button } from '@mui/material';

function ContactTab({ contacts, updateContact, addContact, deleteContacts }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'supplierId', headerName: 'Id', width: 100, hide: true },

        { field: 'firstName', headerName: 'First Name', width: 200, editable: true },
        { field: 'lastName', headerName: 'Last Name', width: 200, editable: true },
        { field: 'jobTitle', headerName: 'Job Title', width: 200, editable: true, hide: true },
        { field: 'phoneNumber', headerName: 'Phone', width: 150, editable: true, hide: true },
        { field: 'emailAddress', headerName: 'Email', width: 400, editable: true },
        {
            field: 'ccList',
            headerName: 'CC list (comma separated if multiple!)',
            width: 400,
            editable: true
        },
        {
            field: 'isMainOrderContact',
            headerName: 'Main Order',
            width: 100,
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true
        },
        {
            field: 'isMainInvoiceContact',
            headerName: 'Main Invoice',
            width: 100,
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true
        },
        { field: 'comments', headerName: 'Comments', width: 250, editable: true },
        { field: 'personId', headerName: 'Id', width: 100, hide: true }
    ];

    const [selectedRows, setSelectedRows] = useState([]);

    return (
        <Grid container spacing={3}>
            <Grid item xs={12}>
                <DataGrid
                    className={classes.gap}
                    rows={contacts}
                    columns={columns}
                    checkboxSelection
                    disableRowSelectionOnClick
                    onRowSelectionModelChange={selected => {
                        setSelectedRows(selected);
                    }}
                    rowHeight={34}
                    autoHeight
                    loading={false}
                    hideFooter
                    processRowUpdate={newRow => {
                        updateContact(newRow);
                        return newRow;
                    }}
                />
            </Grid>
            <Grid item xs={3}>
                <Button onClick={addContact}>Add</Button>
                <Button
                    color="secondary"
                    onClick={() => deleteContacts(selectedRows)}
                    disabled={!selectedRows?.length}
                >
                    Delete Selected
                </Button>
            </Grid>
        </Grid>
    );
}

ContactTab.propTypes = {
    contacts: PropTypes.arrayOf(PropTypes.shape({})),
    updateContact: PropTypes.func.isRequired,
    addContact: PropTypes.func.isRequired,
    deleteContacts: PropTypes.func.isRequired
};

ContactTab.defaultProps = {
    contacts: null
};

export default ContactTab;
