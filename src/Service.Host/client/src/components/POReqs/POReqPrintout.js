import React, { Fragment, useEffect, useState, useRef } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';
import { itemSelectorHelpers, Loading, Page } from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
//import { makeStyles } from '@mui/styles';
import Button from '@mui/material/Button';
import { purchaseOrderReq } from '../../itemTypes';
import purchaseOrderReqActions from '../../actions/purchaseOrderReqActions';
import config from '../../config';
import history from '../../history';
import { savePdf } from '../../helpers/pdf';
import logo from '../../assets/linn-logo.png';

function POReqPrintout() {
    // const useStyles = makeStyles(theme => ({
    // }));
    // const classes = useStyles();
    const dispatch = useDispatch();
    const item = useSelector(state => itemSelectorHelpers.getItem(state[purchaseOrderReq.item]));
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[purchaseOrderReq.item])
    );
    const pdfRef = useRef();

    const [pdfLoading, setPdfLoading] = useState(false);

    const { id } = useParams();

    useEffect(() => {
        if (id) {
            dispatch(purchaseOrderReqActions.fetch(id));
        }
    }, [id, dispatch]);

    const Content = () => (
        <Grid container spacing={3}>
            <Grid item xs={2}>
                <img src={logo} alt="linn logo" />
            </Grid>

            <Grid item xs={12}>
                <Typography variant="h4">{`Linn ${item.reqNumber} ${item.partNumber}`}</Typography>
            </Grid>
        </Grid>
    );
    return (
        <>
            <div style={{ width: '80%', minWidth: '1200px', margin: '0 auto' }}>
                <Page history={history} homeUrl={config.appRoot}>
                    <div style={{ width: '874px', margin: '0 auto', padding: '60px' }} ref={pdfRef}>
                        {loading || pdfLoading || !item ? <Loading /> : <Content />}
                    </div>
                </Page>
            </div>
            <Grid container spacing={3}>
                <Grid item xs={8} />

                <Grid item xs={4}>
                    <Button
                        onClick={async () => {
                            setPdfLoading(true);
                            await savePdf(pdfRef);
                            setPdfLoading(false);
                        }}
                        variant="outlined"
                    >
                        generate pdf
                    </Button>
                </Grid>
            </Grid>
        </>
    );
}

export default POReqPrintout;
