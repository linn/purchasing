import React from 'react';
import PropTypes from 'prop-types';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Close from '@mui/icons-material/Close';

function Panel({ section, close }) {
    const { columns } = section;

    return (
        <Paper
            sx={{
                backgroundColor: '#f5f5f5',
                position: 'fixed',
                zIndex: 1000,
                paddingTop: '80px',
                width: '100%',
                overflow: 'auto',
                height: '100vh'
            }}
        >
            <Button
                onClick={close}
                color="secondary"
                sx={{
                    marginRight: '10px',
                    marginTop: '10px',
                    float: 'right',
                    top: 0,
                    right: 0
                }}
            >
                <Close />
            </Button>
            <Grid container>
                {columns.map((col, i) => (
                    //eslint-disable-next-line react/no-array-index-key
                    <Grid item xs={12} sm={6} md={4} lg={3} xl={3} key={i}>
                        {col.categories
                            .filter(e => e.items.filter(item => item.showInMenu).length > 0)
                            .map(category => (
                                <List key={category.title} dense>
                                    <ListItem
                                        sx={{
                                            paddingTop: '0px !important',
                                            paddingBottom: '0px !important'
                                        }}
                                    >
                                        <Typography variant="button" gutterBottom>
                                            {category.title.replace('&amp;', '&')}
                                        </Typography>
                                    </ListItem>
                                    {category.items.map(
                                        entry =>
                                            entry.showInMenu && (
                                                <a
                                                    href={entry.href}
                                                    key={entry.href}
                                                    style={{ textDecoration: 'none' }}
                                                >
                                                    <ListItem
                                                        sx={{
                                                            paddingTop: 0,
                                                            paddingBottom: 0,
                                                            margin: 0
                                                        }}
                                                    >
                                                        <Typography
                                                            sx={{ margin: 0, lineHeight: 1.8 }}
                                                            variant="overline"
                                                            color="primary"
                                                        >
                                                            {entry.title}
                                                        </Typography>
                                                    </ListItem>
                                                </a>
                                            )
                                    )}
                                </List>
                            ))}
                    </Grid>
                ))}
                <Grid size={12}>
                    <a href={`/${section.id}`}>
                        <Button
                            onClick={close}
                            color="primary"
                            sx={{
                                marginRight: '10px',
                                marginBottom: '10px',
                                float: 'right'
                            }}
                        >
                            SHOW ALL OPTIONS...
                        </Button>
                    </a>
                </Grid>
            </Grid>
        </Paper>
    );
}

Panel.propTypes = {
    section: PropTypes.shape({
        id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        columns: PropTypes.arrayOf(PropTypes.shape({}))
    }).isRequired,
    close: PropTypes.func.isRequired
};

export default Panel;
