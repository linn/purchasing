import { RSAA } from 'redux-api-middleware';
import { getAccessToken } from '../selectors/getAccessToken';

export default ({ getState }) =>
    next =>
    action => {
        if (action[RSAA]) {
            if (action[RSAA].options && action[RSAA].options.requiresAuth) {
                // eslint-disable-next-line no-param-reassign
                action[RSAA].headers = {
                    Authorization: `Bearer ${getAccessToken(getState())}`,
                    ...action[RSAA].headers
                };
            }
        }

        return next(action);
    };
