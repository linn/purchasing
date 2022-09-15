import React from 'react';
import { Page } from '@linn-it/linn-form-components-library';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import LinearProgress from '@mui/material/LinearProgress';

import TreeView from '@mui/lab/TreeView';
import TreeItem from '@mui/lab/TreeItem';
import history from '../history';
import config from '../config';

const set = new Set();

export default function BomTree() {
    const root = {
        bomName: 'SK HUB',
        children: [{ bomName: 'SOME SWEET BOM' }, { bomName: 'B' }, { bomName: 'C' }]
    };
    const fakeState = [
        { root },
        {
            bomName: 'SOME SWEET BOM',
            children: [{ bomName: 'x' }, { bomName: 'y' }, { bomName: 'z' }]
        },
        {
            bomName: 'x',
            children: [{ bomName: 'xx' }, { bomName: 'yx' }, { bomName: 'zx' }]
        }
    ];
    const [expanded, setExpanded] = React.useState([root.bomName]);

    const handleToggle = (event, nodeIds) => {
        const previousLength = set.size;
        nodeIds.forEach(id => {
            set.add(id);
        });
        if (set.size !== previousLength) {
            console.log(`fetching  ${Array.from(set).pop()}`);
        }

        set.add(event.target.children);
        setExpanded(nodeIds);
    };

    const renderNode = nodeName => {
        const nodes = fakeState.find(x => x.bomName === nodeName)?.children;
        if (nodes) {
            return nodes.map(c => (
                <TreeItem nodeId={c.bomName} label={c.bomName}>
                    {renderNode(c.bomName)}
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
                <TreeItem nodeId={root.bomName} label={root.bomName}>
                    {root.children.map(c => (
                        <TreeItem id={c.bomName} nodeId={c.bomName} label={c.bomName}>
                            {renderNode(c.bomName)}
                        </TreeItem>
                    ))}
                </TreeItem>
            </TreeView>
        </Page>
    );
}
