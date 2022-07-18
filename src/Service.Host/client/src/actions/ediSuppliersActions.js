import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { ediSuppliersActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.ediSuppliers.item,
    itemTypes.ediSuppliers.actionType,
    itemTypes.ediSuppliers.uri,
    actionTypes,
    config.appRoot
);
