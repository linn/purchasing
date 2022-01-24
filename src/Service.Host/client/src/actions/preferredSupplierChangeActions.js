import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { preferredSupplierChangeActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.preferredSupplierChange.item,
    itemTypes.preferredSupplierChange.actionType,
    itemTypes.preferredSupplierChange.uri,
    actionTypes,
    config.appRoot
);
