import { ReportActions } from '@linn-it/linn-form-components-library';
import { mrUsedOnReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.mrUsedOnReport.item,
    reportTypes.mrUsedOnReport.actionType,
    reportTypes.mrUsedOnReport.uri,
    actionTypes,
    config.appRoot
);
