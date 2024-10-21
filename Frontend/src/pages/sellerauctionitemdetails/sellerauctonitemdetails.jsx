import React from 'react';
import { useLocation } from 'react-router-dom';
import './sellerauctionitemdetails.css';  // Add the relevant CSS styles here

const SellerAuctionItemDetails = () => {
  const location = useLocation();
  const { item } = location.state;

  return (
    <div className="item-details-container">
      <h2>Details</h2>
      <div className="item-details">
        <img src={item.image} alt={item.productName} />
        <h3>{item.productName}</h3>
        <p>Category: {item.category}</p>
        <p>Starting bid price: {item.startingBidPrice}</p>
        <p>Bidding Date: {item.biddingDate}</p>
        <p>Bidding Time: {item.biddingTime}</p>
        <p>Highest Bidder: {item.highestBidder}</p>
        <p>Highest Bid Price: {item.highestBidPrice}</p>
        <p>Seller Location: {item.sellerLocation}</p>
        <p>Shipping Address: {item.shippingAddress}</p>
        <h4>Description</h4>
        <p>{item.description}</p>
      </div>
    </div>
  );
};

export default SellerAuctionItemDetails;
