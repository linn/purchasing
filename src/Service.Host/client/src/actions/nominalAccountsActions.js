import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { nominalAccountsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.nominalAccounts.item,
    itemTypes.nominalAccounts.actionType,
    itemTypes.nominalAccounts.uri,
    actionTypes,
    config.proxyRoot
);
