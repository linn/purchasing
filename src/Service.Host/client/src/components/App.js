import React, { useEffect } from 'react';
import Typography from '@material-ui/core/Typography';
import { useSelector, useDispatch } from 'react-redux';
import { Page } from '@linn-it/linn-form-components-library';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import { Link } from 'react-router-dom';
import getName from '../selectors/userSelectors';
import { testAction } from '../actions';
import config from '../config';
import history from '../history';

function App() {
    const name = useSelector(state => getName(state));
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
            </List>
        </Page>
    );
}

export default App;
