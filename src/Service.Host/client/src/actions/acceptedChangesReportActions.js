import { ReportActions } from '@linn-it/linn-form-components-library';
import { acceptedChangesReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.acceptedChangesReport.item,
    reportTypes.acceptedChangesReport.actionType,
    reportTypes.acceptedChangesReport.uri,
    actionTypes,
    config.appRoot
);
