const {VueLoaderPlugin} = require('vue-loader'), path = require("path");
const bundleOutputDir = './wwwroot/dist';
const webpack = require('webpack')

module.exports = () => {

    const isDevBuild = !(process.env.NODE_ENV && process.env.NODE_ENV === 'production')
    
    console.log(isDevBuild);
    
    return [{
        entry: { main: './Client/main.js' },
        mode: (isDevBuild ? 'development' : 'production'),
        devtool: isDevBuild ? "source-map" : false,
        module: {
            rules: [
                {
                    test: /\.vue$/,
                    loader: 'vue-loader',
                },
                {
                    test: /\.css$/,
                    use: [
                        'vue-style-loader',
                        'css-loader',
                    ]
                },
                {
                    test: /\.(png|jpe?g|gif|svg)$/i,
                    use: [
                        {
                            loader: 'file-loader',
                        },
                    ],
                },
                {
                    test: /\.js?$/,
                    exclude: /node_modules/,
                    use: [
                        {
                            loader: 'babel-loader',
                            query: {
                                plugins: ['lodash'],
                            },
                        },
                        {
                            loader: 'thread-loader'
                        }
                    ],
                    
                }
            ]
        },
        plugins: [
            new VueLoaderPlugin(),
            new webpack.EnvironmentPlugin([
                'APP_API_HOST'
            ]),
        ],
        output: {
            path: path.join(__dirname, bundleOutputDir),
            filename: '[name].js',
            publicPath: '/dist/'
        },
        resolve: {
            alias: {
                'vue$': 'vue/dist/vue.esm-bundler.js',
                '@': path.join(__dirname, "Client"),
            },
            extensions: ['.js']
        },
    }]
}

