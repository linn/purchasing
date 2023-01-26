import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { bomVerificationHistoryActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.bomVerificationHistory.item,
    itemTypes.bomVerificationHistory.actionType,
    itemTypes.bomVerificationHistory.uri,
    actionTypes,
    config.appRoot
);
