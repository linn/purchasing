import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
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
        { field: 'name', headerName: 'Name', width: 150 },
        { field: 'description', headerName: 'Description', width: 200 },
        { field: 'deleteChangeDocumentNumber', headerName: 'Delete Chg', width: 100 }
    ];

    const [selectedDetailIds, setSelectedDetailIds] = useState(null);
    const [globalReplace, setGlobalReplace] = useState(item?.globalReplace);

    const handleSelectChange = selected => {
        setSelectedDetailIds(selected);
    };

    const replace = request => {
        if (request.changeState === 'PROPOS' || request.changeState === 'ACCEPT') {
            dispatch(
                changeRequestReplaceActions.add({
                    documentNumber: request.documentNumber,
                    globalReplace: request.globalReplace,
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
                            variant="outlined"
                            disabled={!replaceUri || (!selectedDetailIds && !globalReplace)}
                            onClick={() => replace(item)}
                        >
                            Replace
                        </Button>
                    </Grid>
                    <Grid item xs={8}>
                        <CheckboxWithLabel
                            label="Global Replace"
                            checked={globalReplace}
                            onChange={() => setGlobalReplace(!globalReplace)}
                        />
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
