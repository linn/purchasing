import { useLocation, useParams } from 'react-router';
import queryString from 'query-string';
import history from '../history';

export default function usePreviousNextNavigation(urlBuilder, idStyle = 'param', idFieldName) {
    const { search } = useLocation();
    const { id } = useParams();
    const parsed = queryString.parse(search);

    let idField;

    if (idStyle === 'query') {
        idField = parsed?.[idFieldName];
    } else {
        idField = id;
    }

    const searchResultsString = parsed?.searchResults;

    const resultsArray = searchResultsString?.split(',');
    const currentIndex = resultsArray?.indexOf(idField);
    const nextResult = resultsArray?.[currentIndex + 1];
    const prevResult = resultsArray?.[currentIndex - 1];

    if (!resultsArray?.length) {
        return [null, null];
    }
    const goNext = nextResult
        ? () => {
              history.push(`${urlBuilder(nextResult, searchResultsString)}`);
          }
        : null;
    const goPrev = prevResult
        ? () => {
              history.push(`${urlBuilder(prevResult, searchResultsString)}`);
          }
        : null;

    return [goPrev, goNext, prevResult, nextResult];
}
