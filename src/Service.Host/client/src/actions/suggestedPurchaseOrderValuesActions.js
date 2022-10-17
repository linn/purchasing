import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { suggestedPurchaseOrderValuesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.suggestedPurchaseOrderValues.item,
    itemTypes.suggestedPurchaseOrderValues.actionType,
    itemTypes.suggestedPurchaseOrderValues.uri,
    actionTypes,
    config.appRoot
);
