import React from 'react';
import { CallbackComponent } from 'redux-oidc';
import { Loading } from '@linn-it/linn-form-components-library';
import history from '../history';

import userManager from '../helpers/userManager';

function Callback() {
    console.log('hit callback');
    return (
        <CallbackComponent
            userManager={userManager}
            successCallback={user => {
                console.log('doing success redirect')
                history.push(user.state.redirect);
            }}
        >
            <Loading />
        </CallbackComponent>
    );
}

export default Callback;
