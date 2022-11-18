import React, { useEffect, useMemo } from 'react';
import { Page, Loading, ExportButton } from '@linn-it/linn-form-components-library';
import { useLocation } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import SvgIcon from '@mui/material/SvgIcon';
import { alpha, styled } from '@mui/material/styles';
import TreeView from '@mui/lab/TreeView';
import TreeItem, { treeItemClasses } from '@mui/lab/TreeItem';
import Grid from '@mui/material/Grid';
import queryString from 'query-string';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import history from '../history';
import config from '../config';
import { bomTree as bomTreeItemType } from '../itemTypes';
import bomTreeActions from '../actions/bomTreeActions';

/* eslint react/jsx-props-no-spreading: 0 */
/* eslint react/destructuring-assignment: 0 */

function MinusSquare(props) {
    return (
        <SvgIcon fontSize="inherit" style={{ width: 14, height: 14 }} {...props}>
            <path d="M22.047 22.074v0 0-20.147 0h-20.12v0 20.147 0h20.12zM22.047 24h-20.12q-.803 0-1.365-.562t-.562-1.365v-20.147q0-.776.562-1.351t1.365-.575h20.147q.776 0 1.351.575t.575 1.351v20.147q0 .803-.575 1.365t-1.378.562v0zM17.873 11.023h-11.826q-.375 0-.669.281t-.294.682v0q0 .401.294 .682t.669.281h11.826q.375 0 .669-.281t.294-.682v0q0-.401-.294-.682t-.669-.281z" />
        </SvgIcon>
    );
}

function PlusSquare(props) {
    return (
        <SvgIcon fontSize="inherit" style={{ width: 14, height: 14 }} {...props}>
            <path d="M22.047 22.074v0 0-20.147 0h-20.12v0 20.147 0h20.12zM22.047 24h-20.12q-.803 0-1.365-.562t-.562-1.365v-20.147q0-.776.562-1.351t1.365-.575h20.147q.776 0 1.351.575t.575 1.351v20.147q0 .803-.575 1.365t-1.378.562v0zM17.873 12.977h-4.923v4.896q0 .401-.281.682t-.682.281v0q-.375 0-.669-.281t-.294-.682v-4.896h-4.923q-.401 0-.682-.294t-.281-.669v0q0-.401.281-.682t.682-.281h4.923v-4.896q0-.401.294-.682t.669-.281v0q.401 0 .682.281t.281.682v4.896h4.923q.401 0 .682.281t.281.682v0q0 .375-.281.669t-.682.294z" />
        </SvgIcon>
    );
}

function CloseSquare(props) {
    return (
        <SvgIcon className="close" fontSize="inherit" style={{ width: 14, height: 14 }} {...props}>
            <path d="M17.485 17.512q-.281.281-.682.281t-.696-.268l-4.12-4.147-4.12 4.147q-.294.268-.696.268t-.682-.281-.281-.682.294-.669l4.12-4.147-4.12-4.147q-.294-.268-.294-.669t.281-.682.682-.281.696 .268l4.12 4.147 4.12-4.147q.294-.268.696-.268t.682.281 .281.669-.294.682l-4.12 4.147 4.12 4.147q.294.268 .294.669t-.281.682zM22.047 22.074v0 0-20.147 0h-20.12v0 20.147 0h20.12zM22.047 24h-20.12q-.803 0-1.365-.562t-.562-1.365v-20.147q0-.776.562-1.351t1.365-.575h20.147q.776 0 1.351.575t.575 1.351v20.147q0 .803-.575 1.365t-1.378.562v0z" />
        </SvgIcon>
    );
}

const StyledTreeItem = styled(props => <TreeItem {...props} />)(({ theme }) => ({
    [`& .${treeItemClasses.iconContainer}`]: {
        '& .close': {
            opacity: 0.3
        }
    },
    [`& .${treeItemClasses.group}`]: {
        marginLeft: 15,
        paddingLeft: 18,
        borderLeft: `1px dashed ${alpha(theme.palette.text.primary, 0.4)}`
    }
}));

export default function BomTree() {
    const dispatch = useDispatch();
    const { search } = useLocation();
    const { bomName, levels, requirementOnly, showChanges, treeType } = queryString.parse(search);

    const bomTree = useSelector(state => state[bomTreeItemType.item].item);

    const bomTreeLoading = useSelector(state => state[bomTreeItemType.item].loading);

    useEffect(() => {
        dispatch(
            bomTreeActions.fetchByHref(
                `/purchasing/boms/tree?bomName=${bomName}&levels=${levels}&requirementOnly=${requirementOnly}&showChanges=${showChanges}&treeType=${treeType}`
            )
        );
    }, [bomName, levels, requirementOnly, showChanges, treeType, dispatch]);

    const nodesWithChildren = useMemo(() => {
        const result = [];
        if (!bomTree) {
            return result;
        }
        const q = [];
        q.push(bomTree);
        while (q.length !== 0) {
            let n = q.length;
            while (n > 0) {
                const current = q[0];
                q.shift();
                if (current.children?.length) {
                    result.push(current.name);
                    for (let i = 0; i < current.children.length; i += 1) {
                        q.push(current.children[i]);
                    }
                }
                n -= 1;
            }
        }
        return result;
    }, [bomTree]);

    const renderTree = nodes => {
        const label = (
            <>
                <Typography
                    display="inline"
                    variant="subtitle1"
                    color={!nodes.children?.length ? '' : 'primary'}
                >
                    {nodes.name}
                </Typography>
                {nodes.name !== bomName && (
                    <>
                        <Typography display="inline" variant="subtitle2">
                            {' '}
                            (x{nodes.qty})
                        </Typography>
                        <Typography display="inline" variant="caption">
                            {' - '}
                            {nodes.description}
                        </Typography>
                    </>
                )}
            </>
        );
        return (
            <StyledTreeItem key={nodes.name} nodeId={nodes.name} label={label}>
                {Array.isArray(nodes.children)
                    ? nodes.children.map(node => renderTree(node))
                    : null}
            </StyledTreeItem>
        );
    };

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
                {bomTree && (
                    <>
                        <Grid item xs={12}>
                            <Typography variant="h4" color="primary" display="inline">
                                {bomTree.name}
                            </Typography>
                        </Grid>
                        <Grid item xs={12}>
                            <TreeView
                                aria-label="customized"
                                defaultCollapseIcon={<MinusSquare />}
                                defaultExpandIcon={<PlusSquare />}
                                defaultEndIcon={<CloseSquare />}
                                expanded={nodesWithChildren}
                            >
                                {renderTree(bomTree)}
                            </TreeView>
                        </Grid>
                    </>
                )}
                {bomTreeLoading && (
                    <>
                        <Grid item xs={12}>
                            <Typography variant="subtitle2">
                                Filling out the tree... May take a while...
                            </Typography>
                        </Grid>
                        <Grid item xs={12}>
                            <Loading />
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}
