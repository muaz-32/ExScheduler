const LocalStrategy = require("passport-local").Strategy;
const bcrypt = require("bcrypt");
const passport = require("passport");
const axios = require("axios");
const https = require("https");
const { response } = require("express");

const agent = new https.Agent({
  rejectUnauthorized: false,
});

function initialize(passport) {
  console.log("Admin Passport Config Started");
  console.log("Initialized");

  const authenticateAdmin = async (adminemail, adminpassword, done) => {
    console.log("admin email: " + adminemail);
    console.log("admin password: " + adminpassword);

    let error = [];
    try {
      const response = await axios.post(
        'https://localhost:7227/api/Admin/adminlogin',
        {
          "adminEmail": adminemail,
          "adminPassword": adminpassword
        },
        {
          httpsAgent: agent,
          headers: {
            'Content-Type': 'application/json',
          },
        }
      );

      // Check response status
      if (response.status == 200) {
        console.log("Admin Login Successful");
        console.log(response.data);
        let token = response.data.token;
        return done(null, {token});
      } else if (response.status == 400){
        console.log("Admin Login Failed");
        console.log(response.data);
        return done(null, false, { message: "Incorrect Password or email" });
      }
    } 
    catch (err) {
      return done(null, false, { message: "Incorrect Password or email" });
    }
  };

  passport.use(
    'admin',
    new LocalStrategy(
      { usernameField: "adminemail", passwordField: "adminpassword" },
      authenticateAdmin
    )
  );
  passport.serializeUser((token, done) => done(null, token));

  passport.deserializeUser((token, done) => {
    return done(null, token);
  });
}

module.exports = initialize;
