import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import './payment.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const Payment = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [cardNumber, setCardNumber] = useState('');
  const { auctionId, bidAmount } = location.state;
  const [expires, setExpires] = useState('');
  const [cvc, setCvc] = useState('');
  const [shippingName, setShippingName] = useState('');
  const [phone, setPhone] = useState('');
  const [address, setAddress] = useState('');

  const navigateToHomePage = () => {
    navigate('/'); 
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const jwtToken = await getJwtToken();

    try{
      const response = await axios.post('http://localhost:5101/api/Bid', {
        Price: bidAmount,
        AuctionId: auctionId,
        ShippingName: shippingName,
        ShippingAddress: address,
        ShippingPhoneNumber: phone,
      },
      {
        headers: {
            Authorization: `Bearer ${jwtToken}`,
        },
      });

      if(response.status === 200){
          SuccessDialogBox({
            title: 'Bid Placed Successfully',
            text: 'Your bid has been placed',
            onConfirm: navigateToHomePage,
          });
      }
    }
    catch (error) {
      if(error.response && error.response.status == 401){
        navigate('/signin');
      }
      else{
        ErrorDialogBox({
          title: 'Unable To Place The Bid',
          text: error.response.data,
        })
      }
      console.error(error);
    }
  };

  return (
    <div className='payment-container'>
      <h1>PAYMENT DETAILS</h1>
      <br></br>
      <div className='payment-form'>
        <form onSubmit={handleSubmit}>
        <div className='payment-item'>
            <label htmlFor="customer">Shipping Name:</label>
            <input
              type="text"
              id="customer"
              value={shippingName}
              onChange={(e) => setShippingName(e.target.value)}
              placeholder="Enter the shipping name"
              required
            />
          </div>
          <div className='payment-item'>
            <label htmlFor="phone">Phone Number:</label>
            <input
              type="text"
              id="phone"
              value={phone}
              onChange={(e) => setPhone(e.target.value)}
              placeholder="Enter the phone number"
              required
            />
          </div>
          <div className='payment-item'>
            <label htmlFor="address">Shipping Address:</label>
            <input
              type="text"
              id="address"
              value={address}
              onChange={(e) => setAddress(e.target.value)}
              placeholder="Enter the shipping address"
              required
            />
          </div>
          <br></br>
          <p>Please enter your card details below</p>
          <br></br>
          <div className='payment-item'>
            <label htmlFor="cardnumber">Card Number:</label>
            <input
              type="text"
              id="cardnumber"
              value={cardNumber}
              onChange={(e) => setCardNumber(e.target.value)}
              placeholder="xxxx xxxx xxxx"
              required
            />
          </div>
          <div className='payment-item'>
            <label htmlFor="expires">Card Expires:</label>
            <input
              type="text"
              id="expires"
              value={expires}
              onChange={(e) => setExpires(e.target.value)}
              placeholder="MM/YY"
              required
            />
          </div>
          <div className='payment-item'>
            <label htmlFor="cvc">CVC:</label>
            <input
              type="text"
              id="cvc"
              value={cvc}
              onChange={(e) => setCvc(e.target.value)}
              placeholder="xxx"
              required
            />
          </div>
          <button className="pay-btn" type="submit">Submit
          </button>
        </form>
      </div>
    </div>
  );
};

export default Payment;

