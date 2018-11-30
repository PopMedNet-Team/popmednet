// Type definitions for linq.jquery (from linq.js)
// Project: http://linqjs.codeplex.com/
// Definitions by: neuecc <http://www.codeplex.com/site/users/view/neuecc>
// Definitions: https://github.com/DefinitelyTyped/DefinitelyTyped

/// <reference path="..\..\..\..\node_modules\@types\jquery\index.d.ts"/>
/// <reference path="linq.d.ts"/>

declare namespace linqjs {
    interface Enumerable {
        tojQuery(): JQuery;
        tojQueryAsArray(): JQuery;
    }
}

interface JQuery {
    toEnumerable(): linqjs.Enumerable;
}

interface JQueryStatic {
    Enumerable: linq.EnumerableStatic;
}
