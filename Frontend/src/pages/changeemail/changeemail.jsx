import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './changeemail.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const ChangeEmail = () => {
  const navigate = useNavigate();
  const [newEmail, setNewEmail] = useState('');
  const [password, setPassword] = useState('');

  const navigateTo = () => {
    navigate('/myaccount');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if(!(emailRegex.test(newEmail))){
      ErrorDialogBox({
        title: 'Email Change Failed!',
        text: 'Please enter a valid email address'
      });
      return;
    }

    const jwtToken = await getJwtToken();

    try {
      const response = await axios.put('http://localhost:5101/api/User/UpdateEmail', 
        {
          Email: newEmail,
          Password: password,
        },
        {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
          },
        }
      );

      if (response.status === 200) {
        SuccessDialogBox({
          title: 'Email Changed Successfully',
          text: 'Your email has been updated',
          onConfirm: navigateTo,
        });
      }
    } catch (error) {
      if (error.response && error.response.status === 401 && error.response.data !== "Incorrect Password") {
        navigate('/signin');
      } else {
        ErrorDialogBox({
          title: 'Email Change Failed',
          text: error.response.data,
        });
      }
      console.error(error.response.data);
    }
  };

  return (
    <div className="change-email-container">
      <div className="email-form">
        <h2 className="form-title">CHANGE EMAIL</h2>

        <form onSubmit={handleSubmit}>
          <div className="input-row">
            <label>New Email:</label>
            <input type="text" value={newEmail} placeholder="Enter New Email" onChange={(e) => setNewEmail(e.target.value)} required />
          </div>

          <div className="input-row">
            <label>Password:</label>
            <input type="password" value={password} placeholder="Enter Password" onChange={(e) => setPassword(e.target.value)} required />
          </div>

          <div className="submit-row">
            <button type="submit">Submit</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ChangeEmail;
