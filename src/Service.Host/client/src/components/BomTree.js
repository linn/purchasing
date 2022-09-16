import React, { useEffect, useState } from 'react';
import { Page } from '@linn-it/linn-form-components-library';
import { useDispatch, useSelector } from 'react-redux';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import LinearProgress from '@mui/material/LinearProgress';

import TreeView from '@mui/lab/TreeView';
import TreeItem from '@mui/lab/TreeItem';
import history from '../history';
import config from '../config';
import bomTreeNodeActions from '../actions/bomTreeNodeActions';

const set = new Set();

export default function BomTree() {
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(bomTreeNodeActions.fetch(40149));
    }, [dispatch]);

    const boms = useSelector(state => state.bomTreeNodes.items);

    const root = boms.find(x => x.bomId === 40149);

    const [expanded, setExpanded] = useState(['SK HUB']);

    const handleToggle = (_, nodeIds) => {
        console.log(nodeIds);
        const previousLength = set.size;
        nodeIds.forEach(id => {
            if (id) {
                set.add(id);
            }
        });
        if (set.size !== previousLength) {
            console.log(`fetching  ${Array.from(set).pop()}`);
            dispatch(bomTreeNodeActions.fetch(Array.from(set).pop()));
        }

        //set.add(event.target.children);
        setExpanded(nodeIds);
    };

    const renderNode = bomName => {
        const nodes = boms.find(x => x.bomName === bomName)?.children;
        if (nodes) {
            return nodes.map(c => (
                <TreeItem nodeId={c.bomId ?? c.partNumber} label={c.partNumber}>
                    {c.partNumber}
                </TreeItem>
            ));
        }
        return <LinearProgress />;
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <TreeView
                aria-label="controlled"
                defaultCollapseIcon={<ExpandMoreIcon />}
                defaultExpandIcon={<ChevronRightIcon />}
                expanded={expanded}
                selected={false}
                onNodeToggle={handleToggle}
                multiSelect
            >
                {root && (
                    <TreeItem nodeId={root.bomName} label={root.bomName}>
                        {root?.children?.map(c => (
                            <TreeItem
                                id={c.bomId ?? c.partNumber}
                                nodeId={c.bomId ?? c.partNumber}
                                label={c.partNumber}
                            >
                                {renderNode(c.partNumber)}
                            </TreeItem>
                        ))}
                    </TreeItem>
                )}
            </TreeView>
        </Page>
    );
}
