// src/pages/itemDetails/ItemDetails.jsx
import React from 'react';
import { useLocation } from 'react-router-dom';
import './itemDetails.css';

const ItemDetails = () => {
  const location = useLocation();
  const item = location.state.item; // Get the item data from state

  return (
    <section className="item-details-section">
      <div className="item-details-container">
        <div className="image-and-info">
          <img src={item.image} alt={item.name} className="item-image" />
          <div className="item-info">
            <h2>{item.name}</h2>
            <div className="current-bid-container">
              <p className="current-bid">Current Bid</p>
              <p className="item-price">{item.price}</p>
            </div>
      
          </div>
        </div>

        <div className="auction-details">
          <div className="timer">
            <p>Closes in <span>16h 23m 10s</span></p>
          </div>

          <div className="seller-details">
            <h4>Sold By: <span className="seller-name">Siripala</span></h4>
            <p className="seller-location">Kalutara, Sri Lanka</p>

            <h4>Description:</h4>
            <p className="item-description">{item.description}</p>

          </div>

          <div className="place-bid-container">
            <button className="place-bid-button">Place Bid</button>
            <p>Bid Wave member since July 27, 2020</p>
          </div>
        </div>
      </div>
    </section>
  );
};

export default ItemDetails;
