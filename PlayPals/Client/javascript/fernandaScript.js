document.addEventListener('DOMContentLoaded', function () {
    const container = document.getElementById('content-container');


    const paragraph = document.createElement('p');

   
    paragraph.innerHTML = 'This is a dynamically added paragraph.';

    
    container.appendChild(paragraph);

   
    function displayUserProfile(userProfile) {
        
        const profileBioElement = document.getElementById('profileBio');
        const profilePictureElement = document.getElementById('profilePicture');

       
        profileBioElement.textContent = userProfile.bio;
        profilePictureElement.src = userProfile.profilePictureUrl;
    }

   
    async function login(email, password) {
        try {
            const response = await fetch('http://localhost:5054/api/Users/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password })
            });

            if (!response.ok) throw new Error('Login failed');

            const data = await response.json();
            localStorage.setItem('userToken', data.token);
            window.location.href = 'profile.html';
        } catch (error) {
            console.error('Error:', error);
        }
    }

    
    async function uploadProfilePicture() {
        const input = document.getElementById('newProfilePic');
        if (!input.files[0]) return;

        let formData = new FormData();
        formData.append('file', input.files[0]);

        const token = localStorage.getItem('userToken');
        if (!token) {
            console.error('Authentication token not found');
            return;
        }

        try {
            const response = await fetch('http://localhost:5054/api/UserProfile/uploadProfilePicture', {
                method: 'POST',
                headers: { 'Authorization': `Bearer ${token}` },
                body: formData
            });

            if (!response.ok) throw new Error('Failed to upload profile picture');
            alert('Profile picture uploaded successfully.');
        } catch (error) {
            console.error('Error:', error);
        }
    }

    // Function to update the user's bio
    async function updateBio() {
        const bio = document.getElementById('newBio').value;
        const token = localStorage.getItem('userToken');
        if (!token) {
            console.error('Authentication token not found');
            return;
        }

        try {
            const response = await fetch('http://localhost:5054/api/UserProfile/updateBio', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
                body: JSON.stringify({ newBio: bio })
            });

            if (!response.ok) throw new Error('Failed to update bio');
            alert('Bio updated successfully.');
        } catch (error) {
            console.error('Error:', error);
        }
    }

    // Function to load the user's profile
    async function loadUserProfile() {
        const token = localStorage.getItem('userToken');
        if (!token) {
            window.location.href = 'login.html';
            return;
        }

        try {
            const response = await fetch('http://localhost:5054/api/UserProfile/profile', {
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (!response.ok) throw new Error('Failed to fetch user profile.');

            const userProfile = await response.json();
            displayUserProfile(userProfile);
        } catch (error) {
            console.error('Error:', error);
        }
    }

    // Add event listener to form submission
    const editForm = document.querySelector('.edit-form');
    if (editForm) {
        editForm.addEventListener('submit', async function (event) {
            event.preventDefault();
            await uploadProfilePicture();
            await updateBio();
            // Reload the user profile after making changes
            await loadUserProfile();
        });
    } else {
        console.error('Edit form not found');
    }

    loadUserProfile();
});
