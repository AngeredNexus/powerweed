const {VueLoaderPlugin} = require('vue-loader'), path = require("path");
const bundleOutputDir = './wwwroot/dist';

module.exports = () => {

    const isDevBuild = !(process.env.NODE_ENV && process.env.NODE_ENV === 'production')
    
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
                // {
                //     test: /\.jsx?$/,
                //     loader: 'babel-loader',
                //     exclude: /node_modules/,
                // }
            ]
        },
        plugins: [
            new VueLoaderPlugin(),
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

