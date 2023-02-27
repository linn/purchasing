import { reportResultsFactory } from '@linn-it/linn-form-components-library';
import { bomDifferenceReportActionTypes as actionTypes } from '../actions';
import * as reportTypes from '../reportTypes';

const defaultState = { loading: false, data: null };

export default reportResultsFactory(
    reportTypes.bomDifferenceReport.actionType,
    actionTypes,
    defaultState
);
