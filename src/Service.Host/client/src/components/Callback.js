import React from 'react';
import { CallbackComponent } from 'redux-oidc';
import { Loading } from '@linn-it/linn-form-components-library';
import history from '../history';

import userManager from '../helpers/userManager';

function Callback() {
    console.log('I WAS HIT');
    return (
        <CallbackComponent
            userManager={userManager}
            successCallback={user => {
                history.push(user.state.redirect);
            }}
        >
            <Loading />
        </CallbackComponent>
    );
}

export default Callback;
