import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { accountingCompaniesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.accountingCompanies.item,
    itemTypes.accountingCompanies.actionType,
    itemTypes.accountingCompanies.uri,
    actionTypes,
    config.proxyRoot
);
