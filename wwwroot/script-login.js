
const apiBaseUrl = "https://localhost:44392/api/users"; // Update if needed

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
        const response = await fetch(`${apiBaseUrl}/register`, {
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
        const response = await fetch(`${apiBaseUrl}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        const result = await response.json();

        if (!response.ok) {
            alert(result.message || "Login failed.");
            return;
        }

        if (result.redirectUrl) {
            window.location.href = result.redirectUrl;
        } else {
            alert("Login failed: No redirect URL in response.");
        }


    } catch (error) {
        alert("Network error: " + error.message);
    }
}

