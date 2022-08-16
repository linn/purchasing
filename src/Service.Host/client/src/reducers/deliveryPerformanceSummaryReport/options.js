import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { deliveryPerformanceSummaryReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.deliveryPerformanceSummaryReport.actionType,
    actionTypes,
    defaultState
);
