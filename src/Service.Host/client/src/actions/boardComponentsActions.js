import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { boardComponentsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.boardComponents.item,
    itemTypes.boardComponents.actionType,
    itemTypes.boardComponents.uri,
    actionTypes,
    config.appRoot
);
