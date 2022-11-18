import React, { useState } from 'react';
import {
    Page,
    ExportButton,
    Search,
    InputField,
    collectionSelectorHelpers,
    CheckboxWithLabel,
    OnOffSwitch
} from '@linn-it/linn-form-components-library';
import { useLocation } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import Grid from '@mui/material/Grid';
import queryString from 'query-string';
import Button from '@mui/material/Button';
import history from '../history';
import config from '../config';
import { parts } from '../itemTypes';
import partsActions from '../actions/partsActions';
import bomTreeActions from '../actions/bomTreeActions';

/* eslint react/jsx-props-no-spreading: 0 */
/* eslint react/destructuring-assignment: 0 */

export default function BomTreeOptions() {
    const { search } = useLocation();
    const { bomName } = queryString.parse(search);
    const [searchTerm, setSearchTerm] = useState('');
    const [explode, setExplode] = useState(0);
    const [requirementOnly, setRequirementOnly] = useState(true);
    const [showChanges, setShowChanges] = useState(false);
    const [whereUsed, setWhereUsed] = useState(false);

    const dispatch = useDispatch();

    const partsSearchResults = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchItems(
            reduxState[parts.item],
            100,
            'id',
            'partNumber',
            'description'
        )
    );
    const partsSearchLoading = useSelector(reduxState =>
        collectionSelectorHelpers.getSearchLoading(reduxState[parts.item])
    );

    if (bomName && !searchTerm) {
        setSearchTerm(bomName);
    }

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <ExportButton
                        href={`${
                            config.appRoot
                        }/purchasing/boms/tree/export?bomName=${searchTerm}&levels=${
                            explode || 0
                        }&requirementOnly=${requirementOnly}&showChanges=${showChanges}&treeType=${
                            whereUsed ? 'whereUsed' : 'bom'
                        }`}
                    />
                </Grid>
                <Grid item xs={3}>
                    <Search
                        propertyName="searchTerm"
                        label="Bom Name"
                        resultsInModal
                        resultLimit={100}
                        value={searchTerm}
                        handleValueChange={(_, newVal) => setSearchTerm(newVal)}
                        search={partNumber => {
                            dispatch(partsActions.search(partNumber));
                        }}
                        searchResults={partsSearchResults}
                        helperText="Enter a value. Press the enter key if you want to search parts."
                        loading={partsSearchLoading}
                        priorityFunction="closestMatchesFirst"
                        onResultSelect={newValue => {
                            setSearchTerm(newValue.partNumber);
                        }}
                        clearSearch={() => {}}
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        value={explode}
                        type="number"
                        propertyName="explode"
                        label="Explode levels"
                        helperText="Leave as zero to see the whole tree"
                        onChange={(_, newVal) => {
                            setExplode(newVal);
                        }}
                    />
                </Grid>
                <Grid item xs={3}>
                    <OnOffSwitch
                        label="Where Used?"
                        value={whereUsed}
                        onChange={() => {
                            setWhereUsed(!whereUsed);
                        }}
                        propertyName="whereUsed"
                    />
                </Grid>
                <Grid item xs={3} />

                <Grid item xs={3}>
                    <CheckboxWithLabel
                        label="Show only Parts with Material Reqt"
                        checked={requirementOnly}
                        onChange={() => {
                            setRequirementOnly(!requirementOnly);
                        }}
                    />
                </Grid>
                <Grid item xs={3}>
                    <CheckboxWithLabel
                        label="Show Proposed/Accepted changes"
                        checked={showChanges}
                        onChange={() => {
                            setShowChanges(!showChanges);
                        }}
                    />
                </Grid>
                <Grid item xs={12}>
                    <Button
                        color="primary"
                        variant="contained"
                        disabled={!searchTerm}
                        onClick={() => {
                            dispatch(bomTreeActions.clearItem());
                            history.push(
                                `/purchasing/boms/tree?bomName=${searchTerm}&levels=${
                                    explode || 0
                                }&requirementOnly=${requirementOnly}&showChanges=${showChanges}&treeType=${
                                    whereUsed ? 'whereUsed' : 'bom'
                                }`
                            );
                        }}
                    >
                        Run
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}
