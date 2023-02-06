import { Page } from '@linn-it/linn-form-components-library';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import queryString from 'query-string';

import { useLocation } from 'react-router';
import history from '../history';
import config from '../config';
import useInitialise from '../hooks/useInitialise';
import bomStandardPricesActions from '../actions/bomStandardPricesActions';
import { bomStandardPrices } from '../itemTypes';

const SetBomStandardPriceUtility = () => {
    // hello
    const reduxDispatch = useDispatch();
    const { search } = useLocation();
    const { bomName } = queryString.parse(search);

    const [results, loading] = useInitialise(
        () => bomStandardPricesActions.search(bomName),
        bomStandardPrices.item,
        'searchItems',
        bomStandardPricesActions.clearErrorsForItem
    );

    return (
        <Page history={history} homeUrl={config.appRoot}>
            hello
        </Page>
    );
};

export default SetBomStandardPriceUtility;
