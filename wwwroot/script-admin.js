async function uploadImages() {
    const input = document.getElementById('imageUpload');
    const files = input.files;

    if (files.length === 0) {
        alert("Please select at least one image.");
        return;
    }

    const formData = new FormData();
    for (let file of files) {
        formData.append("files", file);
    }

    try {
        const response = await fetch("https://localhost:44392/api/upload", {
            method: "POST",
            body: formData
        });

        if (!response.ok) {
            const err = await response.text();
            throw new Error(err);
        }

        const data = await response.json();
        console.log("Uploaded files:", data.files);
        document.getElementById("uploadStatus").innerHTML = "✅ Upload successful!";
        // Save image URLs to localStorage or backend DB if needed

    } catch (error) {
        console.error("Upload error:", error);
        alert("Upload failed: " + error.message);
    }
}


    function loadAdminImages() {
        fetch("https://localhost:44392/api/images/all")
            .then(res => res.json())
            .then(images => {
                const container = document.getElementById("image-list");
                container.innerHTML = "";

                images.forEach(img => {
                    const card = document.createElement("div");
                    card.className = "image-card";

                    card.innerHTML = `
          <img src="${img.filePath}" alt="${img.fileName}">
          <p>${img.fileName}</p>
          <label>Sort Order: 
            <input type="number" value="${img.sortOrder}" min="0" style="width: 60px;" 
              onchange="updateSort(${img.id}, this.value)">
          </label>
          <br><br>
          <button onclick="deleteImage(${img.id})" style="color: red">Delete</button>
        `;

                    container.appendChild(card);
                });
            });
}

    function deleteImage(id) {
  if (!confirm("Are you sure you want to delete this image?")) return;

    fetch(`https://localhost:44392/api/images/${id}`, {method: "DELETE" })
    .then(() => loadAdminImages())
    .catch(err => alert("Failed to delete image"));
}

    function updateSort(id, newSort) {
        fetch(`https://localhost:44392/api/images/sort/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(parseInt(newSort))
        })
            .then(() => loadAdminImages())
            .catch(err => alert("Failed to update sort order"));
}

    window.onload = loadAdminImages;

