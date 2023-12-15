fetch("http://localhost:5054/api/Users", {
    method: "GET",
    headers: {
        "Content-Type": "application/json"
    }
})
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.log(error));

// Function to store the authentication token in localStorage
function storeToken(token) {
    localStorage.setItem('userToken', token);
}

// Function to store the email in localStorage
function storeEmail(email) {
    localStorage.setItem('userEmail', email);
}

// Function to retrieve the authentication token from localStorage
function getAuthToken() {
    return localStorage.getItem('userToken');
}

// Function to load user profile data
async function loadUserProfile() {
    const token = getAuthToken();
    if (!token) {
        console.error('Authentication token not found');
        window.location.href = 'signin.html'; // Redirect to login page
        return;
    }

    try {
        
        const response = await fetch(`http://localhost:5054/api/Users/profile`, {
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json', 
            }
        });

        if (!response.ok) {
            throw new Error('Failed to fetch user profile.');
        }

        const user = await response.json();
       
        console.log('User Profile:', user);
    } catch (error) {
        console.error('Error:', error);
    }
}

document.getElementById('createAccountForm').addEventListener('submit', async function (event) {
    event.preventDefault();

    try {
        const email = document.getElementById('email').value;
        const password = document.getElementById('password').value;
        const confirmPassword = document.getElementById('confirmPassword').value;

        if (password !== confirmPassword) {
            alert('Passwords do not match.');
            return;
        }

        console.log('Submitting:', email, password);

        const response = await fetch('http://localhost:5054/api/Users/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email: email, password: password })
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`HTTP error! Status: ${response.status}, Text: ${errorText}`);
        }

        const data = await response.json();
        console.log('Success:', data);

        // Store the authentication token and email in localStorage
        storeEmail(data.email);
        console.log(data.token);
        console.log(data.email);
        // Redirect to the login page after successful registration
        window.location.href = '../AymanHtml/GenrePage.html';
       

     
        

        
    } catch (error) {
        console.error('Error:', error);
    }
});