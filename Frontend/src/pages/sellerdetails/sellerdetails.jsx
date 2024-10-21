import React from 'react'
import './sellerdetails.css';
const SellerDetails = () => {
  return (
    <div>
    <div className="details-container">

      <div className="details-form">
        <div className="input-row">
          <label>Full Name:</label>
          <input type="text" placeholder="Mr. Jude Doe"  />
          
        </div>
        <div className="input-row">
          <label>Username:</label>
          <input type="text" placeholder="Jdoe123" />
          
        </div>
        <div className="input-row">
          <label>Email:</label>
          <input type="email" placeholder="judedoe@email.com" />
          
        </div>
        <div className="input-row">
          <label>Contact Number:</label>
          <input type="tel" placeholder="+94 123456789" />
          
        </div>
        <div className="input-row">
          <label>Whatsapp Number (Optional):</label>
          <input type="tel" placeholder="+94 123456789" />
          
        </div>
        <div className="input-row">
          <label>Address:</label>
          <input type="text" placeholder="12/A" />
          <input type="text" placeholder="ABC Street"  />
          <input type="text" placeholder="Colombo" /> 
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
  )
}
export default SellerDetails;
