import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { supplierLeadTimesReportActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.supplierLeadTimesReport.actionType,
    actionTypes,
    defaultState
);
