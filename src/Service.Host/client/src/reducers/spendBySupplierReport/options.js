import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { spendBySupplierReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.spendBySupplierReport.actionType,
    actionTypes,
    defaultState
);
