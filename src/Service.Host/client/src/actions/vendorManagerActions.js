import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { vendorManagerActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.vendorManager.item,
    itemTypes.vendorManager.actionType,
    itemTypes.vendorManager.uri,
    actionTypes,
    config.proxyRoot
    // null,
    // false
);
