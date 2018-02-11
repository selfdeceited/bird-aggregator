var webpack = require('webpack');
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const extractSass = new ExtractTextPlugin("my-styles.css");
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');

module.exports = {
    module: {
        rules: [
            {
                test: /\.(scss|sass|css)$/i, 
                use: ExtractTextPlugin.extract({
                    use: [
                        { loader: 'css-loader?sourceMap' },
                        { loader: 'resolve-url-loader' },
                        { loader: 'sass-loader?sourceMap' }
                    ]
                })
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
        new UglifyJSPlugin(
            { 
                sourceMap: true, 
                parallel: true,
                uglifyOptions: {
                    compress: {
                        warnings: false,
                        comparisons: false, // to avoid https://github.com/mapbox/mapbox-gl-js/issues/4359
                        pure_getters: true,
                        unsafe_comps: true
                    }
                },
            }),
        new webpack.LoaderOptionsPlugin({
                 minimize: true
           }),
        new BundleAnalyzerPlugin({analyzerMode: 'disabled', generateStatsFile: true})
    ],
    devtool: "source-map",
};