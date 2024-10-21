import React from 'react'
import './sellerdetails.css';
const SellerDetails = () => {
  return (
    <div>
    <div className="details-container">

      <div className="details-form">
        <div className="input-row">
          <label>Full Name:</label>
          <input type="text" value="Mr. Jude Doe" readOnly />
          
        </div>
        <div className="input-row">
          <label>Username:</label>
          <input type="text" value="Jdoe123" readOnly />
          
        </div>
        <div className="input-row">
          <label>Email:</label>
          <input type="email" value="judedoe@email.com" readOnly />
          
        </div>
        <div className="input-row">
          <label>Contact Number:</label>
          <input type="tel" value="+94 123456789" readOnly />
          
        </div>
        <div className="input-row">
          <label>Whatsapp Number (Optional):</label>
          <input type="tel" value="+94 123456789" readOnly />
          
        </div>
        <div className="input-row">
          <label>Address:</label>
          <input type="text" value="12/A" readOnly />
          <input type="text" value="ABC Street" readOnly />
          <input type="text" value="Colombo" readOnly /> 
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
