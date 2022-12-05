import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    collectionSelectorHelpers,
    Dropdown,
    InputField,
    itemSelectorHelpers,
    userSelectors
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import AssemblyChange from './ChangeTypes/AssemblyChange';
import employeesActions from '../../actions/employeesActions';
import changeRequestActions from '../../actions/changeRequestActions';
import history from '../../history';

function CreateChangeRequest() {
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(employeesActions.fetch());
    }, [dispatch]);

    const currentUserId = useSelector(state => userSelectors.getUserNumber(state));
    const currentUserName = useSelector(state => userSelectors.getName(state));

    const defaultChangeRequest = {
        enteredBy: {
            id: parseInt(currentUserId, 10),
            fullName: currentUserName
        },
        proposedBy: {
            id: parseInt(currentUserId, 10),
            fullName: currentUserName
        },
        changeState: 'PROPOS',
        changeType: 'PARTEDIT',
        reasonForChange: '',
        descriptionOfChange: ''
    };

    const [item, setItem] = useState(defaultChangeRequest);
    const employees = useSelector(state => collectionSelectorHelpers.getItems(state.employees));

    const handleFieldChange = (propertyName, newValue) => {
        if (propertyName === 'proposedBy') {
            const newProposed = employees.find(e => e.id === parseInt(newValue, 10));
            setItem(r => ({
                ...r,
                proposedBy: { id: newProposed.id, fullName: newProposed.fullName }
            }));
        } else {
            setItem(r => ({ ...r, [propertyName]: newValue }));
        }
    };

    const create = () => {
        dispatch(changeRequestActions.add(item));
    };

    const createdItem = useSelector(reduxState =>
        itemSelectorHelpers.getItem(reduxState.changeRequest)
    );

    useEffect(() => {
        if (createdItem?.documentNumber) {
            history.push(`/purchasing/change-requests/${createdItem?.documentNumber}/`);
        }
    }, [createdItem]);

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={12}>
                    <Typography variant="h3">Raise Change Request</Typography>
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={`${item.enteredBy?.fullName} (${item.enteredBy?.id})`}
                        label="Entered By"
                        disabled
                    />
                </Grid>
                <Grid item xs={8}>
                    <Dropdown
                        fullWidth
                        value={item.proposedBy?.id}
                        label="Proposed By"
                        items={employees.map(e => ({
                            displayText: `${e.fullName} (${e.id})`,
                            id: parseInt(e.id, 10)
                        }))}
                        propertyName="proposedBy"
                        onChange={handleFieldChange}
                    />
                </Grid>
                <Grid item xs={8}>
                    <Dropdown
                        fullWidth
                        value={item.changeType}
                        label="Change Type"
                        items={[
                            { id: 'PARTEDIT', displayText: 'Assembly (Not Board) Change' },
                            { id: 'BOARDEDIT', displayText: 'Board Change' },
                            { id: 'REPLACE', displayText: 'Replace' }
                        ]}
                        propertyName="changeType"
                        onChange={handleFieldChange}
                        allowNoValue={false}
                    />
                </Grid>
                <Grid item xs={4} />
                <>
                    {
                        {
                            PARTEDIT: (
                                <AssemblyChange
                                    item={item}
                                    creating
                                    handleFieldChange={handleFieldChange}
                                />
                            ),
                            BOARDEDIT: <Typography>Coming Soon...</Typography>,
                            REPLACE: <Typography>Coming Soon...</Typography>
                        }[item?.changeType]
                    }
                </>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={item.reasonForChange}
                        label="Reason For Change"
                        propertyName="reasonForChange"
                        onChange={handleFieldChange}
                        rows={2}
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={item.descriptionOfChange}
                        label="Description Of Change"
                        propertyName="descriptionOfChange"
                        onChange={handleFieldChange}
                        rows={2}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button variant="outlined" color="primary" onClick={create}>
                        Create
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default CreateChangeRequest;
