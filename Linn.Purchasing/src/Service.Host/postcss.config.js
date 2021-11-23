const autoprefixer = require('autoprefixer');

module.exports = {
    plugins: [
        autoprefixer({
            overrideBrowserslist: ['>1%', 'last 4 versions', 'Firefox ESR', 'not ie < 9']
        })
    ]
};
