mergeInto(LibraryManager.library, {
    gameOrientationMap: [],
    orientationInitialized: false,

    // Initialize the orientation map with games and their required scene orientations
    InitializeOrientationMap: function () {
        if (window.orientationInitialized) return;

        // Initialize the orientation map for all games and scenes
        window.gameOrientationMap = [
            { gameName: "Game1", scenes: [
                { sceneName: "Menu Game 1", orientation: "portrait" },
                { sceneName: "Gameplay Game 1", orientation: "portrait" },
                { sceneName: "Submit Score and Name Game 1", orientation: "landscape" }
            ]},
            { gameName: "Game2", scenes: [
                { sceneName: "Menu Game 2", orientation: "portrait" },
                { sceneName: "Gameplay Game 2", orientation: "portrait" },
                { sceneName: "Submit Score and Name Game 2", orientation: "landscape" }
            ]},
            { gameName: "Game3", scenes: [
                { sceneName: "Menu Game 3", orientation: "portrait" },
                { sceneName: "Gameplay Game 3", orientation: "portrait" },
                { sceneName: "Submit Score and Name Game 3", orientation: "landscape" }
            ]}
        ];

        window.orientationInitialized = true;
    },

    // Fetch the required orientation for a given game and scene
    GetRequiredOrientation: function (gameName, sceneName) {
        if (!window.gameOrientationMap.length) {
            console.error("Orientation map not initialized.");
            return "portrait"; // Default orientation
        }

        for (let game of window.gameOrientationMap) {
            if (game.gameName === gameName) {
                for (let scene of game.scenes) {
                    if (scene.sceneName === sceneName) {
                        return scene.orientation;
                    }
                }
            }
        }

        return "portrait"; // Default orientation if not found
    },

    // Function to prompt the user to switch orientation
    PromptOrientationChange: function (requiredOrientation) {
        if (requiredOrientation === "landscape") {
            alert("Please switch your device to landscape mode for a better experience.");
        } else if (requiredOrientation === "portrait") {
            alert("Please switch your device to portrait mode for a better experience.");
        }
    }
});
