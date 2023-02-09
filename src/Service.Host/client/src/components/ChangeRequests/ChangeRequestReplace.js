import React from 'react';
import { useLocation } from 'react-router-dom';
import { Page, Loading, InputField } from '@linn-it/linn-form-components-library';
import queryString from 'query-string';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import changeRequestActions from '../../actions/changeRequestActions';
import history from '../../history';
import useInitialise from '../../hooks/useInitialise';
import { changeRequest } from '../../itemTypes';

function ChangeRequestReplace() {
    const { search } = useLocation();
    const documentNumber = queryString.parse(search)?.documentNumber;

    const [item, loading] = useInitialise(
        () => changeRequestActions.fetch(documentNumber),
        changeRequest.item
    );

    // const replace = utilities.getHref(item, 'replace');

    return (
        <Page history={history}>
            {loading ? (
                <Loading />
            ) : (
                <Grid container spacing={2} justifyContent="center">
                    <Grid item xs={12}>
                        <Typography variant="h6">Change Request {documentNumber}</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            value={item?.oldPartNumber}
                            label="Old Part"
                            propertyName="oldPartNumber"
                            disabled
                        />
                        <Typography>{item?.newPartDescription}</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            value={item?.newPartNumber}
                            label="New Part"
                            propertyName="newPartNumber"
                            disabled
                        />
                        <Typography>{item?.newPartDescription}</Typography>
                    </Grid>
                    <Grid item xs={12}>
                        coming soon
                    </Grid>
                </Grid>
            )}
        </Page>
    );
}

export default ChangeRequestReplace;
