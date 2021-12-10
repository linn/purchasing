import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { ordersBySupplierReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.ordersBySupplierReport.actionType,
    actionTypes,
    defaultState
);
