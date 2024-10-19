import React from 'react';
import './Navbar.css';
import search_icon from '../../assets/search-b.png';
import logo from '../../assets/logo.png';
import { Link } from 'react-router-dom';

const Navbar = () => {
  return (
    <div className="navbar">
      {/* Logo and Left-side Links */}
      <div className="navbar-left">
        <img src={logo} alt="logo" className="logo" />
        <ul>
          <li><Link to="/">Home</Link></li>
          <li><Link to="/categories">Categories</Link></li>
        </ul>
      </div>

      {/* Search Box in the Center */}
      <div className="navbar-center">
        <div className="search-box">
          <input type="text" placeholder="Search for brand, model,..." />
          <img src={search_icon} alt="search icon" />
        </div>
      </div>

      {/* Right-side Buttons */}
      <div className="navbar-right">
        <ul>
          <li><Link to="/sell">Admin</Link></li>
          <li><Link to="/signup">Seller</Link></li>
          <li><button><Link to="/signin">Sign in</Link></button></li>
        </ul>
      </div>
    </div>
  );
};

export default Navbar;
