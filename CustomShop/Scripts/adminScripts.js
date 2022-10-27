//FINCTIONS
function disabelElements(...args) {
    for (let el of args) {
        el.style.visibility = "hidden";
    }
}

function viewImg(fileInput, imgPlace) {
    let file = fileInput.files[0];
    if (!file) return;
    imgPlace.src = URL.createObjectURL(file);
}

function createColorsList(fileInputText, place) {
    let arrOfColors = fileInputText
        .split(' ')
        .join('')
        .split(",")
        .filter((el) => el);

    let str = "";

    for (let el of arrOfColors) {
        str += "<div style='background-color: " + el + "'></div>";
    }

    place.innerHTML = str;
}

function createMapOfTags() {
    const Tags = new Map();

    Tags.set(`[b]`, `<b>`)
    Tags.set(`[\/b]`, `<\/b>`)
    Tags.set(`[i]`, `<i>`)
    Tags.set(`[\/i]`, `<\/i>`)
    Tags.set(`[br]`, `<br>`)
    Tags.set(`[hr]`, `<hr>`)

    return Tags;
}

//EVENTS


//STARTER FUNCTION

(function addImgContainerEvents() {
    let imgContainer = document.querySelector(".create-form .img-editor-container");
    let imgContainerText = document.querySelector(".create-form .img-editor-container p");

    let imgView = document.querySelector(".create-form .img-editor-container .img-view");
    let fileInput = document.querySelector(".create-form .img-editor-container .disabeled-file-input");

    if (imgContainer)
        imgContainer.addEventListener("click", () => fileInput.click());

    if (fileInput)
        fileInput.addEventListener("change", () => {
            viewImg(fileInput, imgView)
            imgContainer.style.border = "none";
            imgContainerText.style.visibility = "hidden";
        });
})();

(function convertTeztTocolor() {
    let colorsText = document.querySelector(".colors-file-input");
    let colorsOutput = document.querySelector(".color-list");

    if (colorsOutput && colorsText)
        setInterval(() => createColorsList(colorsText.value, colorsOutput), 500)

    
})();

(function convertTextToHTML() {
    const Tags = createMapOfTags();
    Tags.set();

    let textNodes = document.querySelectorAll(".convertToHTML");

    for (let a of textNodes) {
        let text = a.innerHTML;
        for (const [key, value] of Tags) {
            console.log(key)
            text = text.replaceAll(key, value);
        }
        a.innerHTML = text;
    }

})();

(function () {
    let elements = document.querySelectorAll(".deleteButton");
    for (let element of elements) {
        element.addEventListener("click",
            (e) => {
                if (!confirm("Ви впевнені що бажаєте видалити?")) e.preventDefault();
            }
        );
    }
})();