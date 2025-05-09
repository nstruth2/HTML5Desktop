mergeInto(LibraryManager.library, {
    IsMobile: function () {
        var check = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        return check ? 1 : 0;
    }
});
