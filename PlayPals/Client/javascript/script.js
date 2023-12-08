fetch("http://localhost:5054/api/Users",{
    method: "GET",
    headers: {
        "Content-Type": "application/json"
    }
})
.then(response => response.json())
.then(data => console.log(data))
.catch(error => console.log(error));

document.getElementById('createAccountForm').addEventListener('submit', function(event) {
    event.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (password !== confirmPassword) {
        alert('Passwords do not match.');
        return;
    }

    console.log('Submitting:', email, password); // For debugging

    fetch('http://localhost:5054/api/users/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email: email, password: password })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        console.log('Response received:', data); // For debugging
        window.location.href = 'Home.html'; // Redirect to sign-in page
    })
    .catch((error) => {
        console.error('Error:', error);
    });
});
