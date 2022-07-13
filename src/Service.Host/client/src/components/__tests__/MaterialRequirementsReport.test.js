/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../test-utils';
import MaterialRequirementsReport from '../materialRequirements/MaterialRequirementsReport';
import mrReportActions from '../../actions/mrReportActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

jest.mock('react-router', () => ({
    ...jest.requireActual('react-router'),
    useLocation: jest.fn()
}));

const fetchReportSpy = jest.spyOn(mrReportActions, 'postByHref');

const options = {
    state: {
        jobRef: 'ABCD',
        partNumbers: []
    }
};

const initialState = {};
const stateWithReport = {
    mrReport: {
        loading: false,
        item: {
            results: [
                {
                    jobRef: 'AAJAEF',
                    partNumber: 'CAP 401',
                    partDescription: '2200UF,+20%,-20%,25V,ELTR,TH, PCO300,,,,,,,,,,,',
                    quantityInStock: 218,
                    quantityForSpares: 0,
                    quantityInInspection: 6,
                    quantityFaulty: 6,
                    quantityAtSupplier: 0,
                    preferredSupplierId: 41193,
                    preferredSupplierName: 'ABACUS POLAR SCOTLAND LTD',
                    annualUsage: 49,
                    baseUnitPrice: 0.0994,
                    ourUnits: 'ONES',
                    orderUnits: 'ONES',
                    leadTimeWeeks: 1,
                    currencyCode: 'GBP',
                    currencyUnitPrice: 0.0994,
                    minimumOrderQuantity: 100,
                    minimumDeliveryQuantity: 100,
                    orderIncrement: 100,
                    vendorManager: 'O',
                    vendorManagerInitials: 'HMG',
                    planner: null,
                    details: [
                        {
                            title: 'Week',
                            segment: 0,
                            displaySequence: 0,
                            immediate: 'IMM',
                            week0: '19/22',
                            week1: '20/22',
                            week2: '21/22',
                            week3: '22/22',
                            week4: '23/22',
                            week5: '24/22',
                            week6: '25/22',
                            week7: '26/22',
                            week8: '27/22',
                            week9: '28/22',
                            week10: '29/22',
                            week11: '30/22',
                            week12: '31/22',
                            immediateItem: {
                                textValue: 'IMM',
                                value: null,
                                tag: null
                            },
                            week0Item: {
                                textValue: '19/22',
                                value: null,
                                tag: null
                            },
                            week1Item: {
                                textValue: '20/22',
                                value: null,
                                tag: null
                            },
                            week2Item: {
                                textValue: '21/22',
                                value: null,
                                tag: null
                            },
                            week3Item: {
                                textValue: '22/22',
                                value: null,
                                tag: null
                            },
                            week4Item: {
                                textValue: '23/22',
                                value: null,
                                tag: null
                            },
                            week5Item: {
                                textValue: '24/22',
                                value: null,
                                tag: null
                            },
                            week6Item: {
                                textValue: '25/22',
                                value: null,
                                tag: null
                            },
                            week7Item: {
                                textValue: '26/22',
                                value: null,
                                tag: null
                            },
                            week8Item: {
                                textValue: '27/22',
                                value: null,
                                tag: null
                            },
                            week9Item: {
                                textValue: '28/22',
                                value: null,
                                tag: null
                            },
                            week10Item: {
                                textValue: '29/22',
                                value: null,
                                tag: null
                            },
                            week11Item: {
                                textValue: '30/22',
                                value: null,
                                tag: null
                            },
                            week12Item: {
                                textValue: '31/22',
                                value: null,
                                tag: null
                            }
                        },
                        {
                            title: 'Ending',
                            segment: 0,
                            displaySequence: 10,
                            immediate: 'IMM',
                            week0: '13May',
                            week1: '20May',
                            week2: '27May',
                            week3: '03Jun',
                            week4: '10Jun',
                            week5: '17Jun',
                            week6: '24Jun',
                            week7: '01Jul',
                            week8: '08Jul',
                            week9: '15Jul',
                            week10: '22Jul',
                            week11: '29Jul',
                            week12: '05Aug',
                            immediateItem: {
                                textValue: 'IMM',
                                value: null,
                                tag: 'TagValue'
                            },
                            week0Item: {
                                textValue: '13May',
                                value: null,
                                tag: null
                            },
                            week1Item: {
                                textValue: '20May',
                                value: null,
                                tag: null
                            },
                            week2Item: {
                                textValue: '27May',
                                value: null,
                                tag: null
                            },
                            week3Item: {
                                textValue: '03Jun',
                                value: null,
                                tag: null
                            },
                            week4Item: {
                                textValue: '10Jun',
                                value: null,
                                tag: null
                            },
                            week5Item: {
                                textValue: '17Jun',
                                value: null,
                                tag: null
                            },
                            week6Item: {
                                textValue: '24Jun',
                                value: null,
                                tag: null
                            },
                            week7Item: {
                                textValue: '01Jul',
                                value: null,
                                tag: null
                            },
                            week8Item: {
                                textValue: '08Jul',
                                value: null,
                                tag: null
                            },
                            week9Item: {
                                textValue: '15Jul',
                                value: null,
                                tag: null
                            },
                            week10Item: {
                                textValue: '22Jul',
                                value: null,
                                tag: null
                            },
                            week11Item: {
                                textValue: '29Jul',
                                value: null,
                                tag: null
                            },
                            week12Item: {
                                textValue: '05Aug',
                                value: null,
                                tag: null
                            }
                        },
                        {
                            title: 'Purchases',
                            segment: 0,
                            displaySequence: 150,
                            immediate: '2433',
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: {
                                textValue: null,
                                value: 2433,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            }
                        },
                        {
                            title: 'Unauthorised POs',
                            segment: 0,
                            displaySequence: 160,
                            immediate: '4503',
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: {
                                textValue: null,
                                value: 4503,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            }
                        },
                        {
                            title: 'Sales Orders',
                            segment: 0,
                            displaySequence: 300,
                            immediate: '1',
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: {
                                textValue: null,
                                value: 1,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            }
                        },
                        {
                            title: 'Production Reqt',
                            segment: 0,
                            displaySequence: 400,
                            immediate: '36',
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '12',
                            week11: '0',
                            week12: '0',
                            immediateItem: {
                                textValue: null,
                                value: 36,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: 12,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            }
                        },
                        {
                            title: 'Status',
                            segment: 0,
                            displaySequence: 990,
                            immediate: '',
                            week0: 'H',
                            week1: 'H',
                            week2: 'H',
                            week3: 'H',
                            week4: 'H',
                            week5: 'H',
                            week6: 'H',
                            week7: 'H',
                            week8: 'H',
                            week9: 'H',
                            week10: 'H',
                            week11: 'H',
                            week12: 'H',
                            immediateItem: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week0Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week1Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week2Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week3Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week4Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week5Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week6Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week7Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week8Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week9Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week10Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week11Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            },
                            week12Item: {
                                textValue: 'H',
                                value: null,
                                tag: null
                            }
                        },
                        {
                            title: 'Stock',
                            segment: 0,
                            displaySequence: 1000,
                            immediate: '181',
                            week0: '7117',
                            week1: '7117',
                            week2: '7117',
                            week3: '7117',
                            week4: '7117',
                            week5: '7117',
                            week6: '7117',
                            week7: '7117',
                            week8: '7117',
                            week9: '7117',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: {
                                textValue: null,
                                value: 181,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: 7105,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: 7105,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: 7105,
                                tag: null
                            }
                        },
                        {
                            title: 'Min Rail',
                            segment: 0,
                            displaySequence: 1100,
                            immediate: '',
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: 0,
                                tag: null
                            }
                        },
                        {
                            title: 'Max Rail',
                            segment: 0,
                            displaySequence: 1200,
                            immediate: '',
                            week0: '200',
                            week1: '200',
                            week2: '200',
                            week3: '200',
                            week4: '200',
                            week5: '200',
                            week6: '200',
                            week7: '200',
                            week8: '200',
                            week9: '200',
                            week10: '200',
                            week11: '200',
                            week12: '200',
                            immediateItem: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: 200,
                                tag: null
                            }
                        },
                        {
                            title: 'Ideal Stock',
                            segment: 0,
                            displaySequence: 1300,
                            immediate: '',
                            week0: '7117',
                            week1: '7117',
                            week2: '7117',
                            week3: '7117',
                            week4: '7117',
                            week5: '7117',
                            week6: '7117',
                            week7: '7117',
                            week8: '7117',
                            week9: '7117',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: 7105,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: 7105,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: 7105,
                                tag: null
                            }
                        },
                        {
                            title: 'Recom Orders',
                            segment: 0,
                            displaySequence: 1400,
                            immediate: '',
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: null,
                                tag: null
                            }
                        },
                        {
                            title: 'Recom Stock',
                            segment: 0,
                            displaySequence: 1500,
                            immediate: '',
                            week0: '7117',
                            week1: '7117',
                            week2: '7117',
                            week3: '7117',
                            week4: '7117',
                            week5: '7117',
                            week6: '7117',
                            week7: '7117',
                            week8: '7117',
                            week9: '7117',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: {
                                textValue: null,
                                value: null,
                                tag: null
                            },
                            week0Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week1Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week2Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week3Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week4Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week5Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week6Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week7Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week8Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week9Item: {
                                textValue: null,
                                value: 7117,
                                tag: null
                            },
                            week10Item: {
                                textValue: null,
                                value: 7105,
                                tag: null
                            },
                            week11Item: {
                                textValue: null,
                                value: 7105,
                                tag: null
                            },
                            week12Item: {
                                textValue: null,
                                value: 7105,
                                tag: null
                            }
                        },
                        {
                            title: 'Week',
                            segment: 1,
                            displaySequence: 0,
                            immediate: null,
                            week0: '32/22',
                            week1: '33/22',
                            week2: '34/22',
                            week3: '35/22',
                            week4: '36/22',
                            week5: '37/22',
                            week6: '38/22',
                            week7: '39/22',
                            week8: '40/22',
                            week9: '41/22',
                            week10: '42/22',
                            week11: '43/22',
                            week12: '44/22',
                            immediateItem: null
                        },
                        {
                            title: 'Ending',
                            segment: 1,
                            displaySequence: 10,
                            immediate: null,
                            week0: '12Aug',
                            week1: '19Aug',
                            week2: '26Aug',
                            week3: '02Sep',
                            week4: '09Sep',
                            week5: '16Sep',
                            week6: '23Sep',
                            week7: '30Sep',
                            week8: '07Oct',
                            week9: '14Oct',
                            week10: '21Oct',
                            week11: '28Oct',
                            week12: '04Nov',
                            immediateItem: null
                        },
                        {
                            title: 'Purchases',
                            segment: 1,
                            displaySequence: 150,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Unauthorised POs',
                            segment: 1,
                            displaySequence: 160,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Sales Orders',
                            segment: 1,
                            displaySequence: 300,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Production Reqt',
                            segment: 1,
                            displaySequence: 400,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Status',
                            segment: 1,
                            displaySequence: 990,
                            immediate: null,
                            week0: 'H',
                            week1: 'H',
                            week2: 'H',
                            week3: 'H',
                            week4: 'H',
                            week5: 'H',
                            week6: 'H',
                            week7: 'H',
                            week8: 'H',
                            week9: 'H',
                            week10: 'H',
                            week11: 'H',
                            week12: 'H',
                            immediateItem: null
                        },
                        {
                            title: 'Stock',
                            segment: 1,
                            displaySequence: 1000,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Min Rail',
                            segment: 1,
                            displaySequence: 1100,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Max Rail',
                            segment: 1,
                            displaySequence: 1200,
                            immediate: null,
                            week0: '200',
                            week1: '200',
                            week2: '200',
                            week3: '200',
                            week4: '200',
                            week5: '200',
                            week6: '200',
                            week7: '200',
                            week8: '200',
                            week9: '200',
                            week10: '200',
                            week11: '200',
                            week12: '200',
                            immediateItem: null
                        },
                        {
                            title: 'Ideal Stock',
                            segment: 1,
                            displaySequence: 1300,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Orders',
                            segment: 1,
                            displaySequence: 1400,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Stock',
                            segment: 1,
                            displaySequence: 1500,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Week',
                            segment: 2,
                            displaySequence: 0,
                            immediate: null,
                            week0: '45/22',
                            week1: '46/22',
                            week2: '47/22',
                            week3: '48/22',
                            week4: '49/22',
                            week5: '50/22',
                            week6: '51/22',
                            week7: '52/22',
                            week8: '01/23',
                            week9: '02/23',
                            week10: '03/23',
                            week11: '04/23',
                            week12: '05/23',
                            immediateItem: null
                        },
                        {
                            title: 'Ending',
                            segment: 2,
                            displaySequence: 10,
                            immediate: null,
                            week0: '11Nov',
                            week1: '18Nov',
                            week2: '25Nov',
                            week3: '02Dec',
                            week4: '09Dec',
                            week5: '16Dec',
                            week6: '23Dec',
                            week7: '30Dec',
                            week8: '06Jan',
                            week9: '13Jan',
                            week10: '20Jan',
                            week11: '27Jan',
                            week12: '03Feb',
                            immediateItem: null
                        },
                        {
                            title: 'Purchases',
                            segment: 2,
                            displaySequence: 150,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Unauthorised POs',
                            segment: 2,
                            displaySequence: 160,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Sales Orders',
                            segment: 2,
                            displaySequence: 300,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Production Reqt',
                            segment: 2,
                            displaySequence: 400,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Status',
                            segment: 2,
                            displaySequence: 990,
                            immediate: null,
                            week0: 'H',
                            week1: 'H',
                            week2: 'H',
                            week3: 'H',
                            week4: 'H',
                            week5: 'H',
                            week6: 'H',
                            week7: 'H',
                            week8: 'H',
                            week9: 'H',
                            week10: 'H',
                            week11: 'H',
                            week12: 'H',
                            immediateItem: null
                        },
                        {
                            title: 'Stock',
                            segment: 2,
                            displaySequence: 1000,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Min Rail',
                            segment: 2,
                            displaySequence: 1100,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Max Rail',
                            segment: 2,
                            displaySequence: 1200,
                            immediate: null,
                            week0: '200',
                            week1: '200',
                            week2: '200',
                            week3: '200',
                            week4: '200',
                            week5: '200',
                            week6: '200',
                            week7: '200',
                            week8: '200',
                            week9: '200',
                            week10: '200',
                            week11: '200',
                            week12: '200',
                            immediateItem: null
                        },
                        {
                            title: 'Ideal Stock',
                            segment: 2,
                            displaySequence: 1300,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Orders',
                            segment: 2,
                            displaySequence: 1400,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Stock',
                            segment: 2,
                            displaySequence: 1500,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Week',
                            segment: 3,
                            displaySequence: 0,
                            immediate: null,
                            week0: '06/23',
                            week1: '07/23',
                            week2: '08/23',
                            week3: '09/23',
                            week4: '10/23',
                            week5: '11/23',
                            week6: '12/23',
                            week7: '13/23',
                            week8: '14/23',
                            week9: '15/23',
                            week10: '16/23',
                            week11: '17/23',
                            week12: '18/23',
                            immediateItem: null
                        },
                        {
                            title: 'Ending',
                            segment: 3,
                            displaySequence: 10,
                            immediate: null,
                            week0: '10Feb',
                            week1: '17Feb',
                            week2: '24Feb',
                            week3: '03Mar',
                            week4: '10Mar',
                            week5: '17Mar',
                            week6: '24Mar',
                            week7: '31Mar',
                            week8: '07Apr',
                            week9: '14Apr',
                            week10: '21Apr',
                            week11: '28Apr',
                            week12: '05May',
                            immediateItem: null
                        },
                        {
                            title: 'Purchases',
                            segment: 3,
                            displaySequence: 150,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Unauthorised POs',
                            segment: 3,
                            displaySequence: 160,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Sales Orders',
                            segment: 3,
                            displaySequence: 300,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Production Reqt',
                            segment: 3,
                            displaySequence: 400,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Status',
                            segment: 3,
                            displaySequence: 990,
                            immediate: null,
                            week0: 'H',
                            week1: 'H',
                            week2: 'H',
                            week3: 'H',
                            week4: 'H',
                            week5: 'H',
                            week6: 'H',
                            week7: 'H',
                            week8: 'H',
                            week9: 'H',
                            week10: 'H',
                            week11: 'H',
                            week12: 'H',
                            immediateItem: null
                        },
                        {
                            title: 'Stock',
                            segment: 3,
                            displaySequence: 1000,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Min Rail',
                            segment: 3,
                            displaySequence: 1100,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Max Rail',
                            segment: 3,
                            displaySequence: 1200,
                            immediate: null,
                            week0: '200',
                            week1: '200',
                            week2: '200',
                            week3: '200',
                            week4: '200',
                            week5: '200',
                            week6: '200',
                            week7: '200',
                            week8: '200',
                            week9: '200',
                            week10: '200',
                            week11: '200',
                            week12: '200',
                            immediateItem: null
                        },
                        {
                            title: 'Ideal Stock',
                            segment: 3,
                            displaySequence: 1300,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Orders',
                            segment: 3,
                            displaySequence: 1400,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Stock',
                            segment: 3,
                            displaySequence: 1500,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Week',
                            segment: 4,
                            displaySequence: 0,
                            immediate: null,
                            week0: '19/23',
                            week1: '20/23',
                            week2: '21/23',
                            week3: '22/23',
                            week4: '23/23',
                            week5: '24/23',
                            week6: '25/23',
                            week7: '26/23',
                            week8: '27/23',
                            week9: '28/23',
                            week10: '29/23',
                            week11: '30/23',
                            week12: '31/23',
                            immediateItem: null
                        },
                        {
                            title: 'Ending',
                            segment: 4,
                            displaySequence: 10,
                            immediate: null,
                            week0: '12May',
                            week1: '19May',
                            week2: '26May',
                            week3: '02Jun',
                            week4: '09Jun',
                            week5: '16Jun',
                            week6: '23Jun',
                            week7: '30Jun',
                            week8: '07Jul',
                            week9: '14Jul',
                            week10: '21Jul',
                            week11: '28Jul',
                            week12: '04Aug',
                            immediateItem: null
                        },
                        {
                            title: 'Purchases',
                            segment: 4,
                            displaySequence: 150,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Unauthorised POs',
                            segment: 4,
                            displaySequence: 160,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Sales Orders',
                            segment: 4,
                            displaySequence: 300,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Production Reqt',
                            segment: 4,
                            displaySequence: 400,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Status',
                            segment: 4,
                            displaySequence: 990,
                            immediate: null,
                            week0: 'H',
                            week1: 'H',
                            week2: 'H',
                            week3: 'H',
                            week4: 'H',
                            week5: 'H',
                            week6: 'H',
                            week7: 'H',
                            week8: 'H',
                            week9: 'H',
                            week10: 'H',
                            week11: 'H',
                            week12: 'H',
                            immediateItem: null
                        },
                        {
                            title: 'Stock',
                            segment: 4,
                            displaySequence: 1000,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Min Rail',
                            segment: 4,
                            displaySequence: 1100,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Max Rail',
                            segment: 4,
                            displaySequence: 1200,
                            immediate: null,
                            week0: '200',
                            week1: '200',
                            week2: '200',
                            week3: '200',
                            week4: '200',
                            week5: '200',
                            week6: '200',
                            week7: '200',
                            week8: '200',
                            week9: '200',
                            week10: '200',
                            week11: '200',
                            week12: '200',
                            immediateItem: null
                        },
                        {
                            title: 'Ideal Stock',
                            segment: 4,
                            displaySequence: 1300,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Orders',
                            segment: 4,
                            displaySequence: 1400,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Stock',
                            segment: 4,
                            displaySequence: 1500,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7105',
                            week6: '7105',
                            week7: '7105',
                            week8: '7105',
                            week9: '7105',
                            week10: '7105',
                            week11: '7105',
                            week12: '7105',
                            immediateItem: null
                        },
                        {
                            title: 'Week',
                            segment: 5,
                            displaySequence: 0,
                            immediate: null,
                            week0: '32/23',
                            week1: '33/23',
                            week2: '34/23',
                            week3: '35/23',
                            week4: '36/23',
                            week5: '37/23',
                            week6: '38/23',
                            week7: '39/23',
                            week8: '40/23',
                            week9: '41/23',
                            week10: '42/23',
                            week11: '43/23',
                            week12: '44/23',
                            immediateItem: null
                        },
                        {
                            title: 'Ending',
                            segment: 5,
                            displaySequence: 10,
                            immediate: null,
                            week0: '11Aug',
                            week1: '18Aug',
                            week2: '25Aug',
                            week3: '01Sep',
                            week4: '08Sep',
                            week5: '15Sep',
                            week6: '22Sep',
                            week7: '29Sep',
                            week8: '06Oct',
                            week9: '13Oct',
                            week10: '20Oct',
                            week11: '27Oct',
                            week12: '03Nov',
                            immediateItem: null
                        },
                        {
                            title: 'Purchases',
                            segment: 5,
                            displaySequence: 150,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Unauthorised POs',
                            segment: 5,
                            displaySequence: 160,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Sales Orders',
                            segment: 5,
                            displaySequence: 300,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Production Reqt',
                            segment: 5,
                            displaySequence: 400,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '12',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Status',
                            segment: 5,
                            displaySequence: 990,
                            immediate: null,
                            week0: 'H',
                            week1: 'H',
                            week2: 'H',
                            week3: 'H',
                            week4: 'H',
                            week5: 'H',
                            week6: 'H',
                            week7: 'H',
                            week8: 'H',
                            week9: 'H',
                            week10: 'H',
                            week11: 'H',
                            week12: 'H',
                            immediateItem: null
                        },
                        {
                            title: 'Stock',
                            segment: 5,
                            displaySequence: 1000,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7093',
                            week6: '7093',
                            week7: '7093',
                            week8: '7093',
                            week9: '7093',
                            week10: '7093',
                            week11: '7093',
                            week12: '7093',
                            immediateItem: null
                        },
                        {
                            title: 'Min Rail',
                            segment: 5,
                            displaySequence: 1100,
                            immediate: null,
                            week0: '0',
                            week1: '0',
                            week2: '0',
                            week3: '0',
                            week4: '0',
                            week5: '0',
                            week6: '0',
                            week7: '0',
                            week8: '0',
                            week9: '0',
                            week10: '0',
                            week11: '0',
                            week12: '0',
                            immediateItem: null
                        },
                        {
                            title: 'Max Rail',
                            segment: 5,
                            displaySequence: 1200,
                            immediate: null,
                            week0: '200',
                            week1: '200',
                            week2: '200',
                            week3: '200',
                            week4: '200',
                            week5: '200',
                            week6: '200',
                            week7: '200',
                            week8: '200',
                            week9: '200',
                            week10: '200',
                            week11: '200',
                            week12: '200',
                            immediateItem: null
                        },
                        {
                            title: 'Ideal Stock',
                            segment: 5,
                            displaySequence: 1300,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7093',
                            week6: '7093',
                            week7: '7093',
                            week8: '7093',
                            week9: '7093',
                            week10: '7093',
                            week11: '7093',
                            week12: '7093',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Orders',
                            segment: 5,
                            displaySequence: 1400,
                            immediate: null,
                            week0: '',
                            week1: '',
                            week2: '',
                            week3: '',
                            week4: '',
                            week5: '',
                            week6: '',
                            week7: '',
                            week8: '',
                            week9: '',
                            week10: '',
                            week11: '',
                            week12: '',
                            immediateItem: null
                        },
                        {
                            title: 'Recom Stock',
                            segment: 5,
                            displaySequence: 1500,
                            immediate: null,
                            week0: '7105',
                            week1: '7105',
                            week2: '7105',
                            week3: '7105',
                            week4: '7105',
                            week5: '7093',
                            week6: '7093',
                            week7: '7093',
                            week8: '7093',
                            week9: '7093',
                            week10: '7093',
                            week11: '7093',
                            week12: '7093',
                            immediateItem: null
                        }
                    ],
                    links: [
                        {
                            href: '/purchasing/material-requirements/used-on-report?partNumber=CAP 401',
                            rel: 'part-used-on'
                        },
                        {
                            href: '/parts/6546',
                            rel: 'part'
                        }
                    ]
                }
            ]
        },
        editStatus: 'view',
        snackbarVisible: true
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        useLocation.mockReturnValue(options);
        render(<MaterialRequirementsReport />);
    });

    test('Should fetch report', () => {
        expect(fetchReportSpy).toBeCalledTimes(1);
    });
});

describe('When report is returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithReport));
        render(<MaterialRequirementsReport />);
    });

    test('Should show part info', () => {
        expect(screen.getByText('CAP 401')).toBeInTheDocument();
        expect(
            screen.getByText('2200UF,+20%,-20%,25V,ELTR,TH, PCO300,,,,,,,,,,,')
        ).toBeInTheDocument();
    });

    test('Should show supplier info', () => {
        expect(screen.getByText('Supplier: 41193 ABACUS POLAR SCOTLAND LTD')).toBeInTheDocument();
    });

    test('Should show stock info', () => {
        expect(screen.getByText('Stock:')).toBeInTheDocument();
        expect(screen.getByText('218')).toBeInTheDocument();
        expect(screen.getByText('For Spares: 0')).toBeInTheDocument();
        expect(screen.getByText('Inspection: 6')).toBeInTheDocument();
        expect(screen.getByText('Faulty: 6')).toBeInTheDocument();
        expect(screen.getByText('Supplier: 0')).toBeInTheDocument();
    });

    test('Should show price info', () => {
        expect(screen.getByText('GBP Price: 0.0994')).toBeInTheDocument();
        expect(screen.getByText('Currency Price: 0.0994')).toBeInTheDocument();
    });

    test('Should show buying info', () => {
        expect(screen.getByText('Lead Time: 1')).toBeInTheDocument();
        expect(screen.getByText('MOQ: 100')).toBeInTheDocument();
        expect(screen.getByText('MDQ: 100')).toBeInTheDocument();
        expect(screen.getByText('Order Increment: 100')).toBeInTheDocument();
        expect(screen.getByText('Our Units: ONES')).toBeInTheDocument();
        expect(screen.getByText('Annual Usage: 49')).toBeInTheDocument();
    });

    test('Should render nav buttons', () => {
        expect(screen.getByRole('button', { name: 'At first' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: 'At first' })).toBeDisabled();
        expect(screen.getByRole('button', { name: 'At last' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: 'Order' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: 'Used On' })).toBeInTheDocument();
        expect(screen.getByText('View Part')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: 'Earlier Weeks' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: 'Earlier Weeks' })).toBeDisabled();
        expect(screen.getByRole('button', { name: 'Later Weeks' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: 'Later Weeks' })).toBeEnabled();
        expect(screen.getByRole('button', { name: 'Show All Weeks' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: 'Show All Weeks' })).toBeEnabled();
    });

    test('Should show immediate details', () => {
        expect(screen.getByText('Week')).toBeInTheDocument();
        expect(screen.getAllByText('IMM')).toHaveLength(2);
        expect(screen.getByText('Purchases')).toBeInTheDocument();
        expect(screen.getByText('2433')).toBeInTheDocument();
        expect(screen.getByText('Unauthorised POs')).toBeInTheDocument();
        expect(screen.getByText('4503')).toBeInTheDocument();
        expect(screen.getByText('Sales Orders')).toBeInTheDocument();
        expect(screen.getByText('1')).toBeInTheDocument();
        expect(screen.getByText('Production Reqt')).toBeInTheDocument();
        expect(screen.getByText('36')).toBeInTheDocument();
        expect(screen.getByText('Stock')).toBeInTheDocument();
        expect(screen.getByText('181')).toBeInTheDocument();
    });

    test('Should show details', () => {
        expect(screen.getByText('Week')).toBeInTheDocument();
        expect(screen.getByText('19/22')).toBeInTheDocument();
        expect(screen.getByText('13May')).toBeInTheDocument();
        expect(screen.getByText('27May')).toBeInTheDocument();
        expect(screen.getByText('Status')).toBeInTheDocument();
        expect(screen.getAllByText('H')).toHaveLength(13);
        expect(screen.getAllByText('200')).toHaveLength(13);
    });
});
