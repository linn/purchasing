import { createUserManager } from 'redux-oidc';
import { WebStorageStateStore } from 'oidc-client';
import config from '../config';

const authority = config.cognitoHost;
const clientId = config.cognitoClientId;
const domainPrefix = config.cognitoDomainPrefix;
const { origin } = window.location;

const redirectUri = `${origin}/purchasing/auth/`;
const logoutUri = `${origin}/purchasing/auth/logged-out`;

function getCognitoDomain(prefix, authorityUri) {
    if (prefix && authorityUri) {
        const regionMatch = authorityUri.match(/cognito-idp\.(.+)\.amazonaws\.com/);
        const region = regionMatch ? regionMatch[1] : '';
        return `https://${prefix}.auth.${region}.amazoncognito.com`;
    }
    return '';
}

const cognitoDomain = getCognitoDomain(domainPrefix, authority);

export const oidcConfig = {
    authority,
    client_id: clientId,
    redirect_uri: redirectUri,
    response_type: 'code',
    scope: 'email openid profile',
    post_logout_redirect_uri: logoutUri,
    userStore: new WebStorageStateStore({ store: window.localStorage }),
    automaticSilentRenew: false
};

const userManager = createUserManager(oidcConfig);

export const signOut = () => {
    if (!cognitoDomain) return;
    window.location.href = `${cognitoDomain}/logout?client_id=${clientId}&logout_uri=${encodeURIComponent(
        logoutUri
    )}`;
};

export const signOutEntra = async () => {
    const { entraLogoutUri } = config;
    await userManager.removeUser();

    window.location.href = `${entraLogoutUri}?post_logout_redirect_uri=${encodeURIComponent(
        logoutUri
    )}`;
};

export default userManager;
