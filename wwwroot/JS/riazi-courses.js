
function riaziClick() {
    var x = document.getElementById("courses");
    x.remove(1);
    x.remove(1);
    x.remove(1);

    var option1 = document.createElement("option");
    option1.text = "معادلات";
    option1.value = "معادلات"
    x.add(option1);


    var option2 = document.createElement("option");
    option2.text = "هندسه";
    option2.value = "هندسه"
    x.add(option2);

    var option3 = document.createElement("option");

    option3.text = "جبر";
    option3.value = "جبر"
    x.add(option3);

}