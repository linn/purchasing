module.exports = {
    presets: [['@babel/preset-env', { modules: 'commonjs' }], '@babel/react'],
    plugins: [
        'react-hot-loader/babel',
        '@babel/plugin-transform-object-assign',
        '@babel/plugin-proposal-optional-chaining',
        ['@babel/plugin-proposal-class-properties']
    ]
};
