import { useSelector, useDispatch } from 'react-redux';
import { useEffect, useRef } from 'react';
import { getItemError } from '@linn-it/linn-form-components-library';

export default function useInitialise(action, itemType) {
    const data = useSelector(state => state[itemType].item);
    const dataLoading = useSelector(state => state[itemType].loading);
    const error = useSelector(state => getItemError(state, itemType));
    const dispatch = useDispatch();
    const hasFetched = useRef(false);
    useEffect(() => {
        if (!hasFetched.current) {
            dispatch(action());
            hasFetched.current = true;
        }
    }, [action, dispatch]);
    return [data, dataLoading, error];
}
