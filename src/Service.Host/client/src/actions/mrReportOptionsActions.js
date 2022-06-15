import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { mrReportOptionsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.mrReportOptions.item,
    itemTypes.mrReportOptions.actionType,
    itemTypes.mrReportOptions.uri,
    actionTypes,
    config.appRoot
);
