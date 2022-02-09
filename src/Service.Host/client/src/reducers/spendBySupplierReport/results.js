import { reportResultsFactory } from '@linn-it/linn-form-components-library';
import { spendBySupplierReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = { loading: false, data: null };

export default reportResultsFactory(
    reportTypes.spendBySupplierReport.actionType,
    actionTypes,
    defaultState
);
