import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { changeRequestPhaseInsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.changeRequestPhaseIns.item,
    itemTypes.changeRequestPhaseIns.actionType,
    itemTypes.changeRequestPhaseIns.uri,
    actionTypes,
    config.appRoot
);
