import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './myaccount.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const MyAccount = () => {
  const navigate = useNavigate();
  const [userData, setUserData] = useState({
    firstName: '',
    lastName: '',
    phoneNumber: '',
    email: '',
    address: '',
    userImage: null, // For storing image file
  });
  
  useEffect(() => {
    const fetchUserData = async () => {
      const jwtToken = await getJwtToken();

      try {
        const response = await axios.get('http://localhost:5101/api/User', {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
          },
        });
        setUserData(response.data);
      } catch (error) {
        console.error(error);
      }
    };

    fetchUserData();
  }, []);

  const handleChangeEmail = () => {
    navigate('/myaccount/changeemail');
  };

  const handleChangePassword = () => {
    navigate('/myaccount/changepassword');
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUserData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    setUserData((prevData) => ({ ...prevData, userImage: file }));
  };

  const navigateTo = () => {
    navigate('/myaccount');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const jwtToken = await getJwtToken();

    // Create FormData to handle file upload
    const formData = new FormData();
    Object.entries(userData).forEach(([key, value]) => {
      formData.append(key, value);
    });

    try {
      const response = await axios.put('http://localhost:5101/api/User/UpdatePersonalDetails', formData, {
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          'Content-Type': 'multipart/form-data',
        },
      });
      if (response.status === 200) {
        SuccessDialogBox({
          title: 'Personal Details Updated Successfully',
          text: 'Your personal details have been updated',
          onConfirm: navigateTo,
        });
      }
    } catch (error) {
      ErrorDialogBox({
        title: 'Personal Details Update Failed',
        text: error.response.data,
      });
      console.error(error);
    }
  };

  return (
    <div>
      <h2 className="myaccount-header">My Account</h2>
      <div className="myaccount-details-container">
        <form onSubmit={handleSubmit} className="myaccount-details-form">
          <div className="myaccount-form-left">
            <div className="myaccount-input-row">
              <label>First Name:</label>
              <input 
                type="text" 
                name="firstName" 
                value={userData.firstName} 
                onChange={handleChange} 
                placeholder="Enter First Name" 
              />
              <i className="myaccount-edit-icon">&#9998;</i>
            </div>
            <div className="myaccount-input-row">
              <label>Last Name:</label>
              <input 
                type="text" 
                name="lastName" 
                value={userData.lastName} 
                onChange={handleChange} 
                placeholder="Enter Last Name" 
              />
              <i className="myaccount-edit-icon">&#9998;</i>
            </div>
            <div className="myaccount-input-row">
              <label>Contact Number:</label>
              <input 
                type="tel" 
                name="contactNumber" 
                value={userData.phoneNumber} 
                onChange={handleChange} 
                placeholder="Enter Contact Number" 
              />
              <i className="myaccount-edit-icon">&#9998;</i>
            </div>
          </div>

          <div className="myaccount-form-right">
            <div className="myaccount-input-row">
              <label>Address:</label>
              <input 
                type="text" 
                name="address" 
                value={userData.address} 
                onChange={handleChange} 
                placeholder="Enter Address" 
                style={{ width: '100%', height: '50px' }} // Increased height for larger input
              />
              <i className="myaccount-edit-icon">&#9998;</i>
            </div>

            {/* New Input Field for User Image */}
            <div className="myaccount-input-row">
              <label>User Image:</label>
              <input 
                type="file" 
                name="image" 
                onChange={handleImageChange} 
                accept="image/*" // Accept only image files
                style={{ width: '100%' }} // Similar styling as address
              />
              <i className="myaccount-edit-icon">&#9998;</i>
            </div>

            <div className="myaccount-submit-row">
              <button type="submit">Submit</button>
            </div>
          </div>
        </form>

        {/* Email Section with Buttons Aligned to the Right */}
        <div className="myaccount-email-section">
          <div className="myaccount-email-content">
            <label className="myaccount-email-text">Email:</label>
            <span className="myaccount-email-text">{userData.email}</span>
          </div>
          <div className="myaccount-email-actions">
            <button className="myaccount-email-btn" onClick={handleChangeEmail}>Change Email</button>
            <button className="myaccount-password-btn" onClick={handleChangePassword}>Change Password</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default MyAccount;






