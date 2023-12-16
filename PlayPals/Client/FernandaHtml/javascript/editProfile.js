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

    if (newProfilePicUrl) {
        await updateProfilePicture(newProfilePicUrl);
    }

    if (newBio) {
        await updateBio(newBio);
    }

    alert('Profile updated successfully!');
}

async function updateProfilePicture(imageUrl) {
    const userEmail = localStorage.getItem('userEmail');
    const url = `http://localhost:5054/api/userProfile/${userEmail}/uploadProfilePicture`;
    const response = await fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ userEmail: userEmail, imageUrl: imageUrl }),
    });

    if (!response.ok) {
        const message = await response.text();
        alert('Failed to update profile picture: ' + message);
    }
}

async function updateBio(bio) {
    console.log(bio);
    const userEmail = localStorage.getItem('userEmail');
    console.log(userEmail);
    const url = `http://localhost:5054/api/userProfile/${userEmail}/updateBio`;
    var data = { userEmail: userEmail, bio: bio };
    const response = await fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });

    if (!response.ok) {
        const message = await response.text();
        alert('Failed to update bio: ' + message);
    }
}
