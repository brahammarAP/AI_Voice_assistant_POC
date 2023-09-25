
window.ScrollToBottom = (elementName) => {
    element = document.getElementById(elementName);
    element.scrollTop = element.scrollHeight - element.clientHeight;
}

window.blazorExtensions = {
    AdjustInputField: function () {
        var promptInput = document.getElementById('promptInput');
        promptInput.style.height = 'auto';
        var maxHeight = 6 * parseFloat(getComputedStyle(promptInput).lineHeight); // Calculate maximum height in pixels based on line height
        promptInput.style.height = Math.min(promptInput.scrollHeight, maxHeight) + 'px';
        return promptInput.scrollHeight;
    }
}