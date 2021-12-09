import { utilities } from '@linn-it/linn-form-components-library';
import history from '../history';

export default () => next => action => {
    const result = next(action);
    if (action.type.startsWith('RECEIVE_NEW_')) {
        if (action.type !== 'RECEIVE_NEW_SIGNING_LIMIT') {
            history.push(utilities.getSelfHref(action.payload.data));
        }
    }

    return result;
};
