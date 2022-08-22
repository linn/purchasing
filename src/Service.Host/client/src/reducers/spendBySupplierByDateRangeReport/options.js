import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { spendBySupplierByDateRangeReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.spendBySupplierByDateRangeReport.actionType,
    actionTypes,
    defaultState
);
