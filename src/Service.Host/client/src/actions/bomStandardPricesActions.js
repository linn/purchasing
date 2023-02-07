import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { bomStandardPricesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.bomStandardPrices.item,
    itemTypes.bomStandardPrices.actionType,
    itemTypes.bomStandardPrices.uri,
    actionTypes,
    config.appRoot
);
