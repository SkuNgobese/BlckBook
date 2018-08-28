// Validation Method
$.validator.addMethod("rsaid", function (value, element, param) {
    var idnumber = value;
 
    //1. numeric and 13 digits
    if (isNaN(idnumber) || (idnumber.length != 13)) {
        return false;
    }
    //2. first 6 numbers is a valid date
    var tempDate = new Date(idnumber.substring(0, 2), idnumber.substring(2, 4) - 1, idnumber.substring(4, 6));
    if (!((tempDate.getYear() == idnumber.substring(0, 2)) &&
        (tempDate.getMonth() == idnumber.substring(2, 4) - 1) &&
        (tempDate.getDate() == idnumber.substring(4, 6))))
    {
        return false;
    }
 
    //3. luhn formula
    var tempTotal = 0; var checkSum = 0; var multiplier = 1;
    for (var i = 0; i < 13; ++i)
    {
        tempTotal = parseInt(idnumber.charAt(i)) * multiplier;
        if (tempTotal > 9) {
            tempTotal = parseInt(tempTotal.toString().charAt(0)) + parseInt(tempTotal.toString().charAt(1));
        }
        checkSum = checkSum + tempTotal;
        multiplier = (multiplier % 2 == 0) ? 1 : 2;
    }
    if ((checkSum % 10) == 0) {
        return true
    };
    return false;
});
 
// Validation Adapter
jQuery.validator.unobtrusive.adapters.addBool('rsaid');