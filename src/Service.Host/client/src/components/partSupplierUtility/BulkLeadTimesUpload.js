/* eslint-disable react/jsx-props-no-spreading */
import React, { useEffect, useState } from 'react';
import {
    Page,
    Loading,
    processSelectorHelpers,
    itemSelectorHelpers,
    ErrorCard,
    getItemError,
    CheckboxWithLabel,
    FileUploader,
    BackButton
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation } from 'react-router';
import queryString from 'query-string';
import bulkLeadTimesUploadActions from '../../actions/bulkLeadTimesUploadActions';
import history from '../../history';
import config from '../../config';
import { bulkLeadTimesUpload, supplier as supplierItemType } from '../../itemTypes';
import supplierActions from '../../actions/supplierActions';

function BulkLeadTimesUpload() {
    const [wholeGroup, setWholeGroup] = useState(false);
    const dispatch = useDispatch();
    const { search } = useLocation();

    useEffect(() => {
        const id = queryString.parse(search)?.supplierId;
        if (id) {
            dispatch(supplierActions.fetch(id));
            dispatch(bulkLeadTimesUploadActions.clearErrorsForItem());
            dispatch(bulkLeadTimesUploadActions.clearProcessData());
        }
    }, [search, dispatch]);

    const supplier = useSelector(state =>
        itemSelectorHelpers.getItem(state[supplierItemType.item])
    );
    const supplierLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[supplierItemType.item])
    );

    const handleUploadClick = data => {
        dispatch(bulkLeadTimesUploadActions.clearErrorsForItem());
        dispatch(bulkLeadTimesUploadActions.clearProcessData());
        dispatch(
            bulkLeadTimesUploadActions.requestProcessStart(data, {
                supplierId: queryString.parse(search).supplierId,
                groupId: wholeGroup ? supplier.groupId : undefined
            })
        );
    };

    const loading = useSelector(state =>
        processSelectorHelpers.getWorking(state[bulkLeadTimesUpload.item])
    );

    const result = useSelector(state =>
        processSelectorHelpers.getData(state[bulkLeadTimesUpload.item])
    );

    const error = useSelector(state => getItemError(state, bulkLeadTimesUpload.item));
    const snackbarVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[bulkLeadTimesUpload.item])
    );
    const setSnackbarVisible = () => dispatch(bulkLeadTimesUploadActions.setMessageVisible(false));
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                {error && (
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={error.details} />
                    </Grid>
                )}
                {supplierLoading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={12}>
                            <BackButton
                                text="Back to Supplier"
                                backClick={() =>
                                    history.push(`${queryString.parse(search).supplierId}`)
                                }
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Typography variant="h6">Updating: {supplier?.name}</Typography>
                        </Grid>
                        {supplier?.groupId && (
                            <Grid item xs={12}>
                                <CheckboxWithLabel
                                    label="Include whole group"
                                    checked={wholeGroup}
                                    onChange={() => setWholeGroup(!wholeGroup)}
                                />
                            </Grid>
                        )}{' '}
                        <Grid item xs={12}>
                            <FileUploader
                                doUpload={handleUploadClick}
                                snackbarVisible={snackbarVisible}
                                setSnackbarVisible={setSnackbarVisible}
                                result={result}
                                loading={loading}
                                title="Bulk Lead Time Changes Uploader"
                                helperText="Upload a CSV file with two columns: Part Number and Lead Time Weeks value"
                            />{' '}
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default BulkLeadTimesUpload;
