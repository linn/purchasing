import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { subAssemblyActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.subAssembly.item,
    itemTypes.subAssembly.actionType,
    itemTypes.subAssembly.uri,
    actionTypes,
    config.appRoot
);
