//var path = require('path');

module.exports = {
    devtool: 'inline-source-map',
    module: {
        loaders: [
            { 
                test: /\.(scss|sass|css)$/i, loader: 'css-loader?sourceMap!resolve-url-loader!sass-loader?sourceMap',
                options: {
                    includePaths: ["/node_modules"]
                }
            },
            { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: 'url-loader?limit=100000' } 
        ],
    }
};