import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { boardsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.boards.item,
    itemTypes.boards.actionType,
    itemTypes.boards.uri,
    actionTypes,
    config.appRoot
);
