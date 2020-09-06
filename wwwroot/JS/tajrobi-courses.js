function tajrobiClick() {
    var x = document.getElementById("courses");
    x.remove(1);
    x.remove(1);
    x.remove(1);
    var option1 = document.createElement("option");
    option1.text = "زیست";
    option1.value = "زیست"
    x.add(option1);


    var option2 = document.createElement("option");
    option2.text = "زمین";
    option2.value = "زمین"
    x.add(option2);

    var option3 = document.createElement("option");

    option3.text = "شیمی";
    option3.value = "شیمی"
    x.add(option3);
}