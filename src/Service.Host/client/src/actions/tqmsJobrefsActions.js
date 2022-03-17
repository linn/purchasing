import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { tqmsJobrefsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.tqmsJobrefs.item,
    itemTypes.tqmsJobrefs.actionType,
    itemTypes.tqmsJobrefs.uri,
    actionTypes,
    config.appRoot,
    null,
    false
);
