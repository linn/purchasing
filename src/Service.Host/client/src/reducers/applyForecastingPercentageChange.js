import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { applyForecastingPercentageChangeActionTypes as actionTypes } from '../actions';
import { applyForecastingPercentageChange } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    applyForecastingPercentageChange.actionType,
    actionTypes,
    defaultState
);
