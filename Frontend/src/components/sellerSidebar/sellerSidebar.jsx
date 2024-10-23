import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { Link, useLocation } from 'react-router-dom'; 
import './sellerSidebar.css'; 
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const SellerSidebar = () => {
  const location = useLocation(); // Get the current location
  const [hoveredIndex, setHoveredIndex] = useState(null); // Start with no hovered index
  const [sellerId, setSellerId] = useState([]);
  const navigate = useNavigate();

  const sidebarOptions = [
    { name: 'Dashboard', path: '/sellerdashboard' },
    { name: 'Seller Details', path: '/sellerdetails' },
    { name: 'Create Auction', path: '/createauction' },
    { name: 'Auction Details', path: '/sellerauctionlist' },
    { name: 'Notifications', path: '/notifications' },
  ];

  useEffect(() => {
    const currentIndex = sidebarOptions.findIndex(option => {return location.pathname.startsWith(option.path);});

    const fetchSellerDetails = async () => {
      const jwtToken = await getJwtToken();
      try {
        const response = await axios.get(`http://localhost:5101/api/Seller`,{
          headers: {
              Authorization: `Bearer ${jwtToken}`,
          },
        });
        setSellerId(response.data.sellerId);
      } catch (error) {
        if(error.response && error.response.status === 401){
          navigate('/signin');
        }
        console.error('There was an error fetching the seller details!', error);
      }
    };
    setHoveredIndex(currentIndex !== -1 ? currentIndex : 0); 
    fetchSellerDetails();
    
  }, [location.pathname]);

  return (
    <div className="sidebar">
      <div className="profile-pic-container">
        <img src={`http://localhost:5101/Images/SellerImages/${sellerId}.png`} alt="Profile" className="profile-pic" />
      </div>
      <ul className="sidebar-menu">
        {sidebarOptions.map((item, index) => (
          <li
            key={index}
            className={`sidebar-menu-item ${hoveredIndex === index ? 'hovered' : ''}`} // Keep the current tab hovered
          >
            <Link to={item.path} className={`sidebar-link ${hoveredIndex === index ? 'hovered' : ''}`}>
              {item.name}
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default SellerSidebar;



