// import React, { useState, useEffect, useMemo } from 'react';
// import { useSelector, useDispatch } from 'react-redux';
// import { useParams } from 'react-router-dom';

// import {
//     Page as PdfPage,
//     Text,
//     View,
//     Document,
//     StyleSheet,
//     Image,
//     PDFViewer,
//     pdf
// } from '@react-pdf/renderer';
// import {
//     InputField,
//     itemSelectorHelpers,
//     processSelectorHelpers,
//     SnackbarMessage,
//     Loading,
//     Page,
//     ErrorCard
// } from '@linn-it/linn-form-components-library';
// import Grid from '@mui/material/Grid';
// import { purchaseOrderReq, sendPurchaseOrderReqEmail } from '../../itemTypes';
// import purchaseOrderActions from '../../actions/purchaseOrderActions';
// import config from '../../config';
// import history from '../../history';

// function POPrintout() {
//     const dispatch = useDispatch();
//     const item = useSelector(state => itemSelectorHelpers.getItem(state[purchaseOrderReq.item]));
//     const loading = useSelector(state =>
//         itemSelectorHelpers.getItemLoading(state[purchaseOrderReq.item])
//     );

//     const { orderNumber } = useParams();

//     // useEffect(() => {
//     //     if (orderNumber) {
//     //         dispatch(purchaseOrderActions.fetchByPath(orderNumber, '/html'));
//     //     }
//     // }, [id, dispatch]);

//     const snackbarVisible = useSelector(state =>
//         processSelectorHelpers.getMessageVisible(state[sendPurchaseOrderReqEmail.item])
//     );

//     const message = useSelector(state =>
//         processSelectorHelpers.getMessageText(state[sendPurchaseOrderReqEmail.item])
//     );

//     const processResult = useSelector(state =>
//         processSelectorHelpers.getData(state[sendPurchaseOrderReqEmail.item])
//     );

//     const processLoading = useSelector(state =>
//         processSelectorHelpers.getWorking(state[sendPurchaseOrderReqEmail.item])
//     );

//     const PurchaseOrderPdf = useMemo(() => {
//         const styles = StyleSheet.create({
//             page: { padding: 40, fontSize: 11, fontFamily: 'Helvetica' },
//             table: {
//                 width: '100%'
//             },
//             row: {
//                 display: 'flex',
//                 flexDirection: 'row',
//                 paddingTop: 8,
//                 paddingBottom: 8
//             },
//             addressRow: {
//                 display: 'flex',
//                 flexDirection: 'row'
//             },
//             header: {
//                 borderTop: 'none'
//             },
//             bold: {
//                 fontWeight: 'bold'
//             },
//             labelTwoColumns: {
//                 width: '20%',
//                 textAlign: 'right',
//                 paddingRight: '10px',
//                 textDecoration: 'underline'
//             },
//             labelOneColumn: {
//                 width: '10%',
//                 textAlign: 'right',
//                 paddingRight: '10px',
//                 textDecoration: 'underline'
//             },
//             oneColumn: {
//                 width: '10%'
//             },
//             twoColumns: {
//                 width: '20%'
//             },
//             threeColumns: {
//                 width: '30%'
//             },
//             title: {
//                 width: '90%',
//                 fontSize: 18
//             },
//             image: {
//                 width: '10%'
//             },
//             fourColumns: {
//                 width: '40%'
//             },
//             sixColumns: {
//                 width: '60%'
//             },
//             eightColumns: {
//                 width: '80%'
//             }
//         });

//         const print = () => {
//             fetch(`/purchasing/purchase-orders/${orderNumber}/html`)
//                 .then(response => response.json())
//                 .then(
//                     json => {
//                         console.info(json);
//                         const printWindow = window.open('', 'printwindow');
//                         printWindow.document.write(json.body);

//                         printWindow.document.close(); //missing code

//                         printWindow.focus();
//                         printWindow.print();
//                     },
//                     err => {
//                         //handle your error here
//                     }
//                 );
//         };
//         return (
//             <Document>
//                 <PdfPage size="A4" style={styles.page}>
//                     <iframe src={`/purchasing/purchase-orders/${orderNumber}/html`}></iframe>
//                 </PdfPage>
//             </Document>
//         );
//     }, [item]);

//     return (
//         <>
//             <Page history={history} homeUrl={config.appRoot}>
//                 <SnackbarMessage
//                     visible={snackbarVisible && processResult?.success}
//                     onClose={() =>
//                         dispatch(sendPurchaseOrderReqEmailActions.setMessageVisible(false))
//                     }
//                     message={message}
//                 />
//                 {processResult && !processResult.success && (
//                     <Grid style={{ paddingTop: '100px' }} item xs={12}>
//                         <ErrorCard errorMessage={processResult.message} />
//                     </Grid>
//                 )}
//                 {loading || processLoading ? (
//                     <Loading />
//                 ) : (
//                     <Grid container spacing={3}>
//                         <Grid item xs={1} />
//                         <Grid item xs={10}>
//                             <PDFViewer showToolbar width="100%" height="800">
//                                 {PurchaseOrderPdf}
//                             </PDFViewer>
//                         </Grid>
//                     </Grid>
//                 )}
//             </Page>
//         </>
//     );
// }

// export default POPrintout;
