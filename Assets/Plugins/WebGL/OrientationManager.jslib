mergeInto(LibraryManager.library, {
  gameScenes: [
    {
      gameName: "Game1",
      scenes: [
        { sceneName: "Menu Game 1", orientation: "portrait" },
        { sceneName: "Gameplay Game 1", orientation: "portrait" },
        { sceneName: "Submit Score and Name Game 1 Landscape", orientation: "landscape" }
      ]
    },
    {
      gameName: "Game2",
      scenes: [
        { sceneName: "Menu Game 2", orientation: "portrait" },
        { sceneName: "Gameplay Game 2", orientation: "portrait" },
        { sceneName: "Submit Score and Name Game 2 Lansdscape", orientation: "landscape" }
      ]
    },
    {
      gameName: "Game3",
      scenes: [
        { sceneName: "Menu Game 3", orientation: "portrait" },
        { sceneName: "Gameplay Game 3", orientation: "portrait" },
        { sceneName: "Submit Score and Name Game 3", orientation: "landscape" }
      ]
    }
  ],

  currentSceneName: null,

  SetCurrentScene: function (sceneNamePtr) {
    const sceneName = UTF8ToString(sceneNamePtr);
    Module.currentSceneName = sceneName;

    if (typeof window !== "undefined") {
      Module.checkOrientation();

      if (!Module._orientationListenersAdded) {
        window.addEventListener("resize", Module.checkOrientation);
        window.addEventListener("orientationchange", Module.checkOrientation);
        Module._orientationListenersAdded = true;
      }
    }
  },

  checkOrientation: function () {
    if (typeof window === "undefined" || !Module.currentSceneName) return;

    let currentOrientation;
    if (window.screen && window.screen.orientation) {
      currentOrientation = window.screen.orientation.type.includes('landscape') ? 'landscape' : 'portrait';
    } else {
      currentOrientation = window.innerWidth > window.innerHeight ? 'landscape' : 'portrait';
    }

    let requiredOrientation = null;

    for (const game of Module.gameScenes) {
      for (const scene of game.scenes) {
        if (scene.sceneName === Module.currentSceneName) {
          requiredOrientation = scene.orientation;
          break;
        }
      }
      if (requiredOrientation) break;
    }

    if (!requiredOrientation) return;

    if (requiredOrientation !== currentOrientation) {
      SendMessage("OrientationWarning", "ShowWarning", requiredOrientation);
    } else {
      SendMessage("OrientationWarning", "HideWarning");
    }
  },

  PromptOrientationChange: function (orientation) {
    if (orientation === "landscape") {
      alert("Please rotate your device to landscape mode to continue.");
    } else if (orientation === "portrait") {
      alert("Please rotate your device to portrait mode to continue.");
    }
  },

  ResetOrientationOverride: function () {
    console.log("Orientation override reset.");
  },

  GetRequiredOrientation: function (gameNamePtr, sceneNamePtr) {
    const gameName = UTF8ToString(gameNamePtr);
    const sceneName = UTF8ToString(sceneNamePtr);

    for (const game of Module.gameScenes) {
      if (game.gameName === gameName) {
        for (const scene of game.scenes) {
          if (scene.sceneName === sceneName) {
            return allocate(intArrayFromString(scene.orientation), 'i8', ALLOC_STACK);
          }
        }
      }
    }

    return allocate(intArrayFromString("unknown"), 'i8', ALLOC_STACK);
  }
});
