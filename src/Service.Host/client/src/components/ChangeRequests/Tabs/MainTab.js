import React, { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    InputField,
    utilities,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import Button from '@mui/material/Button';
import AssemblyChange from '../ChangeTypes/AssemblyChange';
import ChangeState from '../ChangeState';
import changeRequestActions from '../../../actions/changeRequestActions';

function MainTab({ item, approve }) {
    const reduxDispatch = useDispatch();
    const approveUri = utilities.getHref(item, 'approve');
    const editUri = utilities.getHref(item, 'approve');

    const [updated, setUpdated] = useState(item);

    const handleFieldChange = (propertyName, newValue) => {
        console.log(updated);
        setUpdated(r => ({ ...r, [propertyName]: newValue }));
    };

    return (
        <Grid container spacing={3}>
            <Grid item xs={4}>
                <InputField
                    value={item?.documentNumber}
                    label="Change Request"
                    disabled
                    propertyName="documentNumber"
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    value={item?.proposedBy.fullName}
                    label="Proposed By"
                    propertyName="proposedBy"
                    disabled
                />
            </Grid>
            <Grid item xs={4} />
            <>
                {
                    {
                        PARTEDIT: <AssemblyChange item={item} />,
                        BOARDEDIT: <>Board Edit</>,
                        REPLACE: <>Replace</>
                    }[item?.changeType]
                }
            </>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={updated?.reasonForChange}
                    label="Reason For Change"
                    propertyName="reasonForChange"
                    rows={4}
                    disabled={!editUri}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={6}>
                <InputField
                    fullWidth
                    value={updated?.descriptionOfChange}
                    label="Description Of Change"
                    propertyName="descriptionOfChange"
                    rows={4}
                    disabled={!editUri}
                    onChange={handleFieldChange}
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    value={item?.dateEntered}
                    label="Date Entered"
                    propertyName="dateEntered"
                    type="date"
                    disabled
                />
            </Grid>
            <Grid item xs={4}>
                <InputField
                    value={item?.enteredBy.fullName}
                    label="Entered By"
                    propertyName="enteredBy"
                    disabled
                />
            </Grid>
            <Grid item xs={4}>
                {item?.dateAccepted ? (
                    <InputField
                        value={item?.dateAccepted}
                        label="Date Accepted"
                        propertyName="dateAccepted"
                        type="date"
                        disabled
                    />
                ) : (
                    <Button variant="outlined" disabled={!approveUri} onClick={() => approve(item)}>
                        Approve
                    </Button>
                )}
            </Grid>
            <Grid item xs={4}>
                <ChangeState changeState={item?.changeState} />
            </Grid>
            <Grid item xs={8} />
            {editUri && (
                <>
                    <Grid item xs={8} />
                    <Grid item xs={4}>
                        <SaveBackCancelButtons
                            saveDisabled={!editUri}
                            saveClick={() =>
                                reduxDispatch(
                                    changeRequestActions.update(updated.documentNumber, updated)
                                )
                            }
                            cancelClick={() => {}}
                            backClick={() => {}}
                        />
                    </Grid>
                </>
            )}
        </Grid>
    );
}

MainTab.propTypes = {
    item: PropTypes.shape({
        documentNumber: PropTypes.string,
        dateEntered: PropTypes.string,
        dateAccepted: PropTypes.string,
        changeState: PropTypes.string,
        changeType: PropTypes.string,
        reasonForChange: PropTypes.string,
        descriptionOfChange: PropTypes.string,
        enteredBy: PropTypes.shape({
            id: PropTypes.number,
            fullName: PropTypes.string
        }),
        proposedBy: PropTypes.shape({
            id: PropTypes.number,
            fullName: PropTypes.string
        })
    }),
    approve: PropTypes.func
};

MainTab.defaultProps = {
    item: {
        documentNumber: null,
        dateEntered: null,
        changeState: null,
        reasonForChange: null,
        descriptionOfChange: null
    },
    approve: null
};

export default MainTab;
