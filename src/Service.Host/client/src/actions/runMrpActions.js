import { ProcessActions } from '@linn-it/linn-form-components-library';
import { runMrp } from '../itemTypes';
import { runMrpActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    runMrp.item,
    runMrp.actionType,
    runMrp.uri,
    actionTypes,
    config.appRoot
);
