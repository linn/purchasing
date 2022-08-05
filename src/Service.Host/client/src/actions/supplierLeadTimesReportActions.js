import { ReportActions } from '@linn-it/linn-form-components-library';
import { supplierLeadTimesReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.supplierLeadTimesReport.item,
    reportTypes.supplierLeadTimesReport.actionType,
    reportTypes.supplierLeadTimesReport.uri,
    actionTypes,
    config.appRoot
);
