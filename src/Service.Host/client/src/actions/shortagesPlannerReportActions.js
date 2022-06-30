import { ReportActions } from '@linn-it/linn-form-components-library';
import { shortagesPlannerReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.shortagesPlannerReport.item,
    reportTypes.shortagesPlannerReport.actionType,
    reportTypes.shortagesPlannerReport.uri,
    actionTypes,
    config.appRoot
);
