import React from 'react'
import profilepic from '../../assets/profilepic.png';
import './dashboard.css';




const Dashboard = () => {
  return (
    <div className="profile-container">
      <div className="sidebar">
        <div className="profile-pic">
          <img src={profilepic} alt="Profile" />
        </div>
        <ul className="sidebar-menu">
          <li>Dashboard</li>
          <li>Seller Details</li>
          <li>Add product</li>
          <li>Auction Details</li>
          <li>Notifications</li>
        </ul>
        <div className="notification-indicator">
         
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
