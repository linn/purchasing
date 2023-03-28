import React, { useState } from 'react';
import Grid from '@mui/material/Grid';
import {
    Page,
    Title,
    InputField,
    collectionSelectorHelpers,
    Loading
} from '@linn-it/linn-form-components-library';
import { useSelector, useDispatch } from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import history from '../../history';
import config from '../../config';
import boardComponentSummariesActions from '../../actions/boardComponentSummariesActions';

function BoardsSummary() {
    const dispatch = useDispatch();
    const [boardCode, setBoardCode] = useState(null);
    const [revisionCode, setRevisionCode] = useState(null);
    const [cref, setCref] = useState(null);
    const [partNumber, setPartNumber] = useState(null);

    const components = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.boardComponentSummaries)
    );
    const loading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.boardComponentSummaries)
    );

    const getComponents = () => {
        let options = '';
        if (boardCode) {
            options += `&boardCode=${boardCode}`;
        }

        if (revisionCode) {
            options += `&revisionCode=${revisionCode}`;
        }

        if (cref) {
            options += `&cref=${cref}`;
        }

        if (partNumber) {
            options += `&partNumber=${partNumber}`;
        }

        options += '&';

        dispatch(boardComponentSummariesActions.searchWithOptions(options));
    };

    const columns = [
        { field: 'boardCode', headerName: 'Board', minWidth: 130 },
        { field: 'revisionCode', headerName: 'Revision', minWidth: 140 },
        { field: 'cref', headerName: 'CRef', minWidth: 130 },
        { field: 'partNumber', headerName: 'Part Number', minWidth: 140 },
        { field: 'quantity', headerName: 'Qty', minWidth: 100 },
        { field: 'assemblyTechnology', headerName: 'Tech', minWidth: 100 },
        { field: 'bomPartNumber', headerName: 'Assembly', minWidth: 150 },
        { field: 'changeState', headerName: 'Change State', minWidth: 150 }
    ];

    const handleFieldChange = (propertyName, value) => {
        switch (propertyName) {
            case 'boardCode':
                setBoardCode(value);
                break;
            case 'revisionCode':
                setRevisionCode(value);
                break;
            case 'cref':
                setCref(value);
                break;
            case 'partNumber':
                setPartNumber(value);
                break;
            default:
        }
    };

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Title text="Board Quick View" />
            <Grid container>
                <Grid item xs={12}>
                    {loading && <Loading />}
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        propertyName="boardCode"
                        value={boardCode}
                        onChange={handleFieldChange}
                        label="Board Code Search"
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        propertyName="revisionCode"
                        value={revisionCode}
                        onChange={handleFieldChange}
                        label="Revision Code Search"
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        propertyName="cref"
                        value={cref}
                        onChange={handleFieldChange}
                        label="CRef Search"
                    />
                </Grid>
                <Grid item xs={3}>
                    <InputField
                        propertyName="partNumber"
                        value={partNumber}
                        onChange={handleFieldChange}
                        label="Part Number Search"
                    />
                </Grid>
                <Grid item xs={3}>
                    <Button onClick={getComponents} variant="outlined" color="primary">
                        Show Components
                    </Button>
                </Grid>
                <Grid item xs={9}>
                    <Typography style={{ paddingTop: '12px' }} variant="subtitle1">
                        Wildcard * can be used
                    </Typography>
                </Grid>
                {!loading && components && (
                    <>
                        <Grid item xs={12} style={{ paddingTop: '40px' }}>
                            <div>
                                <DataGrid
                                    rows={components.map((d, i) => ({
                                        ...d,
                                        id: i
                                    }))}
                                    columns={columns}
                                    density="compact"
                                    rowHeight={34}
                                    autoHeight
                                    loading={loading}
                                    columnBuffer={8}
                                />
                            </div>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

export default BoardsSummary;
