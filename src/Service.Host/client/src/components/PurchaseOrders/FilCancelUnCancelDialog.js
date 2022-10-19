import React, { useState } from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Close from '@mui/icons-material/Close';
import Dialog from '@mui/material/Dialog';
import Button from '@mui/material/Button';
import { InputField } from '@linn-it/linn-form-components-library';
import { useDispatch } from 'react-redux';
import { makeStyles } from '@mui/styles';
import purchaseOrderActions from '../../actions/purchaseOrderActions';

function FilCancelUnCancelDialog({ open, setOpen, mode, order: orderNumber, line }) {
    const useStyles = makeStyles(theme => ({
        pullRight: {
            float: 'right'
        },
        pad: { padding: theme.spacing(4) }
    }));

    const classes = useStyles();

    const [reason, setReason] = useState('');
    const dispatch = useDispatch();

    return (
        <Dialog open={open} fullWidth maxWidth="md">
            <div className={classes.pad}>
                <IconButton
                    className={classes.pullRight}
                    aria-label="Close"
                    onClick={() => setOpen(false)}
                >
                    <Close />
                </IconButton>
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <Typography variant="h6">
                            {mode === 'cancel' ? 'Fil Cancel' : 'Un-Fil Cancel'} order line {line}?
                        </Typography>
                    </Grid>
                    {mode === 'cancel' && (
                        <Grid item xs={12}>
                            <InputField
                                fullWidth
                                value={reason}
                                label="Enter a reason"
                                number
                                propertyName="reason"
                                onChange={(_, newValue) => {
                                    setReason(newValue);
                                }}
                            />
                        </Grid>
                    )}
                    <Grid item xs={12}>
                        <Button
                            className={classes.buttonMarginTop}
                            aria-label="confirm"
                            variant="contained"
                            disabled={mode === 'cancel' && !reason}
                            onClick={() => {
                                const from = { orderNumber };
                                purchaseOrderActions.clearErrorsForItem();
                                if (mode === 'cancel') {
                                    dispatch(
                                        purchaseOrderActions.patch(orderNumber, {
                                            from,
                                            to: {
                                                orderNumber,
                                                details: [
                                                    {
                                                        line,
                                                        filCancelled: 'Y',
                                                        reasonFilCancelled: reason
                                                    }
                                                ]
                                            }
                                        })
                                    );
                                } else if (mode === 'uncancel') {
                                    dispatch(
                                        purchaseOrderActions.patch(orderNumber, {
                                            from,
                                            to: {
                                                orderNumber,
                                                details: [
                                                    {
                                                        line,
                                                        filCancelled: 'N'
                                                    }
                                                ]
                                            }
                                        })
                                    );
                                }
                                setOpen(false);
                            }}
                        >
                            CONFIRM
                        </Button>
                    </Grid>
                </Grid>
            </div>
        </Dialog>
    );
}

FilCancelUnCancelDialog.propTypes = {
    open: PropTypes.bool.isRequired,
    setOpen: PropTypes.func.isRequired,
    mode: PropTypes.string.isRequired,
    order: PropTypes.number.isRequired,
    line: PropTypes.number.isRequired
};

export default FilCancelUnCancelDialog;
