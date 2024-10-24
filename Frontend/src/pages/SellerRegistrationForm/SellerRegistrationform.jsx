import React from 'react';
 import './SellerRegistrationform.css';



const SellerRegistrationform = () => {
  return (
    <div className='registrationform-container'>
      <h1>Seller Registration Form</h1>
     <div className='registrationform'>
      <label htmlFor = "rapid">Name</label>
      <input type='text' id='rapid' name='rapid' />

     </div>
      
    </div>
  )
}

export default SellerRegistrationform;
