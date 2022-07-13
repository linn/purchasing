import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { shortagesPlannerReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.shortagesPlannerReport.actionType,
    actionTypes,
    defaultState
);
