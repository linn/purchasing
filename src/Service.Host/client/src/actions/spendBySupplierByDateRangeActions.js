import { ReportActions } from '@linn-it/linn-form-components-library';
import { spendBySupplierByDateRangeReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.spendBySupplierByDateRangeReport.item,
    reportTypes.spendBySupplierByDateRangeReport.actionType,
    reportTypes.spendBySupplierByDateRangeReport.uri,
    actionTypes,
    config.appRoot
);
