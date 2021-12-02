import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { signingLimitActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.signingLimit.item,
    itemTypes.signingLimit.actionType,
    itemTypes.signingLimit.uri,
    actionTypes,
    config.appRoot
);
