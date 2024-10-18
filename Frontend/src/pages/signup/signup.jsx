import React from 'react';
import './signup.css';
import googleIcon from '../../assets/google-icon.png';
import facebookIcon from '../../assets/facebook-icon.png';
import logo from '../../assets/logo.png';
import signupimg from '../../assets/signupimg.png';

const Signup = () => {
  return (
    
      
      <div className="signup-container">
        <div className="signup-image">
          <img src={signupimg} alt="Signup" />
        </div>

        <div className="signup-form">
          <img src={logo} alt="logo" className="logo1" />
          <h1>SIGNUP</h1>
          <form>
            <div className="form-group">
              <i className="fa fa-user"></i>
              <input type="text" placeholder="Enter your Name" />
            </div>
            <div className="form-group">
              <i className="fa fa-envelope"></i>
              <input type="email" placeholder="Enter your Email" />
            </div>
            <div className="form-group">
              <i className="fa fa-user-tag"></i>
              <input type="text" placeholder="Enter your Role" />
            </div>
            <div className="form-group">
              <i className="fa fa-phone"></i>
              <input type="text" placeholder="Enter your Phone" />
            </div>
            <div className="form-group">
              <i className="fa fa-lock"></i>
              <input type="password" placeholder="Create Password" />
            </div>
            <button type="submit" className="signup-btn">Sign up</button>
          </form>

          <p className="or-text">or sign up with</p>
          <div className="social-buttons">
            <button className="google-btn">
              <img src={googleIcon} alt="Google" /> Google
            </button>
            <button className="facebook-btn">
              <img src={facebookIcon} alt="Facebook" /> Facebook
            </button>
          </div>
          <p className="login-text">
            Already have an account? <a href="/signin">Sign in</a>
          </p>
        </div>
      </div>

   
  );
};

export default Signup;
