"use strict";
const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const HtmlWebpackDeployAssetsPlugin = require('html-webpack-deploy-assets-plugin');

module.exports = {
    entry: {
        page: './Scripts/page.ts',
        login: './Scripts/login.ts',
        datamartslist: './Scripts/components/DataMartsList.ts',
        datamart: './Scripts/DataMart',
        home: './Scripts/Home.ts',
        routedetail: './Scripts/RouteDetail.ts',
        'application-logs': './Scripts/application-logs.ts'
    },
    plugins: [
        new CleanWebpackPlugin(),
        new HtmlWebpackDeployAssetsPlugin({
            "packages": {

                "bootstrap": {
                    "assets": {
                        "dist/css": "css/",
                        "dist/js": "js/"
                    },
                    "entries": []
                },
                "jquery": {
                    "assets": {
                        "dist": "./"
                    }
                }
            },
            "outputPath": "../assets/[name]"
        })
    ],
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                loader: 'ts-loader'
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader']
            },
            {
                test: require.resolve("lodash"),
                loader: "expose-loader",
                options: {
                    exposes: ["_"]
                }
            }
        ]
    },
    resolve: {
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        },
        extensions: ['.tsx', '.ts', '.js', '.vue', '.json']
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'wwwroot/scripts')
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                commons: {
                    name: 'commons',
                    chunks: 'initial',
                    minChunks: 2
                }
            }
        }
    }
};