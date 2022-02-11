import React from 'react';
import makeStyles from '@mui/styles/makeStyles';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Button from '@mui/material/Button';
import Link from '@mui/material/Link';
import { DataGrid } from '@mui/x-data-grid';
import {
    ErrorCard,
    Title,
    BackButton,
    ExportButton,
    reportSelectorHelpers,
    formatTitle,
    setDrilldown,
    setTextValueDrilldown,
    format,
    LinkOrAnchor,
    reportResultType 
} from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
// import { reportResultType } from '../propTypes/index';

const useStyles = makeStyles(() => ({
    subTotal: {
        fontWeight: 'bolder'
    },
    root: {
        width: '100%',
        overflow: 'auto',
        textAlign: 'center',
        pageBreakInside: 'avoid'
    },
    rootAllowsPageBreaks: {
        width: '100%',
        overflow: 'auto',
        textAlign: 'center'
    },
    numberField: {
        textAlign: 'right'
    },
    noWrap: {
        whiteSpace: 'nowrap'
    },
    smallCol: {
        width: '100px',
        overflow: 'auto'
    },
    mediumCol: {
        width: '200px',
        overflow: 'auto'
    },
    largeCol: {
        width: '300px',
        overflow: 'auto'
    }
}));

const setValueDrilldown = (value, hasExternalLinks) => {
    let displayItem;
    if (value && (value.displayValue || value.displayValue === 0)) {
        if (value.drillDowns && value.drillDowns.length > 0) {
            const text = format(
                value.displayValue,
                value.prefix,
                value.suffix,
                value.decimalPlaces ? value.decimalPlaces : 2
            );
            const { href } = value.drillDowns[0];
            displayItem = LinkOrAnchor({ hasExternalLinks, href, text });
        } else {
            displayItem = format(
                value.displayValue,
                value.prefix,
                value.suffix,
                value.decimalPlaces
            );
        }
    } else {
        displayItem = null;
    }

    return displayItem;
};

const setCellClasses = (
    classes,
    displayValue,
    textDisplayValue,
    rowType,
    varianceColumn,
    textColumn,
    totalColumn,
    allowWrap,
    defaultClasses
) => {
    let generatedClasses = '';
    if (rowType === 'Subtotal' || totalColumn) {
        generatedClasses += `${classes.subTotal} `;
    }

    if (!textColumn && !textDisplayValue) {
        generatedClasses += `${classes.numberField} `;
    }

    if (!allowWrap) {
        generatedClasses += `${classes.noWrap} `;
    }

    if (defaultClasses) {
        generatedClasses += `${defaultClasses} `;
    }

    return generatedClasses;
};

const setHeaderCellClasses = (
    classes,
    varianceColumn,
    textColumn,
    totalColumn,
    columnClass,
    defaultClasses
) => {
    let generatedClasses = '';
    if (!textColumn) {
        generatedClasses += `${classes.numberField} `;
    }

    if (columnClass === 'small') {
        generatedClasses += `${classes.smallCol} `;
    }

    if (columnClass === 'medium') {
        generatedClasses += `${classes.mediumCol} `;
    }

    if (columnClass === 'large') {
        generatedClasses += `${classes.largeCol} `;
    }

    if (defaultClasses) {
        generatedClasses += `${defaultClasses} `;
    }

    return generatedClasses;
};

const Results = ({
    reportData,
    classes,
    title,
    showTitle,
    showTotals,
    hasExternalLinks,
    showRowTitles,
    columnClasses,
    allowPageBreakInside
}) => (
    <Paper className={allowPageBreakInside ? classes.rootAllowsPageBreaks : classes.root}>
        <Title
            text={formatTitle(
                title,
                showTitle,
                !reportData,
                reportData && reportData.error,
                reportData ? reportData.reportHelpText : ''
            )}
        />
        <div style={{ backgroundColor: 'white' }}>
            <Table size="small">
                <TableHead key="headers">
                    <TableRow>
                        {showRowTitles ? (
                            <TableCell> {reportData.headers.rowHeader} </TableCell>
                        ) : null}
                        {reportData.headers.columnHeaders.map((header, i) => (
                            <TableCell
                                className={setHeaderCellClasses(
                                    classes,
                                    reportData.headers.varianceColumns.includes(i),
                                    reportData.headers.textColumns.includes(i),
                                    reportData.headers.totalColumns.includes(i),
                                    columnClasses ? columnClasses[i] : null
                                )}
                                key={header}
                            >
                                {header}
                            </TableCell>
                        ))}
                    </TableRow>
                </TableHead>
                <TableBody>
                    {reportData.results.map((item, j) => (
                        // eslint-disable-next-line react/no-array-index-key
                        <TableRow key={j}>
                            {showRowTitles ? (
                                <TableCell className={classes.noWrap}>
                                    {setDrilldown(item.rowTitle, hasExternalLinks)}
                                </TableCell>
                            ) : null}
                            {item.values.map((value, i) => (
                                <TableCell
                                    className={setCellClasses(
                                        classes,
                                        value ? value.displayValue : null,
                                        value ? value.textDisplayValue : null,
                                        item.rowType,
                                        reportData.headers.varianceColumns.includes(i),
                                        reportData.headers.textColumns.includes(i),
                                        reportData.headers.totalColumns.includes(i),
                                        value ? value.allowWrap : true
                                    )}
                                    // remove this if we implement reordering of columns
                                    // eslint-disable-next-line react/no-array-index-key
                                    key={i}
                                >
                                    {setValueDrilldown(value, hasExternalLinks)}
                                    {setTextValueDrilldown(value, hasExternalLinks)}
                                </TableCell>
                            ))}
                        </TableRow>
                    ))}

                    {showTotals ? (
                        <TableRow key="totals">
                            {showRowTitles ? (
                                <TableCell>{reportData.totals.rowTitle.displayString}</TableCell>
                            ) : null}
                            {reportData.totals.values.map((value, i) => (
                                <TableCell
                                    className={setCellClasses(
                                        classes,
                                        value ? value.displayValue : null,
                                        value ? value.textDisplayValue : null,
                                        'Total',
                                        reportData.headers.varianceColumns.includes(i),
                                        reportData.headers.textColumns.includes(i),
                                        reportData.headers.totalColumns.includes(i),
                                        value ? value.allowWrap : true
                                    )}
                                    // remove this if we implement reordering of columns
                                    // eslint-disable-next-line react/no-array-index-key
                                    key={i}
                                >
                                    {setValueDrilldown(value, hasExternalLinks)}
                                </TableCell>
                            ))}
                        </TableRow>
                    ) : (
                        false
                    )}
                </TableBody>
            </Table>
        </div>
    </Paper>
);

function ReportTable({
    placeholderRows,
    placeholderColumns,
    reportData,
    hasExternalLinks,
    title,
    showTitle,
    showTotals,
    showRowTitles,
    columnClasses,
    allowPageBreakInside
}) {
    const classes = useStyles();
    if (!reportData) {
        return (
            <Paper className={allowPageBreakInside ? classes.rootAllowsPageBreaks : classes.root}>
                <Table>
                    <TableBody>
                        {[...Array(placeholderRows).keys()].map(row => (
                            <TableRow key={row}>
                                {[...Array(placeholderColumns).keys()].map(column => (
                                    <TableCell key={column} />
                                ))}
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </Paper>
        );
    }
    if (reportData.message) {
        return <ErrorCard errorMessage={reportData.message} />;
    }
    return Results({
        reportData,
        hasExternalLinks,
        classes,
        title,
        showRowTitles,
        showTitle,
        showTotals,
        columnClasses,
        allowPageBreakInside
    });
}

Results.propTypes = {
    hasExternalLinks: PropTypes.bool,
    reportData: reportResultType,
    classes: PropTypes.shape({
        root: PropTypes.shape({}),
        noWrap: PropTypes.shape({}),
        rootAllowsPageBreaks: PropTypes.shape({})
    }).isRequired,
    title: PropTypes.oneOfType([PropTypes.string, PropTypes.shape({})]),
    showTitle: PropTypes.bool,
    showTotals: PropTypes.bool,
    showRowTitles: PropTypes.bool,
    columnClasses: PropTypes.arrayOf(PropTypes.string),
    allowPageBreakInside: PropTypes.bool.isRequired
};

Results.defaultProps = {
    reportData: null,
    title: null,
    showTitle: true,
    showTotals: true,
    showRowTitles: true,
    hasExternalLinks: false,
    columnClasses: null
};

ReportTable.propTypes = {
    hasExternalLinks: PropTypes.bool,
    placeholderRows: PropTypes.number.isRequired,
    placeholderColumns: PropTypes.number.isRequired,
    reportData: PropTypes.shape({}),
    columnClasses: PropTypes.arrayOf(PropTypes.string),
    allowPageBreakInside: PropTypes.bool
};

ReportTable.defaultProps = {
    reportData: null,
    placeholderRows: 5,
    placeholderColumns: 6,
    hasExternalLinks: false,
    columnClasses: null,
    allowPageBreakInside: false
};

export default ReportTable;
