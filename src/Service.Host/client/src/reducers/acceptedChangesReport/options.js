import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { acceptedChangesReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.acceptedChangesReport.actionType,
    actionTypes,
    defaultState
);
