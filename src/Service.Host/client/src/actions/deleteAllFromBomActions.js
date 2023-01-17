import { ProcessActions } from '@linn-it/linn-form-components-library';
import { deleteAllFromBom } from '../processTypes';
import { deleteAllFromBomActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    deleteAllFromBom.item,
    deleteAllFromBom.actionType,
    deleteAllFromBom.uri,
    actionTypes,
    config.appRoot,
    'application/json'
);
