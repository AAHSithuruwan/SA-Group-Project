import React, { useState } from 'react';
import { Link } from 'react-router-dom'; 
import profilepic from '../../assets/profilepic.png'; 
import './sidebar.css'; 

const Sidebar = () => {
  const [hoveredIndex, setHoveredIndex] = useState(null);

  const sidebarOptions = [
    { name: 'Dashboard', path: '/dashboard' },
    { name: 'Seller Details', path: '/sellerdetails' },
    { name: 'Add Product', path: '/addproduct' },
    { name: 'Auction Details', path: '/auctiondetails' },
    { name: 'Notifications', path: '/notifications' },
  ];

  return (
    <div className="sidebar">
      <div className="profile-pic-container">
        <img src={profilepic} alt="Profile" className="profile-pic" />
      </div>
      <ul className="sidebar-menu">
        {sidebarOptions.map((item, index) => (
          <li
            key={index}
            className={`sidebar-menu-item ${hoveredIndex === index ? 'hovered' : ''}`}
            onMouseEnter={() => setHoveredIndex(index)}
            onMouseLeave={() => setHoveredIndex(null)}
          >
            <Link to={item.path} className="sidebar-link">
              {item.name}
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Sidebar;
