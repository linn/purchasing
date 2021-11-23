import { createStore, applyMiddleware, compose } from 'redux';
import { apiMiddleware as api } from 'redux-api-middleware';
import thunkMiddleware from 'redux-thunk';
import { createBrowserHistory } from 'history';
import reducer from './reducers';
import authorization from './middleware/authorization';
import itemCreated from './middleware/itemCreated';
import previousLocationMiddleware from './middleware/previousLocation';
// eslint-disable-next-line no-underscore-dangle
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const middleware = [authorization, api, thunkMiddleware, itemCreated, previousLocationMiddleware];

export const history = createBrowserHistory();

const configureStore = initialState => {
    const enhancers = composeEnhancers(applyMiddleware(...middleware));
    const store = createStore(reducer(history), initialState, enhancers);

    return store;
};

export default configureStore;
