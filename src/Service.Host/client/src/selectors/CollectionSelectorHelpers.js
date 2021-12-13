export const getItems = storeItems => {
    if (!storeItems) {
        return [];
    }

    return storeItems.items ? storeItems.items : [];
};

export const getSearchItems = (
    storeItems,
    limit = null,
    idFieldName = null,
    nameFieldName = null,
    descriptionFieldName = null
) => {
    let result = [];

    if (!storeItems) {
        return result;
    }

    if (limit) {
        result = storeItems.searchItems ? storeItems.searchItems.slice(0, limit) : [];
    }

    if (idFieldName) {
        result = result.map(x => ({ ...x, id: x[idFieldName] }));
    }

    if (nameFieldName) {
        result = result.map(x => ({ ...x, name: x[nameFieldName] }));
    }

    if (descriptionFieldName) {
        result = result.map(x => ({ ...x, description: x[descriptionFieldName] }));
    }

    return result || [];
};

export const getItem = (storeItems, id, idField = 'id') => {
    const items = this.getItems(storeItems);
    return items.find(a => a[idField] === id);
};

export const getItemByHref = (storeItems, href) => storeItems.find(a => a.href === href);

export const getLoading = storeItems => {
    if (!storeItems) {
        return null;
    }

    return storeItems.loading;
};

export const getSearchLoading = storeItems => {
    if (!storeItems) {
        return null;
    }

    return storeItems.searchLoading;
};

export const getLinks = storeItems => {
    if (!storeItems) {
        return [];
    }

    return storeItems.links ? storeItems.links : [];
};

export const hasPrivilege = (storeItems, rel) => {
    const links = getLinks(storeItems);

    return links ? links.some(l => l.rel === rel) : false;
};

export const getApplicationState = storeItems => {
    if (!storeItems) {
        return null;
    }

    return storeItems.applicationState ? storeItems.applicationState : null;
};

export const getApplicationStateLoading = storeItems => {
    if (!storeItems) {
        return false;
    }

    return storeItems.applicationState ? storeItems.applicationState.loading : false;
};
