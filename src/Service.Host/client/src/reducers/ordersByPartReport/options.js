import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { ordersByPartReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.ordersByPartReport.actionType,
    actionTypes,
    defaultState
);
