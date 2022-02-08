export const getAccessToken = state => {
    if (!state.oidc.user) {
        return null;
    }

    return state.oidc.user.access_token;
};

export const getToggleContext = state => (state.oidc.user ? state.oidc.user.profile : undefined);

export const getEmail = state => {
    if (!state.oidc.user || !state.oidc.user.profile) {
        return null;
    }

    return state.oidc.user.profile.email;
};

export const getUserNumber = state =>
    Number(state.oidc?.user?.profile?.employee?.split('/')?.slice(-1)[0]);
