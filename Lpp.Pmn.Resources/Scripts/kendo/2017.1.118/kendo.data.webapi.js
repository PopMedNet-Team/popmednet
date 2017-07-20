/*
* Kendo UI Complete v2012.1.515 (http://kendoui.com)
* Copyright 2012 Telerik AD. All rights reserved.
*
* Kendo UI Complete commercial licenses may be obtained at http://kendoui.com/complete-license
* If you do not own a commercial license, this file shall be governed by the trial license terms.
*/
(function($, undefined) {
    var kendo = window.kendo,
        extend = $.extend,
		odataFilters = {
			eq: "eq",
			neq: "ne",
			gt: "gt",
			gte: "ge",
			lt: "lt",
			lte: "le",
			contains: "contains",
			doesnotcontain: "substringof",
			endswith: "endswith",
			startswith: "startswith"
		},
        mappers = {
            pageSize: $.noop,
            page: $.noop,
            filter: function (params, filter) {
            	if (filter) {
            		params.$filter = toOdataFilter(filter);
            	}
            },
            sort: function (params, orderby) {
            	var expr = $.map(orderby, function (value) {
            		var order = value.field.replace(/\./g, "/");

            		if (value.dir === "desc") {
            			order += " desc";
            		}

            		return order;
            	}).join(",");

            	if (expr) {
            		params.$orderby = expr;
            	}
            },
            skip: function (params, skip) {
                if (skip) {
                    params.$skip = skip;
                }
            },
            take: function(params, take) {
                if (take) {
                    params.$top = take;
                }
            }
        },
        defaultDataType = {
            read: {
                dataType: "json"
            }
        };

    function toOdataFilter(filter) {
    	var result = [],
            logic = filter.logic || "and",
            idx,
            length,
            field,
            type,
            format,
            operator,
            value,
            ignoreCase,
            filters = filter.filters;

        for (idx = 0, length = filters.length; idx < length; idx++) {
            filter = filters[idx];
            field = filter.field;
            value = filter.value;
            operator = filter.operator;

            if (filter.filters) {
                filter = toOdataFilter(filter);
            } else {
                ignoreCase = filter.ignoreCase;
                field = field.replace(/\./g, "/");
                filter = odataFilters[operator];

                if (filter && value !== undefined) {
                	type = $.type(value);
                	if (type === "string") {
                		var guid = /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/ig;
                		if (guid.test(value)) {
                			format = "{1}";
                		}
                		else if (value.indexOf('DTO.Enums.') > 0) {
                		    format = "{1}";
                		}
                		else {
                			format = "'{1}'";
                		}
                		if (value.indexOf('DTO.Enums.') <= 0) {
                		    value = value.replace(/'/g, "''");
                		}

                		if (ignoreCase === true) {
                			field = "tolower(" + field + ")";
                		}

                	} else if (type === "date") {
                	    format = "{1:yyyy-MM-ddTHH:mm:ss+00:00}";
                	} else {
                		format = "{1}";
                	}

                	if (filter.length > 3) {
                		if (filter !== "substringof") {
                			format = "{0}({2}," + format + ")";
                		} else {
                			format = "{0}(" + format + ",{2})";
                			if (operator === "doesnotcontain") {
                			    format = "{0}({2},'{1}') eq -1";
                			    filter = "indexof";
                			}
                		}
                	} else {
                		format = "{2} {0} " + format;
                	}

                	filter = kendo.format(format, filter, value, field);
                }
            }

            result.push(filter);
        }

        filter = result.join(" " + logic + " ");

        if (result.length > 1) {
            filter = "(" + filter + ")";
        }

        return filter;
    }

    function FixStringDatesInResults(results) {
        results.forEach(function (data) {
            for (var field in data) {
                if (data[field]) {
                    if ($.isArray(data[field])) {
                        FixStringDatesInResults(data[field]);
                    } else if (data[field].substring && data[field].match(/^\d{4}-\d{2}-\d{2}T{1}\d{2}:\d{2}:\d{2}(\.\d*)?Z?$/g)) {
                        if (data[field].indexOf("Z") > -1 || data[field].indexOf("T00:00:00") > -1) {
                            data[field] = new Date(data[field]);
                        } else {
                            data[field] = new Date(data[field] + "Z");
                        }
                    }
                }
            }
        });
    }

    extend(true, kendo.data, {
        schemas: {
            webapi: {
                data: function (data)
                {
                    if (!$.isArray(data.results))
                        data.results = [data.results];

                    //Fix dates from strings into real dates.
                    FixStringDatesInResults(data.results);

                    return data.results;
                },
                total: function (response) {
                    return response.InlineCount;
                }
            }
        },
        transports: {
            webapi: {
                read: {
                    cache: true, // to prevent jQuery from adding cache buster
                    dataType: "json",
                },
                update: {
                    cache: true,
                    dataType: "json",
                    contentType: "application/json", // to inform the server the the request body is JSON encoded
                    type: "PUT" // can be PUT or MERGE
                },
                create: {
                    cache: true,
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST" // must be POST to create new entity
                },
                destroy: {
                    cache: true,
                    dataType: "json",
                    type: "DELETE"
                },              
                parameterMap: function(options, type) {
                    var params,
                        value,
                        option,
                        dataType;

                    options = options || {};
                    type = type || "read";
                    dataType = (this.options || defaultDataType)[type];
                    dataType = dataType ? dataType.dataType : "json";

                    if (type === "read") {
                        params = {
                            $count: true
                        };

                        for (option in options) {
                            if (mappers[option]) {
                                mappers[option](params, options[option]);
                            } else {
                                params[option] = options[option];
                            }
                        }

                        if (params.$format)
                            delete params.$format;

                    } else {
                        if (dataType !== "json") {
                            throw new Error("Only json dataType can be used for " + type + " operation.");
                        }

                        if (type !== "destroy") {
                            for (option in options) {
                                value = options[option];
                                if (typeof value === "number") {
                                    options[option] = value + "";
                                }
                            }

                            params = kendo.stringify(options);
                        }
                    }

                    return params;
                }
            }
        }
    });
})(jQuery);
