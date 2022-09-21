import React, { useEffect, useState } from 'react';
import { Page, Loading } from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import { useParams } from 'react-router-dom';

import { useDispatch, useSelector } from 'react-redux';
import SvgIcon from '@mui/material/SvgIcon';
import { alpha, styled } from '@mui/material/styles';
import TreeView from '@mui/lab/TreeView';
import TreeItem, { treeItemClasses } from '@mui/lab/TreeItem';
import Collapse from '@mui/material/Collapse';
import Grid from '@mui/material/Grid';
import { useSpring, animated } from 'react-spring';
import LinearProgress from '@mui/material/LinearProgress';
import Button from '@mui/material/Button';

import Typography from '@mui/material/Typography';
import history from '../history';
import config from '../config';
import bomTreeNodeActions from '../actions/bomTreeNodeActions';

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

function TransitionComponent(props) {
    const style = useSpring({
        from: {
            opacity: 0,
            transform: 'translate3d(20px,0,0)'
        },
        to: {
            opacity: props.in ? 1 : 0,
            transform: `translate3d(${props.in ? 0 : 20}px,0,0)`
        }
    });

    return (
        <animated.div style={style}>
            <Collapse {...props} />
        </animated.div>
    );
}

TransitionComponent.propTypes = {
    in: PropTypes.bool.isRequired
};

const StyledTreeItem = styled(props => (
    <TreeItem {...props} sub TransitionComponent={TransitionComponent} />
))(({ theme }) => ({
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
    const [hasBeenExpanded, setHasBeenExpanded] = useState(new Set());
    const [expandAll, setExpandAll] = useState(false);
    const [expanded, setExpanded] = useState([]);

    const { id } = useParams();
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(bomTreeNodeActions.fetch(id));
    }, [dispatch, id]);

    const boms = useSelector(state => state.bomTreeNodes.items);

    const bomsLoading = useSelector(state => state.bomTreeNodes.loading);

    useEffect(() => {
        if (expandAll) {
            setExpanded(
                Object.keys(boms)
                    .map(b => b.toString())
                    .filter(x => x !== id)
            );
        }
    }, [boms, id, expandAll]);

    const root = boms[Number(id)];

    const handleToggle = (event, nodeIds) => {
        const previousLength = hasBeenExpanded.size;
        nodeIds.forEach(i => {
            if (i) {
                setHasBeenExpanded(l => l.add(i));
            }
        });
        if (hasBeenExpanded.size !== previousLength) {
            dispatch(bomTreeNodeActions.fetch(Array.from(hasBeenExpanded).pop()));
        }
        setExpanded(nodeIds);
    };

    const renderNode = bomId => {
        const nodes = bomId ? boms[bomId.toString()]?.children : [];
        if (nodes) {
            return nodes.map(c => {
                const label = (
                    <>
                        <Typography
                            display="inline"
                            variant="subtitle1"
                            color={c.bomType === 'C' ? '' : 'primary'}
                        >
                            {c.partNumber}
                        </Typography>
                        <Typography display="inline" variant="subtitle2">
                            {' '}
                            (x{c.qty})
                        </Typography>
                        <Typography display="inline" variant="caption">
                            {' - '}
                            {c.partDescription}
                        </Typography>
                    </>
                );
                if (c.bomType === 'C' || !c.bomId) {
                    return <StyledTreeItem id={c.partNumber} nodeId={c.partNumber} label={label} />;
                }

                return (
                    <StyledTreeItem
                        id={`${c.bomId}` ?? c.partNumber}
                        nodeId={`${c.bomId}` ?? c.partNumber}
                        label={label}
                    >
                        {renderNode(c.bomId)}
                    </StyledTreeItem>
                );
            });
        }
        return <LinearProgress />;
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                {root && (
                    <>
                        <Grid item xs={12}>
                            <Button onClick={() => setExpandAll(true)}>Expand All</Button>
                            <Button
                                onClick={() => {
                                    setExpandAll(false);
                                    setExpanded([]);
                                }}
                            >
                                Collapse All
                            </Button>
                        </Grid>
                        <Grid item xs={12}>
                            <Typography variant="h4" color="primary" display="inline">
                                {root.bomName} {bomsLoading && '...'}
                            </Typography>
                        </Grid>

                        <Grid item xs={12}>
                            <TreeView
                                aria-label="customized"
                                defaultCollapseIcon={<MinusSquare />}
                                defaultExpandIcon={<PlusSquare />}
                                defaultEndIcon={<CloseSquare />}
                                onNodeToggle={handleToggle}
                                expanded={expanded}
                            >
                                {root && renderNode(root.bomId.toString())}
                            </TreeView>
                        </Grid>
                    </>
                )}
                {!root && (
                    <Grid item xs={6}>
                        <Loading />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}
