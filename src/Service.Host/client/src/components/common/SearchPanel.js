import React, { useState } from 'react';
import PropTypes from 'prop-types';
import withStyles from '@mui/styles/withStyles';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import ListItem from '@mui/material/ListItem';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Close from '@mui/icons-material/Close';
import { SearchInputField } from '@linn-it/linn-form-components-library';

const styles = {
    listItemText: {
        '&:first-child': {
            paddingLeft: 30,
            paddingTop: 0,
            paddingBottom: 0
        }
    },
    paper: {
        backgroundColor: '#f5f5f5',
        position: 'relative',
        zIndex: -1
    },
    menuItems: {
        fontSize: '12px',
        lineHeight: 2
    },
    closeButton: {
        marginRight: '10px',
        marginTop: '10px',
        position: 'absolute',
        top: 0,
        right: 0,
        zIndex: 1
    },
    searchInputField: {
        float: 'right'
    }
};

function SearchPanel({ menu, classes, close }) {
    const [searchTerm, setSearchTerm] = useState();

    const menuEntries = menu
        .map(s => s.columns)
        .flat()
        .map(c => c.categories)
        .flat()
        .map(i => i.items)
        .flat();

    const uniqueEntries = Object.values(
        menuEntries.reduce((uniques, entry) => {
            if (!uniques[entry.href]) {
                return { ...uniques, [entry.href]: entry };
            }
            return uniques;
        }, {})
    );

    const handleFieldChange = (propertyName, newValue) => {
        setSearchTerm(newValue);
    };
    return (
        <Paper classes={{ root: classes.paper }}>
            <Button onClick={close} color="secondary" style={styles.closeButton}>
                <Close />
            </Button>
            <Grid container>
                <Grid item xs={12} sm={6} md={4} lg={3} xl={3} justify-content="flex-end">
                    <SearchInputField
                        value={searchTerm}
                        onChange={handleFieldChange}
                        textFieldProps={{
                            autoFocus: true
                        }}
                        placeholder="start typing..."
                    />
                    {searchTerm?.length > 1 &&
                        uniqueEntries
                            .filter(
                                e =>
                                    e.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
                                    e.href.toLowerCase().includes(searchTerm.toLowerCase())
                            )
                            .map(entry => (
                                <React.Fragment key={entry.href}>
                                    <a href={entry.href} style={{ textDecoration: 'none' }}>
                                        <ListItem classes={{ root: classes.listItemText }} button>
                                            <Typography
                                                variant="overline"
                                                classes={{
                                                    overline: classes.menuItems
                                                }}
                                                color="primary"
                                            >
                                                {entry.title}
                                            </Typography>
                                        </ListItem>
                                    </a>
                                </React.Fragment>
                            ))}
                </Grid>
            </Grid>
        </Paper>
    );
}

SearchPanel.propTypes = {
    classes: PropTypes.shape({
        listItemText: PropTypes.string,
        menuItems: PropTypes.string,
        paper: PropTypes.string
    }).isRequired,
    close: PropTypes.func.isRequired,
    menu: PropTypes.arrayOf(PropTypes.shape({})).isRequired
};

export default withStyles(styles)(SearchPanel);
