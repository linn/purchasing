import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { switchOurQtyPriceActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.switchOurQtyPrice.item,
    itemTypes.switchOurQtyPrice.actionType,
    itemTypes.switchOurQtyPrice.uri,
    actionTypes,
    config.appRoot
);
