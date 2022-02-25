import React, { useState, useCallback } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import { useDispatch, useSelector } from 'react-redux';
import { makeStyles } from '@mui/styles';
import { collectionSelectorHelpers, TypeaheadTable } from '@linn-it/linn-form-components-library';
import contactsActions from '../../../actions/contactsActions';

function ContactTab({ contacts, updateContact, addContact }) {
    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        }
    }));
    const classes = useStyles();

    const columns = [
        { field: 'id', headerName: 'Id', width: 100 },
        { field: 'firstName', headerName: 'First Name', width: 200, editable: true },
        { field: 'lastName', headerName: 'Last Name', width: 200, editable: true },
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

    const contactsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.contacts)
    );
    const contactsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.contacts)
    );

    const dispatch = useDispatch();

    const searchContacts = searchTerm => dispatch(contactsActions.search(searchTerm));

    const contactsTable = {
        totalItemCount: contactsSearchResults.length,
        rows: contactsSearchResults?.map((item, i) => ({
            id: item.contactId,
            data: item,
            values: [
                { id: `${i}-0`, value: `${item.contactId}` },
                { id: `${i}-1`, value: `${item.firstName || ''}` },
                { id: `${i}-2`, value: `${item.lastName || ''}` },
                { id: `${i}-2`, value: `${item.phoneNumber || ''}` },
                { id: `${i}-2`, value: `${item.emailAddress || ''}` },
                {
                    id: `${i}-2`,
                    value: `${
                        item.dateCreated ? new Date(item.dateCreated).toLocaleDateString() : ''
                    }`
                }
            ],
            links: item.links
        }))
    };

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

            <Grid item xs={12}>
                <TypeaheadTable
                    table={contactsTable}
                    columnNames={[
                        'Id',
                        'First Name',
                        'Last Name',
                        'Phone',
                        'Email',
                        'Date Created'
                    ]}
                    fetchItems={searchContacts}
                    modal
                    placeholder="Search by Id or Name"
                    links={false}
                    clearSearch={() => dispatch(contactsActions.clearSearch())}
                    loading={contactsSearchLoading}
                    label="Look up an existing contact"
                    title="Search Contacts"
                    value=""
                    onSelect={newValue =>
                        addContact(
                            newValue.data.contactId,
                            newValue.data.firstName,
                            newValue.data.lastName,
                            newValue.data.phoneNumber,
                            newValue.data.emailAddress,
                            'N',
                            'N',
                            newValue.data.personId
                        )
                    }
                    debounce={1000}
                    minimumSearchTermLength={2}
                />
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
