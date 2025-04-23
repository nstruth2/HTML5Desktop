mergeInto(LibraryManager.library, {
  ResizeCanvasForUnitySceneReload: function () {
    var canvas = document.getElementById("unity-canvas");
    if (!canvas) return;

    var chromeHeight = window.outerHeight - window.innerHeight;
    var adjustedHeight = window.innerHeight - chromeHeight;

    canvas.style.width = window.innerWidth + "px";
    canvas.style.height = adjustedHeight + "px";

    console.log("Canvas resized by ResizeCanvasForUnitySceneReload()");
  }
});
