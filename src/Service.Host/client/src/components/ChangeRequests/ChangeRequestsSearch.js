import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    Dropdown,
    InputField,
    Loading,
    collectionSelectorHelpers,
    utilities
} from '@linn-it/linn-form-components-library';
import { makeStyles } from '@mui/styles';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import { DataGrid } from '@mui/x-data-grid';
import changeRequestsActions from '../../actions/changeRequestsActions';
import ChangeState from './ChangeState';
import history from '../../history';

function ChangeRequestSearch() {
    const dispatch = useDispatch();
    const useStyles = makeStyles(theme => ({
        gap: {
            marginTop: '50px'
        },
        button: {
            marginLeft: theme.spacing(1),
            marginTop: theme.spacing(4)
        },
        a: {
            textDecoration: 'none'
        }
    }));
    const classes = useStyles();

    const [documentNumber, setDocumentNumber] = useState('');
    const [newPartNumber, setNewPartNumber] = useState('');
    const [filter, setFilter] = useState('OUTSTANDING');
    const [outstanding, setOutstanding] = useState(true);
    const [lastMonths, setLastMonths] = useState(60);

    const searchRequestsResults = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.changeRequests)
    );
    const searchRequestsLoading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.changeRequests)
    );

    useEffect(() => {
        dispatch(changeRequestsActions.clearSearch());
    }, [dispatch]);
    const searchResults = searchRequestsResults?.map(s => s.documentNumber).reverse();
    const columns = [
        {
            field: 'documentNumber',
            headerName: 'CRF',
            width: 100,
            renderCell: params => (
                <Link
                    component="button"
                    className={classes.a}
                    onClick={() =>
                        history.push(utilities.getSelfHref(params.row), { searchResults })
                    }
                >
                    {params.row.documentNumber}
                </Link>
            )
        },
        {
            field: 'newPartNumber',
            headerName: 'Part/Board',
            width: 200,
            renderCell: params => (
                <>
                    {
                        {
                            PARTEDIT: <>{params.row.newPartNumber}</>,
                            BOARDEDIT: (
                                <>
                                    {params.row.boardCode} {params.row.revisionCode}
                                </>
                            ),
                            REPLACE: (
                                <>
                                    {params.row.oldPartNumber} -{'>'} {params.row.newPartNumber}
                                </>
                            )
                        }[params.row.changeType]
                    }
                </>
            )
        },
        {
            field: 'changeState',
            headerName: 'State',
            width: 150,
            renderCell: params => (
                <ChangeState changeState={params.row.changeState} showLabel={false} />
            )
        },
        {
            field: 'dateEntered',
            headerName: 'Entered',
            width: 170,
            renderCell: params => (
                <InputField
                    fullWidth
                    value={params.row.dateEntered}
                    propertyName="dateEntered"
                    type="date"
                    disabled
                />
            )
        },
        {
            field: 'proposedBy',
            headerName: 'Proposed By',
            width: 200,
            renderCell: params => <>{params.row.proposedBy?.fullName}</>
        },
        {
            field: 'reasonForChange',
            headerName: 'Reason',
            width: 500,
            renderCell: params => <>{params.row.reasonForChange}</>
        }
    ];

    const handleDropDownChange = (propertyName, newValue) => {
        setFilter(newValue);
        if (newValue === 'OUTSTANDING') {
            setOutstanding(true);
            setLastMonths(60);
        } else {
            setOutstanding(false);
            setLastMonths(newValue);
        }
    };

    const lookup = () => history.push(`/purchasing/change-requests/${documentNumber}/`);

    const create = () => history.push('/purchasing/change-requests/create');

    const search = () =>
        dispatch(
            changeRequestsActions.searchWithOptions(
                newPartNumber,
                `&outstanding=${outstanding}&lastMonths=${lastMonths}`
            )
        );

    return (
        <Page history={history}>
            <Grid container>
                <Grid item xs={10}>
                    <Typography variant="h3">Search Change Request</Typography>
                </Grid>
                <Grid item xs={2}>
                    <Button variant="outlined" className={classes.button} onClick={create}>
                        Create
                    </Button>
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        fullWidth
                        placeholder="Go to Change Request"
                        value={documentNumber}
                        label="Change Request"
                        propertyName="changeRequest"
                        onChange={(_, newValue) => setDocumentNumber(newValue)}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        color="primary"
                        className={classes.button}
                        onClick={lookup}
                    >
                        Go
                    </Button>
                </Grid>
                <Grid item xs={5} />
                <Grid item xs={5} className={classes.gap}>
                    <InputField
                        fullWidth
                        placeholder="Search old/new part number * wildcard"
                        value={newPartNumber}
                        label="Part Number"
                        propertyName="newPartNumber"
                        onChange={(_, newValue) => setNewPartNumber(newValue)}
                    />
                </Grid>
                <Grid item xs={3} className={classes.gap}>
                    <Dropdown
                        fullWidth
                        value={filter}
                        label="Filter"
                        items={[
                            { id: 'OUTSTANDING', displayText: 'Just Outstanding' },
                            { id: 3, displayText: 'Last 3 Months' },
                            { id: 6, displayText: 'Last 6 Months' },
                            { id: 60, displayText: 'Last 5 Years' }
                        ]}
                        propertyName="changeType"
                        onChange={handleDropDownChange}
                        allowNoValue={false}
                    />
                </Grid>
                <Grid item xs={2} className={classes.gap}>
                    <Button
                        variant="outlined"
                        color="primary"
                        className={classes.button}
                        onClick={search}
                    >
                        Search
                    </Button>
                </Grid>
                <Grid item xs={2} />
                <Grid item xs={12}>
                    {searchRequestsLoading && <Loading />}
                    {searchRequestsResults.length > 0 && (
                        <DataGrid
                            getRowId={row => row.documentNumber}
                            className={classes.gap}
                            rows={searchRequestsResults}
                            columns={columns}
                            rowHeight={34}
                            autoHeight
                            loading={false}
                            hideFooter
                        />
                    )}
                </Grid>
            </Grid>
        </Page>
    );
}

export default ChangeRequestSearch;
