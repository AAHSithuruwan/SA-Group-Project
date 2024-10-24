import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { Link, useLocation } from 'react-router-dom'; 
import './adminsidebar.css'; 
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const AdminSidebar = () => {
  const location = useLocation(); // Get the current location
  const [hoveredIndex, setHoveredIndex] = useState(null); // Start with no hovered index
  const [userId, setUserId] = useState([]);
  const navigate = useNavigate();

  const sidebarOptions = [
    { name: 'Dashboard', path: '/admindashboard' },
    { name: 'Auction Details', path: '/adminauctionlist' },
    { name: 'Category Details', path: '/admincategorylist'}
  ];

  useEffect(() => {
    const currentIndex = sidebarOptions.findIndex(option => {return location.pathname.startsWith(option.path);});

    const fetchUserDetails = async () => {
      const jwtToken = await getJwtToken();
      try {
        const response = await axios.get(`http://localhost:5101/api/User`,{
          headers: {
              Authorization: `Bearer ${jwtToken}`,
          },
        });
        setUserId(response.data.userId);
      } catch (error) {
        if(error.response && error.response.status === 401){
          navigate('/signin');
        }
        console.error('There was an error fetching the user details!', error);
      }
    };
    setHoveredIndex(currentIndex !== -1 ? currentIndex : 0); 
    fetchUserDetails();
    
  }, [location.pathname]);

  return (
    <div className="sidebar">
      <div className="profile-pic-container">
        <img src={`http://localhost:5101/Images/UserImages/${userId}.png`} alt="Profile" className="profile-pic" />
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

export default AdminSidebar;
