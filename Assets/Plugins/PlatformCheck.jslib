mergeInto(LibraryManager.library, {
  IsDesktopPlatform: function () {
    var userAgent = navigator.userAgent || navigator.vendor || window.opera;
    var isDesktop = /Win|Macintosh|Linux/i.test(userAgent);
    return isDesktop ? 1 : 0;
  }
});
