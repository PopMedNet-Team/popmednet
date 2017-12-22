/// <reference path="../../../Lpp.Dns.Portal/Scripts/common.ts" />
module ESPQueryBuilder {
    export interface IESPResponseModelData {
        Headers: string[];
        RawData: any;
        Aggregated: boolean;
        Projected: boolean;
        StratifyProjectedViewByAgeGroup: boolean;
        StratificationIncludesLocations: boolean;
        Locations: IPredefinedLocation[];
    }

    export interface IPredefinedLocation {
        StateAbbrev: string;
        Location: string;
        PostalCodes: string[];
    }
}
