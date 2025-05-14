var OrientationManagerLib = {
  $gameLibrary: {
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
          { sceneName: "Submit Score and Name Game 2 Landscape", orientation: "landscape" }
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
    _orientationListenersAdded: false,
    _boundCheckFn: null,

    checkOrientation: function () {
      if (typeof window === "undefined" || !this.currentSceneName) return;

      let currentOrientation;
      if (window.screen && window.screen.orientation) {
        currentOrientation = window.screen.orientation.type.includes('landscape') ? 'landscape' : 'portrait';
      } else {
        currentOrientation = window.innerWidth > window.innerHeight ? 'landscape' : 'portrait';
      }

      let requiredOrientation = null;
      for (const game of this.gameScenes) {
        for (const scene of game.scenes) {
          if (scene.sceneName === this.currentSceneName) {
            requiredOrientation = scene.orientation;
            break;
          }
        }
        if (requiredOrientation) break;
      }

      console.log(`Detected orientation: ${currentOrientation}, Required orientation: ${requiredOrientation}`);

      if (!requiredOrientation) return;

      if (typeof window.Unity !== 'undefined' && typeof window.Unity.call === 'function') {
        const message = JSON.stringify({
          current: currentOrientation,
          required: requiredOrientation,
          scene: this.currentSceneName
        });
        Unity.call(message);
      }
    },

    setupListeners: function () {
      if (this._orientationListenersAdded) return;

      if (!this._boundCheckFn) {
        this._boundCheckFn = this.checkOrientation.bind(this);
      }

      window.addEventListener("resize", this._boundCheckFn);
      window.addEventListener("orientationchange", this._boundCheckFn);
      this._orientationListenersAdded = true;

      console.log("Orientation listeners added");
    }
  },

  SetCurrentScene: function (sceneNamePtr) {
    if (typeof window === 'undefined') return;

    var sceneName = UTF8ToString(sceneNamePtr);
    console.log(`SetCurrentScene called with: ${sceneName}`);

    gameLibrary.currentSceneName = sceneName;
    gameLibrary.setupListeners();
    gameLibrary.checkOrientation();
  },

  PromptOrientationChange: function (orientationPtr) {
    // Removed window.alert logic. Unity can handle displaying this message in UI.
    var orientation = UTF8ToString(orientationPtr);
    console.log("PromptOrientationChange called for: " + orientation);
  },

  ResetOrientationOverride: function () {
    console.log("Orientation override reset.");
  },

  GetRequiredOrientation: function (gameNamePtr, sceneNamePtr) {
    var gameName = UTF8ToString(gameNamePtr);
    var sceneName = UTF8ToString(sceneNamePtr);

    for (const game of gameLibrary.gameScenes) {
      if (game.gameName === gameName) {
        for (const scene of game.scenes) {
          if (scene.sceneName === sceneName) {
            console.log(`Required orientation for "${gameName}" scene "${sceneName}" is: ${scene.orientation}`);
            return allocate(intArrayFromString(scene.orientation), 'i8', ALLOC_NORMAL);
          }
        }
      }
    }

    console.log(`Orientation unknown for game: ${gameName}, scene: ${sceneName}`);
    return allocate(intArrayFromString("unknown"), 'i8', ALLOC_NORMAL);
  }
};

// Register internal shared dependency
autoAddDeps(OrientationManagerLib, '$gameLibrary');

// Merge all into Unity's runtime
mergeInto(LibraryManager.library, OrientationManagerLib);

// Explicit global function export
mergeInto(LibraryManager.library, {
  SetCurrentScene: OrientationManagerLib.SetCurrentScene
});
