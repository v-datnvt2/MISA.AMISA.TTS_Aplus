import ResourceVI from "./ResourceVI";

class ValidateFn {
    /**
     * 
     * @param {*} input xâu input cần validate
     * @param {*} dataType kiểu dữ liệu cần validate
     * @returns 
     */
    validateInputFormat(input, dataType) {
        switch (dataType) {
            case 'PersonName':
                return this.validName(input);
            case 'Email':
                return this.validEmail(input);
            case 'PhoneNumber':
                return this.validPhoneNumber(input);
            case 'IdentityNumber':
                return this.validIdentity(input);
            default:
                if (input.length == 0)
                    return ResourceVI.ValidateMessage.NOT_NULL;
                return 'Correct';
        }
    }

    /**
     * @param {} myname Xâu chứa tên cần validate
     * @returns Message Kết quả validate
     *  Nguyễn Hùng 18/07
     */
    validName(myname) {
        if (myname.length == 0) return ResourceVI.ValidateMessage.NOT_NULL;
        for (var i = 0; i < myname.length; i++) {
            if (!isNaN(parseInt(myname[i], 10))) {
                return ResourceVI.ValidateMessage.CONTAIN_ALPHABETS_ONLY;
            }
        }
        return 'Correct';
    }

    /**
     * @param {} myemail Xâu chứa email cần validate
     * @returns Message Kết quả validate
     *  Nguyễn Hùng 18/07
     */
    validEmail(myemail) {
        if (myemail.length == 0) return ResourceVI.ValidateMessage.NOT_NULL;
        const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (re.test(String(myemail).toLowerCase()) == true) return 'Correct';
        return ResourceVI.ValidateMessage.INVALID_FORMAT;
    }

    /**
     * @param {} myid Xâu chứa CMTND/CCCD cần validate
     * @returns Message Kết quả validate
     *  Nguyễn Hùng 18/07
     */
    validIdentity(myid) {
        if (!myid || myid.length == 0) return ResourceVI.ValidateMessage.NOT_NULL;
        return 'Correct';
    }

    /**
     * @param {} myphone Xâu chứa SDT cần validate
     * @returns Message Kết quả validate
     *  Nguyễn Hùng 18/07
     */
    validPhoneNumber(myphone) {
        if (!myphone || myphone.length == 0) return ResourceVI.ValidateMessage.NOT_NULL;
    }
}
export default new ValidateFn()