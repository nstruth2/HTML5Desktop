mergeInto(LibraryManager.library, {
  RedirectToPage: function (urlPtr) {
    var url = UTF8ToString(urlPtr);
    window.location.href = url;
  }
});