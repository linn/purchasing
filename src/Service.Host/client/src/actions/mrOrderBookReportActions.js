import { ReportActions } from '@linn-it/linn-form-components-library';
import { mrOrderBookReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.mrOrderBookReport.item,
    reportTypes.mrOrderBookReport.actionType,
    reportTypes.mrOrderBookReport.uri,
    actionTypes,
    config.appRoot
);
