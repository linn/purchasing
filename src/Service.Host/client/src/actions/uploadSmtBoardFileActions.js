import { ProcessActions } from '@linn-it/linn-form-components-library';
import { uploadSmtBoardFile } from '../processTypes';
import { uploadSmtBoardFileActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    uploadSmtBoardFile.item,
    uploadSmtBoardFile.actionType,
    uploadSmtBoardFile.uri,
    actionTypes,
    config.appRoot,
    'text/tab-separated-values'
);
