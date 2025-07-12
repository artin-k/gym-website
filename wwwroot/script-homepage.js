const apiUrl = "https://localhost:44392/api/images/homepage-images";

//ham
const hamMenu = document.querySelector(".ham-menu");

document.addEventListener("DOMContentLoaded", () => {
    const menuButton = document.querySelector(".ham-menu");
    const menu = document.querySelector(".off-screen-menu");

    menuButton.addEventListener("click", () => {
        menu.classList.toggle("active");
        menuButton.classList.toggle("active");
    });
});

const loginButton = document.getElementById("login-button");
if (loginButton) {
    loginButton.onclick = () => {
        window.location.href = "login.html";
    };
} else {
    console.error('Element with id "login-button" not found');
}

//shop
const shopButton = document.getElementById("shop-button");
if (shopButton) {
    shopButton.onclick = () => {
        window.location.href = "shopPage.html";
    };
} else {
    console.error('Element with id "shop-button" not found');
}

document.addEventListener("DOMContentLoaded", function () {
    AOS.init();
});


let lastScrollTop = 0;
const navbar = document.getElementById("navbar");

window.addEventListener("scroll", function () {
    let scrollTop = window.scrollY;

    if (scrollTop > lastScrollTop) {
        navbar.style.top = "-60px"; // Hide navbar
    } else {
        navbar.style.top = "0"; // Show navbar
    }
    lastScrollTop = scrollTop;
});


fetch("https://localhost:44392/api/images/homepage")
    .then(response => response.json())
    .then(data => {
        console.log("Fetched images:", data); // ✅ Check what's here

        const container = document.getElementById("banner-container");

        if (!Array.isArray(data) || data.length === 0) {
            alert("No images received.");
            return;
        }

        data.forEach(img => {
            alert(`FilePath: ${img.filePath}`); // ✅ Should show each path
        });

        container.innerHTML = data.map(img => `
            <img src="${img.filePath}" alt="${img.fileName}" width="300">
        `).join("");
    })
    .catch(err => {
        console.error("Image load error:", err);
        alert("Error loading images: " + err);
    });


