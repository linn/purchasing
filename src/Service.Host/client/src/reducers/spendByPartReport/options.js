import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { spendByPartReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.spendByPartReport.actionType,
    actionTypes,
    defaultState
);
