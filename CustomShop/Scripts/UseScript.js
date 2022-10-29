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
    setCookie(name, "", { 'max-age': -1 })
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

    if (adderLink) adderLink.addEventListener('click', listener);

    function listener(e) {
        e.preventDefault();
        addToCartLink(errorPlace, listener, adderLink);
    }

})();

function addToCartLink(errorPlace, listener, target) {
    let size = document.querySelector(".info-panel .sizes-list .active");
    let color = document.querySelector(".info-panel .color-list .active");

    if (!(size && color)) {
        errorPlace.style.display = "block";
        return;
    }

    let goodID = document.querySelector(".info-panel .good-index").value;
    let sizeID = size.getAttribute("name");
    let colorID = color.getAttribute("name");

    addToCart(goodID, colorID, sizeID);
    alert("Товар додано в корзину!");
    target.removeEventListener('click', listener);
    target.click();
}

(function dynamicCart() {
    let countText = document.querySelector(".cart-link .cart-count");

    if (!getCookie("cart")) {
        setCookie("cart", JSON.stringify([]));
    }

    let count;
    try {
        count = JSON.parse(getCookie("cart")).length;
    } catch {
        count = 0;
    }

    if (countText) countText.innerHTML = count;
})();

(function checkCartGoods() {
    let link = document.querySelector(".bottom-links.check-cart-goods a");

    if (link) link.addEventListener('click', listener);

    function listener(e) {
        e.preventDefault();
        checkCart(listener, link);
    }

    function checkCart(listener, link) {
        let count;
        try {
            count = JSON.parse(getCookie("cart")).length;
        } catch {
            count = 0;
        }

        if (count == 0) {
            alert("Ваша корзина порожня!");
            return;
        }

        link.removeEventListener('click', listener);
        link.click();
    }
})();


(function removeFromCart() {
    let links = document.querySelectorAll(".delete-good-from-cart");

    if (links) {
        for (let el of links) {
            el.addEventListener('click', listener);
        }
    }

    function listener(e) {
        e.preventDefault();
        let link = e.target;

        let YorN = confirm("Ви впевненні що бажаєте видалити товар з корзини?");
        if (!YorN) return;

        checkGoodInCart(listener, link);
    }

    function checkGoodInCart(listener, link) {
        let goodId = link.getAttribute("goodId");
        let colorId = link.getAttribute("colorId");
        let sizeId = link.getAttribute("sizeId");


        let arrOfObjects;

        try {
            arrOfObjects = JSON.parse(getCookie("cart"));
        } catch {
            return;
        }

        console.log(1);
        arrOfObjects = arrOfObjects.filter(el => el.goodId != goodId || el.colorId != colorId || el.sizeId != sizeId);
        setCookie("cart", JSON.stringify(arrOfObjects));

        link.removeEventListener('click', listener);
        link.click();
    }
})();

(function viewLink() {
    let p = document.querySelector(".link-viewer");
    if (p) p.innerHTML = location.href;
})()

choser(".color-list.cheker");
choser(".sizes-list.cheker");