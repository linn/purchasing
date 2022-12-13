import React from 'react';
import { Page, ExportButton } from '@linn-it/linn-form-components-library';
import Grid from '@mui/material/Grid';
import queryString from 'query-string';
import { useLocation } from 'react-router-dom';
import Button from '@mui/material/Button';
import history from '../history';
import config from '../config';
import BomTree from './BomTree';
import bomTreeActions from '../actions/bomTreeActions';
import useInitialise from '../hooks/useInitialise';
import { bomTree as bomTreeItemType } from '../itemTypes';
import useExpandNodesWithChildren from '../hooks/useExpandNodesWithChildren';

export default function BomTreeReport() {
    const { search } = useLocation();

    const { bomName, levels, requirementOnly, showChanges, treeType } = queryString.parse(search);
    const url = `/purchasing/boms/tree?bomName=${bomName}&levels=${levels ?? 0}&requirementOnly=${
        requirementOnly ?? false
    }&showChanges=${showChanges ?? false}&treeType=${treeType ?? 'bom'}`;
    const [bomTree, bomTreeLoading] = useInitialise(
        () => bomTreeActions.fetchByHref(url),
        bomTreeItemType.item
    );

    const [expanded, setExpanded] = useExpandNodesWithChildren([], bomTree, bomName);
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
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        onClick={() =>
                            history.push(`/purchasing/boms/bom-utility?bomName=${bomName}`)
                        }
                    >
                        BOM UT
                    </Button>
                </Grid>
                <Grid item xs={10} />
                <BomTree
                    bomTree={bomTree}
                    bomTreeLoading={bomTreeLoading}
                    bomName={bomName}
                    expanded={expanded}
                    setExpanded={setExpanded}
                />
            </Grid>
        </Page>
    );
}
