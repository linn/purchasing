import { StateApiActions } from '@linn-it/linn-form-components-library';
import { runMrpActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new StateApiActions(
    itemTypes.runMrp.item,
    itemTypes.runMrp.actionType,
    itemTypes.runMrp.uri,
    actionTypes,
    config.appRoot,
    'application-state'
);
