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
    const form = document.querySelector('.edit-form');
    form.addEventListener('submit', handleFormSubmit);
});

async function handleFormSubmit(event) {
    event.preventDefault();

    const newProfilePicUrl = document.getElementById('newProfilePic').value;
    const newBio = document.getElementById('newBio').value;

    try {
        if (newProfilePicUrl) {
            await updateProfilePicture(newProfilePicUrl);
        }
        if (newBio) {
            await updateBio(newBio);
        }
        alert('Profile updated successfully!');
        window.location.href = 'Profile.html'; // Redirect to the profile page
    } catch (error) {
        console.error('Failed to update profile:', error);
    }
}

async function updateProfilePicture(imageUrl) {
    const userEmail = localStorage.getItem('userEmail');
    const url = `http://localhost:5054/api/userProfile/${userEmail}/uploadProfilePicture`;
    const imageData = { ProfilePicturePath: imageUrl };

    const response = await fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(imageData),
    });

    if (!response.ok) {
        throw new Error('Failed to update profile picture');
    }
}

async function updateBio(bio) {
    const userEmail = localStorage.getItem('userEmail');
    const url = `http://localhost:5054/api/userProfile/${userEmail}/updateBio`;
    const bioData = { Bio: bio };

    const response = await fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(bioData),
    });

    if (!response.ok) {
        throw new Error('Failed to update bio');
    }
}
