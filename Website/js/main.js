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

    var oldDivCharacters = document.getElementById("characters");

    var newDivCharacters = document.createElement("div");
    newDivCharacters.id = "characters";

    var image = document.createElement("img");
    image.src = imageSrc;
    image.height = 500;
    image.width = 500;

    var imageDescriptionBox = document.createElement("div");
    imageDescriptionBox.className = "img-description";

    var headline = document.createElement("h2");
    headline.appendChild(document.createTextNode(title));

    var line = document.createElement("hr");

    var description = document.createElement("p");
    description.className = "regular_text";
    description.appendChild(document.createTextNode(desc));

    imageDescriptionBox.appendChild(headline);
    imageDescriptionBox.appendChild(line);
    imageDescriptionBox.appendChild(description);

    newDivCharacters.appendChild(image);
    newDivCharacters.appendChild(imageDescriptionBox);

    oldDivCharacters.parentNode.replaceChild(newDivCharacters, oldDivCharacters);
}