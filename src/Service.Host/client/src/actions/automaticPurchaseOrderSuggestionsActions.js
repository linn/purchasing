import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { automaticPurchaseOrderSuggestionsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.automaticPurchaseOrderSuggestions.item,
    itemTypes.automaticPurchaseOrderSuggestions.actionType,
    itemTypes.automaticPurchaseOrderSuggestions.uri,
    actionTypes,
    config.appRoot
);
