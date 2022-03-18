import { toTitleCase, isUpperCase, isEmpty } from '../strings';

describe('when converting string to title case', () => {
    test('should convert correctly', () => {
        const notTitleCase = 'BLAH blah bLaH';
        const titleCase = 'Blah Blah Blah';

        expect(toTitleCase(notTitleCase)).toEqual(titleCase);
    });
});

describe('when checking if uppercase', () => {
    test('should return true for uppercase', () => {
        const uppercase = '000007CD0F3C';

        expect(isUpperCase(uppercase)).toEqual(true);
    });

    test('should return false for lowercase', () => {
        const lowercase = '000007cd0f3c';

        expect(isUpperCase(lowercase)).toEqual(false);
    });
});

describe('when checking string is empty', () => {
    it('should return true for undefined', () => {
        expect(isEmpty(undefined)).toEqual(true);
    });

    it('should return true for null', () => {
        expect(isEmpty(null)).toEqual(true);
    });

    it('should return true for whitespace characters', () => {
        expect(isEmpty(' ')).toEqual(true);
        expect(isEmpty('\t')).toEqual(true);
        expect(isEmpty('\n')).toEqual(true);
    });

    it('should return false for character strings', () => {
        expect(isEmpty('string')).toEqual(false);
        expect(isEmpty(' string\t')).toEqual(false);
    });
});
