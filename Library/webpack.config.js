const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExctractPlugin = require('mini-css-extract-plugin');
const loader = require('sass-loader');
const path = require("path");

let mode = 'development'
if (process.env.NODE_ENV === 'production') {
    mode = 'production'
}

console.log(mode + ' mode')

module.exports = {
    mode: mode,
    // "./src/js/script.js",
    entry: {
        common: "./src/js/common.js",
        books: "./src/js/books.js",
        authors: "./src/js/authors.js",
        genres: "./src/js/genres.js",
    },
    output: {
        path: path.resolve(__dirname, "wwwroot"),
        //filename: "script.js",
        //assetModuleFilename: "assets/[hash][ext][query]",
        clean: true,
    },
    devtool: 'source-map',
    plugins: [
        new MiniCssExctractPlugin({
            filename: '[name].[contenthash].css'
        }),
        new HtmlWebpackPlugin( {
            chunks: ["books", "common"],
            template: "./src/index.html"
            }
        ), // Generates default index.html
        new HtmlWebpackPlugin( {
            chunks: ["authors", "common"],
            filename: "authors.html",
            template: "./src/authors.html"
        }),
        new HtmlWebpackPlugin( {
            chunks: ["genres", "common"],
            filename: "genres.html",
            template: "./src/genres.html"
        })
    ],
    module: {
        rules: [
            {
                test: /\.html$/i,
                loader: 'html-loader',
            },
            {
                test: /\.(sa|sc|c)ss$/,
                use: [
                    (mode === 'development') ? "style-loader" : MiniCssExctractPlugin.loader,
                    "css-loader",
                    {
                        loader: "postcss-loader",
                        options: {
                            postcssOptions: {
                                plugins: [
                                    [
                                        "postcss-preset-env",
                                        {
                                            // Options
                                        },
                                    ]
                                ]
                            }
                        }
                    },
                    "sass-loader",
                    {
                        loader: 'sass-resources-loader',
                        options: {
                            resources: [
                                path.resolve(__dirname, './src/scss/vars.scss'),
                            ]
                        }
                    }
                ]
            },
            {
                test: /\.(png|svg|jpg|jpeg|gif)$/i,
                type: 'asset/resource',
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/i,
                type: 'asset/resource',
            },
        ]
    },
}