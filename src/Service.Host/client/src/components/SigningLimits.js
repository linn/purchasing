import React, { useEffect } from 'react';
import Typography from '@material-ui/core/Typography';
import { useSelector, useDispatch } from 'react-redux';
import { Page } from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';

import { getItems, getLoading } from '../selectors/CollectionSelectorHelpers';
import signingLimitsActions from '../actions/signingLimitsActions';
import history from '../history';

function SigningLimits() {
    const signingLimits = useSelector(state => getItems(state.signingLimits));
    const signingLimitsLoading = useSelector(state => getLoading(state.signingLimits));
    const dispatch = useDispatch();

    useEffect(() => dispatch(signingLimitsActions.fetch()), [dispatch]);

    const displayLimits = limits => {
        if (!limits) {
            return [];
        }

        return limits.map(s => ({ ...s, id: s.userNumber, name: s.user?.fullName }));
    };

    const columns = [
        { field: 'userNumber', headerName: 'User Id', width: 140 },
        { field: 'name', headerName: 'Name', width: 300 },
        { field: 'productionLimit', headerName: 'Production Limit', width: 200, editable: true },
        { field: 'sundryLimit', headerName: 'Sundry Limit', width: 200, editable: true },
        { field: 'returnsAuthorisation', headerName: 'Returns', width: 140, editable: true },
        { field: 'unlimited', headerName: 'Unlimited', width: 140, editable: true }
    ];

    return (
        <Page history={history}>
            <Typography variant="h6">Signing Limits</Typography>
            <div style={{ height: 500, width: '100%' }}>
                <DataGrid
                    rows={displayLimits(signingLimits)}
                    columns={columns}
                    density="compact"
                    autoHeight
                    rowHeight={34}
                    loading={signingLimitsLoading}
                    hideFooter
                />
            </div>
        </Page>
    );
}

export default SigningLimits;
