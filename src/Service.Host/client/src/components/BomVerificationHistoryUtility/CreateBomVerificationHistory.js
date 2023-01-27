import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    collectionSelectorHelpers,
    InputField,
    Page,
    Search,
    userSelectors
} from '@linn-it/linn-form-components-library';

import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import employeesActions from '../../actions/employeesActions';
import bomVerificationHistoryActions from '../../actions/bomVerificationHistoryActions';
import history from '../../history';
import partsActions from '../../actions/partsActions';

function CreateBomVerificationHistory() {
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(employeesActions.fetch());
    }, [dispatch]);

    const currentUserId = useSelector(state => userSelectors.getUserNumber(state));
    const currentUserName = useSelector(state => userSelectors.getName(state));

    const [partSearchTerm, setPartSearchTerm] = useState();

    const partsSearchResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.parts)
    ).map?.(c => ({
        id: c.partNumber,
        name: c.partNumber,
        partNumber: c.partNumber,
        description: c.description
    }));

    const partsSearchLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.parts)
    );

    const defaultBomVerificationHistory = {
        tRef: 1,
        partNumber: '',
        verifiedBy: parseInt(currentUserId, 10),
        dateVerified: new Date().toISOString(),
        documentType: '',
        documentNumber: 0,
        remarks: ''
    };

    const [item, setItem] = useState(defaultBomVerificationHistory);

    const handleFieldChange = (propertyName, newValue) => {
        setItem(r => ({ ...r, [propertyName]: newValue }));
    };

    const inputIsValid = () => item?.partNumber && item?.dateVerified && item?.verifiedBy;

    const create = () => {
        dispatch(bomVerificationHistoryActions.add(item));
    };

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={12}>
                    <Typography variant="h3">Create BOM Verification</Typography>
                </Grid>
                <Grid item xs={11}>
                    <Search
                        propertyName="partNumber"
                        label="Part Number"
                        resultsInModal
                        resultLimit={100}
                        value={partSearchTerm}
                        handleValueChange={(_, newVal) => setPartSearchTerm(newVal)}
                        search={partNumber => {
                            dispatch(partsActions.search(partNumber));
                        }}
                        searchResults={partsSearchResults}
                        helperText="Enter a value. Press the enter key if you want to search parts."
                        loading={partsSearchLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={newValue => {
                            setPartSearchTerm(newValue.partNumber);
                            handleFieldChange('partNumber', newValue.partNumber);
                        }}
                        clearSearch={() => {}}
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField fullWidth label="TRef" disabled />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={`${item.dateVerified}`}
                        label="Date Verified"
                        disabled
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={`${currentUserName} (${item.verifiedBy})`}
                        label="Verified By (UC)"
                        disabled
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={`${item.remarks}`}
                        label="Remarks"
                        propertyName="remarks"
                        onChange={handleFieldChange}
                        rows={2}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        color="primary"
                        onClick={create}
                        disabled={!inputIsValid()}
                    >
                        Create
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default CreateBomVerificationHistory;
