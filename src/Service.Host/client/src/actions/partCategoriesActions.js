import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { partCategoriesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.partCategories.item,
    itemTypes.partCategories.actionType,
    itemTypes.partCategories.uri,
    actionTypes,
    config.appRoot
);
