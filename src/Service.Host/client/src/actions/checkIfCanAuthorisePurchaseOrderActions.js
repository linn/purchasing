import { ProcessActions } from '@linn-it/linn-form-components-library';
import { pOReqCheckIfCanAuthOrder } from '../itemTypes';
import { pOReqCheckIfCanAuthOrderActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    pOReqCheckIfCanAuthOrder.item,
    pOReqCheckIfCanAuthOrder.actionType,
    pOReqCheckIfCanAuthOrder.uri,
    actionTypes,
    config.appRoot
);
