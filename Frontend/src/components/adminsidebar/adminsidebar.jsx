import React, { useState } from 'react';
import { Link } from 'react-router-dom'; 
import profilepic from '../../assets/profilepic.png'; 
import './adminsidebar.css'; 

const Adminsidebar = () => {
  const [hoveredIndex, setHoveredIndex] = useState(null);

  const sidebarOptions = [
    { name: 'Dashboard', path: '/admindashboard' },
    { name: 'Auction Details', path: '/adminauctiondetails' },
    { name: 'Categories', path: '/categorydetails' },
  ];

  return (
    <div className="adminsidebar">
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

export default Adminsidebar;
