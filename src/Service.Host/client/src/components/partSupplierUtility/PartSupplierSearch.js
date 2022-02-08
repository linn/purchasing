import React, { useState, Fragment, useEffect } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { makeStyles } from '@mui/styles';
import Button from '@mui/material/Button';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import Link from '@mui/material/Link';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    InputField,
    Loading,
    utilities,
    CreateButton,
    collectionSelectorHelpers
} from '@linn-it/linn-form-components-library';
import Divider from '@mui/material/Divider';
import { Link as RouterLink } from 'react-router-dom';
import partSuppliersActions from '../../actions/partSuppliersActions';
import history from '../../history';
import config from '../../config';

function PartSupplierSearch() {
    const useStyles = makeStyles(theme => ({
        button: {
            marginLeft: theme.spacing(1),
            marginTop: theme.spacing(4)
        },
        a: {
            textDecoration: 'none'
        }
    }));

    const classes = useStyles();

    const loading = useSelector(state =>
        collectionSelectorHelpers.getSearchLoading(state.partSuppliers)
    );

    const results = useSelector(state =>
        collectionSelectorHelpers.getSearchItems(state.partSuppliers)
    );
    const applicationState = useSelector(state =>
        collectionSelectorHelpers.getApplicationState(state.partSuppliers)
    );

    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(partSuppliersActions.fetchState());
    }, [dispatch]);

    const [options, setOptions] = useState({ partNumber: '', supplierName: '' });

    const handleOptionsChange = (propertyName, newValue) =>
        setOptions({ ...options, [propertyName]: newValue });

    const createUrl = utilities.getHref(applicationState, 'create');

    const search = () =>
        dispatch(
            partSuppliersActions.searchWithOptions(
                '',
                `&partNumber=${options.partNumber}&supplierName=${options.supplierName}`
            )
        );
    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={3}>
                <Grid item xs={10}>
                    <Typography variant="h3">Part Suppliers Search</Typography>
                </Grid>
                <Grid item xs={2}>
                    <CreateButton createUrl={createUrl} disabled={!createUrl} />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        fullWidth
                        value={options.partNumber}
                        label="Part Number"
                        propertyName="partNumber"
                        onChange={handleOptionsChange}
                        textFieldProps={{
                            onKeyDown: data => {
                                if (
                                    options.partNumber &&
                                    (data.keyCode === 13 || data.keyCode === 9)
                                ) {
                                    search();
                                }
                            }
                        }}
                    />
                </Grid>
                <Grid item xs={5}>
                    <InputField
                        fullWidth
                        value={options.supplierName}
                        label="Supplier"
                        propertyName="supplierName"
                        onChange={handleOptionsChange}
                        textFieldProps={{
                            onKeyDown: data => {
                                if (
                                    options.supplierName &&
                                    (data.keyCode === 13 || data.keyCode === 9)
                                ) {
                                    search();
                                }
                            }
                        }}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button
                        variant="outlined"
                        color="primary"
                        className={classes.button}
                        onClick={() => search()}
                    >
                        Go
                    </Button>
                </Grid>
                {loading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
                <Grid item xs={12}>
                    <List dense>
                        {results.map(item => (
                            <Fragment key={item.partNumber + item.supplierName}>
                                <Link
                                    className={classes.a}
                                    component={RouterLink}
                                    to={utilities.getSelfHref(item)}
                                >
                                    <ListItem>
                                        <Grid item xs={3}>
                                            <Typography variant="subtitle1">
                                                {item.partNumber}
                                            </Typography>
                                        </Grid>
                                        <Grid item xs={9}>
                                            <Typography>
                                                {item.supplierRanking === 1
                                                    ? `${item.supplierName} (PREFERRED)`
                                                    : item.supplierName}
                                            </Typography>
                                        </Grid>
                                    </ListItem>
                                </Link>

                                <Divider component="li" />
                            </Fragment>
                        ))}
                    </List>
                </Grid>
            </Grid>
        </Page>
    );
}

export default PartSupplierSearch;
