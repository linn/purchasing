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

    const [changes, setChanges] = useState({});

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

    const addNodeToTree = (tree, newNode) => {
        const newTree = { ...tree };
        const q = [];
        q.push(newTree);
        while (q.length !== 0) {
            let n = q.length;
            while (n > 0) {
                const current = q[0];
                q.shift();
                if (current.id === newNode.parent) {
                    current.children = [...current.children, newNode];
                }
                for (let i = 0; i < current.children?.length || 0; i += 1) {
                    q.push(current.children[i]);
                }
                n -= 1;
            }
        }

        return newTree;
    };

    const processRowUpdate = newRow => {
        if (newRow.type === 'A' && newRow.name) {
            setTreeView(tree => addNodeToTree(tree, newRow));
        }
        console.log(newRow);
        setChanges(c => ({
            ...c,
            [newRow.parent]: [...c[newRow.parent].filter(x => x.id !== newRow.id), newRow]
        }));

        return newRow;
    };
    const addLine = id => {
        setChanges(c => ({
            ...c,
            [id]: [
                ...(Array.isArray(c[id]) ? c[id] : []),
                { id: -(c[id]?.length || 0) - 1, type: 'C', parent: selected.id }
            ]
        }));
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
                            console.log(s);
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
                                rows={
                                    selected && changes[selected.id]
                                        ? changes[selected.id]
                                        : selected?.children || []
                                }
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
