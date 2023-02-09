import React from 'react';
import PropTypes from 'prop-types';
import SvgIcon from '@mui/material/SvgIcon';
import { alpha, styled } from '@mui/material/styles';
import TreeView from '@mui/lab/TreeView';
import TreeItem, { treeItemClasses } from '@mui/lab/TreeItem';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import LinearProgress from '@mui/material/LinearProgress';

/* eslint react/jsx-props-no-spreading: 0 */
/* eslint react/destructuring-assignment: 0 */

function MinusSquare(props) {
    return (
        <SvgIcon fontSize="inherit" style={{ width: 14, height: 14 }} {...props}>
            <path d="M22.047 22.074v0 0-20.147 0h-20.12v0 20.147 0h20.12zM22.047 24h-20.12q-.803 0-1.365-.562t-.562-1.365v-20.147q0-.776.562-1.351t1.365-.575h20.147q.776 0 1.351.575t.575 1.351v20.147q0 .803-.575 1.365t-1.378.562v0zM17.873 11.023h-11.826q-.375 0-.669.281t-.294.682v0q0 .401.294 .682t.669.281h11.826q.375 0 .669-.281t.294-.682v0q0-.401-.294-.682t-.669-.281z" />
        </SvgIcon>
    );
}

function PlusSquare(props) {
    return (
        <SvgIcon fontSize="inherit" style={{ width: 14, height: 14 }} {...props}>
            <path d="M22.047 22.074v0 0-20.147 0h-20.12v0 20.147 0h20.12zM22.047 24h-20.12q-.803 0-1.365-.562t-.562-1.365v-20.147q0-.776.562-1.351t1.365-.575h20.147q.776 0 1.351.575t.575 1.351v20.147q0 .803-.575 1.365t-1.378.562v0zM17.873 12.977h-4.923v4.896q0 .401-.281.682t-.682.281v0q-.375 0-.669-.281t-.294-.682v-4.896h-4.923q-.401 0-.682-.294t-.281-.669v0q0-.401.281-.682t.682-.281h4.923v-4.896q0-.401.294-.682t.669-.281v0q.401 0 .682.281t.281.682v4.896h4.923q.401 0 .682.281t.281.682v0q0 .375-.281.669t-.682.294z" />
        </SvgIcon>
    );
}

function CloseSquare(props) {
    return (
        <SvgIcon className="close" fontSize="inherit" style={{ width: 14, height: 14 }} {...props}>
            <path d="M17.485 17.512q-.281.281-.682.281t-.696-.268l-4.12-4.147-4.12 4.147q-.294.268-.696.268t-.682-.281-.281-.682.294-.669l4.12-4.147-4.12-4.147q-.294-.268-.294-.669t.281-.682.682-.281.696 .268l4.12 4.147 4.12-4.147q.294-.268.696-.268t.682.281 .281.669-.294.682l-4.12 4.147 4.12 4.147q.294.268 .294.669t-.281.682zM22.047 22.074v0 0-20.147 0h-20.12v0 20.147 0h20.12zM22.047 24h-20.12q-.803 0-1.365-.562t-.562-1.365v-20.147q0-.776.562-1.351t1.365-.575h20.147q.776 0 1.351.575t.575 1.351v20.147q0 .803-.575 1.365t-1.378.562v0z" />
        </SvgIcon>
    );
}

const StyledTreeItem = styled(props => <TreeItem {...props} />, {
    shouldForwardProp: prop => prop !== 'changeState'
})(({ changeState, theme }) => ({
    [`& .${treeItemClasses.iconContainer}`]: {
        '& .close': {
            opacity: 0.3
        }
    },
    [`& .${treeItemClasses.group}`]: {
        marginLeft: 15,
        paddingLeft: 18,
        borderLeft: `1px dashed ${alpha(theme.palette.text.primary, 0.4)}`
    },
    [`& .${treeItemClasses.content}`]: {
        backgroundColor: (() => {
            if (changeState === 'PROPOS') {
                return '#FFD580';
            }
            if (changeState === 'ACCEPT') {
                return '#b0f7b9';
            }
            if (changeState === 'LIVE') {
                return '#FFFFFF';
            }
            return '';
        })()
    },
    [`& .${treeItemClasses.selected}`]: {
        backgroundColor: '#e0f5f7 !important'
    }
}));

export default function BomTree({
    onNodeSelect,
    renderComponents,
    renderDescriptions,
    renderQties,
    bomName,
    bomTree,
    bomTreeLoading,
    expanded,
    setExpanded,
    selected
}) {
    const renderTree = nodes => {
        const label = (
            <>
                <Typography
                    display="inline"
                    variant="subtitle1"
                    id={nodes.id}
                    color={!nodes.children?.length ? '' : 'primary'}
                >
                    {nodes.name}
                </Typography>
                {nodes.name !== bomName.toUpperCase() && (
                    <>
                        {renderQties && (
                            <Typography display="inline" variant="subtitle2">
                                {' '}
                                (x{nodes.qty})
                            </Typography>
                        )}
                        {renderDescriptions && (
                            <Typography display="inline" variant="caption">
                                {' - '}
                                {nodes.description}
                            </Typography>
                        )}
                    </>
                )}
            </>
        );

        return (
            <StyledTreeItem
                key={nodes.id || 'root'}
                nodeId={nodes.id || 'root'}
                label={label}
                changeState={nodes.changeState}
            >
                {Array.isArray(nodes.children) &&
                (renderComponents || nodes.children.some(x => x.type !== 'C'))
                    ? nodes.children.map(node => {
                          if (!renderComponents) {
                              if (node.replacementFor) {
                                  return <></>;
                              }

                              return node.type === 'C' ? <></> : renderTree(node);
                          }
                          return renderTree(node);
                      })
                    : null}
            </StyledTreeItem>
        );
    };
    const handleToggle = (event, nodeIds) => {
        if (event.target.closest('.MuiTreeItem-iconContainer')) {
            setExpanded(nodeIds);
        }
    };
    return (
        <Grid container spacing={3}>
            <Grid item xs={10} />
            {bomTree && (
                <>
                    <Grid item xs={12}>
                        <TreeView
                            aria-label="customized"
                            defaultCollapseIcon={<MinusSquare />}
                            defaultExpandIcon={<PlusSquare />}
                            multiSelect={false}
                            expanded={expanded}
                            selected={selected}
                            onNodeToggle={handleToggle}
                            defaultEndIcon={<CloseSquare />}
                            onNodeSelect={(event, id) => {
                                if (!event.target.closest('.MuiTreeItem-iconContainer')) {
                                    onNodeSelect?.(id);
                                }
                            }}
                        >
                            {renderTree(bomTree)}
                        </TreeView>
                    </Grid>
                </>
            )}
            {bomTreeLoading && (
                <>
                    <Grid item xs={12}>
                        <Typography variant="subtitle2">Filling out the tree..</Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <LinearProgress />
                    </Grid>
                </>
            )}
        </Grid>
    );
}

BomTree.propTypes = {
    renderDescriptions: PropTypes.bool,
    onNodeSelect: PropTypes.func,
    renderComponents: PropTypes.bool,
    renderQties: PropTypes.bool,
    bomTree: PropTypes.shape({ children: PropTypes.arrayOf(PropTypes.shape({})) }),
    bomTreeLoading: PropTypes.bool,
    bomName: PropTypes.string.isRequired,
    expanded: PropTypes.arrayOf(PropTypes.shape({})),
    setExpanded: PropTypes.func.isRequired,
    selected: PropTypes.string
};
BomTree.defaultProps = {
    renderDescriptions: true,
    onNodeSelect: null,
    renderComponents: true,
    renderQties: true,
    bomTree: null,
    bomTreeLoading: false,
    expanded: [],
    selected: null
};
