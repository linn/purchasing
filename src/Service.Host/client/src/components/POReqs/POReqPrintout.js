import React, { Fragment, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';

import {
    Page as PdfPage,
    Text,
    View,
    Document,
    StyleSheet,
    Image,
    PDFViewer
} from '@react-pdf/renderer';
import { itemSelectorHelpers, Loading, Page } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';

import { purchaseOrderReq } from '../../itemTypes';
import purchaseOrderReqActions from '../../actions/purchaseOrderReqActions';
import config from '../../config';
import history from '../../history';
import logo from '../../assets/linn-logo.png';

function POReqPrintout() {
    const dispatch = useDispatch();
    const item = useSelector(state => itemSelectorHelpers.getItem(state[purchaseOrderReq.item]));
    const loading = useSelector(state =>
        itemSelectorHelpers.getItemLoading(state[purchaseOrderReq.item])
    );

    const { id } = useParams();

    useEffect(() => {
        if (id) {
            dispatch(purchaseOrderReqActions.fetch(id));
        }
    }, [id, dispatch]);

    const styles = StyleSheet.create({
        page: { padding: 40, fontSize: 11, fontFamily: 'Helvetica' },
        table: {
            width: '100%'
        },
        row: {
            display: 'flex',
            flexDirection: 'row',
            //borderTop: '1px solid #EEE',
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
            width: '10%',
            textAlign: 'left'
        },
        twoColumns: {
            width: '20%',
            textAlign: 'left'
        },
        threeColumns: {
            width: '30%',
            textAlign: 'left'
        },
        title: {
            width: '90%',
            fontSize: 18
        },
        image: {
            width: '10%'
        },
        sixColumns: {
            width: '60%',
            textAlign: 'left'
        },
        eightColumns: {
            width: '80%',
            textAlign: 'left'
        }
    });

    // Create Document Component
    const MyDocument = () => (
        <Document>
            <PdfPage size="A4" style={styles.page}>
                <View style={styles.table}>
                    <View style={[styles.row, styles.bold, styles.header]}>
                        <Text style={styles.title}>Purchase Order Requisition Details</Text>
                        <Image style={styles.image} src={logo} />
                    </View>
                    <View style={[styles.row]}>
                        <Text style={styles.labelTwoColumns}>Req Number:</Text>
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
                        <Text style={styles.oneColumn}>{item.totalReqPrice?.toFixed(2)}</Text>
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
                </View>
            </PdfPage>
        </Document>
    );

    return (
        <>
            <Page history={history} homeUrl={config.appRoot}>
                {loading || !item ? (
                    <Loading />
                ) : (
                    <Grid container spacing={3}>
                        <Grid item xs={1} />
                        <Grid item xs={10}>
                            <PDFViewer showToolbar width="100%" height="800">
                                <MyDocument />
                            </PDFViewer>
                        </Grid>
                        <Grid item xs={1} />
                    </Grid>
                )}
            </Page>
        </>
    );
}

export default POReqPrintout;
