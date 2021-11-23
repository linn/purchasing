export const toTitleCase = str =>
    str
        .replace('-', ' ')
        .replace(
            /([^\W_]+[^\s-]*) */g,
            txt => txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase()
        );

export const isUpperCase = str => str === str.toUpperCase();

export const isEmpty = str => !str || str.trim().length === 0;
