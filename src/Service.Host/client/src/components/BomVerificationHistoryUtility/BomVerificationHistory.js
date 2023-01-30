import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import {
    InputField,
    itemSelectorHelpers,
    Loading,
    Page,
    SnackbarMessage
} from '@linn-it/linn-form-components-library';

import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import history from '../../history';
import bomVerificationHistoryActions from '../../actions/bomVerificationHistoryActions';

function BomVerificationHistory() {
    const reduxDispatch = useDispatch();
    const { id } = useParams();

    const item = useSelector(reduxState =>
        itemSelectorHelpers.getItem(reduxState.bomVerificationHistory)
    );
    const loading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.bomVerificationHistory)
    );

    const snackbarVisible = useSelector(reduxState =>
        itemSelectorHelpers.getSnackbarVisible(reduxState.bomVerificationHistory)
    );

    const getDateString = isoString => {
        if (!isoString) {
            return null;
        }
        const date = new Date(isoString);
        return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
    };

    useEffect(() => {
        if (id) {
            reduxDispatch(bomVerificationHistoryActions.fetch(id));
        }
    }, [id, reduxDispatch]);

    return (
        <Page history={history}>
            {loading ? (
                <Loading />
            ) : (
                <Grid container>
                    <SnackbarMessage
                        visible={snackbarVisible}
                        onClose={() =>
                            reduxDispatch(bomVerificationHistoryActions.setSnackbarVisible(false))
                        }
                        message="Create Successful"
                        timeOut={3000}
                    />
                    <Grid item xs={12}>
                        <Typography variant="h3">BOM Verification Entry</Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <InputField
                            fullWidth
                            value={item?.partNumber}
                            label="Part Number"
                            disabled
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <InputField fullWidth value={item?.tRef} label="TRef" disabled />
                    </Grid>
                    <Grid item xs={12}>
                        <InputField
                            fullWidth
                            value={getDateString(item?.dateVerified)}
                            label="Date Verified"
                            disabled
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <InputField
                            fullWidth
                            value={item?.verifiedBy}
                            label="Verified By"
                            disabled
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <InputField
                            fullWidth
                            value={item?.remarks}
                            label="Remarks"
                            propertyName="remarks"
                            rows={2}
                            disabled
                        />
                    </Grid>
                </Grid>
            )}
        </Page>
    );
}

export default BomVerificationHistory;
