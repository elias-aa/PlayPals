fetch("http://localhost:5054/api/Users", {
    method: "GET",
    headers: {
        "Content-Type": "application/json"
    }
})
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.log(error));
    
document.addEventListener("DOMContentLoaded", function () {
    loadUserProfile();

    var editLink = document.getElementById("editLink");
    editLink.addEventListener("click", function (event) {
        event.preventDefault();
        window.location.href = "EditProfile.html";
    });
});

function loadUserProfile() {
    const userEmail = localStorage.getItem('userEmail');
    
    if (!userEmail) {
        console.error('User email not found in localStorage');
        return;
    }

    fetch(`http://localhost:5054/api/userProfile/${userEmail}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch user profile');
            }
            return response.json();
        })
        .then(userData => {
            updateProfilePage(userData);
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function updateProfilePage(userData) {
    if (!userData) {
        console.error('No user data provided');
        return;
    }

    const profilePictureElement = document.getElementById('profilePicture');
    const profileBioElement = document.getElementById('profileBio');
    const emailElement = document.getElementById('email');
    const genresElement = document.getElementById('genres');
    const platformsElement = document.getElementById('platforms');

    profilePictureElement.src = userData.profilePicturePath || 'default-profile.png';
    profileBioElement.textContent = userData.bio || 'No bio provided';
    emailElement.textContent = `Email: ${userData.email}`;

    // Display genres
    genresElement.textContent = 'Genres: ' + (userData.genres ? userData.genres.join(', ') : 'No genres specified');

    // Display platforms
    platformsElement.textContent = 'Platforms: ' + (userData.platforms ? userData.platforms.join(', ') : 'No platforms specified');   
}
