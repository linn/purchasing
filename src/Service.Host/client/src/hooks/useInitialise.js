import { useSelector, useDispatch } from 'react-redux';
import { useEffect, useRef } from 'react';
import { getItemError } from '@linn-it/linn-form-components-library';

export default function useInitialise(action, itemType, type = 'item', clearErrorsAction = null) {
    const data = useSelector(state => state[itemType][type]);
    const loading = useSelector(state => state[itemType].loading);
    const error = useSelector(state => getItemError(state, itemType));
    const dispatch = useDispatch();
    const hasFetched = useRef(false);

    useEffect(() => {
        if (!hasFetched.current) {
            dispatch(action());
            if (clearErrorsAction) {
                dispatch(clearErrorsAction());
            }
            hasFetched.current = true;
        }
    }, [action, dispatch, clearErrorsAction]);
    return [data, loading, error];
}
