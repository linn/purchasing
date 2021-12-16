export const getReportState = reportState => reportState || {};

export const getReportData = state => {
    const reportState = getReportState(state);
    return reportState.results ? reportState.results.data : null;
};

export const getReportLoading = state => {
    const reportState = getReportState(state);
    return reportState.results ? reportState.results.loading : false;
};

export const getReportOptions = state => {
    const reportState = getReportState(state);
    return reportState.options;
};
