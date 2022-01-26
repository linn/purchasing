import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { partPriceConversionsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.partPriceConversions.item,
    itemTypes.partPriceConversions.actionType,
    itemTypes.partPriceConversions.uri,
    actionTypes,
    config.appRoot
);
