import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import {
    Page,
    SaveBackCancelButtons,
    collectionSelectorHelpers,
    Typeahead,
    InputField,
    SnackbarMessage,
    itemSelectorHelpers,
    Loading
} from '@linn-it/linn-form-components-library';
import history from '../history';
import config from '../config';

function SplitDeliveriesUtility({ orderNumber, orderLine, inDialogBox, deliveries }) {
    const dispatch = useDispatch();

    const content = () => <>{deliveries.map(d => JSON.stringify(d))} </>;

    return (
        <>
            {inDialogBox ? (
                content()
            ) : (
                <Page history={history} homeUrl={config.appRoot}>
                    {content()}
                </Page>
            )}
        </>
    );
}

SplitDeliveriesUtility.propTypes = {
    orderNumber: PropTypes.number.isRequired,
    orderLine: PropTypes.number.isRequired,
    inDialogBox: PropTypes.bool,
    deliveries: PropTypes.arrayOf(PropTypes.shape({}))
};

SplitDeliveriesUtility.defaultProps = {
    inDialogBox: false,
    deliveries: null
};

export default SplitDeliveriesUtility;
