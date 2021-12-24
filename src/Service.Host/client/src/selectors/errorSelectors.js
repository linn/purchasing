export const getItemError = (state, itemName) =>
    state.errors.itemErrors?.find(e => e.item === itemName);

export const getRequestErrors = state => state.errors.getRequestErrors;
