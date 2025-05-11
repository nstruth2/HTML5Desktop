mergeInto(LibraryManager.library, {
  SendSceneNameToBrowser: function(ptr) {
    const sceneName = UTF8ToString(ptr);
    window.currentUnitySceneName = sceneName;
    if (typeof checkOrientation === 'function') {
      checkOrientation(); // call immediately
    }
  }
});
