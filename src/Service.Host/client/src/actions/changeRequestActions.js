import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { changeRequestActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.changeRequest.item,
    itemTypes.changeRequest.actionType,
    itemTypes.changeRequest.uri,
    actionTypes,
    config.appRoot
);
