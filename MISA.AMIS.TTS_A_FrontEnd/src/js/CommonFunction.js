import ResourceVI from "./ResourceVI";
import EnumGeneral from "./enum/EnumGeneral";
class CommonFn {
    getEnumText(enumClass, enumValue) {

        let enumResource = ResourceVI[enumClass];
        let enumEnum = EnumGeneral[enumClass];

        for (let eName in enumResource) {
            if (enumEnum[eName] == enumValue) {
                return enumResource[eName]
            }
        }
    }
}

export default new CommonFn()