import { ProcessActions } from '@linn-it/linn-form-components-library';
import { sendEdiEmail } from '../itemTypes';
import { sendEdiEmailActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    sendEdiEmail.item,
    sendEdiEmail.actionType,
    sendEdiEmail.uri,
    actionTypes,
    config.appRoot
);
