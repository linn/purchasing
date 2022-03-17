import { ProcessActions } from '@linn-it/linn-form-components-library';
import { sendPlNoteEmail } from '../itemTypes';
import { sendPlNoteEmailActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    sendPlNoteEmail.item,
    sendPlNoteEmail.actionType,
    sendPlNoteEmail.uri,
    actionTypes,
    config.appRoot,
    'application/pdf'
);
