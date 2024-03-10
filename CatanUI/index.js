const baseAddress = "http://localhost:5218/api/catan/";

const canvas = document.getElementById('gameCanvas');
const ctx = canvas.getContext('2d');

const tileSize = 35;
const a = Math.PI / 3;

let gameSeed = 0;
let gameId = undefined;
let gameState = undefined;

function drawHexagon(x, y, r) {
    ctx.beginPath();
    for (var i = 0; i < 6; i++) {
        ctx.lineTo(x + r * Math.sin(a * i), y + r * Math.cos(a * i));
    }
    ctx.closePath();
    ctx.fill();
    ctx.stroke();
}

function hexToXYCoords(originX, originY, x, y, r) {

    let h = Math.pow(3, 0.5) * 0.5 * r;
    return [
        originX + 2 * x * h + y * h,
        originY + (2 / Math.pow(2, 0.5)) * y * r * 1.05
    ]
}

function houseToXYCoords(originX, originY, x, y, r) {

    let h = Math.pow(3, 0.5) * 0.5 * r;
    return [
        (originX + h * 0.99) + x * h,
        (originY - h * 0.6) + (2 / Math.pow(2, 0.5)) * y * r * 1.05 - ((x + y) % 2) * h * 0.5
    ]
}

function getHexColourFromResource(resource) {
    switch (resource) {
        default:
        case 0:
            return "khaki";
        case 1:
            return "darkgreen";
        case 2:
            return "brown";
        case 3:
            return "yellowgreen";
        case 4:
            return "gold";
        case 5:
            return "darkgray";
    }

}

function getColourFromPlayerColour(playerColour) {
    switch (playerColour) {
        default:
        case -1:
            return "none";
        case 0:
            return "red";
        case 1:
            return "dodgerblue";
        case 2:
            return "lawngreen";
        case 3:
            return "yellow";
    }
}

function drawTile(x, y, r, colour, activationNumber = 0) {
    ctx.fillStyle = colour;
    ctx.strokeStyle = 'black';
    ctx.lineWidth = 2;
    let [x2, y2] = hexToXYCoords(tileSize * 0.5, tileSize * 1.5, x, y, r);
    drawHexagon(x2, y2, tileSize);
    if (activationNumber != 0) {
        let fontScale = 0.6 * tileSize;
        ctx.fillStyle = 'black';
        ctx.font = fontScale + "px Arial";
        ctx.fillText(activationNumber, x2 - fontScale * 0.3, y2 + fontScale * 0.3);
    }
}

function drawSettlement(x, y, r, colour) {
    ctx.fillStyle = colour;
    ctx.strokeStyle = 'none';
    ctx.lineWidth = 0;
    let [x2, y2] = houseToXYCoords(tileSize * 0.5, tileSize * 1.5, x, y, r);
    ctx.fillRect(x2 - tileSize * 0.2, y2 - tileSize * 0.2, tileSize * 0.4, tileSize * 0.4);
}

function drawCity(x, y, r, colour) {
    ctx.fillStyle = colour;
    ctx.strokeStyle = 'black';
    ctx.lineWidth = 2;
    let [x2, y2] = houseToXYCoords(tileSize * 0.5, tileSize * 1.5, x, y, r);
    ctx.fillRect(x2 - tileSize * 0.3, y2 - tileSize * 0.3, tileSize * 0.6, tileSize * 0.6);
}

function drawRoad(x1, y1, x2, y2, r, colour) {
    ctx.strokeStyle = colour;
    ctx.lineWidth = 4;
    let [x3, y3] = houseToXYCoords(tileSize * 0.5, tileSize * 1.5, x1, y1, r);
    let [x4, y4] = houseToXYCoords(tileSize * 0.5, tileSize * 1.5, x2, y2, r);
    ctx.beginPath();
    ctx.moveTo(x3, y3);
    ctx.lineTo(x4, y4);
    ctx.stroke();
}

function drawGameFromGameState(gameState) {
    gameState["board"]["hexes"].forEach(tile => {
        drawTile(tile.x, tile.y, tileSize, getHexColourFromResource(tile.resource), tile.rollNumber);
    });

    gameState["board"]["settlements"].forEach(settlement => {
        drawSettlement(settlement.x, settlement.y, tileSize, getColourFromPlayerColour(settlement.playerColour));
    });

    gameState["board"]["roads"].forEach(road => {
        if (road.playerColour != -1) {
            drawRoad(road.startX, road.startY, road.endX, road.endY, tileSize, getColourFromPlayerColour(road.playerColour));
        }
    });
}

async function createNewGame(playerCount, seed) {
    console.log("Creating new game with " + playerCount + " players");

    return new Promise((resolve, reject) => {
        fetch(baseAddress + "?seed=" + seed, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                "playerCount": playerCount
            })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.text();
            })
            .then(data => {
                console.log(data);
                resolve(data);
            })
            .catch(error => {
                console.error('Error:', error);
                reject(error);
            })
    });
}

async function getGameState(id, colour) {
    console.log("Getting game state for game " + id + " with colour " + colour);

    await fetch(baseAddress + id + "/" + colour)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
        })
        .catch(error => {
            console.error('Error:', error);
        })
}

async function updateGameState(colour) {
    console.log("Updating game state");

    await fetch(baseAddress + gameId + "/" + colour)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
            gameState = data;
        })
        .catch(error => {
            console.error('Error:', error);
        })
}

function mainDraw() {
    requestAnimationFrame(mainDraw);
    ctx.clearRect(0, 0, canvas.clientWidth, canvas.clientHeight);
    ctx.fillStyle = 'grey';
    ctx.fillRect(0, 0, canvas.clientWidth, canvas.clientHeight);
    if (gameState != undefined) {
        drawGameFromGameState(gameState);
    }
}

async function main() {
    gameId = await createNewGame(3, gameSeed);

    while (true) {
        if (gameId != undefined) {
            await updateGameState(0);
        }

        await new Promise(r => setTimeout(r, 5000));
    }
}

main();
mainDraw();