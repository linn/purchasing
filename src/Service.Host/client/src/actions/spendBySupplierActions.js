import { ReportActions } from '@linn-it/linn-form-components-library';
import { spendBySupplierReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.spendBySupplierReport.item,
    reportTypes.spendBySupplierReport.actionType,
    reportTypes.spendBySupplierReport.uri,
    actionTypes,
    config.appRoot
);
