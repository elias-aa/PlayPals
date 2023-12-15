// platform-selection.js
fetch("http://localhost:5054/api/Users", {
    method: "GET",
    headers: {
        "Content-Type": "application/json"
    }
})
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.log(error));

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('save-platform').addEventListener('click', () => {
        const selectedPlatform = document.getElementById('platform-select').value;
        const userEmail = localStorage.getItem('userEmail'); // Retrieve the email from local storage

        const platformData = { Name: selectedPlatform }; // Construct platform data

        fetch(`http://localhost:5054/api/preferences/${userEmail}/platforms`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(platformData)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Platform saved:', data);
            window.location.href = '../EliasHtml/Home.html'; 
        })
        .catch(error => {
            console.error('Error:', error);
        });
    });
});
