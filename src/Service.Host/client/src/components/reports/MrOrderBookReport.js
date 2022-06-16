import React, { useEffect, useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import queryString from 'query-string';
import {
    Page,
    Title,
    Typeahead,
    collectionSelectorHelpers,
    ExportButton,
    Loading,
    Dropdown,
    DatePicker,
    MultiReportTable
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import history from '../../history';
import config from '../../config';