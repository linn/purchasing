import { useSelector, useDispatch } from 'react-redux';
import { useEffect, useRef } from 'react';
import { itemSelectorHelpers } from '@linn-it/linn-form-components-library';

export default function useApplicationState(action, itemType) {
    const result = useSelector(reduxState =>
        itemSelectorHelpers.getApplicationState(reduxState[itemType])
    );
    const loading = useSelector(reduxState =>
        itemSelectorHelpers.getApplicationStateLoading(reduxState[itemType])
    );
    const dispatch = useDispatch();
    const hasFetched = useRef(false);

    useEffect(() => {
        if (!hasFetched.current) {
            dispatch(action());
            hasFetched.current = true;
        }
    }, [action, dispatch]);
    return [result, loading];
}
