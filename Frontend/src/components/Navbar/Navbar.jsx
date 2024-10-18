import React from 'react';
import './Navbar.css';
import search_icon from '../../assets/search-b.png';
import logo from '../../assets/logo.png';
import { Link } from 'react-router-dom'; 

const Navbar = () => {
  return (
    <div className='navbar'>
      <img src={logo} alt='logo' className='logo' />
      <ul>
        <li><Link to="/">Home</Link></li>  
        <li><Link to="/categories">Categories</Link></li> 
        <li><Link to="/sell">Sell</Link></li> 
        <li><Link to="/signup">Sign up</Link></li>  
        <li><button><Link to="/signin">Sign in</Link></button></li> 
      </ul>
      
      <div className='search-box'>
        <input type='text' placeholder='Search for brand model' />
        <img src={search_icon} alt='search icon' />
      </div>
    </div>
  );
};

export default Navbar;
