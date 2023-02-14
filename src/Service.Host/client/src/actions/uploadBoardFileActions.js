import { ProcessActions } from '@linn-it/linn-form-components-library';
import { uploadBoardFile } from '../processTypes';
import { uploadBoardFileActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    uploadBoardFile.item,
    uploadBoardFile.actionType,
    uploadBoardFile.uri,
    actionTypes,
    config.appRoot,
    'text/tab-separated-values'
);
