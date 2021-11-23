const path = require('path');
const webpack = require('webpack');

function localResolve(preset) {
    return Array.isArray(preset)
        ? [require.resolve(preset[0]), preset[1]]
        : require.resolve(preset);
}

module.exports = {
    mode: 'development',
    entry: {
        app: [
            'babel-polyfill',
            'react-hot-loader/patch', // activate HMR for React
            'webpack-dev-server/client?http://localhost:3000', // bundle the client for webpack-dev-server and connect to the provided endpoint
            'webpack/hot/only-dev-server', // bundle the client for hot reloading (only- means to only hot reload for successful updates)
            './client/src/index.js' // the entry point of our app
        ],
        'silent-renew': './client/silent-renew/index.js'
    },
    output: {
        path: path.join(__dirname, '../client/build'),
        filename: '[name].js',
        publicPath: '/template/build/'
    },
    module: {
        rules: [
            {
                enforce: 'pre',
                test: /\.js$/,
                exclude: /(node_modules)/,
                use: {
                    loader: 'eslint-loader',
                    options: {
                        emitWarning: true
                    }
                }
            },
            {
                test: /.js$/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: [
                            ['@babel/preset-env', { modules: 'commonjs' }],
                            '@babel/preset-react'
                        ].map(localResolve),
                        plugins: ['@babel/plugin-transform-runtime'].map(localResolve)
                    }
                },
                exclude: /node_modules/
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    {
                        loader: 'css-loader',
                        options: {
                            importLoaders: 1
                        }
                    },
                    'postcss-loader'
                ]
            },
            {
                test: /\.scss$/,
                use: [
                    'style-loader',
                    {
                        loader: 'css-loader',
                        options: {
                            importLoaders: 1
                        }
                    },
                    'fast-sass-loader',
                    'postcss-loader'
                ]
            },
            {
                test: /\.(jpe?g|svg|png|gif|ico|eot|ttf|woff2?)(\?v=\d+\.\d+\.\d+)?$/i,
                type: 'asset/resource'
            }
        ]
    },
    resolve: {
        alias: {
            '@material-ui/pickers': path.resolve('./node_modules/@material-ui/pickers'),
            'react-redux': path.resolve('./node_modules/react-redux'),
            react: path.resolve('./node_modules/react'),
            notistack: path.resolve('./node_modules/notistack'),
            '@material-ui/styles': path.resolve('./node_modules/@material-ui/styles')
        }
        //modules: [path.resolve('node_modules'), 'node_modules'].concat(/* ... */)
    },
    optimization: {
        moduleIds: 'named'
    },
    devtool: 'eval-cheap-module-source-map',
    // From https://github.com/gaearon/react-hot-boilerplate/blob/next/webpack.config.js
    plugins: [
        new webpack.NoEmitOnErrorsPlugin(), // do not emit compiled assets that include errors
        new webpack.DefinePlugin({
            'PROCESS.ENV': {
                appRoot: JSON.stringify('http://localhost:51698')
            }
        })
    ]
};
