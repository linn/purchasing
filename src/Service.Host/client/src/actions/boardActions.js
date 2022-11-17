import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { boardActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.board.item,
    itemTypes.board.actionType,
    itemTypes.board.uri,
    actionTypes,
    config.appRoot
);
