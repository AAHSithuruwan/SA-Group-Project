import React from 'react'
import './payment.css';

const Payment = () => {
  return (
    <div className='payment-container'>
      <h1>Your Payment Details</h1>
      <div className='payment-form'>
        <p>Please enter your card details below</p>
        <form action='#' method='POST'>
        <div className='payment-item'>
        <label htmlFor="cardnumber">Card Number:</label>
        <input type="text" id="cardnumber" name="name" placeholder="0000 0000 0000" />
        </div>
        <div className='payment-item'>
        <label htmlFor="expires">Card Expires:</label>
        <input type="text" id="expires" name="name" placeholder="MM/YY" />
        </div>
        <div className='payment-item'>
        <label htmlFor="cvc">CVC:</label>
        <input type="text" id="cvc" name="name" placeholder="103" />
        </div>
        <div className='payment-item'>
        <label htmlFor="customer">Customer Name:</label>
        <input type="text" id="customer" name="name" placeholder="Ranjith Kumara" />
        </div>
        <div className='payment-item'>
        <label htmlFor="phone">Phone Number:</label>
        <input type="text" id="phone" name="name" placeholder="077 100100" />
        </div>
        <div className='payment-item'>
        <label htmlFor="address">Shipping Address:</label>
        <input type="text" id="address" name="name" placeholder="24, Main Street" />
        </div>
        <button type="submit">Submit</button>
        </form>
      </div>

     
    </div>
  )
}

export default Payment;
