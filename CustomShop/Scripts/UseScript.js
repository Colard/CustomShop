class Good {
    constructor(goodId, colorId, sizeId) {
        this.goodId = goodId;
        this.colorId = colorId;
        this.sizeId = sizeId;
    }
}

function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function setCookie(name, value) {

    let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value) + "; " + "path=/";
    document.cookie = updatedCookie;
}

function deleteCookie(name) {
    setCookie(name, "", {'max-age': -1 })
}

function addToCart(goodId, colorId, sizeId) {
    if (!getCookie("cart")) {
        setCookie("cart", JSON.stringify([]));
    }

    let cart;

    try {
        cart = JSON.parse(getCookie("cart"));
    } catch {
        setCookie("cart", JSON.stringify([]));
        cart = JSON.parse(getCookie("cart"));
    }

    cart.push(new Good(goodId, colorId, sizeId));
    setCookie("cart", JSON.stringify(cart));
}

(function newAdder() {
    let nodeList = document.querySelectorAll(".good-grid-box .good-img");

    let elCount = (nodeList.length < 5) ? nodeList.length : 5;

    for (let i = 0; i < elCount; i++) {
        let div = document.createElement('div');
        div.setAttribute("class", "new-text");
        div.innerHTML = "Новинка!";
        nodeList[i].append(div);
    }
})();

function choser(parentSelector) {
    let nodeList = document.querySelector(parentSelector);

    if (nodeList) nodeList.onclick = (e) => {
        let target = e.target;
        if (target.tagName != "DIV" && target.parentNode != nodeList) return;

        for (let el of nodeList.children) {
            el.classList.remove("active");
        }

        target.classList.add("active");
    }
};

(function convertTextToHTML() {
    const Tags = createMapOfTags();
    Tags.set();

    let textNodes = document.querySelectorAll(".convertToHTML");

    for (let a of textNodes) {
        let text = a.innerHTML;
        for (const [key, value] of Tags) {
            text = text.replaceAll(key, value);
        }
        a.innerHTML = text;
    }

})();

function createMapOfTags() {
    const Tags = new Map();

    Tags.set(`[b]`, `<b>`)
    Tags.set(`[\/b]`, `<\/b>`)
    Tags.set(`[i]`, `<i>`)
    Tags.set(`[\/i]`, `<\/i>`)
    Tags.set(`[br]`, `<br>`)
    Tags.set(`[hr]`, `<hr>`)

    return Tags;
};

(function activeCartAdder() {
    let errorPlace = document.querySelector(".error-place");
    let adderLink = document.querySelector(".add-to-cart");

    if (adderLink) adderLink.onclick = (e) => {
        addToCartLink(errorPlace, e);
    } 

})();

function addToCartLink(errorPlace, event) {
    let size = document.querySelector(".info-panel .sizes-list .active");
    let color = document.querySelector(".info-panel .color-list .active");

    if (!(size && color)) {
        errorPlace.style.display = "block";
        e.preventDefault();
        return;
    } 

    let goodID = document.querySelector(".info-panel .good-index").value;
    let sizeID = size.getAttribute("name");
    let colorID = color.getAttribute("name");

    addToCart(goodID, colorID, sizeID);

}

choser(".color-list.cheker");
choser(".sizes-list.cheker");