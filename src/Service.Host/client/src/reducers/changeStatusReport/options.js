import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { changeRequestActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.changeStatusReport.actionType,
    actionTypes,
    defaultState
);
