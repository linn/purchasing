import React, { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { InputField, Page } from '@linn-it/linn-form-components-library';
import { makeStyles } from '@mui/styles';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import purchaseOrdersActions from '../../actions/purchaseOrdersActions';
import history from '../../history';
import config from '../../config';

function OverbooksSearch() {
    const useStyles = makeStyles(theme => ({
        button: {
            marginLeft: theme.spacing(1),
            marginTop: theme.spacing(4)
        }
    }));
    const classes = useStyles();
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(purchaseOrdersActions.fetch());
    }, [dispatch]);
    useEffect(() => {
        dispatch(purchaseOrdersActions.fetchState());
    }, [dispatch]);

    const [orderNumber, setOrderNumber] = useState('');

    const search = () =>
        history.push(`/purchasing/purchase-orders/${orderNumber}/allow-over-book/`);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={11}>
                    <Typography variant="h3">Allow Overbook UT</Typography>
                </Grid>
                <Grid item xs={6}>
                    <InputField
                        fullWidth
                        placeholder="Search by Order Number"
                        value={orderNumber}
                        label="Order Number"
                        propertyName="orderNumber"
                        onChange={(_, newValue) => setOrderNumber(newValue)}
                        textFieldProps={{
                            onKeyDown: data => {
                                if (data.keyCode === 13 || data.keyCode === 9) {
                                    search();
                                }
                            }
                        }}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        color="primary"
                        className={classes.button}
                        onClick={search}
                    >
                        Go
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default OverbooksSearch;
