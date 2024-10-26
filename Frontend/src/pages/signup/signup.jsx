
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './signup.css';
import googleIcon from '../../assets/google-icon.png';
import facebookIcon from '../../assets/facebook-icon.png';
import logo from '../../assets/logo.png';
import signupimg from '../../assets/signupimg.png';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';

const Signup = () => {

  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const navigateToSignIn = () => {
    navigate('/signin');
  };

  const handleSignUp = async (e) => {
    e.preventDefault();

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if(!(emailRegex.test(email))){
      ErrorDialogBox({
        title: 'Sign Up Failed!',
        text: 'Please enter a valid email address'
      });
      return;
    }

    if(password.length < 8){
      ErrorDialogBox({
        title: 'Sign Up Failed!',
        text: 'The password must have at least 8 characters'
      });
      return;
    }

    //check if the passwords match
    if(password != confirmPassword){
      ErrorDialogBox({
        title: 'Sign Up Failed!',
        text: 'The Provided Passwords does not match'
      });
      return;
    }

    try{
      const response = await axios.post('http://localhost:5101/api/User/SignUp', {
        Email: email,
        Password: password,
      });

      if(response.status === 200){
        SuccessDialogBox({
          title: 'Sign Up Successfull',
          text: 'Please Log In',
          onConfirm: navigateToSignIn,
        })
        
      }
    }
    catch (error) {
      if(error.response && error.response.status == 400){
        ErrorDialogBox({
          title: 'Sign Up Failed!',
          text: error.response.data,
        });
      }
      else{
        ErrorDialogBox({
          title: 'Sign Up Failed!',
          text: 'Please Try Again',
        })
      }
      console.error(error);
    }};

  return (
      <div className="signup-container">
        <div className="signup-image">
          <img src={signupimg} alt="Signup" />
        </div>

        <div className="signup-form">
          <img src={logo} alt="logo" className="logo1" />
          <h2>SIGNUP</h2>
          <form onSubmit={handleSignUp}>
           
            <div className="form-group">
              <i className="fa fa-envelope"></i>
              <input type="text" placeholder="Enter your Email" value={email} onChange={(e) => setEmail(e.target.value)} required/>
            </div>
           
            <div className="form-group">
              <i className="fa fa-lock"></i>
              <input type="password" placeholder="Create Password" value={password} onChange={(e) => setPassword(e.target.value)} required/>
            </div>
            <div className="form-group">
              <i className="fa fa-lock"></i>
              <input type="password" placeholder="Retype Password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} required/>
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