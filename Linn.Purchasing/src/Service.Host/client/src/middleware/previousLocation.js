let prevPathname = '';
let prevSearch = '';

export default () => next => action => {
    if (action.type === '@@router/LOCATION_CHANGE') {
        const newAction = {
            ...action,
            payload: {
                ...action.payload,
                prevPathname,
                prevSearch
            }
        };
        prevPathname = action.payload.location.pathname;
        prevSearch = action.payload.location?.search;
        return next(newAction);
    }
    return next(action);
};
