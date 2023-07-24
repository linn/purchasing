import React from 'react';
import PropTypes from 'prop-types';
import Grid from '@mui/material/Grid';
import { InputField, Dropdown } from '@linn-it/linn-form-components-library';

function BoardTab({
    boardCode,
    description,
    coreBoard,
    clusterBoard,
    idBoard,
    handleFieldChange,
    defaultPcbNumber,
    variantOfBoardCode,
    splitBom,
    creating
}) {
    return (
        <Grid container spacing={2} style={{ paddingTop: '30px' }}>
            <Grid item xs={2}>
                <InputField
                    fullWidth
                    value={boardCode}
                    label="Code"
                    disabled={!creating}
                    propertyName="boardCode"
                    onChange={(propertyName, newValue) => handleFieldChange(propertyName, newValue)}
                />
            </Grid>
            <Grid item xs={10} />
            <Grid item xs={4}>
                <InputField
                    fullWidth
                    value={description}
                    label="Description"
                    propertyName="description"
                    onChange={(propertyName, newValue) => handleFieldChange(propertyName, newValue)}
                />
            </Grid>
            <Grid item xs={8} />
            <Grid item xs={3}>
                <Dropdown
                    value={coreBoard}
                    label="Core Board"
                    propertyName="coreBoard"
                    items={[
                        { id: 'N', displayText: 'No' },
                        { id: 'Y', displayText: 'Yes' }
                    ]}
                    allowNoValue={false}
                    onChange={(propertyName, newValue) => handleFieldChange(propertyName, newValue)}
                />
            </Grid>
            <Grid item xs={3}>
                <Dropdown
                    value={clusterBoard}
                    label="Cluster Board"
                    propertyName="clusterBoard"
                    items={[
                        { id: 'N', displayText: 'No' },
                        { id: 'Y', displayText: 'Yes' }
                    ]}
                    allowNoValue={false}
                    onChange={(propertyName, newValue) => handleFieldChange(propertyName, newValue)}
                />
            </Grid>
            <Grid item xs={3}>
                <Dropdown
                    value={idBoard}
                    label="Id Board"
                    propertyName="idBoard"
                    items={[
                        { id: 'N', displayText: 'No' },
                        { id: 'Y', displayText: 'Yes' }
                    ]}
                    allowNoValue={false}
                    onChange={(propertyName, newValue) => handleFieldChange(propertyName, newValue)}
                />
            </Grid>
            <Grid item xs={3} />
            <Grid item xs={3}>
                <InputField
                    fullWidth
                    value={defaultPcbNumber}
                    label="PCB Number"
                    propertyName="defaultPcbNumber"
                    onChange={(propertyName, newValue) => handleFieldChange(propertyName, newValue)}
                />
            </Grid>
            <Grid item xs={3}>
                <InputField
                    fullWidth
                    value={variantOfBoardCode}
                    label="Variant Of"
                    propertyName="variantOfBoardCode"
                    onChange={(propertyName, newValue) => handleFieldChange(propertyName, newValue)}
                />
            </Grid>
            <Grid item xs={6} />
            <Grid item xs={3}>
                <Dropdown
                    value={splitBom}
                    label="Split Bom"
                    propertyName="splitBom"
                    items={[
                        { id: 'N', displayText: 'No' },
                        { id: 'Y', displayText: 'Yes' }
                    ]}
                    allowNoValue={false}
                    onChange={(propertyName, newValue) => handleFieldChange(propertyName, newValue)}
                />
            </Grid>
            <Grid item xs={9} />
        </Grid>
    );
}

BoardTab.propTypes = {
    handleFieldChange: PropTypes.func.isRequired,
    boardCode: PropTypes.string,
    description: PropTypes.string,
    coreBoard: PropTypes.string,
    clusterBoard: PropTypes.string,
    idBoard: PropTypes.string,
    defaultPcbNumber: PropTypes.string,
    variantOfBoardCode: PropTypes.string,
    splitBom: PropTypes.string,
    creating: PropTypes.bool
};

BoardTab.defaultProps = {
    boardCode: null,
    description: null,
    coreBoard: null,
    clusterBoard: null,
    idBoard: null,
    defaultPcbNumber: null,
    variantOfBoardCode: null,
    splitBom: 'Y',
    creating: false
};

export default BoardTab;
