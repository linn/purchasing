import React, { useEffect, useState } from 'react';
import { Dropdown, Page } from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
import LinearProgress from '@mui/material/LinearProgress';

import Grid from '@mui/material/Grid';
import { useLocation } from 'react-router';
import queryString from 'query-string';
import Button from '@mui/material/Button';

import BomTree from '../BomTree';
import history from '../../history';
import config from '../../config';
import {
    changeRequests as changeRequestsItemType,
    bomTree as bomTreeItemType
} from '../../itemTypes';

import changeRequestsActions from '../../actions/changeRequestsActions';
import bomTreeActions from '../../actions/bomTreeActions';
import useInitialise from '../../hooks/useInitialise';

// unique id generator
const uid = () => Date.now().toString(36) + Math.random().toString(36).substr(2);

function BomUtility() {
    const { search } = useLocation();
    const { bomName } = queryString.parse(search);
    const [crNumber, setCrNumber] = useState(12345);
    const [changeRequests, changeRequestsLoading] = useInitialise(
        () => changeRequestsActions.search(bomName),
        changeRequestsItemType.item,
        'searchItems'
    );

    const [treeView, setTreeView] = useState();

    const url = `/purchasing/boms/tree?bomName=${bomName}&levels=${0}&requirementOnly=${false}&showChanges=${false}&treeType=${'bom'}`;
    const [bomTree, bomTreeLoading] = useInitialise(
        () => bomTreeActions.fetchByHref(url),
        bomTreeItemType.item
    );

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'parent', headerName: 'Id', width: 100, hide: true },
        { field: 'type', headerName: 'Type', width: 100, editable: true },
        { field: 'name', headerName: 'Part', width: 100, editable: true },
        { field: 'description', headerName: 'Description', width: 500, editable: true },
        { field: 'qty', headerName: 'Qty', width: 100, editable: true }
    ];
    const [selected, setSelected] = useState(null);

    useEffect(() => {
        if (bomTree === null) {
            setSelected({ id: 'root', name: bomName, children: [] });
        }
        setTreeView(
            bomTree !== null || bomTreeLoading
                ? bomTree
                : { id: 'root', name: bomName, children: [] }
        );
    }, [bomTree, bomName, bomTreeLoading]);

    // updates the tree with changes passed via a 'newNode' object
    const updateTree = (tree, newNode) => {
        const newTree = { ...tree };
        const q = [];
        q.push(newTree);
        while (q.length !== 0) {
            let n = q.length;
            while (n > 0) {
                const current = q[0];
                q.shift();
                if (current.id === newNode.parent) {
                    current.children = [
                        ...current.children.filter(x => x.id !== newNode.id),
                        { ...newNode, children: [{ id: uid(), type: 'C', parent: newNode.id }] }
                    ];
                    return newTree;
                }
                for (let i = 0; i < current.children?.length || 0; i += 1) {
                    q.push(current.children[i]);
                }
                n -= 1;
            }
        }
        return newTree;
    };

    // find a node in the tree, by its id field
    const getNode = id => {
        if (treeView == null) return null;
        const q = [];
        q.push(treeView);
        while (q.length !== 0) {
            let n = q.length;
            while (n > 0) {
                const current = q[0];
                q.shift();
                if (current.id === id) return current;
                if (current.children)
                    for (let i = 0; i < current.children.length; i += 1)
                        q.push(current.children[i]);
                n -= 1;
            }
        }
        return null;
    };

    const processRowUpdate = newRow => {
        if (newRow.name) {
            setTreeView(tree => updateTree(tree, newRow));
        }
        return newRow;
    };

    // add a new line to the children list of the selected node
    const addLine = () => {
        setTreeView(tree => updateTree(tree, { id: uid(), type: 'C', parent: selected.id }));
    };

    const getRows = () => {
        if (selected) {
            const node = getNode(selected.id);
            if (node?.children) return node.children;
            return [];
        }
        return [];
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                {changeRequestsLoading ? (
                    <Grid item xs={12}>
                        <LinearProgress />
                    </Grid>
                ) : (
                    <Grid item xs={12}>
                        <Dropdown
                            items={changeRequests?.map(c => ({
                                id: c.documnerNumber,
                                displayText: `${c.documentType}${c.documentNumber}`
                            }))}
                            allowNoValue
                            label="CRF Number"
                            helperText="Select a corresponding CRF to start editing"
                            value={crNumber}
                            onChange={(_, n) => {
                                setCrNumber(n);
                            }}
                        />
                    </Grid>
                )}
                <Grid item xs={4}>
                    <BomTree
                        renderDescriptions={false}
                        renderComponents={false}
                        renderQties={false}
                        onNodeSelect={s => {
                            setSelected(s);
                        }}
                        bomName={bomName}
                        bomTree={treeView}
                        bomTreeLoading={bomTreeLoading}
                    />
                </Grid>
                <Grid item xs={8}>
                    <>
                        <Grid item xs={12}>
                            <DataGrid
                                columnBuffer={6}
                                rows={getRows()}
                                autoHeight
                                processRowUpdate={processRowUpdate}
                                hideFooter
                                experimentalFeatures={{ newEditingApi: true }}
                                checkboxSelection
                                disableSelectionOnClick
                                columns={columns}
                            />
                        </Grid>
                        <Grid item xs={1}>
                            <Button variant="outlined" onClick={() => addLine(selected.id)}>
                                +
                            </Button>
                        </Grid>
                    </>
                </Grid>
            </Grid>
        </Page>
    );
}

export default BomUtility;
