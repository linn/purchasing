import React from 'react';
import { Page, ExportButton } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import queryString from 'query-string';
import { useLocation } from 'react-router-dom';
import Button from '@mui/material/Button';
import history from '../history';
import config from '../config';
import BomTree from './BomTree';

export default function BomTreeReport() {
    const { search } = useLocation();

    const { bomName, levels, requirementOnly, showChanges, treeType } = queryString.parse(search);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <ExportButton
                        href={`${config.appRoot}/purchasing/boms/tree/export?bomName=${bomName}&levels=${levels}&requirementOnly=${requirementOnly}&showChanges=${showChanges}&treeType=${treeType}`}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        onClick={() =>
                            history.push(`/purchasing/boms/tree/options?bomName=${bomName}`)
                        }
                    >
                        Back
                    </Button>
                </Grid>
                <Grid item xs={10} />
                <BomTree />
            </Grid>
        </Page>
    );
}
