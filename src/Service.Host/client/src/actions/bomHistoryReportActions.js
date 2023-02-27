import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { bomHistoryReportActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.bomHistoryReport.item,
    itemTypes.bomHistoryReport.actionType,
    itemTypes.bomHistoryReport.uri,
    actionTypes,
    config.appRoot
);
