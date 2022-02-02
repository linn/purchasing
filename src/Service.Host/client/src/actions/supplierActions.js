import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { supplierActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.supplier.item,
    itemTypes.supplier.actionType,
    itemTypes.supplier.uri,
    actionTypes,
    config.appRoot
);
