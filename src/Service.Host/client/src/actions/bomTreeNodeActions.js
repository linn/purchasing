import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { bomTreeNodesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.bomTreeNodes.item,
    itemTypes.bomTreeNodes.actionType,
    itemTypes.bomTreeNodes.uri,
    actionTypes,
    config.appRoot
);
