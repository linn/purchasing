import { ProcessActions } from '@linn-it/linn-form-components-library';
import { applyForecastingPercentageChange } from '../itemTypes';
import { applyForecastingPercentageChangeActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    applyForecastingPercentageChange.item,
    applyForecastingPercentageChange.actionType,
    applyForecastingPercentageChange.uri,
    actionTypes,
    config.appRoot
);
