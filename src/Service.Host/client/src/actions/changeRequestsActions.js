import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { changeRequestsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.changeRequests.item,
    itemTypes.changeRequests.actionType,
    itemTypes.changeRequests.uri,
    actionTypes,
    config.appRoot
);
