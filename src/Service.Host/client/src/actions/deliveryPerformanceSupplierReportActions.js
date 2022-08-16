import { ReportActions } from '@linn-it/linn-form-components-library';
import { deliveryPerformanceSupplierReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.deliveryPerformanceSupplierReport.item,
    reportTypes.deliveryPerformanceSupplierReport.actionType,
    reportTypes.deliveryPerformanceSupplierReport.uri,
    actionTypes,
    config.appRoot
);
