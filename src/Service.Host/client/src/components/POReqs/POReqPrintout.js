import React, { Fragment, useEffect, useState, useRef } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';
import { itemSelectorHelpers, Loading, Page } from '@linn-it/linn-form-components-library';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Divider from '@mui/material/Divider';

import { purchaseOrderReq } from '../../itemTypes';
import purchaseOrderReqActions from '../../actions/purchaseOrderReqActions';
import config from '../../config';
import history from '../../history';
import { savePdf } from '../../helpers/pdf';
import logo from '../../assets/linn-logo.png';

function POReqPrintout() {
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
            <Grid item xs={10} />
            <Grid item xs={2}>
                <img src={logo} alt="linn logo" />
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Req Number:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.reqNumber}</Typography>
            </Grid>
            <Grid item xs={8}>
                <Typography variant="subtitle1">
                    {new Date(item.reqDate).toLocaleDateString()}
                </Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Req State:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.state}</Typography>
            </Grid>
            <Grid item xs={8}>
                <Typography variant="subtitle1">{item.stateDescription}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Part Number:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.partNumber}</Typography>
            </Grid>
            <Grid item xs={7}>
                <Typography variant="subtitle1">{item.description}</Typography>
            </Grid>

            <Grid item xs={12}>
                <Divider light />
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Quantity:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.qty}</Typography>
            </Grid>
            <Grid item xs={8} />

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Unit Price:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.unitPrice?.toFixed(2)}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Carriage:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.carriage?.toFixed(2)}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Total Price:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.totalReqPrice?.toFixed(2)}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Currency:
                </Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography variant="subtitle1">{item.currency?.name}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Date Required:
                </Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography variant="subtitle1">
                    {item.dateRequired ? new Date(item.dateRequired).toLocaleDateString() : ''}
                </Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Supplier:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.supplier?.id}</Typography>
            </Grid>
            <Grid item xs={8}>
                <Typography variant="subtitle1">{item.supplier?.name}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Address:
                </Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography variant="subtitle1">{item.addressLine1}</Typography>
                <Typography variant="subtitle1">{item.addressLine2}</Typography>
                <Typography variant="subtitle1">{item.addressLine3}</Typography>
                <Typography variant="subtitle1">{item.addressLine4}</Typography>
                <Typography variant="subtitle1">{item.postCode}</Typography>
                <Typography variant="subtitle1">{item.country?.countryName}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Contact:
                </Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography variant="subtitle1">{item.supplierContact}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Phone Number:
                </Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography variant="subtitle1">{item.phoneNumber}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Cost Centre:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.department.departmentCode}</Typography>
            </Grid>
            <Grid item xs={8}>
                <Typography variant="subtitle1">{item.department.description}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Nominal:
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography variant="subtitle1">{item.nominal.nominalCode}</Typography>
            </Grid>
            <Grid item xs={8}>
                <Typography variant="subtitle1">{item.nominal.description}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Quote Ref:
                </Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography variant="subtitle1">{item.quoteRef}</Typography>
            </Grid>

            <Grid item xs={12}>
                <Divider light />
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Requested By:
                </Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.requestedBy?.fullName}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Authorised By:
                </Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.authorisedBy?.fullName}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    2nd Auth By:
                </Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.secondAuthBy?.fullName}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Finance Check By:
                </Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.financeCheckBy?.fullName}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Order By:
                </Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.turnedIntoOrderBy?.fullName}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Order Number:
                </Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.orderNumber}</Typography>
            </Grid>

            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Order Notes:
                </Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.remarksForOrder}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography align="right" variant="subtitle2">
                    Internal Notes:
                </Typography>
            </Grid>
            <Grid item xs={4}>
                <Typography variant="subtitle1">{item.internalNotes}</Typography>
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
