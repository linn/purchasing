import React, { useState } from 'react';
import PropTypes from 'prop-types';
import withStyles from '@mui/styles/withStyles';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import AppBar from '@mui/material/AppBar';
import { utilities } from '@linn-it/linn-form-components-library';
import ClickAwayListener from '@mui/material/ClickAwayListener';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Toolbar from '@mui/material/Toolbar';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Badge from '@mui/material/Badge';
import Button from '@mui/material/Button';
import { useSnackbar } from 'notistack';
import AccountCircle from '@mui/icons-material/AccountCircle';
import Search from '@mui/icons-material/Search';
import Notifications from '@mui/icons-material/Notifications';
import Panel from './Panel';
import SearchPanel from './SearchPanel';

const styles = theme => ({
    body: { margin: 0, padding: 0 },
    root: {
        position: 'absolute',
        width: '100%',
        amrgin: 0,
        top: 0,
        zIndex: 10
    },
    tabLabel: {
        fontSize: theme.typography.fontSize,
        color: theme.palette.grey[200]
    },
    snackbarNew: {
        background: theme.palette.primary.dark,
        width: '800px'
    },
    snackbarSeen: {
        width: '800px'
    },
    panel: {
        position: 'relative'
    },
    menuButton: {
        marginLeft: -12,
        marginRight: 20
    },
    tab: {
        ...theme.mixins.toolbar,
        minWidth: '100px'
    },
    toolbar: {
        paddingLeft: 0,
        paddingRight: 0
    },
    tabs: {
        ...theme.mixins.toolbar,
        paddingLeft: 40
    },
    container: {
        width: '100%',
        margin: 0
    },
    appBar: {
        backgroundColor: theme.palette.grey[800],
        width: '100% !important',
        margin: 0
    },
    icons: {
        cursor: 'pointer',
        color: 'white'
    }
});

function Navigation({
    classes,
    sections,
    loading,
    username,
    myStuff,
    seenNotifications,
    unseenNotifications,
    markNotificationSeen,
    handleSignOut
}) {
    const [selected, setSelected] = useState(false);
    const [anchorEl, setAnchorEl] = useState();
    const { enqueueSnackbar, closeSnackbar } = useSnackbar();

    if (sections) {
        const menuIds = sections.map(item => item.id);

        const handleClick = event => {
            setAnchorEl(event.currentTarget);
        };
        const handleClose = () => {
            setAnchorEl();
        };

        const actions = (key, e) => (
            <>
                <Button
                    variant="contained"
                    onClick={() => {
                        window.location = utilities.getSelfHref(e);
                    }}
                >
                    View
                </Button>
                <Button
                    onClick={() => {
                        closeSnackbar(key);
                        localStorage.setItem(e.title, e.content);
                        markNotificationSeen(e);
                    }}
                >
                    Dismiss
                </Button>
            </>
        );

        const noNotifications = () => {
            if (!seenNotifications && !unseenNotifications) {
                return true;
            }
            return seenNotifications.length + unseenNotifications.length === 0;
        };

        const queueNotifications = () => {
            if (noNotifications()) {
                enqueueSnackbar('No notifications to show!', {
                    anchorOrigin: {
                        vertical: 'bottom',
                        horizontal: 'right'
                    },
                    variant: 'info',
                    preventDuplicate: true
                });
            } else {
                unseenNotifications.concat(seenNotifications).forEach((e, i) => {
                    setTimeout(() => {
                        enqueueSnackbar(`${e.title} ${e.content}`, {
                            anchorOrigin: {
                                vertical: 'bottom',
                                horizontal: 'right'
                            },
                            ContentProps: {
                                classes: {
                                    root: localStorage.getItem(e.title)
                                        ? classes.snackbarSeen
                                        : classes.snackbarNew
                                }
                            },
                            action: key => actions(key, e),
                            preventDuplicate: true
                        });
                    }, i * 200);
                });
            }
        };

        return (
            <>
                <ClickAwayListener onClickAway={() => setSelected(false)}>
                    <div className="hide-when-printing">
                        <div className={classes.root}>
                            <Grid container item spacing={3}>
                                <Grid item />
                                {sections && !loading && (
                                    <AppBar position="static" classes={{ root: classes.appBar }}>
                                        <Toolbar classes={{ gutters: classes.toolbar }}>
                                            <Grid
                                                container
                                                alignItems="center"
                                                justifyContent="space-between"
                                                spacing={0}
                                                classes={{ container: classes.container }}
                                            >
                                                <Grid item xs={9}>
                                                    <Tabs
                                                        classes={{
                                                            root: classes.tabs
                                                        }}
                                                        value={selected}
                                                        onChange={(event, value) => {
                                                            if (selected === value) {
                                                                setSelected(false);
                                                            } else {
                                                                setSelected(value);
                                                            }
                                                        }}
                                                        scrollButtons="auto"
                                                        variant="scrollable"
                                                        indicatorColor="primary"
                                                        textColor="primary"
                                                    >
                                                        {sections.map(item => (
                                                            <Tab
                                                                id={item.id}
                                                                key={item.id}
                                                                classes={{ root: classes.tab }}
                                                                label={
                                                                    <span
                                                                        className={classes.tabLabel}
                                                                    >
                                                                        {item.title}
                                                                    </span>
                                                                }
                                                                selected={false}
                                                            />
                                                        ))}
                                                    </Tabs>
                                                </Grid>
                                                <Grid item xs={1}>
                                                    <Typography variant="h4">
                                                        <AccountCircle
                                                            className={classes.icons}
                                                            aria-owns={
                                                                anchorEl ? 'simple-menu' : undefined
                                                            }
                                                            onClick={handleClick}
                                                            id={sections.length}
                                                            key={sections.length}
                                                        />
                                                    </Typography>
                                                </Grid>
                                                <Grid item xs={1}>
                                                    <Typography variant="h4">
                                                        <Badge
                                                            badgeContent={
                                                                unseenNotifications
                                                                    ? unseenNotifications.length
                                                                    : 0
                                                            }
                                                            color="primary"
                                                            variant="dot"
                                                        >
                                                            <Notifications
                                                                className={classes.icons}
                                                                onClick={queueNotifications}
                                                            />
                                                        </Badge>
                                                    </Typography>
                                                </Grid>
                                                <Grid item xs={1}>
                                                    <Typography variant="h4">
                                                        <Search
                                                            className={classes.icons}
                                                            onClick={() =>
                                                                setSelected(sections.length)
                                                            }
                                                        />
                                                    </Typography>
                                                </Grid>
                                                <Menu
                                                    id="simple-menu"
                                                    anchorEl={anchorEl}
                                                    open={Boolean(anchorEl)}
                                                    onClose={handleClose}
                                                >
                                                    <MenuItem onClick={handleClose}>
                                                        {username}
                                                    </MenuItem>
                                                    {username &&
                                                        myStuff.groups.map(item => (
                                                            <span key={item.items[0].href}>
                                                                <a href={item.items[0].href}>
                                                                    <MenuItem onClick={handleClose}>
                                                                        {item.items[0].title}
                                                                    </MenuItem>
                                                                </a>
                                                            </span>
                                                        ))}
                                                    {handleSignOut && (
                                                        <MenuItem
                                                            style={{
                                                                color: 'blue',
                                                                textDecoration: 'underline',
                                                                cursor: 'pointer',
                                                                background: 'none'
                                                            }}
                                                            onClick={handleSignOut}
                                                        >
                                                            Sign Out (Newer apps pages)
                                                        </MenuItem>
                                                    )}
                                                </Menu>
                                            </Grid>
                                        </Toolbar>
                                    </AppBar>
                                )}
                            </Grid>
                            {menuIds.map(
                                (item, i) =>
                                    selected === i && (
                                        <Panel
                                            key={item}
                                            section={sections.find(e => e.id === item)}
                                            id={item}
                                            style={{ align: 'right' }}
                                            anchorEl={item.id}
                                            close={() => setSelected(false)}
                                        />
                                    )
                            )}
                            {selected === sections.length && (
                                <SearchPanel menu={sections} close={() => setSelected(false)} />
                            )}
                        </div>
                    </div>
                </ClickAwayListener>
            </>
        );
    }
    return (
        <div className={classes.root}>
            <AppBar position="static" color="default">
                <Toolbar classes={{ gutters: classes.toolbar }} />
            </AppBar>
        </div>
    );
}

Navigation.propTypes = {
    classes: PropTypes.shape({
        snackbarNew: PropTypes.string,
        snackbarSeen: PropTypes.string,
        root: PropTypes.string,
        tabs: PropTypes.string,
        tab: PropTypes.string,
        tabLabel: PropTypes.string,
        toolbar: PropTypes.string,
        container: PropTypes.string,
        appBar: PropTypes.string,
        icons: PropTypes.string
    }).isRequired,
    sections: PropTypes.arrayOf(PropTypes.shape({})),
    loading: PropTypes.bool,
    username: PropTypes.string,
    myStuff: PropTypes.shape({ groups: PropTypes.arrayOf(PropTypes.shape({})) }),
    seenNotifications: PropTypes.arrayOf(PropTypes.shape({})),
    unseenNotifications: PropTypes.arrayOf(PropTypes.shape({})),
    markNotificationSeen: PropTypes.func.isRequired,
    handleSignOut: PropTypes.func
};

Navigation.defaultProps = {
    sections: null,
    myStuff: null,
    seenNotifications: [],
    unseenNotifications: [],
    loading: false,
    username: '',
    handleSignOut: null
};

export default withStyles(styles)(Navigation);
