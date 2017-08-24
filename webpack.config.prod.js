var webpack = require('webpack');
const ExtractTextPlugin = require("extract-text-webpack-plugin");

const extractSass = new ExtractTextPlugin("my-styles.css");


module.exports = {
    module: {
        loaders: [
            { 
                test: /\.(scss|sass|css)$/i, 
                loader: extractSass.extract(
                    [
                        'css-loader?sourceMap', 
                        'resolve-url-loader', 
                        'sass-loader?sourceMap'
                    ])
            },
            { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: 'url-loader?limit=100000' }
        ]
    },
    plugins: [
        extractSass,
        new webpack.DefinePlugin({
            'process.env': {
              NODE_ENV: JSON.stringify('production')
            }
        }),
        new webpack.optimize.UglifyJsPlugin({ minimize: true, compressor: { warnings: false } })
    ]
};