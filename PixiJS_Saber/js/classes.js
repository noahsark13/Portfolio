
const sprites = ["Astro A.png", "Astro B.png", "Astro C.png", "Astro D.png", "Battle Droid.png", "BB A.png", "BB B.png", "Buzz Droid.png", "Gonk.png", "IG.png", "Mouse.png", "Pit Droid.png", "Probe.png"];

// basic droid
class Droid extends PIXI.Sprite {
    constructor(radius = 50, x = 300, y = 700, velocityX = 0, velocityY = -500) {

        // random sprite 
        let num = Math.round(Math.random() * (sprites.length - 1))
        super(app.loader.resources["images/" + sprites[num]].texture);


        this.anchor.set(.5, .5);
        this.scale.set(0.5);
        this.x = x;
        this.y = y;
        this.radius = radius;
        this.rotation = 0;

        //variables
        this.gravity = 200;
        this.speed = 50;

        // vector math
        this.position = new PIXI.Point(x, y);
        this.direction = new PIXI.Point();
        this.velocity = new PIXI.Point(velocityX, velocityY);
        this.acceleration = new PIXI.Point();

        this.downVector = new PIXI.Point(0, 1);

    }

    physics(dt = 1 / 60) {
        //gravity using vector math
        this.acceleration.add(this.downVector.multiplyScalar(this.gravity, new PIXI.Point), this.acceleration);

        this.velocity.add(this.acceleration.multiplyScalar(dt, new PIXI.Point), this.velocity);

        this.position.add(this.velocity.multiplyScalar(dt, new PIXI.Point), this.position);

        // update position
        this.x = this.position.x;
        this.y = this.position.y;

        this.acceleration = new PIXI.Point();
        this.velocity.normalize(this.direction);

        // rotation in angles
        let degree = (Math.atan2(this.direction.y, this.direction.x) * 180) / Math.PI;
        this.angle = degree + 90;

        // same physics repeated for other clases.

    }
}

class Bomb extends PIXI.Sprite {
    constructor(radius = 50, x = 300, y = 700, velocityX = 0, velocityY = -500) {

        super(app.loader.resources["images/thermal.png"].texture);
        this.anchor.set(.5, .5);
        this.scale.set(0.5);
        this.x = x;
        this.y = y;
        this.radius = radius;
        this.rotation = 0;

        //variables
        this.gravity = 200;
        this.speed = 50;

        this.position = new PIXI.Point(x, y);
        this.direction = new PIXI.Point();
        this.velocity = new PIXI.Point(velocityX, velocityY);
        this.acceleration = new PIXI.Point();


        this.downVector = new PIXI.Point(0, 1);

    }

    physics(dt = 1 / 60) {
        //gravity
        this.acceleration.add(this.downVector.multiplyScalar(this.gravity, new PIXI.Point), this.acceleration);

        this.velocity.add(this.acceleration.multiplyScalar(dt, new PIXI.Point), this.velocity);

        this.position.add(this.velocity.multiplyScalar(dt, new PIXI.Point), this.position);

        this.x = this.position.x;
        this.y = this.position.y;

        this.acceleration = new PIXI.Point();
        this.velocity.normalize(this.direction);

        let degree = (Math.atan2(this.direction.y, this.direction.x) * 180) / Math.PI;
        this.angle = degree + 90;
        //direction = velocity.normalize;
    }
}

class ComboDroid extends PIXI.Sprite {
    constructor(radius = 70, x = 300, y = 700, velocityX = 0, velocityY = -500) {

        super(app.loader.resources["images/Super Droid.png"].texture);
        this.tint = 0xFFD320;
        this.anchor.set(.5, .5);
        this.scale.set(0.5);
        this.x = x;
        this.y = y;
        this.radius = radius;
        this.rotation = 0;

        //variables
        this.gravity = 200;
        this.speed = 50;

        // exclusive variables to allow for multi hitting.
        this.hasHit = false;
        this.life = 10;

        this.position = new PIXI.Point(x, y);
        this.direction = new PIXI.Point();
        this.velocity = new PIXI.Point(velocityX, velocityY);
        this.acceleration = new PIXI.Point();


        this.downVector = new PIXI.Point(0, 1);

    }

    physics(dt = 1 / 60) {
        //gravity
        this.acceleration.add(this.downVector.multiplyScalar(this.gravity, new PIXI.Point), this.acceleration);

        this.velocity.add(this.acceleration.multiplyScalar(dt, new PIXI.Point), this.velocity);

        this.position.add(this.velocity.multiplyScalar(dt, new PIXI.Point), this.position);

        this.x = this.position.x;
        this.y = this.position.y;

        this.acceleration = new PIXI.Point();
        this.velocity.normalize(this.direction);

        let degree = (Math.atan2(this.direction.y, this.direction.x) * 180) / Math.PI;
        this.angle = degree + 90;
    }
}