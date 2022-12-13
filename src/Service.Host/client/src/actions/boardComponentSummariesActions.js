import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { boardComponentSummariesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.boardComponentSummaries.item,
    itemTypes.boardComponentSummaries.actionType,
    itemTypes.boardComponentSummaries.uri,
    actionTypes,
    config.appRoot
);
