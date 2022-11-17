import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { bomTreeActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.bomTree.item,
    itemTypes.bomTree.actionType,
    itemTypes.bomTree.uri,
    actionTypes,
    config.appRoot
);
