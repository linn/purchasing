const initialState = [];

function history(state = initialState, action) {
    if (typeof state === 'undefined') {
        return state;
    }

    if (action.type === '@@router/LOCATION_CHANGE') {
        return [
            ...state,
            { path: action.payload.prevPathname, search: action.payload?.prevSearch }
        ];
    }
    return state;
}

export default history;
