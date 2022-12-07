import React from 'react';
import PropTypes from 'prop-types';
import TextField from '@mui/material/TextField';
import InputLabel from '@mui/material/InputLabel';
import makeStyles from '@mui/styles/makeStyles';

function ChangeState({ changeState, showLabel }) {
    const stateColor = state => {
        if (state === 'PROPOS') {
            return 'yellow';
        }
        if (state === 'CANCEL') {
            return 'pink';
        }
        if (state === 'ACCEPT') {
            return '#ACE1AF'; // Celadon light green
        }
        return 'transparent';
    };

    const useStyles = makeStyles(theme => ({
        root: {
            paddingTop: 0,
            marginTop: theme.spacing(1),
            backgroundColor: stateColor(changeState)
        },
        label: {
            fontSize: theme.typography.fontSize
        }
    }));

    const classes = useStyles();

    return (
        <>
            {showLabel && <InputLabel classes={{ root: classes.label }}>Change State</InputLabel>}
            <TextField
                classes={{
                    root: classes.root
                }}
                value={changeState}
                propertyName="changeState"
                size="small"
                variant="outlined"
                color="success"
                disabled
            />
        </>
    );
}

ChangeState.propTypes = {
    changeState: PropTypes.string,
    showLabel: PropTypes.bool
};

ChangeState.defaultProps = {
    changeState: null,
    showLabel: true
};

export default ChangeState;
