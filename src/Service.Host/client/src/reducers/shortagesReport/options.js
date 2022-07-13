import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { shortagesReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.shortagesReport.actionType,
    actionTypes,
    defaultState
);
