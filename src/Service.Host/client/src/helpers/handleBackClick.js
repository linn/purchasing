export default (previousPaths, goBack) => {
    const prevPath =
        previousPaths?.[previousPaths.length - 1]?.path +
        previousPaths?.[previousPaths.length - 1]?.search;
    if (prevPath.includes('signin-oidc')) {
        global.window.history.go(-3);
    } else if (previousPaths?.length) {
        goBack();
    } else {
        global.window.history.back();
    }
};
