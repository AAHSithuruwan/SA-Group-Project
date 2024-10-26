import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { FaFacebook, FaInstagram, FaWhatsapp, FaGoogle } from 'react-icons/fa';
import './signin.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox'; 
import { storeJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';
import logo from '../../assets/logo.png'; 
import necklace from '../../assets/necklace.png'; 

const SignIn = () => {

  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const navigateToHomePage = () => {
    navigate('/');
  };

  const handleSignIn = async (e) => {
    e.preventDefault();

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if(!(emailRegex.test(email))){
      ErrorDialogBox({
        title: 'Sign In Failed!',
        text: 'Please enter a valid email address'
      });
      return;
    }

    try{
      const response = await axios.post('http://localhost:5101/api/User/SignIn', {
        Email: email,
        Password: password,
      });

      if(response.status === 200){
        if(response.data && response.data.token){
          await storeJwtToken(response.data.token);
          SuccessDialogBox({
            title: 'Sign In Successfull',
            text: 'Welcome To BidWave',
            onConfirm: navigateToHomePage,
          });
        }
        else{
          ErrorDialogBox({
            title: 'Sign In Failed!',
            text: 'Please Try Again',
          });
        }
      }
    }
    catch (error) {
      if(error.response && error.response.status == 401){
        ErrorDialogBox({
          title: 'Sign In Failed!',
          text: error.response.data,
        });
      }
      else{
        ErrorDialogBox({
          title: 'Sign In Failed!',
          text: 'Please Try Again',
        })
      }
      console.error(error);
    }};


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
        <form className="sign-in-form" onSubmit={handleSignIn}>
          <div className="input-group">
            <label htmlFor="email">Email Address</label>
            <input type="email" id="email" placeholder="Enter your email" value={email} onChange={(e) => setEmail(e.target.value)} required/>
          </div>

          <div className="input-group">
            <label htmlFor="password">Password</label>
            <input type="password" id="password" placeholder="Enter your password" value={password} onChange={(e) => setPassword(e.target.value)} required/>
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
