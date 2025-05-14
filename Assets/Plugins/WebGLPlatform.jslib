mergeInto(LibraryManager.library, {
    IsDesktopPlatform: function () {
        var userAgent = navigator.userAgent || navigator.vendor || window.opera;
        userAgent = userAgent.toLowerCase();

        // Check common mobile identifiers
        if (/android|iphone|ipad|ipod|mobile/i.test(userAgent)) {
            return 0; // Mobile
        }

        return 1; // Desktop
    }
});
