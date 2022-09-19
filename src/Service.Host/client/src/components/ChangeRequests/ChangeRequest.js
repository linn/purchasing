import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import {
    Page,
    InputField,
    Loading,
    itemSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import changeRequestActions from '../../actions/changeRequestActions';
import BomChanges from './BomChanges';

import history from '../../history';

function ChangeRequest() {
    const { id } = useParams();

    const reduxDispatch = useDispatch();
    useEffect(() => {
        if (id) {
            reduxDispatch(changeRequestActions.fetch(id));
        }
    }, [id, reduxDispatch]);

    const loading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.changeRequest)
    );

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.changeRequest));

    return (
        <Page history={history}>
            {loading ? (
                <Loading />
            ) : (
                <Grid container spacing={2} justifyContent="center">
                    <Grid item xs={12}>
                        <Typography variant="h6">Change Request</Typography>
                    </Grid>
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
                            value={item?.dateEntered}
                            label="Date Entered"
                            propertyName="dateEntered"
                            type="date"
                            disabled
                        />
                    </Grid>
                    <Grid item xs={4}>
                        <InputField
                            value={item?.changeState}
                            label="Change State"
                            propertyName="changeState"
                            disabled
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            fullWidth
                            value={item?.reasonForChange}
                            label="Reason For Change"
                            propertyName="reasonForChange"
                            rows={4}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            fullWidth
                            value={item?.descriptionOfChange}
                            label="Description Of Change"
                            propertyName="descriptionOfChange"
                            rows={4}
                        />
                    </Grid>
                    <BomChanges bomChanges={item?.bomChanges} />
                </Grid>
            )}
        </Page>
    );
}

export default ChangeRequest;
