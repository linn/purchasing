import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { deliveryAddressesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.deliveryAddresses.item,
    itemTypes.deliveryAddresses.actionType,
    itemTypes.deliveryAddresses.uri,
    actionTypes,
    config.appRoot
);
