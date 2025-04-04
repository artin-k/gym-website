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
