export default function FormatDate(date) {
    if (!date)
        return "";
    if (!(date instanceof Date))
        date = new Date(date);
    return kendo.toString(date, DateFormat);
}
export function FormatDateTime(date) {
    if (!date)
        return "";
    if (!(date instanceof Date))
        date = new Date(date);
    return kendo.toString(date, DateTimeFormat);
}
export class Guid {
    static newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
    static compare(a, b) {
        if (a == null && b == null)
            return 0;
        if (a != null && b == null) {
            return 1;
        }
        if (a == null && b != null) {
            return -1;
        }
        return a.localeCompare(b, 'en', { usage: 'search', sensitivity: 'accent' });
    }
    static equals(a, b) {
        if ((a == null && b != null) || (a != null && b == null)) {
            return false;
        }
        return (a == null && b == null) || a.toLowerCase() === b.toLowerCase();
    }
}
export const GuidEmpty = "00000000-0000-0000-0000-000000000000";
export const DateFormat = "MM/d/yyyy";
export const DateFormatter = "{0:MM/d/yyyy}";
export const DateTimeFormat = "MM/d/yyyy h:mm tt";
export const DateTimeFormatter = "{0:MM/d/yyyy h:mm tt}";
export const LineBreak = "<br/><br/>";
export const QueryComposerModelID = "455C772A-DF9B-4C6B-A6B0-D4FD4DD98488";
//# sourceMappingURL=constants.js.map