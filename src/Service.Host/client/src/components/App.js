import React, { useEffect } from 'react';
import Typography from '@mui/material/Typography';
import { useSelector, useDispatch } from 'react-redux';
import { Page, userSelectors } from '@linn-it/linn-form-components-library';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import { Link } from 'react-router-dom';
import { testAction } from '../actions';
import config from '../config';
import history from '../history';

function App() {
    const name = useSelector(state => userSelectors.getName(state));
    const dispatch = useDispatch();

    useEffect(() => dispatch(testAction()), [dispatch]);

    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Typography variant="h6">App</Typography>
            <Typography>Hello {name}</Typography>

            <List>
                <ListItem component={Link} to="/purchasing/signing-limits" button>
                    <Typography color="primary">Signing Limits</Typography>
                </ListItem>
                <ListItem component={Link} to="/purchasing/part-suppliers" button>
                    <Typography color="primary">Part Supplier Utility</Typography>
                </ListItem>
                <Typography variant="h6">Reports</Typography>
                <ListItem component={Link} to="/purchasing/reports/orders-by-supplier" button>
                    <Typography color="primary">Orders by Supplier Report</Typography>
                </ListItem>
            </List>
        </Page>
    );
}

export default App;
