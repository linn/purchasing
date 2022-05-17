import React from 'react';
import Typography from '@mui/material/Typography';
import { useSelector } from 'react-redux';
import { Page, userSelectors } from '@linn-it/linn-form-components-library';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
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
                <ListItem component={Link} to="/purchasing/edi" button>
                    <Typography color="primary">Send EDI Orders</Typography>
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
        </Page>
    );
}

export default App;
