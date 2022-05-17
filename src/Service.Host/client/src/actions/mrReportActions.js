import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { mrReportActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.mrReport.item,
    itemTypes.mrReport.actionType,
    itemTypes.mrReport.uri,
    actionTypes,
    config.appRoot
);
