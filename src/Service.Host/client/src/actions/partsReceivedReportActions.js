import { ReportActions } from '@linn-it/linn-form-components-library';
import { partsReceivedReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.partsReceivedReport.item,
    reportTypes.partsReceivedReport.actionType,
    reportTypes.partsReceivedReport.uri,
    actionTypes,
    config.appRoot
);
