import { utilities } from '@linn-it/linn-form-components-library';
import history from '../history';

export default () => next => action => {
    const result = next(action);
    if (action.type.startsWith('RECEIVE_NEW_')) {
        history.push(utilities.getSelfHref(action.payload.data));
    }

    return result;
};
