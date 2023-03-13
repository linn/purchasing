import { ReportActions } from '@linn-it/linn-form-components-library';
import { proposedChangesReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.proposedChangesReport.item,
    reportTypes.proposedChangesReport.actionType,
    reportTypes.proposedChangesReport.uri,
    actionTypes,
    config.appRoot
);
