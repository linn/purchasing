import React, { useState, Fragment } from 'react';
import PropTypes from 'prop-types';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import { InputField, Loading } from '@linn-it/linn-form-components-library';
import SearchIcon from '@mui/icons-material/Search';
import Divider from '@mui/material/Divider';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import Dialog from '@mui/material/Dialog';
import makeStyles from '@mui/styles/makeStyles';

const useStyles = makeStyles(theme => ({
    nameText: {
        fontWeight: theme.typography.fontWeightBold
    },
    bodyText: {
        color: theme.palette.text.primary
    },
    pullRight: {
        float: 'right'
    },
    dialog: {
        margin: theme.spacing(6),
        minWidth: theme.spacing(62)
    },
    pad: { padding: theme.spacing(2) }
}));
function Search({
    propertyName,
    label,
    value,
    handleValueChange,
    disabled,
    search,
    searchResults,
    loading,
    priorityFunction,
    onResultSelect,
    resultLimit,
    resultsInModal,
    clearSearch,
    searchOnEnter,
    onKeyPressFunctions,
    helperText
}) {
    const classes = useStyles();
    const [dialogOpen, setDialogOpen] = useState(false);
    const [hasSearched, setHasSearched] = useState(false);

    const countMatchingCharacters = (item, searchTerm) => {
        let count = 0;
        if (searchTerm) {
            for (let i = 0; i < searchTerm.length; i += 1) {
                if (item.name.toUpperCase?.()[i] === searchTerm.toUpperCase()[i]) {
                    count += 1;
                }
            }
        }

        return count;
    };

    const Item = ({ item }) => (
        <ListItem
            className={classes.pad}
            button
            onClick={() => {
                clearSearch();
                if (resultsInModal) {
                    setDialogOpen(false);
                }
                onResultSelect(item);
                setHasSearched(false);
            }}
        >
            <Grid container spacing={3}>
                <Grid item xs={3}>
                    <Typography classes={{ root: classes.nameText }}>{item.name}</Typography>
                </Grid>
                <Grid item xs={9}>
                    <Typography classes={{ root: classes.bodyText }}>{item.description}</Typography>
                </Grid>
            </Grid>
        </ListItem>
    );

    Item.propTypes = {
        item: PropTypes.shape({
            name: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
            description: PropTypes.string
        }).isRequired
    };

    const priority = (item, searchTerm) => {
        if (priorityFunction === 'closestMatchesFirst') {
            return countMatchingCharacters(item, searchTerm);
        }

        return priorityFunction(item, searchTerm);
    };

    const results = () => {
        if (loading) {
            return <Loading />;
        }

        let result = searchResults;

        if (priorityFunction) {
            result = result
                .map(i => ({
                    ...i,
                    priority: priority(i, value)
                }))
                .sort((a, b) => {
                    if (a.priority > b.priority) {
                        return -1;
                    }
                    if (a.priority < b.priority) {
                        return 1;
                    }
                    return 0;
                });
        }

        if (resultLimit) {
            result = result.slice(0, resultLimit);
        }

        if (result?.length > 0 || !hasSearched) {
            return (
                <List dense>
                    {result.map(r => (
                        <Fragment key={r.id}>
                            <Item item={r} />
                            <Divider component="li" />
                        </Fragment>
                    ))}
                </List>
            );
        }
        return <Typography>No matching items</Typography>;
    };
    return (
        <>
            <InputField
                value={value}
                propertyName={propertyName}
                label={label}
                adornment={<SearchIcon />}
                onChange={handleValueChange}
                helperText={helperText}
                textFieldProps={{
                    disabled,
                    onKeyDown: data => {
                        if (searchOnEnter && data.keyCode === 13) {
                            if (resultsInModal) {
                                setDialogOpen(true);
                            }
                            search(value);
                            setHasSearched(true);
                        }
                        onKeyPressFunctions.forEach(element => {
                            if (data.keyCode === element.keyCode) {
                                element.action();
                            }
                        });
                    }
                }}
            />
            {resultsInModal ? (
                <Dialog data-testid="modal" open={dialogOpen} fullWidth maxWidth="md">
                    <div>
                        <IconButton
                            className={classes.pullRight}
                            aria-label="Close"
                            onClick={() => setDialogOpen(false)}
                            size="large"
                        >
                            <CloseIcon />
                        </IconButton>
                        <div className={classes.dialog}>{loading ? <Loading /> : results()}</div>
                    </div>
                </Dialog>
            ) : (
                results()
            )}
        </>
    );
}

Search.propTypes = {
    propertyName: PropTypes.string.isRequired,
    label: PropTypes.string.isRequired,
    clearSearch: PropTypes.func.isRequired,
    handleValueChange: PropTypes.func.isRequired,
    search: PropTypes.func.isRequired,
    onResultSelect: PropTypes.func.isRequired,
    value: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    disabled: PropTypes.bool,
    searchResults: PropTypes.arrayOf(PropTypes.shape({})),
    loading: PropTypes.bool,
    priorityFunction: PropTypes.oneOfType([PropTypes.string, PropTypes.func]),
    resultLimit: PropTypes.number,
    resultsInModal: PropTypes.bool,
    searchOnEnter: PropTypes.bool,
    onKeyPressFunctions: PropTypes.arrayOf(
        PropTypes.shape({ keyCode: PropTypes.number, action: PropTypes.func })
    ),
    helperText: PropTypes.string
};
Search.defaultProps = {
    searchOnEnter: true,
    onKeyPressFunctions: [],
    value: null,
    disabled: false,
    searchResults: [],
    loading: false,
    priorityFunction: null,
    resultLimit: null,
    resultsInModal: false,
    helperText: 'PRESS ENTER TO SEARCH'
};

export default Search;
