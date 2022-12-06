import React, { useEffect, useState } from 'react';
import {
    Dropdown,
    Page,
    collectionSelectorHelpers,
    Search
} from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
import LinearProgress from '@mui/material/LinearProgress';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Grid from '@mui/material/Grid';
import { useLocation } from 'react-router';
import queryString from 'query-string';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import { useSelector, useDispatch } from 'react-redux';

import ManageSearchIcon from '@mui/icons-material/ManageSearch';
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
import partsActions from '../../actions/partsActions';

// unique id generator
const uid = () => Date.now().toString(36) + Math.random().toString(36).substr(2);

function BomUtility() {
    const reduxDispatch = useDispatch();
    const { search } = useLocation();
    const { bomName } = queryString.parse(search);
    const [crNumber, setCrNumber] = useState(12345);
    const [changeRequests, changeRequestsLoading] = useInitialise(
        () => changeRequestsActions.search(bomName),
        changeRequestsItemType.item,
        'searchItems'
    );

    const [treeView, setTreeView] = useState();

    const [partLookUp, setPartLookUp] = useState({ open: false, forRow: null });

    const url = `/purchasing/boms/tree?bomName=${bomName}&levels=${0}&requirementOnly=${false}&showChanges=${false}&treeType=${'bom'}`;
    const [bomTree, bomTreeLoading] = useInitialise(
        () => bomTreeActions.fetchByHref(url),
        bomTreeItemType.item
    );

    const openPartLookUp = forRow => {
        setPartLookUp({ open: true, forRow });
    };

    const [partSearchTerm, setPartSearchTerm] = useState();

    const searchParts = searchTerm => reduxDispatch(partsActions.search(searchTerm));
    const partsSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(
            reduxState.parts,
            100,
            'id',
            'partNumber',
            'description'
        )
    );
    const partsSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState.parts)
    );

    const partLookUpCell = params => (
        <>
            <span>
                {params.row.name}
                <IconButton onClick={() => openPartLookUp(params.row)}>
                    <ManageSearchIcon />
                </IconButton>
            </span>
        </>
    );

    const columns = [
        { field: 'id', headerName: 'Id', width: 100, hide: true },
        { field: 'parent', headerName: 'Id', width: 100, hide: true },
        { field: 'type', headerName: 'Type', width: 100, editable: true },
        {
            field: 'name',
            headerName: 'Part',
            width: 180,
            renderCell: partLookUpCell,
            align: 'right'
        },
        { field: 'description', headerName: 'Description', width: 500 },
        { field: 'qty', headerName: 'Qty', width: 100, editable: true, type: 'numer' }
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
    const updateTree = (tree, newNode, addNode) => {
        const newTree = { ...tree };
        const q = [];
        q.push(newTree);
        while (q.length !== 0) {
            let n = q.length;
            while (n > 0) {
                const current = q[0];
                q.shift();
                if (current.id === newNode.parent) {
                    if (addNode) {
                        current.children = [
                            ...current.children.filter(x => x.id !== newNode.id),
                            { ...newNode, children: [{ id: uid(), type: 'C', parent: newNode.id }] }
                        ];
                    } else {
                        current.children = [
                            ...current.children.filter(x => x.id !== newNode.id),
                            newNode
                        ];
                    }

                    return newTree;
                }
                // if (current.id === newNode.id) {
                //     q[0] = { ...newNode, children: current.children };
                //     console.log(q[0]);
                //     return newTree;
                // }
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

    const processRowUpdate = React.useCallback(newRow => {
        // if (newRow.name) {
        setTreeView(tree => updateTree(tree, newRow, false));
        // }
        console.log(newRow);
        return newRow;
    }, []);

    // add a new line to the children list of the selected node
    const addLine = () => {
        setTreeView(tree => updateTree(tree, { id: uid(), type: 'C', parent: selected.id }, true));
    };

    const getRows = () => {
        if (selected) {
            const node = getNode(selected.id);
            if (node?.children) return node.children;
        }
        return [];
    };

    function renderPartLookUp() {
        return (
            <Dialog open={partLookUp.open}>
                <DialogTitle>Search For A Part</DialogTitle>
                <DialogContent dividers>
                    <Search
                        propertyName="partNuimber"
                        label="Part Number"
                        resultsInModal
                        resultLimit={100}
                        value={partSearchTerm}
                        handleValueChange={(_, newVal) => setPartSearchTerm(newVal)}
                        search={searchParts}
                        searchResults={partsSearchResults}
                        loading={partsSearchLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={newValue => {
                            setPartLookUp(p => ({ ...p, selectedPart: newValue }));
                            processRowUpdate({
                                ...partLookUp.forRow,
                                name: newValue.partNumber,
                                type: newValue.bomType,
                                description: newValue.description
                            });
                            setPartLookUp({ open: false, forRow: null });
                        }}
                        clearSearch={() => {}}
                    />
                </DialogContent>
                <DialogActions>
                    <Button
                        onClick={() =>
                            setPartLookUp({ open: false, forRow: null, selectedPart: null })
                        }
                    >
                        Cancel
                    </Button>
                </DialogActions>
            </Dialog>
        );
    }

    return (
        <Page history={history} homeUrl={config.appRoot}>
            {renderPartLookUp()}
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
