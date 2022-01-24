/**
 * @jest-environment jsdom
 */
import itemCreated from '../itemCreated';
import history from '../../history';

jest.mock('../../history', () => ({
    push: jest.fn()
}));

const historyPushSpy = jest.spyOn(history, 'push');
const fakeStore = { dispatch: jest.fn() };
const fakeNext = jest.fn();

beforeEach(() => {
    jest.clearAllMocks();
});

describe('When RECEIVE_NEW_SIGNING_LIMIT...', () => {
    test('should not redirect', () => {
        itemCreated(fakeStore)(fakeNext)({ type: 'RECEIVE_NEW_SIGNING_LIMIT', payload: {} });
        expect(historyPushSpy).not.toBeCalled();
    });
});

describe('When RECEIVE_NEW_SIGNING_LIMIT...', () => {
    test('should not redirect', () => {
        itemCreated(fakeStore)(fakeNext)({
            type: 'RECEIVE_NEW_SIGNING_LIMIT',
            payload: {}
        });
        expect(historyPushSpy).not.toBeCalled();
    });
});

describe('When any other RECEIVE_NEW_ action', () => {
    test('should redirect', () => {
        expect(historyPushSpy).not.toBeCalled();
    });

    itemCreated(fakeStore)(fakeNext)({
        type: 'RECEIVE_NEW_TYPE',
        payload: { data: { links: [{ rel: 'self', href: '/location' }] } }
    });
    expect(historyPushSpy).toBeCalledWith('/location');
});
