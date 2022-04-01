const path = require('path');
const MomentLocalesPlugin = require('moment-locales-webpack-plugin');
const webpack = require('webpack');

module.exports = {
    entry: {
        app: ['babel-polyfill', './client/src/index.js'],
        'silent-renew': './client/silent-renew/index.js'
    },
    mode: 'production',
    output: {
        path: path.resolve(__dirname, '../client/build'), // string
        filename: '[name].js',
        publicPath: '/purchasing/build/'
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /(node_modules)/,
                use: {
                    loader: 'babel-loader'
                }
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
                    'sass-loader',
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
        fallback: {
            process: path.resolve('./node_modules/process'),
            zlib: path.resolve('./node_modules/browserify-zlib/lib/index.js'),
            stream: path.resolve('./node_modules/stream-browserify/index.js'),
            util: path.resolve('./node_modules/util'),
            buffer: path.resolve('./node_modules/buffer'),
            asset: path.resolve('./node_modules/assert')
        }
        //modules: [path.resolve('node_modules'), 'node_modules'].concat(/* ... */)
    },
    plugins: [
        new webpack.ProvidePlugin({
            Buffer: ['buffer', 'Buffer'],
            process: 'process/browser'
        }),
        // To strip all locales except “en”
        new MomentLocalesPlugin()
    ]
};
