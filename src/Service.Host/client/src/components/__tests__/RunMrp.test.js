/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../test-utils';
import RunMrp from '../RunMrp';
import mrMasterActions from '../../actions/mrMasterActions';
import mrpRunLogActions from '../../actions/mrpRunLogActions';
import runMrpActions from '../../actions/runMrpActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchMrMasterSpy = jest.spyOn(mrMasterActions, 'fetchByHref');
const fetchRunLogByHrefSpy = jest.spyOn(mrpRunLogActions, 'fetchByHref');
const processStartSpy = jest.spyOn(runMrpActions, 'requestProcessStart');

const initialState = {
    mrMaster: {},
    runMrp: {},
    mrpRunLog: {}
};

const stateWithMasterRunNotAllowed = {
    mrMaster: {
        item: {
            jobRef: 'AAJADE',
            runDate: '2022-05-04T09:48:49.0000000',
            runLogIdCurrentlyInProgress: null,
            links: [
                { href: '/purchasing/material-requirements/last-run', rel: 'self' },
                {
                    href: '/purchasing/material-requirements/run-logs?jobRef=AAJADE',
                    rel: 'last-run-log'
                }
            ]
        },
        loading: false
    },
    runMrp: {},
    mrpRunLog: {}
};

const stateWithMasterNotRunning = {
    mrMaster: {
        item: {
            jobRef: 'AAJADE',
            runDate: '2022-05-04T09:48:49.0000000',
            runLogIdCurrentlyInProgress: null,
            links: [
                { href: '/purchasing/material-requirements/last-run', rel: 'self' },
                {
                    href: '/purchasing/material-requirements/run-logs?jobRef=AAJADE',
                    rel: 'last-run-log'
                },
                { href: '/purchasing/material-requirements/run-mrp', rel: 'run-mrp' }
            ]
        },
        loading: false
    },
    runMrp: {},
    mrpRunLog: {
        item: {
            mrRunLogId: 11662,
            jobRef: 'AAJADE',
            buildPlanName: 'MASTER',
            runDate: '2022-05-04T09:36:39',
            runDetails:
                'rdf_name:MASTER\nrdf source:Remote MRP Run\ninput build plan:MASTER\noutput build plan:MASTER\ndo lrp:Y\ntest only:N\n',
            fullRun: 'Y',
            kill: null,
            success: 'Y',
            loadMessage:
                ' 09:36:39:Starting \n 09:37:22:Loaded BOMS.\n 09:37:22:Starting to load sales forecast \nRead:33167, Written:33167 09:37:36:Loaded sales forecast.\n 09:37:36:Making build plan \n 09:48:18: Build plan made successfully. Jobref:AAJADE\n 09:48:18:Starting LRP ..\n 09:48:49:ST_LRP_PACK.LRP  Run successful\n 09:48:49:* RUN FINISHED SUCCESSFULLY *\n',
            mrMessage:
                'Updating mr for part WOOD 002Updating mr for part WOOD 003Updating mr for part WOOD 003/2Updating mr for part WOOD 004Updating mr for part WOOD 005Updating mr for part WOOD 006Updating mr for part WOOD 007Updating mr for part WOOD 008Updating mr for part WOOD 008/AUpdating mr for part WOOD 008/PUpdating mr for part WOOD 009Updating mr for part WOOD 010Updating mr for part WOOD 010/LUpdating mr for part WOOD 010/RUpdating mr for part WOOD 010/SUpdating mr for part WOOD 011Updating mr for part WOOD 011/BUpdating mr for part WOOD 011/SUpdating mr for part WOOD 012Updating mr for part WOOD 012/BUpdating mr for part WOOD 012/SUpdating mr for part WOOD 013Updating mr for part WOOD 013/SUpdating mr for part WOOD 014Updating mr for part WOOD 015Updating mr for part WORKTOPUpdating mr for part WPLATEUpdating mr for part WPLATE/PUpdating mr for part WPLINTHUpdating mr for part WPLINTH/2Updating mr for part WPLINTH/PUpdating mr for part WPLINTHBUpdating mr for part XELOS VR PLATEUpdating mr for part XOVER 001Updating mr for part YPLINTHUpdating mr for part lowercase partUpdating mr for part new98Updating mr for part newishset recommended purch order Finished successfully.',
            dateTidied: null,
            links: [{ href: '/purchasing/material-requirements/run-logs/11662', rel: 'self' }]
        },
        loading: false
    }
};
const stateWithMrpRunResult = {
    ...stateWithMasterNotRunning,
    runMrp: {
        data: {
            success: true,
            message: 'MRP requested with run log id 11663',
            links: [{ rel: 'status', href: '/purchasing/material-requirements/run-logs/11663' }]
        }
    }
};

const stateWithMrpRunning = {
    mrMaster: {
        item: {
            jobRef: 'AAJADE',
            runDate: '2022-05-04T09:48:49.0000000',
            runLogIdCurrentlyInProgress: 11663,
            links: [
                { href: '/purchasing/material-requirements/last-run', rel: 'self' },
                {
                    href: '/purchasing/material-requirements/run-logs?jobRef=AAJADE',
                    rel: 'last-run-log'
                }
            ]
        }
    },
    runMrp: {},
    mrpRunLog: {
        item: {
            mrRunLogId: 11663,
            jobRef: null,
            buildPlanName: 'MASTER',
            runDate: '2022-05-04T10:58:52',
            runDetails:
                'rdf_name:MASTER\nrdf source:Remote MRP Run\ninput build plan:MASTER\noutput build plan:MASTER\ndo lrp:Y\ntest only:N\n',
            fullRun: 'Y',
            kill: null,
            success: 'N',
            loadMessage:
                ' 10:58:52:Starting \n 10:59:20:Loaded BOMS.\n 10:59:20:Starting to load sales forecast \nRead:33167, Written:33167 10:59:43:Loaded sales forecast.\n 10:59:43:Making build plan \n',
            mrMessage: null,
            dateTidied: null,
            links: [{ href: '/purchasing/material-requirements/run-logs/11663', rel: 'self' }]
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        render(<RunMrp />);
    });

    test('Should fetch master details', () => {
        expect(fetchMrMasterSpy).toBeCalledTimes(1);
    });

    test('Should show run button', () => {
        expect(screen.getByText('Run New MRP')).toBeInTheDocument();
    });

    test('Run button should be disabled', () => {
        expect(screen.getByRole('button', { name: 'Run New MRP' })).toBeDisabled();
    });
});

describe('When last run received and MRP run not allowed...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithMasterRunNotAllowed));
        render(<RunMrp />);
    });

    test('Should show last run details', () => {
        expect(screen.getByText('Last MRP Run')).toBeInTheDocument();
        expect(screen.getByText('JobRef:')).toBeInTheDocument();
        expect(screen.getByText('AAJADE')).toBeInTheDocument();
    });

    test('Run button should be disabled', () => {
        expect(screen.getByText('Run New MRP')).toBeInTheDocument();
        expect(screen.getByText('Run New MRP')).toBeDisabled();
    });
});

describe('When last run received and MRP not running...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithMasterNotRunning));
        render(<RunMrp />);
    });

    test('Should fetch last run log', () => {
        expect(fetchRunLogByHrefSpy).toBeCalledTimes(1);
    });

    test('Should show last run details', () => {
        expect(screen.getByText('Last MRP Run')).toBeInTheDocument();
        expect(screen.getAllByText('JobRef:')).toHaveLength(2);
        expect(screen.getAllByText('AAJADE')).toHaveLength(2);
        expect(screen.getByText('04 May 2022 09:48')).toBeInTheDocument();
    });

    test('Run button should be enabled', () => {
        expect(screen.getByText('Run New MRP')).toBeInTheDocument();
        expect(screen.getByText('Run New MRP')).toBeEnabled();
    });

    test('Should show last run log', () => {
        expect(screen.getByText('Latest MRP Run Log')).toBeInTheDocument();
        expect(screen.getByText('Run Log Id:')).toBeInTheDocument();
        expect(screen.getByText('11662')).toBeInTheDocument();
        expect(screen.getByText('04 May 2022 09:36')).toBeInTheDocument();
        expect(screen.getByText('Success:')).toBeInTheDocument();
        expect(screen.getByText('Y')).toBeInTheDocument();
    });
});

describe('When click run mrp...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithMasterNotRunning));
        render(<RunMrp />);
        const runButton = screen.getByText('Run New MRP');
        fireEvent.click(runButton);
    });

    test('Should run process', () => {
        expect(processStartSpy).toBeCalledTimes(1);
    });
});

describe('When process result is returned successfully...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithMrpRunResult));
        render(<RunMrp />);
    });

    test('Should run get new master and run log details', () => {
        expect(fetchRunLogByHrefSpy).toBeCalledWith(
            '/purchasing/material-requirements/run-logs/11663'
        );
        expect(fetchMrMasterSpy).toBeCalled();
    });
});

describe('When MRP is running...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithMrpRunning));
        render(<RunMrp />);
    });

    test('Should show last run details', () => {
        expect(screen.getByText('Last MRP Run')).toBeInTheDocument();
        expect(screen.getAllByText('JobRef:')).toHaveLength(2);
        expect(screen.getByText('AAJADE')).toBeInTheDocument();
        expect(screen.getByText('04 May 2022 09:48')).toBeInTheDocument();
    });

    test('Run button should be disabled', () => {
        expect(screen.getByText('Run New MRP')).toBeInTheDocument();
        expect(screen.getByText('Run New MRP')).toBeDisabled();
    });

    test('Should show current run log', () => {
        expect(screen.getByText('Current MRP Run Log')).toBeInTheDocument();
        expect(screen.getByText('Run Log Id:')).toBeInTheDocument();
        expect(screen.getByText('11663')).toBeInTheDocument();
        expect(screen.getByText('04 May 2022 10:58')).toBeInTheDocument();
    });

    test('Should show progress bar', () => {
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });
});
