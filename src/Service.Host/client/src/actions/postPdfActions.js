import { RSAA } from 'redux-api-middleware';

import config from '../config';

const requestPostPdf = {
    type: 'REQUEST_POST_PDF',
    payload: {}
};

const receivePostPdf = {
    type: `RECEIVE_POST_PDF`,
    payload: () => async (action, state, res) => ({
        data: await res.json(),
        item: 'postPdf'
    })
};

const error = {
    type: 'POST_PDF_ERROR',
    payload: async (action, state, res) =>
        res
            ? {
                  error: {
                      status: res.status,
                      statusText: `Error - ${res.status} ${res.statusText}`,
                      details: await res.json()
                  }
              }
            : `Network request failed`
};

const postPdf = (blob, id) => ({
    [RSAA]: {
        endpoint: `${config.appRoot}/purchasing/pl-credit-debit-notes/email/${id}`,
        method: 'POST',
        options: { requiresAuth: true },
        headers: {
            Accept: 'application/json',
            'Content-type': 'application/pdf'
        },
        body: blob,
        types: [requestPostPdf, receivePostPdf, error]
    }
});

export default postPdf;
