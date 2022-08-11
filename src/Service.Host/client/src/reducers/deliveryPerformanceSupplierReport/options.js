import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { deliveryPerformanceSupplierReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.deliveryPerformanceSupplierReport.actionType,
    actionTypes,
    defaultState
);
