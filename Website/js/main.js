function switchHero(index, element) {
    var title = "";
    var desc = "";
    var imageSrc = "";

    switch (index) {
        case 1:
            title = "Knight";
            desc = "Der Hauptcharakter ist ein wiederauferstandener Ritter. " +
                "Durch seinen Tod hat er seine Ehre verloren und möchte diese wiedererlangen." +
                " Bewaffnet mit einem Langschwert und einem Wurfmesser versucht er sich durchzukämpfen. " +
                "Durch seine Schnelligkeit und durch seine Kampfstärke ist es ihm möglich gegen mehrere " +
                "Gegner zu kämpfen und diese im Idealfall zu besiegen.";
            imageSrc = "images/heroweb.png";
            break;
        case 2:
            title = "Frog";
            desc = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr," +
                "sed diam nonumy eirmod tempor invidunt ut labore et dolore m" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "usto duo dolores et ea rebum. Stet clita kasd gubergren,";
            imageSrc = "images/froschweb.png";
            break;
        case 3:
            title = "Archer";
            desc = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr," +
                "sed diam nonumy eirmod tempor invidunt ut labore et dolore m" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "usto duo dolores et ea rebum. Stet clita kasd gubergren,";
            imageSrc = "images/bogiweb.png";
            break;
        case 4:
            title = "Weak Knight";
            desc = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr," +
                "sed diam nonumy eirmod tempor invidunt ut labore et dolore m" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "usto duo dolores et ea rebum. Stet clita kasd gubergren,";
            imageSrc = "images/weakritterweb.png";
            break;
        case 5:
            title = "Wolf";
            desc = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr," +
                "sed diam nonumy eirmod tempor invidunt ut labore et dolore m" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "usto duo dolores et ea rebum. Stet clita kasd gubergren,";
            imageSrc = "images/wolfweb.png";
            break;
        case 6:
            title = "Ganon";
            desc = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr," +
                "sed diam nonumy eirmod tempor invidunt ut labore et dolore m" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "agna aliquyam erat, sed diam voluptua. At vero eos et accusam et j" +
                "usto duo dolores et ea rebum. Stet clita kasd gubergren,";
            imageSrc = "images/ganonweb2.png";
            break;
        default:
            break;
    }

    var a = document.getElementsByClassName("drop-shadow");
    for(var i = 0; i < a.length; i++){
        a[i].classList.remove("drop-shadow");
    }

    element.classList.add("drop-shadow");


    document.querySelector("#characters > img").src = imageSrc;
    document.querySelector("#characters > div > h2").textContent = title;
    document.querySelector("#characters > div > p").textContent = desc;

}