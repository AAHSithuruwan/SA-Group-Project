import React from 'react';
import { FaFacebook, FaInstagram, FaWhatsapp, FaGoogle } from 'react-icons/fa';
import './signin.css'; 

import logo from '../../assets/logo.png'; 
import necklace from '../../assets/necklace.png'; 

const SignIn = () => {
  return (
    <div className="container-signin">
     

      {/* Left Section */}
      <div className="left-section-signin">
        <img src={logo} alt="bidWave Logo" className="logo2" />
        <h1>WELCOME BACK</h1>
        <p>
          Your gateway to seamless and exciting online auctions. Sign in to manage your bids, track auctions,
          and discover unique items from sellers around the world. Whether you're buying or selling, our platform
          provides a secure, easy-to-use experience tailored to your needs.
        </p>
        <div className="social-icons">
          <a href="#"><FaFacebook /></a>
          <a href="#"><FaInstagram /></a>
          <a href="#"><FaWhatsapp /></a>
          <a href="#"><FaGoogle /></a>
        </div>
      </div>

      {/* Right Section */}
      <div className="right-section-signin">
        <div className="necklace-image">
          <img src={necklace} alt="Jewelry" />
        </div>
        <h2>Sign in</h2>

        {/* Sign-in Form */}
        <form className="sign-in-form">
          <div className="input-group">
            <label htmlFor="email">Email Address</label>
            <input type="email" id="email" placeholder="Enter your email" />
          </div>

          <div className="input-group">
            <label htmlFor="password">Password</label>
            <input type="password" id="password" placeholder="Enter your password" />
          </div>

          <div className="remember-me">
            <input type="checkbox" id="remember" />
            <label htmlFor="remember">Remember Me</label>
          </div>

          <button className="sign-in-btn" type="submit">Sign in</button>
        </form>

        <a href="#" className="forgot-password">Lost your password?</a>

        <p className="terms">
          By clicking on "Sign in" you agree to <br />
          <a href="#">Terms of services</a> | <a href="#">Privacy Policy</a>
        </p>
      </div>
    </div>
  );
};

export default SignIn;
