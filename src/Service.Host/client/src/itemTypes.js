import { ItemType } from '@linn-it/linn-form-components-library';

export const signingLimit = new ItemType(
    'signingLimit',
    'SIGNING_LIMIT',
    '/purchasing/signing-limits'
);
export const signingLimits = new ItemType(
    'signingLimits',
    'SIGNING_LIMITS',
    '/purchasing/signing-limits'
);

export const partSuppliers = new ItemType(
    'partSuppliers',
    'PART_SUPPLIERS',
    '/purchasing/part-suppliers'
);

export const partSupplier = new ItemType(
    'partSupplier',
    'PART_SUPPLIER',
    '/purchasing/part-suppliers/record'
);

export const supplier = new ItemType('supplier', 'SUPPLIER', '/purchasing/suppliers');

export const suppliers = new ItemType('suppliers', 'SUPPLIERS', '/purchasing/suppliers');

export const employees = new ItemType('employees', 'EMPLOYEES', '/inventory/employees');

export const currentEmployees = new ItemType(
    'currentEmployees',
    'CURRENT_EMPLOYEES',
    '/employees?currentEmployees=true&sortBy=name'
);

export const parts = new ItemType('parts', 'PARTS', '/parts');

export const part = new ItemType('part', 'PART', '/parts/manufacturer-data');

export const purchaseOrder = new ItemType(
    'purchaseOrder',
    'PURCHASE_ORDER',
    '/purchasing/purchase-orders'
);

export const purchaseOrders = new ItemType(
    'purchaseOrders',
    'PURCHASE_ORDERS',
    '/purchasing/purchase-orders'
);

export const currencies = new ItemType(
    'currencies',
    'CURRENCIES',
    '/purchasing/purchase-orders/currencies'
);

export const orderMethods = new ItemType(
    'orderMethods',
    'ORDER_METHODS',
    '/purchasing/purchase-orders/methods'
);

export const deliveryAddresses = new ItemType(
    'deliveryAddresses',
    'DELIVERY_ADDRESSES',
    '/purchasing/purchase-orders/delivery-addresses'
);

export const unitsOfMeasure = new ItemType(
    'unitsOfMeasure',
    'UNITS_OF_MEASURE',
    '/purchasing/purchase-orders/units-of-measure'
);

export const tariffs = new ItemType('tariffs', 'TARIFFS', '/purchasing/purchase-orders/tariffs');

export const packagingGroups = new ItemType(
    'packagingGroups',
    'PACKAGING_GROUPS',
    '/purchasing/purchase-orders/packaging-groups'
);

export const manufacturers = new ItemType(
    'manufacturers',
    'MANUFACTURERS',
    '/purchasing/manufacturers'
);

export const preferredSupplierChange = new ItemType(
    'preferredSupplierChange',
    'PREFERRED_SUPPLIER_CHANGE',
    '/purchasing/preferred-supplier-changes'
);

export const priceChangeReasons = new ItemType(
    'priceChangeReasons',
    'PRICE_CHANGE_REASONS',
    '/purchasing/price-change-reasons'
);

export const partPriceConversions = new ItemType(
    'partPriceConversions',
    'PART_PRICE_CONVERSIONS',
    '/purchasing/part-suppliers/part-price-conversions'
);

export const partCategories = new ItemType(
    'partCategories',
    'PART_CATEGORIES',
    '/purchasing/part-categories'
);

export const accountingCompanies = new ItemType(
    'accountingCompanies',
    'ACCOUNTING_COMPANIES',
    '/inventory/accounting-companies'
);

export const putSupplierOnHold = new ItemType(
    'putSupplierOnHold',
    'PUT_SUPPLIER_ON_HOLD',
    '/purchasing/suppliers/hold'
);

export const address = new ItemType('address', 'ADDRESS', '/purchasing/addresses');

export const addresses = new ItemType('addresses', 'ADDRESSES', '/purchasing/addresses');

export const countries = new ItemType('countries', 'COUNTRIES', '/purchasing/countries');

export const vendorManagers = new ItemType(
    'vendorManagers',
    'VENDOR_MANAGERS',
    '/purchasing/vendor-managers'
);

export const vendorManager = new ItemType(
    'vendorManager',
    'VENDOR_MANAGER',
    '/purchasing/vendor-managers'
);

export const planners = new ItemType('planners', 'PLANNERS', '/purchasing/suppliers/planners');

export const contacts = new ItemType('contacts', 'CONTACTS', '/purchasing/suppliers/contacts');

export const plCreditDebitNote = new ItemType(
    'plCreditDebitNote',
    'PL_CREDIT_DEBIT_NOTE',
    '/purchasing/pl-credit-debit-notes'
);

export const plCreditDebitNotes = new ItemType(
    'plCreditDebitNotes',
    'PL_CREDIT_DEBIT_NOTES',
    '/purchasing/pl-credit-debit-notes'
);

export const openDebitNotes = new ItemType(
    'plCreditDebitNotes',
    'PL_CREDIT_DEBIT_NOTES',
    '/purchasing/open-debit-notes'
);

export const sendPlNoteEmail = new ItemType(
    'sendPlNoteEmail',
    'SEND_PL_NOTE_EMAIL',
    '/purchasing/pl-credit-debit-notes/email/'
);

export const sendPurchaseOrderReqEmail = new ItemType(
    'sendPurchaseOrderReqEmail',
    'SEND_PURCHASE_ORDER_REQ_EMAIL',
    '/purchasing/purchase-orders/reqs/email'
);

export const bulkLeadTimesUpload = new ItemType(
    'bulkLeadTimesUpload',
    'BULK_LEAD_TIMES_UPLOAD',
    '/purchasing/suppliers/bulk-lead-times/'
);

export const supplierGroups = new ItemType(
    'supplierGroups',
    'SUPPLIER_GROUPS',
    '/purchasing/supplier-groups/'
);

export const tqmsJobrefs = new ItemType('tqmsJobrefs', 'TQMS_JOBREFS', '/purchasing/tqms-jobrefs/');
export const purchaseOrderReq = new ItemType(
    'purchaseOrderReq',
    'PURCHASE_ORDER_REQ',
    '/purchasing/purchase-orders/reqs'
);

export const departments = new ItemType('departments', 'DEPARTMENTS', '/ledgers/departments');

export const nominals = new ItemType('nominals', 'NOMINALS', '/ledgers/nominals');

export const nominalAccounts = new ItemType(
    'nominalAccounts',
    'NOMINAL_ACCOUNTS',
    '/ledgers/nominal-accounts'
);

export const purchaseOrderReqs = new ItemType(
    'purchaseOrderReqs',
    'PURCHASE_ORDER_REQS',
    '/purchasing/purchase-orders/reqs'
);

export const purchaseOrderReqStates = new ItemType(
    'purchaseOrderReqStates',
    'PURCHASE_ORDER_REQ_STATES',
    '/purchasing/purchase-orders/reqs/states'
);

export const sendPurchaseOrderReqAuthEmail = new ItemType(
    'sendPurchaseOrderReqAuthEmail',
    'SEND_PURCHASE_ORDER_REQ_AUTH_EMAIL',
    '/purchasing/purchase-orders/reqs/email-for-authorisation'
);

export const sendPurchaseOrderReqFinanceEmail = new ItemType(
    'sendPurchaseOrderReqFinanceEmail',
    'SEND_PURCHASE_ORDER_REQ_FINANCE_EMAIL',
    '/purchasing/purchase-orders/reqs/email-for-finance'
);

export const pOReqCheckIfCanAuthOrder = new ItemType(
    'pOReqCheckIfCanAuthOrder',
    'CHECK_IF_CAN_AUTH_ORDER',
    '/purchasing/purchase-orders/reqs/check-signing-limit-covers-po-auth'
);

export const mrpRunLog = new ItemType(
    'mrpRunLog',
    'MRP_RUN_LOG',
    '/purchasing/material-requirements/run-logs'
);

export const runMrp = new ItemType(
    'runMrp',
    'RUN_MRP',
    '/purchasing/material-requirements/run-mrp'
);

export const mrMaster = new ItemType(
    'mrMaster',
    'MR_MASTER',
    '/purchasing/material-requirements/last-run'
);

export const applyForecastingPercentageChange = new ItemType(
    'applyForecastingPercentageChange',
    'APPLY_FORECASTING_PERCENTAGE_CHANGE',
    '/purchasing/forecasting/apply-percentage-change'
);

export const ediOrders = new ItemType('ediOrders', 'EDI_ORDERS', '/purchasing/edi/orders');

export const sendEdiEmail = new ItemType(
    'sendEdiEmail',
    'SEND_EDI_EMAIL',
    '/purchasing/edi/orders'
);

export const mrReport = new ItemType('mrReport', 'MR_REPORT', '/purchasing/material-requirements');
export const mrReportOptions = new ItemType(
    'mrReportOptions',
    'MR_REPORT_OPTIONS',
    '/purchasing/material-requirements/options'
);
export const mrReportOrders = new ItemType(
    'mrReportOrders',
    'MR_REPORT_ORDERS',
    '/purchasing/material-requirements/orders'
);

export const purchaseOrderDelivery = new ItemType(
    'purchaseOrderDelivery',
    'PURCHASE_ORDER_DELIVERY',
    '/purchasing/purchase-orders/deliveries'
);

export const purchaseOrderDeliveries = new ItemType(
    'purchaseOrderDeliveries',
    'PURCHASE_ORDER_DELIVERIES',
    '/purchasing/purchase-orders/deliveries'
);

export const batchPurchaseOrderDeliveriesUpload = new ItemType(
    'batchPurchaseOrderDeliveriesUpload',
    'BATCH_PURCHASE_ORDER_DELIVERIES_UPLOAD',
    '/purchasing/purchase-orders/deliveries'
);

export const ediSuppliers = new ItemType(
    'ediSuppliers',
    'EDI_SUPPLIERS',
    '/purchasing/edi/suppliers'
);

export const sendPurchaseOrderPdfEmail = new ItemType(
    'sendPurchaseOrderPdfEmail',
    'SEND_PURCHASE_ORDER_PDF_EMAIL',
    '/purchasing/purchase-orders/email-pdf'
);

export const automaticPurchaseOrderSuggestions = new ItemType(
    'automaticPurchaseOrderSuggestions',
    'AUTOMATIC_PL_ORDER_SUGGESTIONS',
    '/purchasing/automatic-purchase-order-suggestions'
);

export const automaticPurchaseOrder = new ItemType(
    'automaticPurchaseOrder',
    'AUTOMATIC_PURCHASE_ORDER',
    '/purchasing/automatic-purchase-orders'
);

export const sendPurchaseOrderSupplierAssEmail = new ItemType(
    'sendPurchaseOrderSupplierAssEmail',
    'SEND_PURCHASE_ORDER_SUPP_ASS_EMAIL',
    '/purchasing/purchase-orders/email-supplier-ass'
);

export const ledgerPeriods = new ItemType(
    'ledgerPeriods',
    'LEDGER_PERIODS',
    '/purchasing/ledger-periods'
);

export const changeRequest = new ItemType(
    'changeRequest',
    'CHANGE_REQUEST',
    '/purchasing/change-requests'
);

export const changeRequests = new ItemType(
    'changeRequests',
    'CHANGE_REQUESTS',
    '/purchasing/change-requests'
);

export const bomTree = new ItemType('bomTree', 'BOM_TREE', '/purchasing/boms/tree');

export const sendPurchaseOrderAuthEmail = new ItemType(
    'sendPurchaseOrderAuthEmail',
    'SEND_PURCHASE_ORDER_AUTH_EMAIL',
    '/purchasing/purchase-orders/email-for-authorisation'
);

export const sendPurchaseOrderDeptEmail = new ItemType(
    'sendPurchaseOrderDeptEmail',
    'SEND_PURCHASE_ORDER_DEPT_EMAIL',
    ''
);

export const suggestedPurchaseOrderValues = new ItemType(
    'suggestedPurchaseOrderValues',
    'SUGGESTED_PURCHASE_ORDER_VALUES',
    '/purchasing/purchase-orders/generate-order-from-supplier-id'
);

export const bomTypeChange = new ItemType(
    'bomTypeChange',
    'BOM_TYPE_CHANGES',
    '/purchasing/bom-type-change'
);

export const board = new ItemType('board', 'BOARD', '/purchasing/boms/boards');
export const boardComponents = new ItemType(
    'boardComponents',
    'BOARD_COMPONENTS',
    '/purchasing/boms/board-components'
);
export const boards = new ItemType('boards', 'BOARDS', '/purchasing/boms/boards');

export const changeRequestStatusChange = new ItemType(
    'changeRequestStatusChange',
    'CHANGE_REQUEST_STATUS',
    '/purchasing/change-requests/status'
);

export const changeRequestPhaseIns = new ItemType(
    'changeRequestPhaseInsChange',
    'CHANGE_REQUEST_PHASE_INS',
    '/purchasing/change-requests/phase-ins'
);

export const subAssembly = new ItemType('subAssembly', 'SUB_ASSEMBLY', '/purchasing/boms/tree');

export const boardComponentSummaries = new ItemType(
    'boardComponentSummaries',
    'BOARD_COMPONENT_SUMMARIES',
    '/purchasing/boms/boards-summary'
);

export const bomVerificationHistory = new ItemType(
    'bomVerificationHistory',
    'BOM_VERIFICATION_HISTORY',
    '/purchasing/bom-verification'
);

export const bomStandardPrices = new ItemType(
    'bomStandardPrices',
    'BOM_STANDARD_PRICES',
    '/purchasing/boms/prices'
);

export const bomHistoryReport = new ItemType(
    'bomHistoryReport',
    'BOM_HISTORY_REPORT',
    '/purchasing/reports/bom-history'
);

export const partDataSheetValuesList = new ItemType(
    'partDataSheetValuesList',
    'PART_DATA_SHEET_VALUES_LIST',
    '/purchasing/part-data-sheet-values'
);

export const partDataSheetValues = new ItemType(
    'partDataSheetValues',
    'PART_DATA_SHEET_VALUES',
    '/purchasing/part-data-sheet-values'
);

export const bomVerificationHistoryEntries = new ItemType(
    'bomVerificationHistoryEntries',
    'BOM_VERIFICATION_HISTORY_ENTRIES',
    '/purchasing/bom-verification'
);

export const changeRequestReplace = new ItemType(
    'changeRequestReplace',
    'CHANGE_REQUEST_REPLACE',
    '/purchasing/change-requests/replace'
);

export const switchOurQtyPrice = new ItemType(
    'switchOurQtyPrice',
    'SWITCH_OUT_QTY_PRICE',
    '/purchasing/purchase-orders/orderNumber/switch-our-qty-price'
);
