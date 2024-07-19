import { reportsResultsFactory } from '@linn-it/linn-form-components-library';
import { bomPrintReportActionTypes as actionTypes } from '../actions';
import * as reportTypes from '../reportTypes';

const defaultState = { loading: false, data: null };

export default reportsResultsFactory(
    reportTypes.bomPrintReport.actionType,
    actionTypes,
    defaultState
);
