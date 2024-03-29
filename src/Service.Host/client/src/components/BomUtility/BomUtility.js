import React, { useEffect, useState, useCallback } from 'react';
import {
    Dropdown,
    Page,
    collectionSelectorHelpers,
    Search,
    itemSelectorHelpers,
    SaveBackCancelButtons,
    SnackbarMessage,
    getItemError,
    ErrorCard,
    InputField,
    OnOffSwitch
} from '@linn-it/linn-form-components-library';
import { DataGrid } from '@mui/x-data-grid';
import LinearProgress from '@mui/material/LinearProgress';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import DialogContentText from '@mui/material/DialogContentText';
import Grid from '@mui/material/Grid';
import { useLocation } from 'react-router';
import queryString from 'query-string';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import { useSelector, useDispatch } from 'react-redux';
import { Link } from 'react-router-dom';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Typography from '@mui/material/Typography';
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
import partsActions from '../../actions/partsActions';
import subAssemblyActions from '../../actions/subAssemblyActions';
import useExpandNodesWithChildren from '../../hooks/useExpandNodesWithChildren';
import usePreviousNextNavigation from '../../hooks/usePreviousNextNavigation';
import PrevNextButtons from '../PrevNextButtons';

// unique id generator
const uid = () => Date.now().toString(36) + Math.random().toString(36).substr(2);

function BomUtility() {
    const reduxDispatch = useDispatch();
    const { search, state } = useLocation();
    const { bomName, changeRequest } = queryString.parse(search);

    const [goPrev, goNext, prevResult, nextResult] = usePreviousNextNavigation(
        id => `/purchasing/boms/bom-utility?bomName=${id}`,
        state?.searchResults,
        'query',
        'bomName'
    );

    const [crNumber, setCrNumber] = useState(changeRequest === 'null' ? null : changeRequest);

    const [showChanges, setShowChanges] = useState(true);
    const [disableChangesButton, setDisableChangesButton] = useState(false);
    const [searchBomTerm, setSearchBomTerm] = useState();

    const url = changes =>
        `/purchasing/boms/tree?bomName=${bomName}&levels=${0}&requirementOnly=${false}&showChanges=${changes}&treeType=${'bom'}`;

    const bomTree = useSelector(reduxState => reduxState[bomTreeItemType.item].item);
    const bomTreeLoading = useSelector(reduxState => reduxState[bomTreeItemType.item].loading);

    const changeRequests = useSelector(
        reduxState => reduxState[changeRequestsItemType.item].searchItems
    );
    const changeRequestsLoading = useSelector(
        reduxState => reduxState[changeRequestsItemType.item].searchLoading
    );

    useEffect(() => {
        if (bomTree?.name !== bomName) {
            reduxDispatch(
                bomTreeActions.fetchByHref(
                    `/purchasing/boms/tree?bomName=${bomName}&levels=${0}&requirementOnly=${false}&showChanges=${showChanges}&treeType=${'bom'}`
                )
            );
            reduxDispatch(
                changeRequestsActions.searchWithOptions(bomName, '&includeAllForBom=True')
            );
        }
    }, [bomName, reduxDispatch, bomTree, showChanges]);

    const [treeView, setTreeView] = useState();

    const { nodesWithChildren } = useExpandNodesWithChildren([], treeView, bomName);

    const [expanded, setExpanded] = useState(['root']);

    const [newAssemblies, setNewAssemblies] = useState([]);

    const [partLookUp, setPartLookUp] = useState({ open: false, forRow: null });

    const subAssembly = useSelector(reduxState =>
        itemSelectorHelpers.getItem(reduxState.subAssembly)
    );
    const subAssemblyLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.subAssembly)
    );
    const snackbarVisible = useSelector(reduxState =>
        itemSelectorHelpers.getSnackbarVisible(reduxState.bomTree)
    );
    const itemError = useSelector(reduxState => getItemError(reduxState, 'bomTree'));

    const [partSearchTerm, setPartSearchTerm] = useState();
    const [partMessage, setPartMessage] = useState();

    const openPartLookUp = forRow => {
        setPartMessage();
        setPartLookUp({ open: true, forRow });
        setPartSearchTerm(null);
    };

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

    const [contextMenu, setContextMenu] = useState(null);
    const [selected, setSelected] = useState(null);

    const onContextMenu = e => {
        e.preventDefault();
        const { target } = e;
        const detail = selected.children.find(x => x.id === target.id);
        const index = selected.children.findIndex(x => x.id === target.id);
        setContextMenu(
            crNumber && contextMenu === null
                ? {
                      mouseX: e.clientX + 2,
                      mouseY: e.clientY - 6,
                      part: target.innerText,
                      position: index + 1,
                      canDelete: !detail.deleteReplaceSeq,
                      canReplace:
                          Number(crNumber) !== detail.addChangeDocumentNumber &&
                          !detail.deleteReplaceSeq &&
                          !detail.replacementFor,
                      detail: { ...detail, parentId: selected.id }
                  }
                : null
        );
    };

    const partLookUpCell = params => (
        <>
            <span id={params.row.id} onContextMenu={crNumber ? onContextMenu : null}>
                {params.row.isReplaced || params.row.toDelete ? (
                    <s id={params.row.id}>{params.row.name}</s>
                ) : (
                    params.row.name
                )}
                <IconButton
                    onClick={() => openPartLookUp(params.row)}
                    data-testid="part-lookup-button"
                    disabled={
                        !crNumber ||
                        subAssemblyLoading ||
                        params.row.isReplaced ||
                        crNumber !== params.row.addChangeDocumentNumber.toString()
                    }
                >
                    <ManageSearchIcon />
                </IconButton>
            </span>
        </>
    );

    const columns = [
        { field: 'parent', headerName: 'Id', width: 100, hide: true },
        {
            field: 'type',
            headerName: 'Type',
            editable: false,
            width: 75,
            renderCell: params =>
                params.row.isReplaced || params.row.toDelete ? (
                    <s>{params.row.type}</s>
                ) : (
                    <span>{params.row.type}</span>
                )
        },
        {
            field: 'name',
            headerName: 'Part',
            width: 175,
            editable: false,
            renderCell: partLookUpCell,
            align: 'right'
        },
        {
            field: 'description',
            headerName: 'Description',
            hide: false,
            width: 350,
            editable: false,
            renderCell: params =>
                params.row.isReplaced || params.row.toDelete ? (
                    <s>{params.row.description}</s>
                ) : (
                    <span>{params.row.description}</span>
                )
        },
        {
            field: 'safetyCritical',
            headerName: 'Safety',
            width: 75,
            editable: false
        },
        {
            field: 'qty',
            headerName: 'Qty',
            width: 75,
            editable: true,
            type: 'number',
            renderCell: params =>
                params.row.isReplaced || params.row.toDelete ? (
                    <s>{params.row.qty}</s>
                ) : (
                    <span>{params.row.qty}</span>
                )
        },
        {
            field: 'requirement',
            headerName: 'Req',
            width: 75,
            editable: true,
            type: 'singleSelect',
            valueOptions: ['Y', 'N']
        },
        {
            field: 'addChangeDocumentNumber',
            headerName: 'Add',
            width: 75,
            hide: !showChanges,
            editable: false,
            renderCell: params => (
                <Link to={`/purchasing/change-requests/${params.row.addChangeDocumentNumber}`}>
                    {params.row.addChangeDocumentNumber}
                </Link>
            )
        },
        {
            field: 'addReplaceSeq',
            headerName: 'In',
            width: 75,
            hide: true,

            editable: false
        },
        {
            field: 'deleteChangeDocumentNumber',
            headerName: 'Del',
            width: 75,
            hide: !showChanges,
            editable: false,
            renderCell: params =>
                params.row.deleteChangeDocumentNumber ? (
                    <Link
                        to={`/purchasing/change-requests/${params.row.deleteChangeDocumentNumber}`}
                    >
                        {params.row.deleteChangeDocumentNumber}
                    </Link>
                ) : (
                    <span>{params.row.deleteChangeDocumentNumber}</span>
                )
        },
        {
            field: 'deleteReplaceSeq',
            headerName: 'Out',
            hide: true,
            width: 75,
            editable: false
        },
        {
            field: 'drawingReference',
            headerName: 'Drawing Ref',
            width: 150,
            editable: false,
            hide: showChanges
        }
    ];
    const initialise = useCallback(() => {
        if (bomTree === null) {
            setSelected({ id: 'root', name: bomName, children: [] });
        } else {
            setSelected(bomTree);
        }
        setTreeView(
            bomTree !== null || bomTreeLoading
                ? bomTree
                : { id: 'root', name: bomName, children: [] }
        );
    }, [bomTree, bomName, bomTreeLoading]);

    useEffect(() => {
        initialise();
    }, [initialise]);

    const [changesMade, setChangesMade] = useState(false);

    // updates the tree with changes passed via a 'newNode' object
    const updateTree = (tree, newNode, addNode, addChangeDocumentNumber) => {
        setChangesMade(true);
        const newTree = { ...tree };
        const q = [];
        q.push(newTree);
        while (q.length !== 0) {
            let n = q.length;
            while (n > 0) {
                const current = q[0];
                q.shift();
                if (current.id === newNode.parentId) {
                    current.assemblyHasChanges = true;
                    if (addNode) {
                        const newChildren = [...current.children];
                        newChildren.splice(addNode, 0, {
                            ...newNode,
                            addChangeDocumentNumber
                        });
                        current.children = newChildren;
                    } else {
                        let replacedIndex = null;
                        let replacementFor = null;
                        let replacedNode = null;
                        current.children = current.children.map((x, index) => {
                            if (x.id !== newNode.id) {
                                if (
                                    newNode.replacementFor &&
                                    newNode.name &&
                                    newNode.replacementFor === x.id
                                ) {
                                    return { ...x, replacedBy: newNode.name };
                                }
                                if (
                                    newNode.undoReplaceSeq &&
                                    x.deleteReplaceSeq === newNode.undoReplaceSeq
                                ) {
                                    return { ...x, toDelete: false, isReplaced: false };
                                }
                                return x;
                            }
                            if (newNode.isReplaced) {
                                replacedIndex = index;
                                replacementFor = x.id;
                                replacedNode = newNode;
                            }
                            return { ...newNode, hasChanged: true };
                        });
                        if (replacedIndex !== null) {
                            current.children.splice(replacedIndex + 1, 0, {
                                ...replacedNode,
                                id: uid(),
                                parentId: current.id,
                                changeState: 'PROPOS',
                                replacementFor,
                                isReplaced: false,
                                addChangeDocumentNumber
                            });
                        }
                    }

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

    // find a node in the tree
    const getNode = (tree, searchTerm, fieldName = 'id') => {
        if (tree == null) return null;
        const q = [];
        q.push(tree);
        while (q.length !== 0) {
            let n = q.length;
            while (n > 0) {
                const current = q[0];
                q.shift();
                if (current[fieldName] === searchTerm) return current;
                if (current.children)
                    for (let i = 0; i < current.children.length; i += 1)
                        q.push(current.children[i]);
                n -= 1;
            }
        }
        return null;
    };

    const [searchOccurenceCount, setSearchOccurenceCount] = useState(0);

    const searchTree = searchTerm => {
        let count = 0;
        setExpanded(expanded);
        if (treeView == null) return null;
        const q = [];
        q.push(treeView);
        while (q.length !== 0) {
            let n = q.length;
            while (n > 0) {
                const current = q[0];
                q.shift();
                if (current.children) {
                    for (let i = 0; i < current.children.length; i += 1)
                        q.push(current.children[i]);
                    setExpanded(e => [...e, current.id]);
                }
                if (current.name === searchTerm) {
                    count += 1;
                }
                if (current.name === searchTerm && count === searchOccurenceCount + 1) {
                    setSearchOccurenceCount(prevValue => prevValue + 1);
                    return current;
                }

                n -= 1;
            }
        }
        return null;
    };

    const processRowUpdate = useCallback(
        newRow => {
            setDisableChangesButton(true);
            setTreeView(tr => updateTree(tr, newRow, false, crNumber));
            return newRow;
        },
        [crNumber]
    );

    // add a new line to the children list of the selected node
    const addLine = position => {
        setDisableChangesButton(true);
        setTreeView(tree =>
            updateTree(
                tree,
                {
                    id: uid(),
                    type: 'C',
                    parentId: selected.id,
                    changeState: 'PROPOS',
                    qty: 1,
                    requirement: 'Y'
                },
                position,
                crNumber
            )
        );
    };

    const getRows = () => {
        if (selected) {
            const node = getNode(treeView, selected.id);
            if (node?.children) return node.children;
        }
        return [];
    };

    const [bomToCopy, setBomToCopy] = useState();

    const handlePartSelect = newValue => {
        if (selected.children.find(x => x.name === newValue.partNumber)) {
            setPartMessage('Part already on BOM!');
            return;
        }
        setPartLookUp(p => ({ ...p, selectedPart: newValue, open: false }));
        if (newValue.bomType !== 'C' && newValue.bomId) {
            const subAssemblyUrl = `/purchasing/boms/tree?bomName=${
                newValue.partNumber
            }&levels=${0}&requirementOnly=${false}&showChanges=${false}&treeType=${'bom'}`;

            // fetch this subAssembly's bomTree to add it to the tree view
            reduxDispatch(subAssemblyActions.fetchByHref(subAssemblyUrl));
        } else {
            if (newValue.bomType !== 'C') {
                setNewAssemblies(n => [
                    ...n,
                    {
                        id: partLookUp.forRow.id,
                        name: newValue.partNumber,
                        type: newValue.bomType,
                        isNewAddition: true,
                        children: []
                    }
                ]);
            }
            processRowUpdate({
                ...partLookUp.forRow,
                name: newValue.partNumber,
                safetyCritical: newValue.safetyCriticalPart,
                drawingReference: newValue.drawingReference,
                type: newValue.bomType,
                isNewAddition: true,
                description: newValue.description,
                children: newValue.bomType === 'C' ? null : []
            });
        }
    };

    // add the new subAssembly to the bom tree when it arrives
    useEffect(() => {
        if (subAssembly?.name && partLookUp.forRow?.id) {
            const parent = getNode(treeView, partLookUp.forRow?.parentId);
            if (parent?.children && !parent.children.find(x => x.id === subAssembly.id)) {
                processRowUpdate({
                    ...partLookUp.forRow,
                    name: subAssembly.name,
                    type: subAssembly.type,
                    isNewAddition: true,
                    safetyCritical: subAssembly.safetyCritical,
                    drawingReference: subAssembly.drawingReference,
                    description: subAssembly.description,
                    children: subAssembly.children,
                    changeState: 'PROPOS'
                });
            }
            reduxDispatch(subAssemblyActions.clearItem());
            setPartLookUp({ open: false, forRow: null, selectedPart: null });
        }
    }, [subAssembly, partLookUp.forRow, reduxDispatch, processRowUpdate, treeView]);

    const PartLookUp = () => (
        <Dialog open={partLookUp.open}>
            <DialogTitle>Search For A Part</DialogTitle>
            <DialogContent dividers>
                <Search
                    visible={partLookUp.open}
                    autoFocus
                    propertyName="partNumber"
                    label="Part Number"
                    resultsInModal
                    resultLimit={100}
                    value={partSearchTerm}
                    handleValueChange={(_, newVal) => setPartSearchTerm(newVal)}
                    search={searchParts}
                    searchResults={partsSearchResults
                        .filter(p => !!p.bomType)
                        .map(r => ({
                            ...r,
                            description: r.datePhasedOut ? 'PHASED OUT!!' : r.description
                        }))}
                    loading={partsSearchLoading}
                    priorityFunction="closestMatchesFirst"
                    onResultSelect={handlePartSelect}
                    clearSearch={() => {}}
                />
                {partMessage && (
                    <Typography color="secondary" variant="subtitle2">
                        {partMessage}
                    </Typography>
                )}
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => setPartLookUp({ open: false, forRow: null, selectedPart: null })}
                >
                    Cancel
                </Button>
            </DialogActions>
        </Dialog>
    );
    const [copyBomDialogOpen, setCopyBomDialogOpen] = useState(false);
    const [deleteAllFromBomDialogOpen, setDeleteAllFromBomDialogOpen] = useState(false);
    const [safetyCriticalWarningDialogOpen, setSafetyCriticalWarningDialogOpen] = useState(false);
    const [undoReplacementDialogOpen, setUndoReplacementDialogOpen] = useState(false);
    const [explodeSubAssemblyDialogOpen, setExplodeSubAssemblyDialogOpen] = useState(false);
    const [undoDeletionDialogOpen, setUndoDeletionDialogOpen] = useState(false);
    const [addOrOverwrite, setAddOrOverwrite] = useState('A');

    const CopyExplodeBomDialog = () => (
        <Dialog
            open={copyBomDialogOpen || explodeSubAssemblyDialogOpen}
            onClose={() => setPartSearchTerm(null)}
        >
            <DialogTitle>{copyBomDialogOpen ? 'Copy BOM' : 'Explode Sub Assembly'}</DialogTitle>
            <>
                <DialogContent dividers>
                    <Search
                        visible={copyBomDialogOpen || explodeSubAssemblyDialogOpen}
                        autoFocus
                        propertyName="partNumber"
                        label="Search for a BOM"
                        resultsInModal
                        resultLimit={100}
                        value={bomToCopy ?? partSearchTerm}
                        handleValueChange={(_, newVal) => setPartSearchTerm(newVal)}
                        search={searchParts}
                        searchResults={partsSearchResults.filter(x => x.bomType !== 'C')}
                        loading={partsSearchLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={newVal => setBomToCopy(newVal.partNumber)}
                        clearSearch={() => {}}
                    />
                </DialogContent>

                {copyBomDialogOpen && (
                    <>
                        <DialogContent dividers>
                            <OnOffSwitch
                                label="Overwrite / Add"
                                value={addOrOverwrite === 'A'}
                                onChange={() => {
                                    setAddOrOverwrite(ao => (ao === 'A' ? 'O' : 'A'));
                                }}
                                propertyName="addOrOverwrite"
                            />
                        </DialogContent>
                        <DialogContent dividers>
                            <Typography variant="subtitle2">
                                {addOrOverwrite === 'A' &&
                                    `Clicking confirm will append the components on the selected BOM to ${selected.name}`}
                                {addOrOverwrite === 'O' &&
                                    `Clicking confirm will replace all components on ${selected.name} with the components on the selected BOM`}
                            </Typography>
                        </DialogContent>
                    </>
                )}
                {explodeSubAssemblyDialogOpen && (
                    <DialogContent dividers>
                        <Typography variant="subtitle2">
                            {`Clicking confirm will replace the chosen subassembly with all its components on ${selected.name}`}
                        </Typography>
                    </DialogContent>
                )}
            </>
            <DialogActions>
                <Button
                    onClick={() => {
                        setCopyBomDialogOpen(false);
                        setExplodeSubAssemblyDialogOpen(false);
                    }}
                >
                    Cancel
                </Button>
                <Button
                    variant="contained"
                    onClick={() => {
                        setExplodeSubAssemblyDialogOpen(false);
                        setCopyBomDialogOpen(false);
                        setPartSearchTerm(null);
                        if (copyBomDialogOpen) {
                            reduxDispatch(
                                bomTreeActions.postByHref('/purchasing/boms/copy', {
                                    srcPartNumber: bomToCopy,
                                    destPartNumber: selected.name,
                                    crfNumber: crNumber,
                                    addOrOverwrite,
                                    rootName: bomName
                                })
                            );
                        } else {
                            reduxDispatch(
                                bomTreeActions.postByHref('/purchasing/boms/explode', {
                                    destPartNumber: selected.name,
                                    crfNumber: crNumber,
                                    subAssembly: bomToCopy,
                                    rootName: bomName
                                })
                            );
                        }
                    }}
                    disabled={!bomToCopy}
                >
                    Confirm
                </Button>
            </DialogActions>
        </Dialog>
    );

    const DeleteAllFromBomDialog = () => (
        <Dialog open={deleteAllFromBomDialogOpen} onClose={() => setPartSearchTerm(null)}>
            <DialogTitle>Delete All From Bom</DialogTitle>
            <DialogContent dividers>
                <Typography variant="h6">
                    {`Clicking confirm will create a change to remove everything from ${selected?.name}`}
                </Typography>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        setDeleteAllFromBomDialogOpen(false);
                    }}
                >
                    Cancel
                </Button>
                <Button
                    variant="contained"
                    onClick={() => {
                        setDeleteAllFromBomDialogOpen(false);
                        reduxDispatch(
                            bomTreeActions.postByHref('/purchasing/boms/delete', {
                                destPartNumber: selected.name,
                                crfNumber: crNumber,
                                rootName: bomName
                            })
                        );
                    }}
                    disabled={!crNumber}
                >
                    Confirm
                </Button>
            </DialogActions>
        </Dialog>
    );

    const DeleteSafetyCriticalWarningDialog = () => (
        <Dialog open={safetyCriticalWarningDialogOpen} onClose={() => setPartSearchTerm(null)}>
            <DialogTitle>Safety Critical Part</DialogTitle>
            <DialogContent>
                <DialogContentText id="alert-dialog-description">
                    The part your are deleting is Safety Critical
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        setDeleteAllFromBomDialogOpen(false);
                    }}
                >
                    Cancel
                </Button>
                <Button
                    variant="contained"
                    onClick={() => {
                        setSafetyCriticalWarningDialogOpen(false);
                        processRowUpdate({ ...contextMenu.detail, toDelete: true });
                        setContextMenu(null);
                    }}
                >
                    Accept
                </Button>
            </DialogActions>
        </Dialog>
    );

    const UndoReplacementDialog = () => (
        <Dialog open={undoReplacementDialogOpen} onClose={() => setPartSearchTerm(null)}>
            <DialogTitle>Undo Replacement?</DialogTitle>
            <DialogContent>
                <DialogContentText id="alert-dialog-description">
                    The part your are deleting is a replacement part. Do you want to undo the
                    replacement?
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        setUndoReplacementDialogOpen(false);
                    }}
                >
                    Cancel
                </Button>
                <Button
                    variant="contained"
                    onClick={() => {
                        setUndoReplacementDialogOpen(false);
                        processRowUpdate({
                            ...contextMenu.detail,
                            toDelete: true,
                            undoReplaceSeq: contextMenu?.detail.addReplaceSeq
                        });
                        setContextMenu(null);
                    }}
                >
                    Accept
                </Button>
            </DialogActions>
        </Dialog>
    );

    const UndoDeletionDialogOpen = () => (
        <Dialog open={undoDeletionDialogOpen} onClose={() => setPartSearchTerm(null)}>
            <DialogTitle>Undo Deletion?</DialogTitle>
            <DialogContent>
                <DialogContentText id="alert-dialog-description">
                    The part your are deleting is already marked for deletion - Do you want to undo?
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        setUndoDeletionDialogOpen(false);
                    }}
                >
                    Cancel
                </Button>
                <Button
                    variant="contained"
                    onClick={() => {
                        setUndoDeletionDialogOpen(false);
                        processRowUpdate({
                            ...contextMenu.detail,
                            toDelete: false,
                            isReplaced: false
                        });
                        setContextMenu(null);
                    }}
                >
                    Accept
                </Button>
            </DialogActions>
        </Dialog>
    );

    const handleClose = () => setContextMenu(null);

    const handleReplaceClick = () => {
        processRowUpdate({
            ...contextMenu.detail,
            isReplaced: true,
            parentId: contextMenu.detail.parentId
        });
        setContextMenu(null);
    };

    const handleDeleteClick = () => {
        if (contextMenu?.detail.deleteChangeDocumentNumber) {
            setUndoDeletionDialogOpen(true);
        } else if (contextMenu?.detail.addReplaceSeq) {
            setUndoReplacementDialogOpen(true);
        } else if (contextMenu?.detail.safetyCritical === 'Y') {
            setSafetyCriticalWarningDialogOpen(true);
        } else {
            processRowUpdate({ ...contextMenu.detail, toDelete: true });
            setContextMenu(null);
        }
    };

    const doSearch = () => {
        const node = searchTree(searchBomTerm?.toUpperCase?.());
        if (node) {
            const parent = getNode(treeView, node.parentName, 'name');
            setSelected(parent);
            document.getElementById(parent.id).scrollIntoView();
            (() => new Promise(resolve => setTimeout(resolve, 500)))().then(() => {
                document.querySelectorAll(`[data-id="${node.id}"]`)?.[0]?.scrollIntoView();
            });
        }
    };

    return (
        <Page history={history} homeUrl={config.appRoot} width="xl">
            {PartLookUp()}
            {CopyExplodeBomDialog()}
            {DeleteAllFromBomDialog()}
            {DeleteSafetyCriticalWarningDialog()}
            {UndoReplacementDialog()}
            {UndoDeletionDialogOpen()}
            <Grid container spacing={3}>
                <SnackbarMessage
                    visible={snackbarVisible}
                    onClose={() => reduxDispatch(bomTreeActions.setSnackbarVisible(false))}
                    message="Save Successful"
                    timeOut={3000}
                />
                <PrevNextButtons
                    goPrev={goPrev}
                    goNext={goNext}
                    nextResult={nextResult}
                    prevResult={prevResult}
                />
                {changeRequestsLoading ? (
                    <Grid item xs={12}>
                        <LinearProgress />
                    </Grid>
                ) : (
                    <>
                        <Grid item xs={4}>
                            <Dropdown
                                items={changeRequests?.map(c => ({
                                    id: c.documentNumber,
                                    displayText: `${c.documentType}${c.documentNumber} / ${c.newPartNumber} / ${c.changeState}`
                                }))}
                                allowNoValue
                                label="CRF Number"
                                propertyName="crNumber"
                                helperText="Select a corresponding CRF to start editing"
                                value={crNumber}
                                onChange={(_, n) => {
                                    setCrNumber(n);
                                }}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                label="Search Bom"
                                helperText="press enter to search"
                                value={searchBomTerm}
                                onChange={(_, v) => {
                                    setSearchOccurenceCount(0);
                                    setSearchBomTerm(v);
                                }}
                                propertyName="searchBomTerm"
                                textFieldProps={{
                                    onKeyDown: data => {
                                        if (data.keyCode === 13) {
                                            doSearch(searchBomTerm);
                                        }
                                    }
                                }}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            {itemError && (
                                <Grid item xs={12} style={{ maxHeight: '200px' }} overflow="scroll">
                                    <ErrorCard
                                        errorMessage={itemError.details?.error || itemError.details}
                                    />
                                </Grid>
                            )}
                        </Grid>
                        <Grid item xs={12}>
                            <Button
                                disabled={bomTreeLoading || disableChangesButton}
                                variant="outlined"
                                onClick={() => {
                                    reduxDispatch(bomTreeActions.fetchByHref(url(!showChanges)));
                                    setShowChanges(!showChanges);
                                }}
                            >
                                {showChanges ? 'hide' : 'show'} changes{' '}
                            </Button>
                            <Button
                                variant="outlined"
                                onClick={() =>
                                    history.push(
                                        `/purchasing/change-requests/create?bomName=${bomName}`
                                    )
                                }
                            >
                                RAISE NEW CRF
                            </Button>
                            <Button
                                variant="outlined"
                                disabled={!crNumber || selected?.isNewAddition}
                                onClick={() => {
                                    setCopyBomDialogOpen(true);
                                    setBomToCopy(null);
                                }}
                            >
                                Copy Bom
                            </Button>
                            <Button
                                variant="outlined"
                                disabled={!crNumber}
                                onClick={() => {
                                    setDeleteAllFromBomDialogOpen(true);
                                }}
                            >
                                Delete All
                            </Button>
                            <Button
                                variant="outlined"
                                disabled={!crNumber}
                                onClick={() => {
                                    setExplodeSubAssemblyDialogOpen(true);
                                }}
                            >
                                Explode Sub Assembly
                            </Button>
                        </Grid>
                    </>
                )}
                <Grid item xs={4} height="30px">
                    {subAssemblyLoading && <LinearProgress />}
                </Grid>
                <Grid item xs={8} height="30px" />
                <Grid
                    item
                    xs={4}
                    sx={{
                        height: '85vh',
                        overflowY: bomTreeLoading ? 'hidden' : 'scroll',
                        direction: 'rtl',
                        paddingTop: '0px ! important'
                    }}
                >
                    <div style={{ direction: 'ltr' }}>
                        <BomTree
                            renderDescriptions={false}
                            renderComponents={false}
                            renderQties={false}
                            onNodeSelect={id => {
                                setSelected(
                                    [...newAssemblies, ...nodesWithChildren].find(x => x.id === id)
                                );
                            }}
                            bomName={bomName}
                            bomTree={treeView}
                            bomTreeLoading={bomTreeLoading}
                            expanded={expanded}
                            setExpanded={setExpanded}
                            selected={selected?.id}
                        />
                    </div>
                </Grid>
                <Grid item xs={8}>
                    <Grid item xs={12}>
                        <Typography variant="h6">{selected?.description}</Typography>
                    </Grid>
                    <Grid
                        item
                        xs={12}
                        sx={{
                            paddingTop: '0px ! important',
                            height: '85vh',
                            overflowY: bomTreeLoading ? 'hidden' : 'scroll'
                        }}
                    >
                        <DataGrid
                            sx={{
                                '& .propos.MuiDataGrid-row:hover': {
                                    bgcolor: '#FFD580'
                                },
                                '& .propos': {
                                    bgcolor: '#FFD580'
                                },
                                '& .accept.MuiDataGrid-row:hover': {
                                    bgcolor: '#b0f7b9'
                                },
                                '& .accept': {
                                    bgcolor: '#b0f7b9'
                                },
                                border: 0
                            }}
                            columnBuffer={6}
                            rows={getRows()}
                            loading={bomTreeLoading}
                            processRowUpdate={processRowUpdate}
                            onProcessRowUpdateError={() => {}}
                            autoHeight
                            disableSelectionOnClick
                            columns={columns}
                            getRowClassName={params => params.row.changeState?.toLowerCase()}
                        />
                    </Grid>
                    {contextMenu && crNumber && (
                        <Menu
                            open={contextMenu !== null}
                            onClose={handleClose}
                            anchorReference="anchorPosition"
                            anchorPosition={
                                contextMenu !== null
                                    ? { top: contextMenu.mouseY, left: contextMenu.mouseX }
                                    : undefined
                            }
                        >
                            <MenuItem
                                onClick={() => {
                                    addLine(contextMenu.position);
                                    setContextMenu(null);
                                }}
                            >
                                ADD
                            </MenuItem>
                            <MenuItem
                                disabled={!contextMenu.canReplace}
                                onClick={handleReplaceClick}
                            >
                                REPLACE
                            </MenuItem>
                            <MenuItem disabled={!contextMenu.canDelete} onClick={handleDeleteClick}>
                                DELETE
                            </MenuItem>
                        </Menu>
                    )}
                    <Grid item xs={1}>
                        <Button
                            disabled={!crNumber || subAssemblyLoading}
                            variant="outlined"
                            onClick={() => addLine(selected.children.length + 1)}
                        >
                            +
                        </Button>
                    </Grid>
                </Grid>
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveDisabled={!crNumber || subAssemblyLoading || !changesMade}
                        saveClick={() => {
                            setChangesMade(false);
                            reduxDispatch(bomTreeActions.clearErrorsForItem());
                            reduxDispatch(
                                bomTreeActions.postByHref('/purchasing/boms/tree', {
                                    treeRoot: treeView,
                                    crNumber
                                })
                            );
                        }}
                        cancelClick={initialise}
                        backClick={() => {
                            history.push('/purchasing/boms');
                        }}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default BomUtility;
