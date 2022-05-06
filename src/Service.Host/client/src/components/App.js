import React from 'react';
import Typography from '@mui/material/Typography';
import { useSelector } from 'react-redux';
import { Page, userSelectors, DatePicker } from '@linn-it/linn-form-components-library';
import List from '@mui/material/List';
import TextField from '@mui/material/TextField';

import ListItem from '@mui/material/ListItem';
import MuiDatePicker from '@mui/lab/DatePicker';
import moment from 'moment';
import { Link } from 'react-router-dom';
import config from '../config';
import history from '../history';

function App() {
    const name = useSelector(state => userSelectors.getName(state));

    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Typography variant="h6">Purchasing</Typography>
            <Typography>Hello {name}</Typography>

            <List>
                <ListItem component={Link} to="/purchasing/signing-limits" button>
                    <Typography color="primary">Signing Limits</Typography>
                </ListItem>
                <ListItem component={Link} to="/purchasing/part-suppliers" button>
                    <Typography color="primary">Part Supplier Utility</Typography>
                </ListItem>
                <ListItem component={Link} to="/purchasing/purchase-orders/reqs" button>
                    <Typography color="primary">Purchase Order Reqs Utility</Typography>
                </ListItem>
                <Typography variant="h6">Reports</Typography>
                <ListItem component={Link} to="/purchasing/reports/orders-by-supplier" button>
                    <Typography color="primary">Orders by Supplier Report</Typography>
                </ListItem>
                <ListItem component={Link} to="/purchasing/reports/orders-by-part" button>
                    <Typography color="primary">Orders by Part Report</Typography>
                </ListItem>
                <ListItem component={Link} to="/purchasing/reports/spend-by-supplier" button>
                    <Typography color="primary">Spend By Supplier Report</Typography>
                </ListItem>
                <ListItem component={Link} to="/purchasing/reports/spend-by-part" button>
                    <Typography color="primary">Spend By Part Report</Typography>
                </ListItem>
                <ListItem component={Link} to="/purchasing/material-requirements/run-mrp" button>
                    <Typography color="primary">Run Mrp</Typography>
                </ListItem>
                <ListItem
                    component={Link}
                    to="/purchasing/reports/suppliers-with-unacknowledged-orders"
                    button
                >
                    <Typography color="primary">Unacknowledged orders report</Typography>
                </ListItem>
                <ListItem component={Link} to="/purchasing/reports/pref-sup-receipts" button>
                    <Typography color="primary">Receipts vs Pref Sup Report</Typography>
                </ListItem>
            </List>

            <DatePicker
                label="From Date"
                value={new Date()}
                propertyName="fromDate"
                minDate="01/01/2000"
                maxDate="01/01/2050"
                onChange={newVal => {}}
            />
            <MuiDatePicker
                allowKeyboardControl
                margin="dense"
                inputVariant="outlined"
                autoOk
                format="DD/MM/YYYY"
                minDate={moment('01/01/2000')}
                maxDate={moment('01/01/2050')}
                renderInput={props => <TextField {...props} />}
                value={moment(new Date())}
                onChange={() => {}}
            />
        </Page>
    );
}

export default App;
