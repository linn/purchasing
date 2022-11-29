import { ReportActions } from '@linn-it/linn-form-components-library';
import { bomCostReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.bomCostReport.item,
    reportTypes.bomCostReport.actionType,
    reportTypes.bomCostReport.uri,
    actionTypes,
    config.appRoot
);
