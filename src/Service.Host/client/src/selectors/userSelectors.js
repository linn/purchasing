export const getName = state => state.oidc?.user?.profile?.name;

export const getUserNumber = state =>
    state.oidc?.user?.profile?.employee?.split('/employees/')?.[1];
