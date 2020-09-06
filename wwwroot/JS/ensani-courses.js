function ensaniClick() {
    var x = document.getElementById("courses");
    x.remove(1);
    x.remove(1);
    x.remove(1);
    var option1 = document.createElement("option");
    option1.text = "عربی";
    option1.value = "عربی"
    x.add(option1);


    var option2 = document.createElement("option");
    option2.text = "حقوق";
    option2.value = "حقوق"
    x.add(option2);

    var option3 = document.createElement("option");

    option3.text = "ادبیات";
    option3.value = "ادبیات"
    x.add(option3);
}