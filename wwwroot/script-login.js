console.log("kir");
const apiBaseUrl = "https://localhost:5001/api/users"; // Update if needed



async function register() {
    console.log("Register function is working!");

    const usernameInput = document.getElementById("regUsername");
    const passwordInput = document.getElementById("regPassword");

    if (!usernameInput || !passwordInput) {
        console.error("Error: Register form elements not found in the document.");
        alert("Registration form is not loading properly. Please refresh the page.");
        return;
    }

    const username = usernameInput.value.trim();
    const password = passwordInput.value.trim();

    if (!username || !password) {
        alert("Username and password are required.");
        return;
    }

    try {
        const response = await fetch('/api/users/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if (!response.ok) {
            const errorText = await response.text();
            console.error("Error response:", errorText);
            alert(`Registration failed: ${errorText}`);
            return;
        }

        alert("Registration successful! You can now log in.");
        window.location.href = "login.html"; // Redirect to login page

    } catch (error) {
        console.error("Registration error:", error);
        alert("Network error. Please try again.");
    }
}
async function login() {
    const username = document.getElementById("loginUsername").value;
    const password = document.getElementById("loginPassword").value;

    if (!username || !password) {
        alert("Username and password are required");
        return;
    }

    try {
        const response = await fetch('/api/users/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        const resultText = await response.text(); // Get raw response text

        if (!response.ok) {
            alert(resultText || "Invalid username or password");
            return;
        }

        if (resultText.toLowerCase().includes("login successful")) {
            window.location.href = "homepage.html";
        } else {
            alert("Login failed: " + resultText);
        }

    } catch (error) {
        console.error("Login error:", error);
        alert("Network error. Please try again.");
    }
}

