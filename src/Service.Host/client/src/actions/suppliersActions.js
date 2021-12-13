import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { suppliersActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.suppliers.item,
    itemTypes.suppliers.actionType,
    itemTypes.suppliers.uri,
    actionTypes,
    config.appRoot
);
