import { ReportActions } from '@linn-it/linn-form-components-library';
import { prefSupReceiptsReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.prefSupReceiptsReport.item,
    reportTypes.prefSupReceiptsReport.actionType,
    reportTypes.prefSupReceiptsReport.uri,
    actionTypes,
    config.appRoot
);
