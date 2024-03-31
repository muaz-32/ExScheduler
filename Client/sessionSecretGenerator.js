const crypto = require('crypto');

function generateSessionSecret() {
    return crypto.randomBytes(64).toString('hex');
}

console.log(generateSessionSecret());