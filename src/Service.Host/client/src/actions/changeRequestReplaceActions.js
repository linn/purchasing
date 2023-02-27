import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { changeRequestReplaceActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.changeRequestReplace.item,
    itemTypes.changeRequestReplace.actionType,
    itemTypes.changeRequestReplace.uri,
    actionTypes,
    config.appRoot
);
