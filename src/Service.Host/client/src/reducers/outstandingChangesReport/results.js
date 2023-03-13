import { reportResultsFactory } from '@linn-it/linn-form-components-library';
import { outstandingChangesReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = { loading: false, data: null };

export default reportResultsFactory(
    reportTypes.outstandingChangesReport.actionType,
    actionTypes,
    defaultState
);
