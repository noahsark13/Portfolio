
"use strict";
const app = new PIXI.Application({
    autoResize: true,
    backgroundColor: 0x000000,
    width: window.innerWidth,
    height: window.innerHeight
});
document.body.appendChild(app.view);

// constants
const sceneWidth = app.view.width;
const sceneHeight = app.view.height;

app.loader.
    add(["images/Astro A.png",
        "images/Astro B.png",
        "images/Astro C.png",
        "images/Astro D.png",
        "images/Battle Droid.png",
        "images/BB A.png",
        "images/BB B.png",
        "images/Buzz Droid.png",
        "images/Gonk.png",
        "images/IG.png",
        "images/Mouse.png",
        "images/Pit Droid.png",
        "images/Probe.png",
        "images/Super Droid.png",
        "images/logo.png",
        "images/thermal.png",
        "images/stars.png"]);
app.loader.onProgress.add(e => { console.log(`progress=${e.progress}`) });
app.loader.onComplete.add(setup);
app.loader.load();


// aliases
let stage;

// game variables
let startScene;
let backgroundScene;
let gameScene, scoreLabel, lifeLabel, levelLabel;
let igniteSound, swingSound, humSound, offSound, crashSound, thermalSound;
let gameOverScene;
let gameOverScoreLabel;

let objects = [];

let score = 0;
let life = 3;
let levelNum = 1;
let levelTimer = 0;
let round = 0;
let paused = true;

let timer;
let timerCount = 0;

let spawnPaused = false;
let pausedTimer = 0;

let mouseTimer = 0;

const mousePos = new PIXI.Point();
let isHolding = false;
let mouseSpeed;
let lastMouse = new PIXI.Point();

// updates mouse position when click is held
app.renderer.plugins.interaction.on('pointermove', function (e) {
    if (isHolding && gameScene.visible == true) {

        let dt = 1 / app.ticker.FPS;
        if (dt > 1 / 12) dt = 1 / 12;

        mouseTimer += dt;

        // uses delta time to track consistant mouse speed across frame rates.
        if (mouseTimer >= 0.01) {
            mousePos.x = e.data.global.x;
            mousePos.y = e.data.global.y;
            emitter.updateOwnerPos(mousePos.x, mousePos.y);

            // magnitude of change in vectors
            mouseSpeed = mousePos.subtract(lastMouse, new PIXI.Point()).magnitude();

            //(mouseSpeed);

            lastMouse = new PIXI.Point(mousePos.x, mousePos.y);

            mouseTimer = 0;
        }


    }
    else {
        // resets when no longer holding
        mouseTimer = 0;
    }

});

// tracks when holding left click begins
app.renderer.plugins.interaction.on('pointerdown', function (e) {
    if (gameScene.visible) {
        emitter.updateOwnerPos(e.data.global.x, e.data.global.y);
        mousePos.x = e.data.global.x;
        mousePos.y = e.data.global.y;
        lastMouse = new PIXI.Point(mousePos.x, mousePos.y);

        igniteSound.play();
        humSound.play();

        isHolding = true;
    }


});

// tracks when holding left click is released
app.renderer.plugins.interaction.on('pointerup', function (e) {
    if (isHolding) {
        offSound.play();
        isHolding = false;
        mousePos.x = -100000
        mousePos.y = -100000

        humSound.stop();
    }

    //console.log(mouseSpeed);

});

// resets mouse pressed status when pointer goes out
app.renderer.plugins.interaction.on('pointerout', function (e) {
    if (isHolding) {
        isHolding = false;
        mousePos.x = -100000
        mousePos.y = -100000
        offSound.play();
        humSound.stop();
    }

    //console.log(mouseSpeed);

});

// sets up game
function setup() {
    // make scenes
    stage = app.stage;
    startScene = new PIXI.Container();
    stage.addChild(startScene);

    gameScene = new PIXI.Container();
    gameScene.visible = false;
    stage.addChild(gameScene);

    gameOverScene = new PIXI.Container();
    gameOverScene.visible = false;
    stage.addChild(gameOverScene);

    // sets up sounds using howl
    humSound = new Howl({
        src: ['sounds/saberhum.mp3'],
        loop: [true, '0']
    });

    swingSound = new Howl({
        src: ['sounds/saberswing.mp3']
    });

    igniteSound = new Howl({
        src: ['sounds/saberignite.mp3']
    });

    offSound = new Howl({
        src: ['sounds/saberoff.mp3']
    });

    crashSound = new Howl({
        src: ['sounds/sabercrash.mp3']
    });

    thermalSound = new Howl({
        src: ['sounds/thermal.mp3']
    });

    // create labels for scenes
    createLabelsAndButtons();
    // add gameloop to ticker
    app.ticker.add(gameLoop);


}

// set up labels using PIXI objects.
function createLabelsAndButtons() {

    // BUTTON STYLE
    let buttonStyle = new PIXI.TextStyle({
        fill: 0xE2E2E2,
        stroke: 0x0070CD,
        fontSize: 70,
        strokeThickness: 5,
        fontWeight: "bold",
        fontFamily: "Courier New"
    });

    // set up start button
    let startButton = new PIXI.Text("ENTER");
    startButton.style = buttonStyle;
    startButton.anchor.x = 0.5;
    startButton.anchor.y = 0.5;
    startButton.x = sceneWidth / 2;
    startButton.y = sceneHeight / 2 + 300;
    startButton.interactive = true;
    startButton.buttonMode = true;
    startButton.on("pointerup", startGame);
    startButton.on('pointerover', e => e.target.alpha = 0.5);
    startButton.on('pointerout', e => e.currentTarget.alpha = 1.0);
    startScene.addChild(startButton);

    // set up info text
    let infoText = new PIXI.Text("HOLD LEFT CLICK TO BEGIN YOUR SLASH, \nMOVE YOUR MOUSE FAST ENOUGH TO SLICE DROIDS. \nAVOID SLASHING BOMBS, THEY WILL DEAL DAMAGE. \nSUPER BATTLE DROIDS REQUIRE QUICK MULTI HITS, SO LOOK OUT FOR THEM");
    infoText.style = new PIXI.TextStyle({
        fill: 0xE2E2E2,
        stroke: 0x0070CD,
        fontSize: 20,
        strokeThickness: 2,
        fontFamily: "Courier New"
    });
    infoText.align = 'center';
    infoText.anchor.x = 0.5;
    infoText.anchor.y = 0.5;
    infoText.x = sceneWidth / 2;
    infoText.y = sceneHeight / 2 + 100;
    startScene.addChild(infoText);

    let droid = new PIXI.Sprite(new PIXI.Texture.from("images/Astro C.png", undefined, undefined, 3));
    droid.scale.x = 0.5;
    droid.scale.y = 0.5;
    droid.anchor.x = 0.5;
    droid.anchor.y = 0.5;
    droid.x = (sceneWidth / 2) - 600;
    droid.y = (sceneHeight / 2) + 100;
    startScene.addChild(droid);

    // set up starting logo
    let startLogo = new PIXI.Sprite(new PIXI.Texture.from("images/logo.png", undefined, undefined, 3));
    startLogo.scale.x = 0.5;
    startLogo.scale.y = 0.5;
    startLogo.anchor.x = 0.5;
    startLogo.anchor.y = 0.5;
    startLogo.x = sceneWidth / 2;
    startLogo.y = (sceneHeight / 2) - 200;
    startScene.addChild(startLogo);

    // text style for game info
    let textStyle = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 30,
        fontFamily: "Courier New",
        stroke: 0x0070CD,
        strokeThickness: 4
    });

    // set up score label
    scoreLabel = new PIXI.Text();
    scoreLabel.style = textStyle;
    scoreLabel.x = 5;
    scoreLabel.y = 5;
    gameScene.addChild(scoreLabel);
    increaseScoreBy(0);

    // set up life label
    lifeLabel = new PIXI.Text();
    lifeLabel.style = textStyle;
    lifeLabel.x = 5;
    lifeLabel.y = 40;
    gameScene.addChild(lifeLabel);
    decreaseLifeBy(0);

    // set up level label
    levelLabel = new PIXI.Text();
    levelLabel.style = textStyle;
    levelLabel.anchor.x = 1;
    levelLabel.x = sceneWidth - 5;
    levelLabel.y = 5;
    gameScene.addChild(levelLabel);
    increaseLevel(0);

    // set up game over text
    let gameOverText = new PIXI.Text("GAME OVER!");
    gameOverText.style = new PIXI.TextStyle({
        fill: 0xE2E2E2,
        stroke: 0x0070CD,
        fontSize: 350,
        strokeThickness: 20,
        fontFamily: "Courier New"
    });
    gameOverText.scale.x = 0.5;
    gameOverText.scale.y = 0.5;
    gameOverText.anchor.x = 0.5;
    gameOverText.anchor.y = 0.5;
    gameOverText.x = sceneWidth / 2;
    gameOverText.y = (sceneHeight / 2) - 200;
    gameOverScene.addChild(gameOverText);

    // set up game over label
    gameOverScoreLabel = new PIXI.Text();
    gameOverScoreLabel.style = new PIXI.TextStyle({
        fill: 0xE2E2E2,
        stroke: 0x0070CD,
        fontSize: 100,
        strokeThickness: 10,
        fontFamily: "Courier New"
    });
    gameOverScoreLabel.scale.x = 0.5;
    gameOverScoreLabel.scale.y = 0.5;
    gameOverScoreLabel.anchor.x = 0.5;
    gameOverScoreLabel.anchor.y = 0.5;
    gameOverScoreLabel.x = sceneWidth / 2;
    gameOverScoreLabel.y = (sceneHeight / 2) + 100;
    gameOverScene.addChild(gameOverScoreLabel);

    displayGameOver();

}

// sets up values for start of game.
function startGame() {
    startScene.visible = false;
    gameOverScene.visible = false;
    timer = Math.random() * (3.0 - 0.1) + 0.1;

    gameScene.visible = true;

    levelNum = 1;
    score = 0;
    life = 3;

}

// creates the droids used in gameplay
function createDroids(numFruit) {
    for (let i = 0; i < numFruit; i++) {

        let randValue = Math.random();
        // chance for bomb to spawn
        if (randValue < 0.3) {
            let f = new Bomb(50,
                Math.random() * ((sceneWidth - 50) - 50) + 50,
                sceneHeight + 20,
                Math.random() * (200 - (-200)) - 200,
                Math.random() * (-300 - (-600)) - 600);

            objects.push(f);
            gameScene.addChild(f);
        }
        // chance combo droid to spawn
        else if (randValue < 0.4) {
            let f = new ComboDroid(50,
                Math.random() * ((sceneWidth - 50) - 50) + 50,
                sceneHeight + 20,
                Math.random() * (200 - (-200)) - 200,
                Math.random() * (-300 - (-600)) - 600);


            objects.push(f);
            gameScene.addChild(f);
        }
        // other wise spawn regular droid
        else {
            let f = new Droid(50,
                Math.random() * ((sceneWidth - 50) - 50) + 50,
                sceneHeight + 20,
                Math.random() * (200 - (-200)) - 200,
                Math.random() * (-300 - (-600)) - 600);


            objects.push(f);
            gameScene.addChild(f);
        }


    }

}

// Core game loop
function gameLoop() {

    let dt = 1 / app.ticker.FPS;
    if (dt > 1 / 12) dt = 1 / 12;

    // tracks time of round, and the time till spawn next wave of droids
    if (spawnPaused == false && gameScene.visible) {
        timerCount += dt;
        levelTimer += dt;
    }

    // checks when next round should start
    if (levelTimer >= 30) {
        spawnPaused = true;
        pausedTimer += dt;

        //console.log(pausedTimer);

        // actually starts the new round after a brief intermission
        if (pausedTimer >= 10) {
            increaseLevel(1);
            timerCount = 0;
            levelTimer = 0;
            spawnPaused = false;
        }

    }


    // tracks when to spawn fruit
    if (timerCount >= timer) {
        // makes a random amount of fruit, scaling chances with the current level
        createDroids(Math.round(Math.random() * ((1 + (Math.round(levelNum * 0.5))) - 1) + 1));

        timerCount = 0;
        timer = Math.random() * (4.0 - 0.1) + 0.1;
    }

    // checks for things on all objects in game
    for (let c of objects) {
        // runs the gravity physics
        c.physics(dt)

        // checks if mousespeed is above a certain amount
        if (mouseSpeed >= 7 && isHolding) {
            let distX = mousePos.x - c.x;
            let distY = mousePos.y - c.y;
            let distance = Math.sqrt((distX * distX) + (distY * distY));

            // prevents sound overlap
            if (!swingSound.playing()) {
                swingSound.play();
            }

            // checks for point, radius collision
            if (distance <= c.radius) {

                // if collides bomb
                if (c instanceof Bomb) {
                    decreaseLifeBy(1);
                    increaseScoreBy(-100);
                }
                // if it is a combo droid and has not already been hit
                else if (c instanceof ComboDroid && c.hasHit == false) {
                    c.life--;
                    c.scale.set(c.scale.x - 0.02);
                    c.hasHit = true;
                    increaseScoreBy(10);
                }
                // normal droid
                else if (c instanceof Droid) {
                    increaseScoreBy(25);
                }

                // destroys combo droid only when life is 0
                if (c instanceof ComboDroid && c.life <= 0) {
                    increaseScoreBy(100);

                    objects.splice(objects.indexOf(c), 1);
                    gameScene.removeChild(c);
                    crashSound.play();
                }
                // deletes all current objects when bomb is hit
                else if (c instanceof Bomb) {

                    for (let c of objects) {
                        gameScene.removeChild(c);
                    }

                    objects = [];

                    thermalSound.play();

                }
                // all else normally removed
                else if (!(c instanceof ComboDroid)) {
                    objects.splice(objects.indexOf(c), 1);
                    gameScene.removeChild(c);

                    crashSound.play();
                }


            }
            else {
                // no longer in radius, so has hit is now false for combo droid.
                if (c instanceof ComboDroid) c.hasHit = false;

            }
        }

        // deletes when objects go out a range
        if (c.y > sceneHeight + 300) {
            objects.splice(objects.indexOf(c), 1);
            gameScene.removeChild(c);
            //console.log(objects);
        }
    }

    // initiates game over
    if (life <= 0) {
        gameScene.visible = false;
        isHolding = false;
        gameOverScene.visible = true;
        humSound.stop();

        displayGameOver();
    }
}

// increases score
function increaseScoreBy(value) {

    if (score + value <= 0) {
        score = 0;
    }
    else {
        score += value;
    }

    scoreLabel.text = `Score: ${score}`;
}

// decreases life
function decreaseLifeBy(value) {
    life -= value;
    life = parseInt(life);
    lifeLabel.text = `Life: ${life}`;
}

// increase level
function increaseLevel(value) {
    levelNum += value;
    let lev = parseInt(levelNum);
    levelLabel.text = `Level: ${lev}`;
}

// changes game over display
function displayGameOver() {
    gameOverScoreLabel.text = `You reached Level: ${levelNum}, with a Score of: ${score}!`
}

// trail
// (most of this code is borrowed so I don't really know what it does)

const sharpness = 0.1;
const minDelta = 0.05;

const texture = createTexture(0, 8, app.renderer.resolution);
const emitterPos = mousePos.clone();

const container = new PIXI.ParticleContainer(5000, {
    scale: true,
    position: true,
    rotation: false,
    uvs: false,
    tint: true
});

const emitter = new PIXI.particles.Emitter(container, [texture], {
    autoUpdate: true,
    alpha: {
        start: 0.8,
        end: 0.15
    },
    scale: {
        start: 1,
        end: 0.8,
        minimumScaleMultiplier: 1
    },
    color: {
        start: "#e3f9ff",
        end: "#2196F3"
    },
    speed: {
        start: 0,
        end: 0,
        minimumSpeedMultiplier: 1
    },
    acceleration: {
        x: 0,
        y: 0
    },
    maxSpeed: 0,
    startRotation: {
        min: 0,
        max: 0
    },
    noRotation: true,
    rotationSpeed: {
        min: 0,
        max: 0
    },
    lifetime: {
        min: 0.1,
        max: 0.1
    },
    blendMode: "normal",
    frequency: 0.0008,
    emitterLifetime: -1,
    maxParticles: 5000,
    pos: {
        x: 0,
        y: 0
    },
    addAtBack: false,
    spawnType: "point"
});

let resized = false;

emitter.updateOwnerPos(emitterPos.x, emitterPos.y);

app.stage.addChild(container);
app.stage.interactive = true;
app.ticker.add(onTick);

// this i do know how works
function onTick(delta) {

    if (isHolding && gameScene.visible == true) {
        // updates mouse/emitter pos
        if (!emitterPos.equals(mousePos)) {

            const dx = mousePos.x - emitterPos.x;
            const dy = mousePos.y - emitterPos.y;

            if (Math.abs(dx) > minDelta) {
                emitterPos.x += dx * 0.1;
            } else {
                emitterPos.x = mousePos.x;
            }

            if (Math.abs(dy) > minDelta) {
                emitterPos.y += dy * 0.1;
            } else {
                emitterPos.y = mousePos.y;
            }


            emitter.updateOwnerPos(emitterPos.x, emitterPos.y);
        }
    }
    else {
        // moves off screen
        mousePos.x = -1000000;
        mousePos.y = -1000000;
        emitter.updateOwnerPos(mousePos.x, mousePos.y);
    }


}

function createTexture(r1, r2, resolution) {

    const c = (r2 + 1) * resolution;
    r1 *= resolution;
    r2 *= resolution;

    const canvas = document.createElement("canvas");
    const context = canvas.getContext("2d");
    canvas.width = canvas.height = c * 2;

    const gradient = context.createRadialGradient(c, c, r1, c, c, r2);
    gradient.addColorStop(0, "rgba(255,255,255,1)");
    gradient.addColorStop(1, "rgba(255,255,255,0)");

    context.fillStyle = gradient;
    context.fillRect(0, 0, canvas.width, canvas.height);

    return PIXI.Texture.from(canvas);
}
