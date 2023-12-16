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
    

    fetch(`http://localhost:5054/api/userProfile/${userEmail}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch user profile');
            }
            return response.json();
        })
        .then(data => {
            updateProfilePage(data);
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

    const profilePicture = document.getElementById('profilePicture');
    const profileBio = document.getElementById('profileBio');

    if (userData.ProfilePicturePath) {
        profilePicture.src = userData.ProfilePicturePath;
    }

    if (userData.Bio) {
        profileBio.textContent = userData.Bio;
    }
}
