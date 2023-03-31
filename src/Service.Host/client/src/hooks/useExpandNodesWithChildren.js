import { useEffect, useState, useMemo } from 'react';

export default function useExpandNodesWithChildren(initial, tree, rootName) {
    const [expanded, setExpanded] = useState(initial);

    const nodesWithChildren = useMemo(() => {
        const result = [];
        if (!tree) {
            return result;
        }
        const q = [];
        q.push(tree);
        while (q.length !== 0) {
            let n = q.length;
            while (n > 0) {
                const current = q[0];
                q.shift();
                if (current.type !== 'C') {
                    result.push(current);
                    if (current.children?.length) {
                        for (let i = 0; i < current.children.length; i += 1) {
                            q.push(current.children[i]);
                        }
                    }
                }
                n -= 1;
            }
        }
        return [{ name: rootName, id: 'root', children: tree.children }, ...result];
    }, [tree, rootName]);
    useEffect(() => {
        setExpanded(nodesWithChildren.map(x => x.id));
    }, [nodesWithChildren]);
    return { expanded, setExpanded, nodesWithChildren };
}
