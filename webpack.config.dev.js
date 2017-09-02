module.exports = {
    devtool: 'inline-source-map',
    module: {
        rules: [
            { 
                test: /\.(scss|sass|css)$/i, 
                use: ['css-loader?sourceMap','resolve-url-loader','sass-loader?sourceMap'],
                options: {
                    includePaths: ["/node_modules"]
                }
            },
            { test: /\.(png|woff|woff2|eot|ttf|svg)$/, use: ['url-loader?limit=100000'] } 
        ],
    }
};