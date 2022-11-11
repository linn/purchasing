import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import Grid from '@mui/material/Grid';
import { useSelector, useDispatch } from 'react-redux';
import {
    Page,
    itemSelectorHelpers,
    SaveBackCancelButtons
} from '@linn-it/linn-form-components-library';
import boardActions from '../../actions/boardActions';
import boardsActions from '../../actions/boardsActions';
import history from '../../history';
import config from '../../config';

function Board() {
    const reduxDispatch = useDispatch();
    const { id } = useParams();

    const item = useSelector(reduxState => itemSelectorHelpers.getItem(reduxState.board));

    useEffect(() => {
        if (id) {
            reduxDispatch(boardActions.fetch(id));
        }
    }, [id, reduxDispatch]);

    useEffect(() => {
        reduxDispatch(boardsActions.fetchState());
    }, [reduxDispatch]);

    return (
        <Page history={history} homeUrl={config.appRoot}>
            <Grid container spacing={2}>
                <Grid>Here {item?.boardCode}</Grid>
                <Grid item xs={12}>
                    <SaveBackCancelButtons
                        saveDisabled
                        saveClick={() => {}}
                        cancelClick={() => {}}
                        backClick={() => history.push('/purchasing/boms/boards')}
                    />
                </Grid>
            </Grid>
        </Page>
    );
}

export default Board;
