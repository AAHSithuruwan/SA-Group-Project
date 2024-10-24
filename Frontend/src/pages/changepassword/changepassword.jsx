import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './changepassword.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const ChangePassword = () => {
  const navigate = useNavigate();
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [retypePassword, setRetypePassword] = useState('');

  const navigateTo = () => {
    navigate('/myaccount');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (newPassword !== retypePassword) {
      ErrorDialogBox({
        title: 'Password Mismatch',
        text: 'New password and retype password do not match',
      });
      return;
    }

    const jwtToken = await getJwtToken();

    try {
      const response = await axios.put('http://localhost:5101/api/User/UpdatePassword',
        {
          OldPassword: currentPassword,
          NewPassword: newPassword,
        },
        {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
          },
        }
      );

      if (response.status === 200) {
        SuccessDialogBox({
          title: 'Password Changed Successfully',
          text: 'Your password has been updated',
          onConfirm: navigateTo,
        });
      }
    } catch (error) {
      if (error.response && error.response.status === 401 && error.response.data !== "Incorrect Current Password") {
        navigate('/signin');
      } else {
        ErrorDialogBox({
          title: 'Password Change Failed',
          text: error.response.data,
        });
      }
      console.error(error);
    }
  };

  return (
    <div className="change-password-container">
      <div className="password-form">
        <h2 className="form-title">CHANGE PASSWORD</h2>

        <form onSubmit={handleSubmit}>
          <div className="input-row">
            <label>Current Password:</label>
            <input type="password" value={currentPassword} placeholder="Enter Current Password" onChange={(e) => setCurrentPassword(e.target.value)} required />
          </div>

          <div className="input-row">
            <label>New Password:</label>
            <input type="password" value={newPassword} placeholder="Enter New Password" onChange={(e) => setNewPassword(e.target.value)} required />
          </div>

          <div className="input-row">
            <label>Retype New Password:</label>
            <input type="password" value={retypePassword} placeholder="Retype New Password" onChange={(e) => setRetypePassword(e.target.value)} required />
          </div>

          <div className="submit-row">
            <button type="submit">Submit</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ChangePassword;
