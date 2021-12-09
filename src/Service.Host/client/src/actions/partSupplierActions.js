import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { partSupplierActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.partSupplier.item,
    itemTypes.partSupplier.actionType,
    itemTypes.partSupplier.uri,
    actionTypes,
    config.appRoot
);
