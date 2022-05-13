/* eslint-disable react/jsx-props-no-spreading */
import React, { useState, useCallback } from 'react';
import { useDropzone } from 'react-dropzone';
import { ErrorCard, SnackbarMessage, Loading } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Chip from '@mui/material/Chip';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

import PropTypes from 'prop-types';

const FileUploader = ({
    title,
    helperText,
    prepareUpload,
    doUpload,
    loading,
    result,
    error,
    snackbarVisible,
    setSnackbarVisible
}) => {
    const [file, setFile] = useState(null);
    const onDrop = useCallback(acceptedFile => {
        setFile(acceptedFile[0]);
    }, []);
    const { getRootProps, getInputProps } = useDropzone({ onDrop });

    const handleUploadClick = () => {
        prepareUpload();
        const reader = new FileReader();
        reader.onload = () => {
            const binaryStr = reader.result;
            doUpload(binaryStr);
        };
        reader.readAsArrayBuffer(file);
        setFile(null);
    };

    return (
        <>
            {error && (
                <Grid item xs={12}>
                    <ErrorCard errorMessage={error.details} />
                </Grid>
            )}
            <SnackbarMessage
                visible={snackbarVisible && result?.success}
                onClose={() => setSnackbarVisible(false)}
                message={result?.message}
            />
            <Accordion>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon />}
                    aria-controls="panel2a-content"
                    id="panel2a-header"
                >
                    <Typography variant="h5">{title}</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <Grid container spacing={3}>
                        <Grid item xs={12}>
                            <Typography variant="subtitle1">{helperText}</Typography>
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
                                {result && !result.success && (
                                    <Grid item xs={12}>
                                        <ErrorCard errorMessage={result.message} />
                                    </Grid>
                                )}
                                <Grid item xs={11} />
                                <Grid item xs={1}>
                                    <Button
                                        id="save-button"
                                        variant="contained"
                                        color="primary"
                                        onClick={handleUploadClick}
                                        disabled={!file}
                                    >
                                        Save
                                    </Button>
                                </Grid>
                            </>
                        )}
                    </Grid>
                </AccordionDetails>
            </Accordion>
        </>
    );
};

FileUploader.propTypes = {
    helperText: PropTypes.string,
    title: PropTypes.string,
    prepareUpload: PropTypes.func,
    doUpload: PropTypes.func.isRequired,
    loading: PropTypes.bool,
    result: PropTypes.shape({ success: PropTypes.bool, message: PropTypes.string }),
    error: PropTypes.shape({ details: PropTypes.string }),
    snackbarVisible: PropTypes.bool,
    setSnackbarVisible: PropTypes.func
};

FileUploader.defaultProps = {
    helperText: 'Upload a File',
    title: 'File Uploader',
    prepareUpload: null,
    loading: false,
    result: null,
    error: null,
    snackbarVisible: false,
    setSnackbarVisible: null
};

export default FileUploader;
