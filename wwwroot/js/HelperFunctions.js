
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

window.cookieFunctions = {

    setCookie: function (cookie) {
        var expires = "";
        if (cookie.expires) {
            var date = new Date();
            date.setTime(date.getTime() + (cookie.expires * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = cookie.name + "=" + (cookie.value || "") + expires + "; path=/";
    },

    getCookie: function (name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) {
                return {
                    name: name,
                    value: c.substring(nameEQ.length, c.length)
                };
            }
        }
        return null;
    },

    getAllCookies: function () {
        var pairs = document.cookie.split(";");
        var cookies = [];
        for (var i = 0; i < pairs.length; i++) {
            var pair = pairs[i].split("=");
            cookies.push({
                name: pair[0].trim(),
                value: pair[1]
            });
        }
        return cookies;
    },

    deleteCookie: function (name) {
        document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    }
};
