import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { putSupplierOnHoldActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.putSupplierOnHold.item,
    itemTypes.putSupplierOnHold.actionType,
    itemTypes.putSupplierOnHold.uri,
    actionTypes,
    config.appRoot
);
