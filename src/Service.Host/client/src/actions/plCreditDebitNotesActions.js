import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { plCreditDebitNoteActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.plCreditDebitNotes.item,
    itemTypes.plCreditDebitNotes.actionType,
    itemTypes.plCreditDebitNotes.uri,
    actionTypes,
    config.appRoot
);
