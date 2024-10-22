import React, { useState, useEffect } from 'react';
import { Link, useLocation } from 'react-router-dom'; 
import profilepic from '../../assets/profilepic.png'; 
import './sidebar.css'; 

const Sidebar = () => {
  const location = useLocation(); // Get the current location
  const [hoveredIndex, setHoveredIndex] = useState(null); // Start with no hovered index

  const sidebarOptions = [
    { name: 'Dashboard', path: '/dashboard' },
    { name: 'Seller Details', path: '/sellerdetails' },
    { name: 'Create Auction', path: '/addproduct' },
    { name: 'Auction Details', path: '/auctiondetails' },
    { name: 'Notifications', path: '/notifications' },
  ];

  // Effect to set the hovered index based on the current path
  useEffect(() => {
    const currentIndex = sidebarOptions.findIndex(option => option.path === location.pathname);
    setHoveredIndex(currentIndex !== -1 ? currentIndex : 0); // Set to current page index
  }, [location.pathname]);

  return (
    <div className="sidebar">
      <div className="profile-pic-container">
        <img src={profilepic} alt="Profile" className="profile-pic" />
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

export default Sidebar;



