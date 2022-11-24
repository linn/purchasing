import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { changeRequestStatusChangeActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.changeRequestStatusChange.item,
    itemTypes.changeRequestStatusChange.actionType,
    itemTypes.changeRequestStatusChange.uri,
    actionTypes,
    config.appRoot
);
