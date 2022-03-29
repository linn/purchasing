import { ProcessActions } from '@linn-it/linn-form-components-library';
import { poReqsCancel } from '../itemTypes';
import { poReqsCancelActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    poReqsCancel.item,
    poReqsCancel.actionType,
    poReqsCancel.uri,
    actionTypes,
    config.appRoot
);
