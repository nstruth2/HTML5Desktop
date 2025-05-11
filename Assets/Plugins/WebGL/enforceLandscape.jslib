mergeInto(LibraryManager.library, {
  EnforceLandscapeMode: function () {
    if (typeof window === "undefined") return;

    function createOverlay() {
      let overlay = document.getElementById("orientation-overlay");
      if (!overlay) {
        overlay = document.createElement("div");
        overlay.id = "orientation-overlay";
        overlay.style.position = "fixed";
        overlay.style.top = "0";
        overlay.style.left = "0";
        overlay.style.width = "100vw";
        overlay.style.height = "100vh";
        overlay.style.backgroundColor = "rgba(0, 0, 0, 0.85)";
        overlay.style.color = "white";
        overlay.style.fontSize = "24px";
        overlay.style.fontFamily = "sans-serif";
        overlay.style.display = "flex";
        overlay.style.alignItems = "center";
        overlay.style.justifyContent = "center";
        overlay.style.zIndex = "9999";
        overlay.innerText = "Please rotate your device to landscape mode.";
        document.body.appendChild(overlay);
      }
      return overlay;
    }

    function updateOrientation() {
      let overlay = createOverlay();

      if (window.innerHeight > window.innerWidth) {
        // Portrait mode
        overlay.style.display = "flex";
      } else {
        // Landscape mode
        overlay.style.display = "none";
      }
    }

    window.addEventListener("resize", updateOrientation);
    updateOrientation();
  },

  ResetOrientationOverride: function () {
    if (typeof window === "undefined") return;

    window.removeEventListener("resize", updateOrientation);

    let overlay = document.getElementById("orientation-overlay");
    if (overlay && overlay.parentNode) {
      overlay.parentNode.removeChild(overlay);
    }
  }
});
