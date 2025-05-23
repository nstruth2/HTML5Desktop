mergeInto(LibraryManager.library, {
  SetCurrentSceneJS: function (sceneNamePtr) {
    if (typeof window === 'undefined') return;

    var sceneName = UTF8ToString(sceneNamePtr);
    console.log("[OrientationWarning] SetCurrentSceneJS called: " + sceneName);

    // Initialize window.gameLibrary once, if needed
    if (typeof window.gameLibrary === 'undefined') {
      window.gameLibrary = {
        gameScenes: [
          {
            gameName: "Main Menu",
            scenes: [{ sceneName: "Main Menu", orientation: "portrait" }]
          },
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
          console.log("[OrientationWarning] checkOrientation called");
          if (!this.currentSceneName) return;

          var currentOrientation = "portrait";
          if (window.screen && window.screen.orientation && typeof window.screen.orientation.type === "string") {
            currentOrientation = window.screen.orientation.type.indexOf("landscape") >= 0 ? "landscape" : "portrait";
          } else {
            currentOrientation = window.innerWidth > window.innerHeight ? "landscape" : "portrait";
          }

          var requiredOrientation = null;

          for (var g = 0; g < this.gameScenes.length; g++) {
            var game = this.gameScenes[g];
            for (var s = 0; s < game.scenes.length; s++) {
              var scene = game.scenes[s];
              if (scene.sceneName === this.currentSceneName) {
                requiredOrientation = scene.orientation;
                break;
              }
            }
            if (requiredOrientation !== null) break;
          }

          console.log("[OrientationWarning] Detected: " + currentOrientation + ", Required: " + requiredOrientation);

          if (!requiredOrientation) return;

          if (typeof SendMessage === "function") {
            var payload = JSON.stringify({
              current: currentOrientation,
              required: requiredOrientation,
              scene: this.currentSceneName
            });

            SendMessage("OrientationWarning", "ReceiveOrientationData", payload);
          } else {
            console.warn("[OrientationWarning] SendMessage is not available.");
          }
        },

        setupListeners: function () {
          if (this._orientationListenersAdded) return;

          var self = this;
          this._boundCheckFn = function () {
            self.checkOrientation();
          };

          window.addEventListener("resize", this._boundCheckFn);
          window.addEventListener("orientationchange", this._boundCheckFn);
          this._orientationListenersAdded = true;

          console.log("[OrientationWarning] Orientation listeners added.");
        }
      };
    }

    // Set current scene and run the checks
    window.gameLibrary.currentSceneName = sceneName;
    window.gameLibrary.setupListeners();
    window.gameLibrary.checkOrientation();

    console.log("[OrientationWarning] gameLibrary initialized and checked");
  },

  RedirectToPage: function(urlPtr) {
    var url = UTF8ToString(urlPtr);
    window.location.href = url;
  }
});
