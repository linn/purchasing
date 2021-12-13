import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { partsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.parts.item,
    itemTypes.parts.actionType,
    itemTypes.parts.uri,
    actionTypes,
    config.proxyRoot
);
