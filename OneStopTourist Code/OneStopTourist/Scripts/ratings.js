function setRatings() {
    var firstRadio = document.getElementById("1");
    var secRadio = document.getElementById("2");
    var thirdRadio = document.getElementById("3");
    var fourthRadio = document.getElementById("4");
    var fifthRadio = document.getElementById("5");
    if (document.getElementsByName("ratedisplay") != null) {
        var rateNum = document.getElementById("rateNumber").innerText;
        if (rateNum == "1") {
            firstRadio.checked = true;
        }
        else if (rateNum == "2") {
            secRadio.checked = true;
        }
        else if (rateNum == "3") {
            thirdRadio.checked = true;
        }
        else if (rateNum == "4") {
            fourthRadio.checked = true;
        }
        else if (rateNum == "5") {
            fifthRadio.checked = true;
        }
    }
}