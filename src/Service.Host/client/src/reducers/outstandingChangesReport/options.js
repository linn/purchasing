import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { outstandingChangesReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.outstandingChangesReport.actionType,
    actionTypes,
    defaultState
);
