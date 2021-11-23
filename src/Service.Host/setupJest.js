import 'regenerator-runtime/runtime';

global.console = {
    // suppresses errors and warnings in tests
    warn: jest.fn(),
    error: jest.fn(),

    // Keep native behaviour for other console methods
    log: console.log,
    info: console.info,
    debug: console.debug
};
