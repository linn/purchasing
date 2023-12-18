// as of version 8, query-string (and its three dependencies listed here) converted to be pure ESM packages
// https://gist.github.com/sindresorhus/a39789f98801d908bbc7ff3ecc99d99c

// as such babel-jest cannot transform them normally, and so they need to be ignored
// https://github.com/sindresorhus/query-string/issues/366
const esModules = ['query-string', 'decode-uri-component', 'split-on-first', 'filter-obj'];

module.exports = {
    coverageDirectory: './coverage/',
    collectCoverageFrom: ['client/src/**/*.{js}'],
    collectCoverage: true,
    setupFiles: ['./setupJest.js', 'raf/polyfill'],
    testPathIgnorePatterns: ['./client/src/components/__tests__/fakeData/*'],
    transform: {
        '^.+\\.js$': 'babel-jest',
        '.+\\.(css|scss|png|jpg|svg)$': 'jest-transform-stub'
    },
    transformIgnorePatterns: esModules.length ? [`/node_modules/(?!${esModules.join('|')})`] : []
};
