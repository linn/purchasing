import { ProcessActions } from '@linn-it/linn-form-components-library';
import { poReqsAuthorise } from '../itemTypes';
import { poReqsAuthoriseActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    poReqsAuthorise.item,
    poReqsAuthorise.actionType,
    poReqsAuthorise.uri,
    actionTypes,
    config.appRoot
);
