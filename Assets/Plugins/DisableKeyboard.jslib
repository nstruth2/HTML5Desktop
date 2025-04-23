mergeInto(LibraryManager.library, {
  DisableKeyboardOnDesktop: function () {
    var isDesktop = /Win|Macintosh|Linux/i.test(navigator.userAgent);
    if (isDesktop) {
      setTimeout(function () {
        var inputs = document.querySelectorAll("input, textarea");
        inputs.forEach(function (input) {
          input.setAttribute("readonly", true);
          input.style.caretColor = "transparent";
        });
      }, 1000);
    }
  }
});
