import { ProcessActions } from '@linn-it/linn-form-components-library';
import { bulkLeadTimesUpload } from '../itemTypes';
import { bulkLeadTimesUploadActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    bulkLeadTimesUpload.item,
    bulkLeadTimesUpload.actionType,
    bulkLeadTimesUpload.uri,
    actionTypes,
    config.appRoot,
    'text/csv'
);
