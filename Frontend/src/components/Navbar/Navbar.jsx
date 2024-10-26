import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Navbar.css';
import search_icon from '../../assets/search-b.png';
import logo from '../../assets/logo.png';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const Navbar = () => {
  const location = useLocation();
  const [isUser, setIsUser] = useState(null);
  const [userId, setUserId] = useState(null);
  const [isSeller, setIsSeller] = useState(null);
  const [isAdmin, setIsAdmin] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUserDetails = async () => {
      const jwtToken = await getJwtToken();
      try {
        const response = await axios.get(`http://localhost:5101/api/User`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
          },
        });
        if (response.status === 200) {
          setIsUser(1);
          setUserId(response.data.userId);
          setIsAdmin(response.data.isAdmin);
          console.log(response.data);
        }
      } catch (error) {
        setIsUser(0);
        setIsAdmin(0);
      }
    };

    const fetchSellerDetails = async () => {
      const jwtToken = await getJwtToken();
      try {
        const response = await axios.get(`http://localhost:5101/api/Seller`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
          },
        });
        if (response.status === 200) {
          setIsSeller(1);
        }
      } catch (error) {
        setIsSeller(0);
      }
    };

    fetchUserDetails();
    fetchSellerDetails();
    window.scrollTo(0, 0);
  }, [location]);

  const handleSellerLink = (e) => {
    e.preventDefault();

    if (isSeller === 1) {
      navigate('/sellerauctionlist');
    } else {
      navigate('/SellerRegistration');
    }
  };

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
          {/* Show Admin link if user is an admin */}
          {isAdmin === 1 && (
            <li><Link to="/adminauctionlist">Admin</Link></li>
          )}

          {/* Conditionally show Seller and Profile/Signed-in options */}
          {isUser === 1 ? (
            <>
              <li><a href="/" onClick={handleSellerLink}>Seller</a></li>
              <li>
                <Link to="/myaccount">
                  <img src={`http://localhost:5101/Images/UserImages/${userId}.png`} alt="Profile" className="profile-img" />
                </Link>
              </li>
            </>
          ) : (
            isUser === 0 && (
              <>
                <li><Link to="/signup">Sign Up</Link></li>
                <li><button><Link to="/signin">Sign in</Link></button></li>
              </>
            )
          )}
        </ul>
      </div>
    </div>
  );
};

export default Navbar;

