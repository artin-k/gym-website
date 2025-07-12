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
