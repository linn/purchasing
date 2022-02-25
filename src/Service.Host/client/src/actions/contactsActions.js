import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { contactsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.contacts.item,
    itemTypes.contacts.actionType,
    itemTypes.contacts.uri,
    actionTypes,
    config.appRoot,
    null,
    false
);
