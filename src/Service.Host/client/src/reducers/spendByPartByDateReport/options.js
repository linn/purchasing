import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { spendByPartByDateReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.spendByPartByDateReport.actionType,
    actionTypes,
    defaultState
);
