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
    const container = document.getElementById("banner-container");

    if (!Array.isArray(data) || data.length === 0) {
        container.innerHTML = "<p>No images found.</p>";
    return;
    }

    const track = document.createElement("div");
    track.className = "carousel-track";

    data.forEach(img => {
      const image = document.createElement("img");
    image.src = img.filePath;
    image.alt = img.fileName;
    track.appendChild(image);
    });

    container.appendChild(track);

    let index = 0;
    const slideCount = data.length;

    setInterval(() => {
        index = (index + 1) % slideCount;
    track.style.transform = `translateX(-${index * 100}%)`;
    }, 5000); // ✅ change every 5 seconds
  })
  .catch(err => {
        console.error("Image load error:", err);
    document.getElementById("banner-container").innerHTML = "<p>Error loading images</p>";
  });




