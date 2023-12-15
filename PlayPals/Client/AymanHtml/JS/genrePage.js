// genre-selection.js
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
    document.getElementById('save-genre').addEventListener('click', () => {
        const selectedGenreName = document.getElementById('genre-select').value;
        const userEmail = localStorage.getItem('userEmail'); // Retrieve the email from local storage

        const genreData = { Name: selectedGenreName }; // Construct genre data

        fetch(`http://localhost:5054/api/preferences/${userEmail}/genres`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(genreData)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Genre saved:', data);
            window.location.href = 'PlatformPage.html'; // Redirect to home page
        })
        .catch(error => {
            console.error('Error:', error);
        });
    });
});
