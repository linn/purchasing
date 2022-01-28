import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { vendorManagersActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.vendorManagers.item,
    itemTypes.vendorManagers.actionType,
    itemTypes.vendorManagers.uri,
    actionTypes,
    config.proxyRoot
);
