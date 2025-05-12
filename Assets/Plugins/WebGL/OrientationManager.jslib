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
    _orientationListenersAdded: false,
    _boundCheckFn: null,
    
    // Check the current orientation against required orientation for the scene
    checkOrientation: function() {
      if (typeof window === "undefined" || !this.currentSceneName) return;
      
      // Determine current device orientation
      let currentOrientation;
      if (window.screen && window.screen.orientation) {
        currentOrientation = window.screen.orientation.type.includes('landscape') ? 'landscape' : 'portrait';
      } else {
        currentOrientation = window.innerWidth > window.innerHeight ? 'landscape' : 'portrait';
      }
      
      // Find required orientation for current scene
      let requiredOrientation = null;
      for (const game of this.gameScenes) {
        for (const scene of this.gameScenes) {
          for (const scene of game.scenes) {
            if (scene.sceneName === this.currentSceneName) {
              requiredOrientation = scene.orientation;
              break;
            }
          }
          if (requiredOrientation) break;
        }
      }
      
      if (!requiredOrientation) return;
      
      console.log(`Required orientation for scene "${this.currentSceneName}" is: ${requiredOrientation}`);
      
      // Show or hide warning based on orientation match
      if (requiredOrientation !== currentOrientation) {
        _SendMessage("OrientationWarning", "ShowWarning", requiredOrientation);
      } else {
        _SendMessage("OrientationWarning", "HideWarning");
      }
    },
    
    // Setup event listeners for orientation changes
    setupListeners: function() {
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

  // Set the current scene and check orientation
  SetCurrentScene: function(sceneNamePtr) {
    var sceneName = UTF8ToString(sceneNamePtr);
    gameLibrary.currentSceneName = sceneName;
    
    if (typeof window !== "undefined") {
      // Setup listeners if not already done
      gameLibrary.setupListeners();
      
      // Check orientation immediately
      gameLibrary.checkOrientation();
    }
  },
  
  // Show a prompt to change orientation
  PromptOrientationChange: function(orientationPtr) {
    var orientation = UTF8ToString(orientationPtr);
    if (orientation === "landscape") {
      window.alert("Please rotate your device to landscape mode to continue.");
    } else if (orientation === "portrait") {
      window.alert("Please rotate your device to portrait mode to continue.");
    }
  },
  
  // Reset any orientation override
  ResetOrientationOverride: function() {
    console.log("Orientation override reset.");
  },
  
  // Get the required orientation for a specific game and scene
  GetRequiredOrientation: function(gameNamePtr, sceneNamePtr) {
    var gameName = UTF8ToString(gameNamePtr);
    var sceneName = UTF8ToString(sceneNamePtr);
    
    for (const game of gameLibrary.gameScenes) {
      if (game.gameName === gameName) {
        for (const scene of game.scenes) {
          if (scene.sceneName === sceneName) {
            console.log(`Required orientation for game "${gameName}", scene "${sceneName}" is: ${scene.orientation}`);
            return allocate(intArrayFromString(scene.orientation), 'i8', ALLOC_NORMAL);
          }
        }
      }
    }
    
    console.log(`Required orientation for game "${gameName}", scene "${sceneName}" is: unknown`);
    return allocate(intArrayFromString("unknown"), 'i8', ALLOC_NORMAL);
  }
};

// Register the library with Emscripten
autoAddDeps(OrientationManagerLib, '$gameLibrary');
mergeInto(LibraryManager.library, OrientationManagerLib);