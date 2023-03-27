import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { currentPhaseInWeeksReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.currentPhaseInWeeksReport.actionType,
    actionTypes,
    defaultState
);
