import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { addressesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.addresses.item,
    itemTypes.addresses.actionType,
    itemTypes.addresses.uri,
    actionTypes,
    config.appRoot
);
