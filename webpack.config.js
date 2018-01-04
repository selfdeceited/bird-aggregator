var path = require('path');
var merge = require('extendify')({ isDeep: true, arrays: 'concat' });
const devConfig = require('./webpack.config.dev');
const prodConfig = require('./webpack.config.prod');

const isDevelopment = process.env.NODE_ENV === 'development';

//todo: refactor & extract common config here

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
                    { loader: "awesome-typescript-loader" },
                    { loader: "tslint-loader", options: {
                        configFile: "tslint.json"
                    } }
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
    plugins: [],
    // When importing a module whose path matches one of the following, just
    // assume a corresponding global variable exists and use that instead.
    // This is important because it allows us to avoid bundling all of our
    // dependencies, which allows browsers to cache those libraries between builds.
}, isDevelopment ? devConfig : prodConfig);