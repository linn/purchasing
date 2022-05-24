import React, { useState, useEffect, useMemo } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';
import {
    Page as PdfPage,
    Text,
    View,
    Document,
    StyleSheet,
    Image,
    PDFViewer,
    pdf
} from '@react-pdf/renderer';
import {
    InputField,
    itemSelectorHelpers,
    processSelectorHelpers,
    SnackbarMessage,
    Loading,
    Page,
    ErrorCard
} from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';

import { purchaseOrder, sendPurchaseOrderReqEmail } from '../../itemTypes';
import purchaseOrderActions from '../../actions/purchaseOrderActions';
import config from '../../config';
import history from '../../history';
import logo from '../../assets/linn-logo.png';
import sendPurchaseOrderReqEmailActions from '../../actions/sendPurchaseOrderReqEmailActions';

function PurchaseOrderPrintout() {
    const dispatch = useDispatch();
    const item = useSelector(state => itemSelectorHelpers.getItem(state[purchaseOrder.item]));
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[purchaseOrder.item])
    );

    const { orderNumber } = useParams();

    useEffect(() => {
        if (orderNumber) {
            dispatch(purchaseOrderActions.fetch(orderNumber));
        }
    }, [orderNumber, dispatch]);

    const [email, setEmail] = useState('');

    const snackbarVisible = useSelector(state =>
        processSelectorHelpers.getMessageVisible(state[sendPurchaseOrderReqEmail.item])
    );

    const message = useSelector(state =>
        processSelectorHelpers.getMessageText(state[sendPurchaseOrderReqEmail.item])
    );

    const processResult = useSelector(state =>
        processSelectorHelpers.getData(state[sendPurchaseOrderReqEmail.item])
    );

    const processLoading = useSelector(state =>
        processSelectorHelpers.getWorking(state[sendPurchaseOrderReqEmail.item])
    );

    const PurchaseOrderPdf = useMemo(() => {
        const styles = StyleSheet.create({
            page: { padding: 40, fontSize: 11, fontFamily: 'Helvetica' },
            table: {
                width: '100%'
            },
            row: {
                display: 'flex',
                flexDirection: 'row',
                paddingTop: 8,
                paddingBottom: 8
            },
            addressRow: {
                display: 'flex',
                flexDirection: 'row'
            },
            header: {
                borderTop: 'none'
            },
            bold: {
                fontWeight: 'bold'
            },
            labelTwoColumns: {
                width: '20%',
                textAlign: 'right',
                paddingRight: '10px',
                textDecoration: 'underline'
            },
            labelOneColumn: {
                width: '10%',
                textAlign: 'right',
                paddingRight: '10px',
                textDecoration: 'underline'
            },
            oneColumn: {
                width: '10%'
            },
            twoColumns: {
                width: '20%'
            },
            threeColumns: {
                width: '30%'
            },
            title: {
                width: '90%',
                fontSize: 18
            },
            image: {
                width: '10%'
            },
            fourColumns: {
                width: '40%'
            },
            sixColumns: {
                width: '60%'
            },
            eightColumns: {
                width: '80%'
            }
        });
        return (
            <Document>
                {item && (
                    <PdfPage size="A4" style={styles.page}>
                        <View style={styles.table}>
                            <View style={[styles.row, styles.bold, styles.header]}>
                                <Image style={styles.image} src={logo} />
                                <Text style={styles.title}>
                                    <b>Purchase Order - Supplier Copy</b>
                                </Text>
                            </View>
                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Order Number:</Text>
                                <Text style={styles.twoColumns}>{item.orderNumber}</Text>
                                <Text style={styles.sixColumns}>
                                    Date: {item.orderDate?.toDateString()}
                                </Text>
                            </View>
                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Deliver To:</Text>
                                <Text style={styles.eightColumns}>
                                    {item.deliveryAddress.address}
                                </Text>
                            </View>
                            {/* <View style={[styles.addressRow]}>
                                <View style={styles.twoColumns} />
                                <Text style={styles.eightColumns}>{item.addressLine2}</Text>
                            </View>
                            <View style={[styles.addressRow]}>
                                <View style={styles.twoColumns} />
                                <Text style={styles.eightColumns}>{item.addressLine3}</Text>
                            </View>
                            <View style={[styles.addressRow]}>
                                <View style={styles.twoColumns} />
                                <Text style={styles.eightColumns}>{item.addressLine4}</Text>
                            </View>
                            <View style={[styles.addressRow]}>
                                <View style={styles.twoColumns} />
                                <Text style={styles.eightColumns}>{item.country?.countryName}</Text>
                            </View> */}

                            {/* <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Order Number:</Text>
                                <Text style={styles.twoColumns}>{item.reqNumber}</Text>
                                <Text style={styles.sixColumns}>
                                    {new Date(item.reqDate).toDateString()}
                                </Text>
                            </View>
                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Req State:</Text>
                                <Text style={styles.twoColumns}>{item.state}</Text>
                                <Text style={styles.sixColumns}>{item.stateDescription}</Text>
                            </View>
                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Part Number:</Text>
                                <Text style={styles.twoColumns}>{item.partNumber}</Text>
                                <Text style={styles.sixColumns}>{item.description}</Text>
                            </View>
                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Quantity:</Text>
                                <Text style={styles.twoColumns}>{item.qty}</Text>
                                <View style={styles.sixColumns} />
                            </View>
                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Unit Price:</Text>
                                <Text style={styles.oneColumn}>{item.unitPrice?.toFixed(2)}</Text>
                                <Text style={styles.labelTwoColumns}>Carriage:</Text>
                                <Text style={styles.oneColumn}>{item.carriage?.toFixed(2)}</Text>
                                <Text style={styles.labelTwoColumns}>Total Price:</Text>
                                <Text style={styles.oneColumn}>
                                    {item.totalReqPrice?.toFixed(2)}
                                </Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Currency:</Text>
                                <Text style={styles.twoColumns}>{item.currency?.name}</Text>
                                <View style={styles.sixColumns} />
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Supplier:</Text>
                                <Text style={styles.twoColumns}>{item.supplier?.id}</Text>
                                <Text style={styles.sixColumns}>{item.supplier?.name}</Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Address:</Text>
                                <Text style={styles.eightColumns}>{item.addressLine1}</Text>
                            </View>
                            <View style={[styles.addressRow]}>
                                <View style={styles.twoColumns} />
                                <Text style={styles.eightColumns}>{item.addressLine2}</Text>
                            </View>
                            <View style={[styles.addressRow]}>
                                <View style={styles.twoColumns} />
                                <Text style={styles.eightColumns}>{item.addressLine3}</Text>
                            </View>
                            <View style={[styles.addressRow]}>
                                <View style={styles.twoColumns} />
                                <Text style={styles.eightColumns}>{item.addressLine4}</Text>
                            </View>
                            <View style={[styles.addressRow]}>
                                <View style={styles.twoColumns} />
                                <Text style={styles.eightColumns}>{item.country?.countryName}</Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Contact:</Text>
                                <Text style={styles.eightColumns}>{item.supplierContact}</Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Phone:</Text>
                                <Text style={styles.eightColumns}>{item.phoneNumber}</Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Cost Centre:</Text>
                                <Text style={styles.twoColumns}>
                                    {item.department?.departmentCode}
                                </Text>
                                <Text style={styles.sixColumns}>
                                    {item.department?.description}
                                </Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Nominal:</Text>
                                <Text style={styles.twoColumns}>{item.nominal?.nominalCode}</Text>
                                <Text style={styles.sixColumns}>{item.nominal?.description}</Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Quote Ref:</Text>
                                <Text style={styles.eightColumns}>{item.quoteRef}</Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Requested By:</Text>
                                <Text style={styles.eightColumns}>
                                    {item.requestedBy?.fullName}
                                </Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Authorised By:</Text>
                                <Text style={styles.eightColumns}>
                                    {item.authorisedBy?.fullName}
                                </Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>2nd Auth By:</Text>
                                <Text style={styles.eightColumns}>
                                    {item.secondAuthBy?.fullName}
                                </Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Finance Check By:</Text>
                                <Text style={styles.eightColumns}>
                                    {item.financeCheckBy?.fullName}
                                </Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Order By:</Text>
                                <Text style={styles.eightColumns}>
                                    {item.turnedIntoOrderBy?.fullName}
                                </Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Order Number:</Text>
                                <Text style={styles.eightColumns}>{item.orderNumber}</Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Order Notes:</Text>
                                <Text style={styles.eightColumns}>{item.remarksForOrder}</Text>
                            </View>

                            <View style={[styles.row]}>
                                <Text style={styles.labelTwoColumns}>Internal Notes:</Text>
                                <Text style={styles.eightColumns}>{item.internalNotes}</Text>
                            </View> */}
                        </View>
                    </PdfPage>
                )}
            </Document>
        );
    }, [item]);

    return (
        <>
            <Page history={history} homeUrl={config.appRoot}>
                <SnackbarMessage
                    visible={snackbarVisible && processResult?.success}
                    onClose={() =>
                        dispatch(sendPurchaseOrderReqEmailActions.setMessageVisible(false))
                    }
                    message={message}
                />
                {processResult && !processResult.success && (
                    <Grid style={{ paddingTop: '100px' }} item xs={12}>
                        <ErrorCard errorMessage={processResult.message} />
                    </Grid>
                )}
                {loading || processLoading ? (
                    <Loading />
                ) : (
                    <Grid container spacing={3}>
                        <Grid item xs={1} />
                        <Grid item xs={10}>
                            <PDFViewer showToolbar width="100%" height="800">
                                {PurchaseOrderPdf}
                            </PDFViewer>
                        </Grid>
                        <Grid item xs={1} />
                        <Grid item xs={3}>
                            <InputField
                                propertyName="emailAddress"
                                label="Enter an email address"
                                value={email}
                                onChange={(_, newValue) => setEmail(newValue)}
                                fullWidth
                            />
                        </Grid>
                        <Grid item xs={9} />

                        <Grid item xs={3}>
                            <Button
                                variant="contained"
                                color="primary"
                                disabled={!email}
                                onClick={async () => {
                                    dispatch(sendPurchaseOrderReqEmailActions.clearProcessData());

                                    const blob = await pdf(PurchaseOrderPdf).toBlob();
                                    dispatch(
                                        sendPurchaseOrderReqEmailActions.requestProcessStart(blob, {
                                            orderNumber,
                                            toEmailAddress: email
                                        })
                                    );
                                }}
                            >
                                send email
                            </Button>
                        </Grid>
                    </Grid>
                )}
            </Page>
        </>
    );
}

export default PurchaseOrderPrintout;
