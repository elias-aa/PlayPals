fetch("http://localhost:5054/api/Users", {
    method: "GET",
    headers: {
        "Content-Type": "application/json"
    }
})
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.log(error));
    
// findFriends.js
document.addEventListener('DOMContentLoaded', () => {
    const userEmail = localStorage.getItem('userEmail'); // Retrieve the email from local storage
    if (userEmail) {
        fetch(`http://localhost:5054/api/FindFriends/${userEmail}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(friends => {
            displayFriends(friends); // Call function to handle the UI changes
        })
        .catch(error => {
            console.error('Error fetching friends:', error);
        });
    } else {
        console.error('User email not found in localStorage');
    }
});

function displayFriends(friends) {
    const friendsListContainer = document.querySelector('.friends-list');
    friendsListContainer.innerHTML = ''; // Clear the list before adding new entries

    friends.forEach(friend => {
        const friendElement = document.createElement('div');
        friendElement.className = 'friend';
        friendElement.innerHTML = `
            <img src="${friend.profilePicturePath || 'default-profile.png'}" alt="Profile picture" class="friend-profile-pic">
            <div class="friend-info">
                <h2 class="friend-name">${friend.email}</h2>
                <p class="friend-status">${friend.bio || 'No bio available'}</p>
            </div>
        `;
        friendsListContainer.appendChild(friendElement);
    });
}
