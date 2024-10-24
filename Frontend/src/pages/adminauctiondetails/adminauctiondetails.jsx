import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import './adminAuctionDetails.css'; 
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';
import ConfirmDialogBox from '../../components/DialogBoxes/ConfirmDialogBox';

const AdminAuctionDetails = () => {
  const location = useLocation();
  const { auctionId } = location.state;
  const [auction, setAuction] = useState([]);
  const navigate = useNavigate();

  const handleBid = () => {
    navigate('/adminauctionlist/adminauctiondetails/adminauctionallbids', { state: { auctionId: auction.auctionId } });
  };

  useEffect(() => {
    console.log(auctionId);
    const fetchAuction = async () => {
      try {
        const response = await axios.get(`http://localhost:5101/api/Auction/${auctionId}`);
        setAuction(response.data);
      } catch (error) {
        console.error(error);
      }
    };

    fetchAuction();
  }, [auctionId]);

  return (
    <div className="auction-details-container">
      <div className="auction-header">
        <h2>{auction.productName}</h2>
        <p>Category: {auction.categoryName}</p>
      </div>

      <div className="auction-body">
        <div className="auction-img">
          <img src={`http://localhost:5101/Images/ProductImages/${auction.productId}.png`} alt={auction.productName} />
        </div>

        <div className="auction-info">
          <p><strong>Starting Bid Price:</strong> <span style={{ marginLeft: '10px' }}>Rs. {auction.startingPrice}</span></p>
          <p><strong>Bid Increment:</strong> <span style={{ marginLeft: '10px' }}>Rs. {auction.bidIncrement}</span></p>
          <p><strong>Starting Date:</strong> <span style={{ marginLeft: '10px' }}>{auction.startingDate}</span></p>
          <p><strong>Ending Date:</strong> <span style={{ marginLeft: '10px' }}>{auction.endDate}</span></p>
          <p><strong>Highest Bid Price:</strong> <span style={{ marginLeft: '10px' }}>
                                                    {auction.highestBidPrice ? `Rs. ${auction.highestBidPrice}` : "None"}
                                                  </span></p>
          
          <div className="auction-buttons">
            <button className="bid-button" onClick={handleBid}>
              View All Bids
            </button>
          </div>
        </div>
      </div>

      <div className="highest-bidder-details">
        <h3>Highest Bidder Details</h3>
        <p>
          <strong>Highest Bidder:</strong>
          <span style={{ marginLeft: '15px' }}>{auction.highestBidder || "None"}</span>
        </p>
        
        <p>
          <strong>Contact Number:</strong>
          <span style={{ marginLeft: '15px' }}>{auction.highestBidderContact || "None"}</span>
        </p>
        
        <p>
          <strong>Shipping Address:</strong>
          <span style={{ marginLeft: '15px' }}>{auction.highestBidderAddress || "None"}</span>
        </p>
      </div>

      <div className="product-description">
        <h3>Product Description</h3>
        <p>{auction.productDescription}</p>
      </div>
    </div>
  );
};

export default AdminAuctionDetails;