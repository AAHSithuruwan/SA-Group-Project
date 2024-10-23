import React, { useState } from 'react';
import './changepassword.css'; 

const ChangePassword = () => {
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [retypeNewPassword, setRetypeNewPassword] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Current Password:', currentPassword);
    console.log('New Password:', newPassword);
    console.log('Retyped New Password:', retypeNewPassword);
  };

  return (
    <div className="change-password-page"> 
      <h2>Change Password</h2>
      <form onSubmit={handleSubmit}>
        <div className="change-password-input-row"> 
          <div className="change-password-label-container"> 
            <label htmlFor="currentPassword">Current Password:</label>
            <input
              type="password"
              id="currentPassword"
              value={currentPassword}
              onChange={(e) => setCurrentPassword(e.target.value)}
              placeholder="Enter current password"
              required
            />
          </div>
        </div>

        <div className="change-password-input-row">
          <div className="change-password-label-container">
            <label htmlFor="newPassword">New Password:</label>
            <input
              type="password"
              id="newPassword"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              placeholder="Enter new password"
              required
            />
          </div>
        </div>

        <div className="change-password-input-row">
          <div className="change-password-label-container">
            <label htmlFor="retypeNewPassword">Retype Password:</label>
            <input
              type="password"
              id="retypeNewPassword"
              value={retypeNewPassword}
              onChange={(e) => setRetypeNewPassword(e.target.value)}
              placeholder="Retype new password"
              required
            />
          </div>
        </div>

        <button type="submit" className="change-password-submit-button">Submit</button> 
      </form>
    </div>
  );
};

export default ChangePassword;
