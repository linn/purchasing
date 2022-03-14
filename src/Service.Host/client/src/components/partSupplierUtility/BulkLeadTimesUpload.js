/* eslint-disable react/jsx-props-no-spreading */
import React, { useCallback, useState } from 'react';
import { useDropzone } from 'react-dropzone';
import { Page, Loading, processSelectorHelpers } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Chip from '@mui/material/Chip';
import Button from '@mui/material/Button';
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
        const reader = new FileReader();
        reader.onload = () => {
            // Do whatever you want with the file contents
            const binaryStr = reader.result;
            dispatch(bulkLeadTimesUploadActions.requestProcessStart(binaryStr, null));
        };
        reader.readAsArrayBuffer(file);
    };

    const loading = useSelector(state =>
        processSelectorHelpers.getWorking(state[bulkLeadTimesUpload.item])
    );

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h3">Bulk Lead Times Upload</Typography>
                </Grid>
                {loading ? (
                    <Grid item xs={12}>
                        <Loading />{' '}
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={12}>
                            <Box
                                sx={{ border: '1px dashed grey' }}
                                style={{ cursor: 'pointer' }}
                                {...getRootProps()}
                            >
                                <Typography
                                    style={{ paddingTop: '10px', paddingBottom: '50px' }}
                                    variant="subtitle2"
                                >
                                    Drop the file here or click to browse...
                                </Typography>
                                <input {...getInputProps()} />

                                {file && (
                                    <Chip
                                        label={file.name}
                                        onDelete={() => setFile(null)}
                                        variant="outlined"
                                    />
                                )}
                            </Box>
                        </Grid>
                        <Grid item xs={10} />
                        <Grid item xs={2}>
                            <Button onClick={handleUploadClick} variant="outlined" color="primary">
                                UPLOAD
                            </Button>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default BulkLeadTimesUpload;
