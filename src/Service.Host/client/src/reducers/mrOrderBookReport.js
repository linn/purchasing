import { reportsResultsFactory } from '@linn-it/linn-form-components-library';
import { mrOrderBookReportActionTypes as actionTypes } from '../actions';
import * as reportTypes from '../reportTypes';

const defaultState = { loading: false, data: null };

export default reportsResultsFactory(
    reportTypes.mrOrderBookReport.actionType,
    actionTypes,
    defaultState
);
