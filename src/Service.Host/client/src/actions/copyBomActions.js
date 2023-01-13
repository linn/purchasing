import { ProcessActions } from '@linn-it/linn-form-components-library';
import { copyBom } from '../processTypes';
import { copyBomActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    copyBom.item,
    copyBom.actionType,
    copyBom.uri,
    actionTypes,
    config.appRoot,
    'application/json'
);
