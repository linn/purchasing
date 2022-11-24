import { reportResultsFactory } from '@linn-it/linn-form-components-library';
import { bomCostReportActionTypes as actionTypes } from '../actions';
import * as reportTypes from '../reportTypes';

const defaultState = { loading: false, data: null };

export default reportResultsFactory(
    reportTypes.bomCostReport.actionType,
    actionTypes,
    defaultState
);
