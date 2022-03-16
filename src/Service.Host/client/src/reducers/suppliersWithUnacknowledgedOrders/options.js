import { reportOptionsFactory } from '@linn-it/linn-form-components-library';
import { suppliersWithUnacknowledgedOrdersActionTypes as actionTypes } from '../../actions';
import * as reportTypes from '../../reportTypes';

const defaultState = {};

export default reportOptionsFactory(
    reportTypes.suppliersWithUnacknowledgedOrders.actionType,
    actionTypes,
    defaultState
);
