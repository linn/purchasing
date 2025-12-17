import React, { useEffect } from 'react';
import { signOutEntra } from '../helpers/userManager';

function LoggedOut() {
    useEffect(() => {
        signOutEntra();
        // only want to run logout logic once on component mount, not on auth changes
        // so ignore the linter this once
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return <div>You are now logged out.</div>;
}

export default LoggedOut;
