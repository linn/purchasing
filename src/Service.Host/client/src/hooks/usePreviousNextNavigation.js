import { useLocation, useParams } from 'react-router';
import queryString from 'query-string';
import history from '../history';

export default function usePreviousNextNavigation(
    urlBuilder,
    searchResults,
    idStyle = 'param',
    idFieldName
) {
    const { search } = useLocation();
    const parsed = queryString.parse(search);
    const { id } = useParams();

    let idField;

    if (idStyle === 'query') {
        idField = parsed?.[idFieldName];
    } else {
        idField = id;
    }

    const currentIndex = searchResults?.indexOf(idField);
    const nextResult = searchResults?.[currentIndex + 1];
    const prevResult = searchResults?.[currentIndex - 1];

    if (!searchResults?.length || searchResults?.length === 1) {
        return [null, null];
    }
    const goNext = nextResult
        ? () => {
              history.push(`${urlBuilder(nextResult)}`, { searchResults });
          }
        : null;
    const goPrev = prevResult
        ? () => {
              history.push(`${urlBuilder(prevResult)}`, { searchResults });
          }
        : null;

    return [goPrev, goNext, prevResult, nextResult];
}
