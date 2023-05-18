import React from 'react';
import ArrowRightIcon from '@mui/icons-material/ArrowRight';
import ArrowLeftIcon from '@mui/icons-material/ArrowLeft';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import PropTypes from 'prop-types';

function PrevNextButtons({ goPrev, goNext, nextResult, prevResult }) {
    return (
        <>
            {goPrev ? (
                <Grid item xs={2}>
                    <Button startIcon={<ArrowLeftIcon />} onClick={goPrev}>
                        {prevResult}
                    </Button>
                </Grid>
            ) : (
                <Grid itemx xs={2} />
            )}
            <Grid item xs={8} />
            {goNext ? (
                <Grid item xs={2}>
                    <Button endIcon={<ArrowRightIcon />} onClick={goNext}>
                        {nextResult}
                    </Button>
                </Grid>
            ) : (
                <Grid item xs={2} />
            )}
        </>
    );
}

PrevNextButtons.propTypes = {
    goPrev: PropTypes.func,
    goNext: PropTypes.func,
    nextResult: PropTypes.string,
    prevResult: PropTypes.string
};

PrevNextButtons.defaultProps = {
    goPrev: null,
    goNext: null,
    nextResult: '',
    prevResult: ''
};

export default PrevNextButtons;
