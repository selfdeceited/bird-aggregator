var webpack = require('webpack');
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const extractSass = new ExtractTextPlugin("my-styles.css");
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

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
        new webpack.optimize.UglifyJsPlugin(
            { 
                compress: { 
                    warnings: false,
                    comparisons: false,
                    pure_getters: true,
                    unsafe: true,
                    unsafe_comps: true,
                    screw_ie8: true
                },
                sourceMap: true, 
                parallel: true,

            }),
        new webpack.LoaderOptionsPlugin({
                 minimize: true
           }),
        /*new CompressionPlugin({
			asset: "[path].gz[query]",
			algorithm: "gzip",
			test: /\.(js|html)$/,
			threshold: 10240,
			minRatio: 0.8
        }),*/
        new BundleAnalyzerPlugin({analyzerMode: 'disabled', generateStatsFile: true})
    ]
};