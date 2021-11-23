const config = window.APPLICATION_SETTINGS;
const defaultConfig = { appRoot: 'localhost:51698' };

export default { ...defaultConfig, ...config };
