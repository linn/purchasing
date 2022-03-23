import { ReportActions } from '@linn-it/linn-form-components-library';
import { whatsDueInReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.whatsDueInReport.item,
    reportTypes.whatsDueInReport.actionType,
    reportTypes.whatsDueInReport.uri,
    actionTypes,
    config.appRoot
);
