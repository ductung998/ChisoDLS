// upload.js

function showUploadMessage(message) {
    var popup = document.createElement('div');
    popup.className = 'upload-popup';
    popup.innerHTML = '<div class="upload-popup-content">' + message + '</div>';
    document.body.appendChild(popup);

    setTimeout(function () {
        popup.style.display = 'none';
    }, 3000); // Hide the popup after 3 seconds
}