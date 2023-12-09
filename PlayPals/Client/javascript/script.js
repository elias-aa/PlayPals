fetch("http://localhost:5054/api/Users",{
    method: "GET",
    headers: {
        "Content-Type": "application/json"
    }
})
.then(response => response.json())
.then(data => console.log(data))
.catch(error => console.log(error));

document.getElementById('createAccountForm').addEventListener('submit', async function(event) {
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

        console.log('Response:', response);

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`HTTP error! Status: ${response.status}, Text: ${errorText}`);
        }

        const data = await response.json();
        console.log('Success:', data);
        window.location.href = 'Home.html';
    } catch (error) {
        console.error('Error:', error);
    }
});
