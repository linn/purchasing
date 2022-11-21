import { ReportActions } from '@linn-it/linn-form-components-library';
import { partsOnBomReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.partsOnBomReport.item,
    reportTypes.partsOnBomReport.actionType,
    reportTypes.partsOnBomReport.uri,
    actionTypes,
    config.appRoot
);
