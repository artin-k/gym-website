const hamMenu = document.querySelector(".ham-menu");

document.addEventListener("DOMContentLoaded", () => {
    const menuButton = document.querySelector(".ham-menu");
    const menu = document.querySelector(".off-screen-menu");

    menuButton.addEventListener("click", () => {
        menu.classList.toggle("active");
        menuButton.classList.toggle("active");
    });
});

fetch('/api/images')
    .then(response => response.json())
    .then(images => {
        const gallery = document.getElementById('gallery');
        images.forEach(path => {
            const img = document.createElement('img');
            img.src = path;
            img.style.width = '300px';
            img.style.margin = '10px';
            gallery.appendChild(img);
        });
    });