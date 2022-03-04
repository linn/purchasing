import { RSAA } from 'redux-api-middleware';

import config from '../config';

const requestSendPlNoteEmail = {
    type: 'REQUEST_SEND_PL_NOTE_EMAIL',
    payload: {}
};

const receiveSendPlNoteEmail = {
    type: `RECEIVE_SEND_PL_NOTE_EMAIL`,
    payload: async (action, state, res) => ({
        data: await res.json(),
        item: 'sendPlNoteEmail'
    })
};

const error = {
    type: 'SEND_PL_NOTE_EMAIL_ERROR',
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

export const sendPlNoteEmail = (blob, id) => ({
    [RSAA]: {
        endpoint: `${config.appRoot}/purchasing/pl-credit-debit-notes/email/${id}`,
        method: 'POST',
        options: { requiresAuth: true },
        headers: {
            Accept: 'application/json',
            'Content-type': 'application/pdf'
        },
        body: blob,
        types: [requestSendPlNoteEmail, receiveSendPlNoteEmail, error]
    }
});

export const setMessageVisible = visible => {
    if (visible === true) {
        return {
            type: 'SHOW_SEND_PL_NOTE_EMAIL_MESSAGE',
            payload: {}
        };
    }
    return {
        type: 'HIDE_SEND_PL_NOTE_EMAIL_MESSAGE',
        payload: {}
    };
};
