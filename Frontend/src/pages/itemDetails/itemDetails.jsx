// src/pages/itemDetails/ItemDetails.jsx
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useLocation, useNavigate } from 'react-router-dom';
import './itemDetails.css';
import { ThreeDots } from 'react-loader-spinner';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const ItemDetails = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { auctionId } = location.state; 
  const [auction, setAuction] = useState([]);
  const [timer, setTimer] = useState([]);
  const [loading, setLoading] = useState(true);
  const [bidAmount, setBidAmount] = useState('');

  useEffect(() => {
    console.log(auctionId);
    const fetchAuction = async () => {
      try {
        const response = await axios.get(`http://localhost:5101/api/Auction/${auctionId}`);
        setAuction(response.data);
        initializeTimer(response.data);
        window.scrollTo(0, 0);
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

  const handleSubmit = async (e) => {
    e.preventDefault();

    const jwtToken = await getJwtToken();

    if(jwtToken == null){
      navigate('/signin');
      return;
    }

    if(bidAmount < auction.nextBidPrice){
      ErrorDialogBox({
        title: 'Invalid Bid Amount',
        text: `Bid amount must be greater than or equal Rs. ${auction.nextBidPrice}.`, // Correctly formatted
      });
      return;
    }
    navigate('/item-details/payment', { state: { auctionId: auction.auctionId, bidAmount: bidAmount } });
  };

  return (
    <section className="item-details-section">
      <div className="item-details-container">
        <div className="item-info">
          <h2>{auction.productName}</h2>
          <img src={`http://localhost:5101/Images/ProductImages/${auction.productId}.png`} alt={auction.productName} className="item-image" />
        </div>
  
        <div className="auction-details">
          <div className='timer'>
            {loading && (
              <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
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
            <h4>
              {auction.highestBidPrice ? (
                <>
                  Highest Bid: <span className="seller-name">Rs. {auction.highestBidPrice}</span>
                </>
              ) : (
                <>
                  Starting Bid: <span className="seller-name">Rs. {auction.startingPrice}</span>
                </>
              )}
            </h4>
            <h4>Description:</h4>
            <p className="item-description">{auction.productDescription}</p>
          </div>
          
          {/* Conditionally render the bid form if auction is ongoing */}
          {timer.status === 'Ongoing' && (
            <form onSubmit={handleSubmit}>
              <div className="place-bid-container">
                <input 
                  type="number" 
                  placeholder="Enter your bid amount" 
                  className="bid-input" 
                  value={bidAmount} 
                  onChange={(e) => setBidAmount(e.target.value)} 
                  required
                />
                <button className="place-bid-button">Place Bid</button>
              </div>
            </form>
          )}
        </div>
      </div>
    </section>
  );  
};

export default ItemDetails;

