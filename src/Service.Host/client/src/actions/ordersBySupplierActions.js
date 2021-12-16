import { ReportActions } from '@linn-it/linn-form-components-library';
import { ordersBySupplierReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.ordersBySupplierReport.item,
    reportTypes.ordersBySupplierReport.actionType,
    reportTypes.ordersBySupplierReport.uri,
    actionTypes,
    config.appRoot
);
