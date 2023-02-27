import React, { useState, useEffect } from 'react';
import { useLocation, Link as RouterLink } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    Loading,
    CheckboxWithLabel,
    InputField,
    itemSelectorHelpers,
    utilities
} from '@linn-it/linn-form-components-library';
import queryString from 'query-string';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import { makeStyles } from '@mui/styles';
import { DataGrid } from '@mui/x-data-grid';
import changeRequestActions from '../../actions/changeRequestActions';
import changeRequestReplaceActions from '../../actions/changeRequestReplaceActions';
import bomTreeActions from '../../actions/bomTreeActions';
import history from '../../history';
import useInitialise from '../../hooks/useInitialise';
import { changeRequest } from '../../itemTypes';

function ChangeRequestReplace() {
    const dispatch = useDispatch();
    const { search } = useLocation();
    const documentNumber = queryString.parse(search)?.documentNumber;

    const useStyles = makeStyles(() => ({
        gap: {
            marginTop: '20px'
        },
        a: {
            textDecoration: 'none'
        }
    }));
    const classes = useStyles();

    const [item, loading] = useInitialise(
        () => changeRequestActions.fetch(documentNumber),
        changeRequest.item
    );

    useEffect(() => {
        if (item?.oldPartNumber) {
            const showChanges = true;
            const requirementOnly = false;
            const url = `/purchasing/boms/tree?bomName=${
                item?.oldPartNumber
            }&levels=1&requirementOnly=${requirementOnly ?? false}&showChanges=${
                showChanges ?? false
            }&treeType=whereUsed`;
            dispatch(bomTreeActions.fetchByHref(url));
        }
    }, [item, dispatch]);

    const replaceUri = utilities.getHref(item, 'replace');

    const treeLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.bomTypeChange)
    );

    const tree = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.bomTree));

    const columns = [
        { field: 'qty', headerName: 'Qty', width: 100 },
        {
            field: 'name',
            headerName: 'Name',
            width: 150,
            renderCell: params => (
                <Link
                    className={classes.a}
                    component={RouterLink}
                    to={`/purchasing/boms/bom-utility?bomName=${params.row.name}&changeRequest=${documentNumber}`}
                >
                    {params.row.name}
                </Link>
            )
        },
        { field: 'description', headerName: 'Description', width: 450 },
        { field: 'pcasLine', headerName: 'Pcas', width: 80 },
        {
            field: 'deleteChangeDocumentNumber',
            headerName: 'Delete Chg',
            width: 100,
            renderCell: params => (
                <Link
                    className={classes.a}
                    component={RouterLink}
                    to={`/purchasing/change-requests/${params.row.deleteChangeDocumentNumber}`}
                >
                    {params.row.deleteChangeDocumentNumber}
                </Link>
            )
        }
    ];

    const [selectedDetailIds, setSelectedDetailIds] = useState(null);
    const [newQty, setNewQty] = useState(null);
    const [globalReplace, setGlobalReplace] = useState(item?.globalReplace);

    const handleSelectChange = selected => {
        setSelectedDetailIds(selected);
    };

    const handleNewQtyChange = (propertyName, newValue) => {
        setNewQty(newValue);
    };

    const replace = request => {
        if (request.changeState === 'PROPOS' || request.changeState === 'ACCEPT') {
            dispatch(
                changeRequestReplaceActions.add({
                    documentNumber: request.documentNumber,
                    globalReplace,
                    newQty,
                    selectedDetailIds
                })
            );
        }
    };

    return (
        <Page history={history}>
            {loading ? (
                <Loading />
            ) : (
                <Grid container spacing={2} justifyContent="center">
                    <Grid item xs={12}>
                        <Typography variant="h6">Change Request {documentNumber}</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            value={item?.oldPartNumber}
                            label="Old Part"
                            propertyName="oldPartNumber"
                            disabled
                        />
                        <Typography>{item?.newPartDescription}</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <InputField
                            value={item?.newPartNumber}
                            label="New Part"
                            propertyName="newPartNumber"
                            disabled
                        />
                        <Typography>{item?.newPartDescription}</Typography>
                    </Grid>
                    <Grid item xs={4}>
                        <Button
                            variant="contained"
                            disabled={!replaceUri || (!selectedDetailIds && !globalReplace)}
                            onClick={() => replace(item)}
                        >
                            Replace
                        </Button>
                    </Grid>
                    <Grid item xs={4}>
                        <CheckboxWithLabel
                            label="Global Replace"
                            checked={globalReplace}
                            onChange={() => setGlobalReplace(!globalReplace)}
                        />
                    </Grid>
                    <Grid item xs={4}>
                        <InputField label="New Qty" value={newQty} onChange={handleNewQtyChange} />
                    </Grid>
                    {treeLoading ? (
                        <Loading />
                    ) : (
                        <Grid item xs={12}>
                            {tree ? (
                                <DataGrid
                                    getRowId={row => row.id}
                                    className={classes.gap}
                                    rows={tree?.children}
                                    columns={columns}
                                    getRowHeight={() => 'auto'}
                                    autoHeight
                                    loading={false}
                                    checkboxSelection
                                    isRowSelectable={params =>
                                        !params.row.deleteChangeDocumentNumber
                                    }
                                    onSelectionModelChange={handleSelectChange}
                                    hideFooter
                                />
                            ) : (
                                <span>Old Part Not Used Anywhere</span>
                            )}
                        </Grid>
                    )}
                </Grid>
            )}
        </Page>
    );
}

export default ChangeRequestReplace;
