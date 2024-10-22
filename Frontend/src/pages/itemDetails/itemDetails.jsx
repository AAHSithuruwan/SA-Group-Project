// src/pages/itemDetails/ItemDetails.jsx
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useLocation } from 'react-router-dom';
import './itemDetails.css';
import { ThreeDots } from 'react-loader-spinner';

const ItemDetails = () => {
  const location = useLocation();
  const auctionId = location.state.auctionId; 
  const [auction, setAuction] = useState([]);
  const [timer, setTimer] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchAuction = async () => {
      try {
        const response = await axios.get(`http://localhost:5101/api/Auction/${auctionId}`);
        setAuction(response.data);
        initializeTimer(response.data);
      } catch (error) {
        console.error('There was an error fetching the auction', error);
      }
    };

    fetchAuction();
  }, [auctionId]);

  const initializeTimer = (auction) => {
    const interval = setInterval(() => {
      const currentTime = new Date();
      const startingTime = new Date(auction.startingDate);
      const endTime = new Date(auction.endDate);
  
      let updatedTimer;
  
      if (currentTime < startingTime) {
        updatedTimer = { status: 'Not Started', remainingTime: startingTime - currentTime };
      } else if (currentTime > endTime) {
        updatedTimer = { status: 'Ended', remainingTime: 0 };
      } else {
        updatedTimer = { status: 'Ongoing', remainingTime: endTime - currentTime };
      }
  
      setTimer(updatedTimer);
      setLoading(false);
    }, 1000); // Update every second
  
    return () => clearInterval(interval);
  };

  const formatRemainingTime = (milliseconds) => {
    const totalSeconds = Math.floor(milliseconds / 1000);
    const days = Math.floor(totalSeconds / 86400); // 86400 seconds in a day
    const hours = Math.floor((totalSeconds % 86400) / 3600); // 3600 seconds in an hour
    const minutes = Math.floor((totalSeconds % 3600) / 60); // 60 seconds in a minute
    return `${days}d ${hours}h ${minutes}m`;
  };

  return (
    <section className="item-details-section">
      <div className="item-details-container">
        <div className="image-and-info">
          <img src={`http://localhost:5101/Images/ProductImages/${auction.productId}.png`} alt={auction.productName} className="item-image" />
          <div className="item-info">
            <h2>{auction.productName}</h2>
            <div className="current-bid-container">
            <p className="current-bid">Rs. {auction.highestBidPrice ? auction.highestBidPrice : '-'}</p>
              <p className="item-price">Rs. {auction.nextBidPrice}</p>
            </div>
      
          </div>
        </div>

        <div className="auction-details">
          <div className='timer'>
          {loading && (
            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center',}}>
              <ThreeDots
                visible={true}
                height="22"
                width="50"
                color="#4fa94d"
                radius="9"
                ariaLabel="three-dots-loading"
              />
            </div>
          )}
          {!loading && (
            <>
              {timer.status === 'Ongoing' && <h4 style={{ color: 'blue' }}>Time Left: {formatRemainingTime(timer.remainingTime)}</h4>}
              {timer.status === 'Not Started' && <h4 style={{ color: 'brown' }}>Starts At: {new Date(auction.startingDate).toLocaleString()}</h4>}
              {timer.status === 'Ended' && <h4 style={{ color: 'red' }}>Ended</h4>}
            </>
          )}
          </div>

          <div className="seller-details">
            <h4>Sold By: <span className="seller-name">{auction.sellerFirstName} {auction.sellerLastName}</span></h4>
            <p className="seller-location">{auction.sellerAddress}</p>

            <h4>Description:</h4>
            <p className="item-description">{auction.productDescription}</p>

          </div>

          <div className="place-bid-container">
            <button className="place-bid-button">Place Bid</button>
          </div>
        </div>
      </div>
    </section>
  );
};

export default ItemDetails;
