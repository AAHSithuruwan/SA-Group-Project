import React from 'react';
import './sellerdetails.css';

const SellerDetails = () => {
  return (
    <div>
      <h2 className="account-header">Seller Details</h2>
      <div className="details-container">
        <div className="details-form">
          <div className="form-left">
            <div className="input-row">
              <label>Full Name:</label>
              <input type="text" placeholder="Mr. Jude Doe" />
              <i className="edit-icon">&#9998;</i>
            </div>
            <div className="input-row">
              <label>Username:</label>
              <input type="text" placeholder="Jdoe123" />
              <i className="edit-icon">&#9998;</i>
            </div>
            <div className="input-row">
              <label>Email:</label>
              <input type="email" placeholder="judedoe@email.com" />
              <i className="edit-icon">&#9998;</i>
            </div>
            <div className="input-row">
              <label>Contact Number:</label>
              <input type="tel" placeholder="+94 123456789" />
              <i className="edit-icon">&#9998;</i>
            </div>
            <div className="input-row">
              <label>Whatsapp Number (Optional):</label>
              <input type="tel" placeholder="+94 123456789" />
              <i className="edit-icon">&#9998;</i>
            </div>
          </div>

          <div className="form-right">
            <div className="input-row">
              <label>Address:</label>
              <input type="text" placeholder="12/A" />
              <i className="edit-icon">&#9998;</i>
            </div>
            <div className="input-row">
              <input type="text" placeholder="ABC Street" />
              <i className="edit-icon">&#9998;</i>
            </div>
            <div className="input-row">
              <input type="text" placeholder="Colombo" />
              <i className="edit-icon">&#9998;</i>
            </div>
            <div className="input-row">
              <label>Add image:</label>
              <input type="file" />
            </div>

             <div className="submit-row">
          <button type="submit">Submit</button>
        </div>
          </div>
        </div>

     

       
      </div>
    </div>
  );
};

export default SellerDetails;
