import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { mrpRunLogActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.mrpRunLog.item,
    itemTypes.mrpRunLog.actionType,
    itemTypes.mrpRunLog.uri,
    actionTypes,
    config.appRoot
);
