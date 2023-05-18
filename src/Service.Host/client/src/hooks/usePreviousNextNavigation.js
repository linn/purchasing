import { useLocation } from 'react-router';
import queryString from 'query-string';
import history from '../history';

export default function usePreviousNextNavigation(idFieldName, baseUrl) {
    const { search } = useLocation();
    const parsed = queryString.parse(search);

    const id = parsed?.[idFieldName];
    const searchResultsString = parsed?.searchResults;

    const resultsArray = searchResultsString?.split(',');
    const currentIndex = resultsArray?.indexOf(id);
    const nextResult = resultsArray?.[currentIndex + 1];
    const prevResult = resultsArray?.[currentIndex - 1];

    if (!resultsArray?.length) {
        return [null, null];
    }

    const goNext = () => {
        history.push(
            `${baseUrl}?${idFieldName}=${nextResult}&searchResults=${searchResultsString}`
        );
    };
    const goPrev = () => {
        history.push(
            `${baseUrl}?${idFieldName}=${prevResult}&searchResults=${searchResultsString}`
        );
    };

    return [goPrev, goNext, prevResult, nextResult];
}
