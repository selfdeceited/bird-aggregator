var webpack = require('webpack');
const ExtractTextPlugin = require("extract-text-webpack-plugin");

const extractSass = new ExtractTextPlugin("my-styles.css");


module.exports = {
    module: {
        rules: [
            {
                test: /\.(scss|sass|css)$/i, 
                use: [
                    { loader: 'css-loader?sourceMap' },
                    { loader: 'resolve-url-loader' },
                    { loader: 'sass-loader?sourceMap' }, 
                   ]
            },
            {
                test: /\.(png|woff|woff2|eot|ttf|svg)$/, 
                use: ['url-loader?limit=100000']
            }
        ]
    },
    plugins: [
        extractSass,
        new webpack.DefinePlugin({
            'process.env': {
              NODE_ENV: JSON.stringify('production')
            }
        }),
        new webpack.optimize.UglifyJsPlugin({ compress: { warnings: false }, sourceMap: true }),
        new webpack.LoaderOptionsPlugin({
                 minimize: true
           })
    ]
};