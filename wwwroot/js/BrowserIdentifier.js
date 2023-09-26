window.BrowserIdentifyer = {
    IdentifyBrowser: function () {
        var browser, myUserAgent = navigator.userAgent;

        if (myUserAgent.indexOf("Firefox") > -1) {
            browser = "Mozilla FireFox";
        } else if (myUserAgent.indexOf("SamsungBrowser") > -1) {
            browser = "Samsung Internet";
        } else if (myUserAgent.indexOf("Opera") > -1 || myUserAgent.indexOf("OPR") > -1) {
            browser = "Opera";
        } else if (myUserAgent.indexOf("Trident") > -1) {
            browser = "Microsoft Internet Explorer";
        } else if (myUserAgent.indexOf("Edge") > -1) {
            browser = "Microsoft Edge";
        } else if (myUserAgent.indexOf("Chrome") > -1) {
            browser = "Google Chrome or Chromium";
        } else if (myUserAgent.indexOf("Safari") > -1) {
            browser = "Apple Safari";
        } else {
            browser = "unknown";
        }

        return browser;
    }
}
