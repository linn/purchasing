import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { packagingGroupsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.packagingGroups.item,
    itemTypes.packagingGroups.actionType,
    itemTypes.packagingGroups.uri,
    actionTypes,
    config.appRoot
);
