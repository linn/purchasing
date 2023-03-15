import React, { useState, useEffect } from 'react';
import { useLocation, Link as RouterLink } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    Loading,
    CheckboxWithLabel,
    InputField,
    itemSelectorHelpers,
    collectionSelectorHelpers,
    utilities
} from '@linn-it/linn-form-components-library';
import queryString from 'query-string';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Link from '@mui/material/Link';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Typography from '@mui/material/Typography';
import changeRequestActions from '../../actions/changeRequestActions';
import changeRequestReplaceActions from '../../actions/changeRequestReplaceActions';
import boardComponentSummariesActions from '../../actions/boardComponentSummariesActions';
import bomTreeActions from '../../actions/bomTreeActions';
import history from '../../history';
import useInitialise from '../../hooks/useInitialise';
import { changeRequest } from '../../itemTypes';
import BomChangeReplace from './BomChangeReplace';
import PcasChangeReplace from './PcasChangeReplace';

function ChangeRequestReplace() {
    const dispatch = useDispatch();
    const { search } = useLocation();
    const documentNumber = queryString.parse(search)?.documentNumber;

    const [item, loading] = useInitialise(
        () => changeRequestActions.fetch(documentNumber),
        changeRequest.item
    );

    useEffect(() => {
        if (item?.oldPartNumber) {
            const showChanges = true;
            const requirementOnly = false;

            const bomWusedUrl = `/purchasing/boms/tree?bomName=${
                item?.oldPartNumber
            }&levels=1&requirementOnly=${requirementOnly ?? false}&showChanges=${
                showChanges ?? false
            }&treeType=whereUsed`;

            dispatch(bomTreeActions.fetchByHref(bomWusedUrl));

            const pcasOptions = `&partNumber=${item?.oldPartNumber}&`;
            dispatch(boardComponentSummariesActions.searchWithOptions(pcasOptions));
        }
    }, [item, dispatch]);

    const replaceUri = utilities.getHref(item, 'replace');

    const treeLoading = useSelector(reduxState =>
        itemSelectorHelpers.getItemLoading(reduxState.bomTree)
    );

    const tree = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.bomTree));

    const components = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.boardComponentSummaries)
    );
    const componentsLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.boardComponentSummaries)
    );

    const [selectedDetailIds, setSelectedDetailIds] = useState(null);
    const [selectedPcasComponents, setSelectedPcasComponents] = useState(null);
    const [newQty, setNewQty] = useState(null);
    const [globalReplace, setGlobalReplace] = useState(item?.globalReplace);
    const [tab, setTab] = useState(0);

    const handleBomSelectChange = selected => {
        setSelectedDetailIds(selected);
    };

    const handlePcasSelectChange = selected => {
        console.log(selected);
        setSelectedPcasComponents(selected);
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
                    hasPcasLines: components.length > 0,
                    newQty,
                    selectedDetailIds,
                    selectedPcasComponents
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
                        <Link
                            component={RouterLink}
                            to={`/purchasing/change-requests/${documentNumber}`}
                        >
                            <Typography variant="h6">Change Request {documentNumber}</Typography>
                        </Link>
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
                            disabled={
                                !replaceUri ||
                                (!(selectedDetailIds || selectedPcasComponents) && !globalReplace)
                            }
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
                    <Grid item xs={12}>
                        <Box sx={{ width: '100%' }}>
                            <Box sx={{ borderBottom: 0, borderColor: 'divider' }}>
                                <Tabs
                                    value={tab}
                                    onChange={(_, newValue) => {
                                        setTab(newValue);
                                    }}
                                >
                                    <Tab
                                        label={`Bom WUSED${
                                            tree?.children?.length
                                                ? ` (${tree?.children?.length})`
                                                : ''
                                        }`}
                                        disabled={!tree?.children?.length}
                                    />
                                    <Tab
                                        label={`Pcas WUSED${
                                            components?.length ? ` (${components?.length})` : ''
                                        }`}
                                        disabled={!components?.length}
                                    />
                                </Tabs>
                            </Box>
                        </Box>
                    </Grid>

                    {tab === 0 && (
                        <Box sx={{ paddingTop: 3, width: '100%' }}>
                            {treeLoading ? (
                                <Loading />
                            ) : (
                                <Grid item xs={12}>
                                    <BomChangeReplace
                                        wused={tree?.children}
                                        handleSelectChange={handleBomSelectChange}
                                        documentNumber={documentNumber}
                                    />
                                </Grid>
                            )}
                        </Box>
                    )}

                    {tab === 1 && (
                        <Box sx={{ paddingTop: 3, width: '100%' }}>
                            {componentsLoading ? (
                                <Loading />
                            ) : (
                                <Grid item xs={12}>
                                    <PcasChangeReplace
                                        wused={components}
                                        handleSelectChange={handlePcasSelectChange}
                                        documentNumber={documentNumber}
                                    />
                                </Grid>
                            )}
                        </Box>
                    )}
                </Grid>
            )}
        </Page>
    );
}

export default ChangeRequestReplace;
