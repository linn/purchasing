import React, { useState, Fragment, useEffect } from 'react';
import PropTypes from 'prop-types';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { makeStyles } from '@mui/styles';
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
    collectionSelectorHelpers,
    Title,
    useSearch
} from '@linn-it/linn-form-components-library';
import history from '../../history';
import config from '../../config';
import Button from '@mui/material/Button';
import { useParams } from 'react-router-dom';
import Dialog from '@mui/material/Dialog';
import Divider from '@mui/material/Divider';
import { Link as RouterLink } from 'react-router-dom';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import EmailIcon from '@mui/icons-material/Email';
import SendIcon from '@mui/icons-material/Send';

import Tooltip from '@mui/material/Tooltip';

const useStyles = makeStyles(theme => ({
    a: {
        textDecoration: 'none'
    },
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
    button: {
        marginLeft: theme.spacing(1),
        marginTop: theme.spacing(1)
    },
    clearButtonInline: {
        marginTop: theme.spacing(1),
        display: 'inline-block'
    },
    clearButton: {
        padding: '7px 0px'
    }
}));

function Typeahead({
    fetchItems,
    items,
    title,
    loading,
    clearSearch,
    modal,
    openModalOnClick,
    handleFieldChange,
    links,
    label,
    onSelect,
    value,
    placeholder,
    disabled,
    minimumSearchTermLength,
    debounce,
    searchButtonOnly,
    propertyName,
    priorityFunction,
    resultLimit,
    clearable,
    clearTooltipText,
    onClear,
    required
}) {
    const [searchTerm, setSearchTerm] = useState('');
    const [dialogOpen, setDialogOpen] = useState(false);

    const classes = useStyles();

    useSearch(fetchItems, searchTerm, clearSearch, null, null, debounce, minimumSearchTermLength);

    const handleSearchTermChange = (...args) => {
        setSearchTerm(args[1]);
    };

    const handleClick = e => {
        if (modal) {
            setDialogOpen(false);
        }
        if (clearSearch) {
            clearSearch();
        }
        setSearchTerm(null);
        if (!links) {
            onSelect(e);
        }
    };

    const Item = ({ item, onClick }) => (
        <ListItem button onClick={modal ? onClick : undefined}>
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
            name: PropTypes.string,
            description: PropTypes.string,
            href: PropTypes.string
        }).isRequired,
        onClick: PropTypes.func
    };

    Item.defaultProps = { onClick: null };

    const onChange = () => {
        if (modal && !openModalOnClick) {
            return handleFieldChange;
        }
        if (modal && openModalOnClick) {
            return () => setDialogOpen(true);
        }
        return handleSearchTermChange;
    };

    const results = () => {
        if (loading) {
            return <Loading />;
        }

        let result = items;

        if (priorityFunction) {
            result = result
                .map(i => ({ ...i, priority: priorityFunction(i, searchTerm) }))
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

        if (result?.length > 0) {
            return (
                <List dense>
                    {result.map(item => (
                        <Fragment key={item.id}>
                            {links ? (
                                <Link className={classes.a} component={RouterLink} to={item?.href}>
                                    <Item item={item} onClick={() => handleClick(item)} />
                                </Link>
                            ) : (
                                <Item item={item} onClick={() => handleClick(item)} />
                            )}
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
            {!modal ? <Title text={title} /> : <></>}
            {modal && searchButtonOnly ? (
                <Tooltip title={label}>
                    <IconButton
                        disabled={disabled}
                        onClick={() => {
                            setDialogOpen(true);
                            clearSearch();
                        }}
                        size="large"
                    >
                        {SendIcon()}
                    </IconButton>
                </Tooltip>
            ) : (
                <InputField
                    adornment={SendIcon(() => setDialogOpen(true))}
                    propertyName={propertyName}
                    textFieldProps={{
                        onClick: () => {
                            if (!disabled) {
                                if (openModalOnClick) {
                                    setDialogOpen(true);
                                    clearSearch();
                                }
                            }
                        },
                        disabled
                    }}
                    value={modal ? value : searchTerm}
                    label={label}
                    placeholder={placeholder}
                    onChange={onChange()}
                    required={required}
                />
            )}
            {clearable && (
                <div className={classes.clearButtonInline}>
                    <Tooltip title={clearTooltipText}>
                        <Button
                            variant="outlined"
                            onClick={onClear}
                            disabled={disabled}
                            className={classes.clearButton}
                        >
                            X
                        </Button>
                    </Tooltip>
                </div>
            )}
            {modal ? (
                <Dialog
                    data-testid="modal"
                    open={dialogOpen}
                    onClose={() => setDialogOpen(false)}
                    fullWidth
                    maxWidth="md"
                >
                    <div>
                        <IconButton
                            className={classes.pullRight}
                            aria-label="Close"
                            onClick={() => setDialogOpen(false)}
                            size="large"
                        >
                            <CloseIcon />
                        </IconButton>
                        <div className={classes.dialog}>
                            <Typography variant="h5" gutterBottom>
                                {title}
                            </Typography>
                            <InputField
                                propertyName={propertyName}
                                adornment={SendIcon()}
                                textFieldProps={{
                                    autoFocus: true
                                }}
                                placeholder={placeholder}
                                onChange={handleSearchTermChange}
                                value={searchTerm}
                                required={required}
                            />
                            {loading ? <Loading /> : results()}
                        </div>
                    </div>
                </Dialog>
            ) : (
                results()
            )}
        </>
    );
}

const itemShape = {
    id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    name: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    description: PropTypes.string,
    href: PropTypes.string
};

Typeahead.propTypes = {
    propertyName: PropTypes.string,
    items: PropTypes.arrayOf(PropTypes.shape(itemShape)).isRequired,
    title: PropTypes.string,
    loading: PropTypes.bool,
    fetchItems: PropTypes.func.isRequired,
    clearSearch: PropTypes.func.isRequired,
    modal: PropTypes.bool,
    openModalOnClick: PropTypes.bool,
    handleFieldChange: PropTypes.func,
    links: PropTypes.bool,
    label: PropTypes.string,
    onSelect: PropTypes.func,
    value: PropTypes.string,
    placeholder: PropTypes.string,
    disabled: PropTypes.bool,
    minimumSearchTermLength: PropTypes.number,
    debounce: PropTypes.number,
    searchButtonOnly: PropTypes.bool,
    priorityFunction: PropTypes.func,
    resultLimit: PropTypes.number,
    clearable: PropTypes.bool,
    clearTooltipText: PropTypes.string,
    onClear: PropTypes.func,
    required: PropTypes.bool
};

Typeahead.defaultProps = {
    title: '',
    loading: false,
    modal: false,
    openModalOnClick: true,
    links: true,
    label: null,
    onSelect: null,
    value: null,
    placeholder: 'Search by id or by description',
    disabled: false,
    minimumSearchTermLength: 1,
    debounce: 500,
    searchButtonOnly: false,
    propertyName: '',
    priorityFunction: null,
    resultLimit: null,
    handleFieldChange: null,
    clearable: false,
    clearTooltipText: 'Clear',
    required: false,
    onClear: () => {}
};

export default Typeahead;
