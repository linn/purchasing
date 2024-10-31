import { createUserManager } from 'redux-oidc';
import { WebStorageStateStore } from 'oidc-client';
import config from '../config';

const host = window.location.origin;

const oidcConfig = {
    authority: config.authorityUri,
    client_id: 'app2',
    response_type: 'code',
    scope: 'openid profile email associations offline_access',
    redirect_uri: `${host}/purchasing/signin-oidc-client`,
    post_logout_redirect_uri: `${host}`,
    automaticSilentRenew: true,
    filterProtocolClaims: true,
    loadUserInfo: true,
    monitorSession: false,
    userStore: new WebStorageStateStore({ store: window.localStorage })
};

const userManager = createUserManager(oidcConfig);

export default userManager;
