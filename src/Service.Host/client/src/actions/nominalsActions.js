import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { nominalsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.nominals.item,
    itemTypes.nominals.actionType,
    itemTypes.nominals.uri,
    actionTypes,
    config.proxyRoot
);
