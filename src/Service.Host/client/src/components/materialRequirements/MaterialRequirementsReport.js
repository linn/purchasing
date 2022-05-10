import React, { useCallback, useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Page, itemSelectorHelpers, Loading } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Snackbar from '@mui/material/Snackbar';
import Typography from '@mui/material/Typography';
import { useLocation } from 'react-router';

import { mrReport as mrReportItem } from '../../itemTypes';
import mrReportActions from '../../actions/mrReportActions';

import history from '../../history';

function MaterialRequirementsReport() {
    const [showMessage, setShowMessage] = useState(false);
    const [message, setMessage] = useState(null);
    const [selectedItem, setSelectedItem] = useState(null);
    const [nextPart, setNextPart] = useState(null);
    const [previousPart, setPreviousPart] = useState(null);

    const options = useLocation();

    const mrReport = useSelector(state => itemSelectorHelpers.getItem(state.mrReport));
    const mrReportLoading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state.mrReport)
    );

    const displayMessage = useCallback(
        newMessage => {
            setMessage(newMessage);
            setShowMessage(true);
        },
        [setShowMessage]
    );

    const handleClose = () => {
        setShowMessage(false);
        setMessage(null);
    };

    const dispatch = useDispatch();

    useEffect(() => {
        if (options && options.state) {
            dispatch(mrReportActions.postByHref(mrReportItem.uri, options.state));
        } else {
            history.push('/purchasing/material-requirements');
        }
    }, [dispatch, options]);

    useEffect(() => {
        if (mrReport && mrReport.results && mrReport.results.length > 0) {
            setSelectedItem(mrReport.results[0]);
            if (mrReport.results.length > 1) {
                setNextPart(mrReport.results[1].partNumber);
            } else {
                setNextPart(null);
            }
            setPreviousPart(null);
        } else {
            setSelectedItem(null);
            setNextPart(null);
            setPreviousPart(null);
        }
    }, [mrReport]);

    return (
        <Page history={history}>
            <>
                {mrReportLoading && <Loading />}
                {selectedItem && (
                    <Grid container>
                        <Grid item xs={12}>
                            ok
                        </Grid>
                        <Grid item xs={10} />
                        <Grid item xs={2}>
                            <Typography variant="subtitle1">
                                Jobref: {selectedItem.jobRef}
                            </Typography>
                        </Grid>
                    </Grid>
                )}
            </>
            <Snackbar
                open={showMessage}
                autoHideDuration={3000}
                onClose={handleClose}
                message={message}
            />
        </Page>
    );
}

export default MaterialRequirementsReport;
