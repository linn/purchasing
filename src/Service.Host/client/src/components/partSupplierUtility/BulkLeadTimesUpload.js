/* eslint-disable react/jsx-props-no-spreading */
import React, { useCallback, useState } from 'react';
import { useDropzone } from 'react-dropzone';
import {
    Page,
    Loading,
    processSelectorHelpers,
    SaveBackCancelButtons,
    SnackbarMessage,
    ErrorCard
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Chip from '@mui/material/Chip';
import { useSelector, useDispatch } from 'react-redux';

import bulkLeadTimesUploadActions from '../../actions/bulkLeadTimesUploadActions';
import history from '../../history';
import config from '../../config';
import { bulkLeadTimesUpload } from '../../itemTypes';

function BulkLeadTimesUpload() {
    const [file, setFile] = useState(null);
    const onDrop = useCallback(acceptedFile => {
        setFile(acceptedFile[0]);
    }, []);
    const { getRootProps, getInputProps } = useDropzone({ onDrop });

    const dispatch = useDispatch();

    const handleUploadClick = () => {
        dispatch(bulkLeadTimesUploadActions.clearProcessData());
        const reader = new FileReader();
        reader.onload = () => {
            const binaryStr = reader.result;
            dispatch(bulkLeadTimesUploadActions.requestProcessStart(binaryStr, null));
        };
        reader.readAsArrayBuffer(file);
    };

    const loading = useSelector(state =>
        processSelectorHelpers.getWorking(state[bulkLeadTimesUpload.item])
    );

    const result = useSelector(state =>
        processSelectorHelpers.getData(state[bulkLeadTimesUpload.item])
    );

    const message = useSelector(state =>
        processSelectorHelpers.getMessageText(state[bulkLeadTimesUpload.item])
    );

    const snackbarVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[bulkLeadTimesUpload.item])
    );
    const setSnackbarVisible = () => dispatch(bulkLeadTimesUploadActions.setMessageVisible(false));

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <SnackbarMessage
                visible={snackbarVisible && result?.success}
                onClose={() => setSnackbarVisible(false)}
                message={message}
            />
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h3">Bulk Lead Time Changes Uploader</Typography>
                </Grid>
                <Grid item xs={12}>
                    <Typography variant="subtitle1">
                        Upload a CSV file with two columns: Part Number and Lead Time Weeks value
                    </Typography>
                </Grid>
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={12}>
                            <Box
                                sx={{ border: '1px dashed grey', margin: '10px' }}
                                style={{ cursor: 'pointer' }}
                                {...getRootProps()}
                            >
                                <Typography
                                    style={{ paddingTop: '10px', paddingBottom: '20px' }}
                                    variant="subtitle2"
                                >
                                    Drop the file here or click to browse...
                                </Typography>
                                <input {...getInputProps()} />

                                {file && (
                                    <Chip
                                        label={file.name}
                                        color="primary"
                                        onDelete={() => setFile(null)}
                                        variant="outlined"
                                    />
                                )}
                            </Box>
                        </Grid>
                        <Grid item xs={12}>
                            <SaveBackCancelButtons
                                backClick={() => history.push('/purchasing/part-suppliers')}
                                cancelClick={() => setFile(null)}
                                saveDisabled={!file}
                                saveClick={handleUploadClick}
                            />
                        </Grid>
                        {result && !result.success && (
                            <Grid item xs={12}>
                                <ErrorCard errorMessage={result.message} />
                            </Grid>
                        )}
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default BulkLeadTimesUpload;
