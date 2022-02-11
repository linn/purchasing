import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { countriesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.countries.item,
    itemTypes.countries.actionType,
    itemTypes.countries.uri,
    actionTypes,
    config.appRoot
);
