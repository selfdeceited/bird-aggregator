var path = require('path');
var merge = require('extendify')({ isDeep: true, arrays: 'concat' });
var devConfig = require('./webpack.config.dev');
var prodConfig = require('./webpack.config.prod');
var isDevelopment = process.env.ASPNETCORE_ENVIRONMENT === 'Development';

module.exports = merge({
    resolve: {
        extensions: ['.js', '.jsx', '.ts', '.tsx', '.scss'],
        alias: {
            "react": __dirname + '/node_modules/react',
        }
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                exclude: /node_modules/,
                use: [
                    { loader: "awesome-typescript-loader" }
                ]
            },
            {
                test: /\.js$/,
                enforce: "pre",
                use: [
                    { loader: "source-map-loader" }
                ]
            }
        ]
    },
    entry: {
        main: ['./src/index']
    },
    output: {
        path: path.join(__dirname, 'wwwroot', 'dist'),
        filename: 'bundle.js',
        publicPath: '/dist/'
    },
    devtool: "source-map",
    plugins: [],
    // When importing a module whose path matches one of the following, just
    // assume a corresponding global variable exists and use that instead.
    // This is important because it allows us to avoid bundling all of our
    // dependencies, which allows browsers to cache those libraries between builds.
}, isDevelopment ? devConfig : prodConfig);