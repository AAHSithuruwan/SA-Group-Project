import React from 'react';
import { useLocation } from 'react-router-dom';
import './adminauctionitemdetails.css'; 

const AdminAuctionItemDetails = () => {
  const location = useLocation();
  const { item } = location.state;

  return (
    <div className="item-details-container">
      <h2 className="details-heading">Details</h2> {/* Centered heading at the top */}
      <div className="item-details">
      <center> <h3>{item.productName}</h3></center> {/* Item name above the image */}
        <img src={item.image} alt={item.productName} />
   

        

        <div className="item-seller-info">
          <p>Sold by: {item.sellerName}</p>
          <p>{item.sellerLocation}, {item.sellerCountry}</p> {/* Seller location */}
        </div>

        <div className="item-right">
          <p>Category: {item.category}</p>
          <p>Starting bid price: {item.startingBidPrice}</p>
          <p>Bidding Date: {item.biddingDate}</p>
          <p>Bidding Time: {item.biddingTime}</p>
          <p>Highest Bidder: {item.highestBidder}</p>
          <p>Highest Bid Price: {item.highestBidPrice}</p>
          <p>Seller Location: {item.sellerLocation}</p>
          <p>Shipping Address: {item.shippingAddress}</p>
        </div>
      </div>

      {/* Description Section */}
      <div className="item-description-box">
        <h3>Description</h3>
        <ul>
          <li>Designer: CELINE</li>
          <li>Model: Other</li>
          <li>Condition: Very good condition, slightly used with small signs of wear. Scratches on hardware, minor signs of wear on one base corner, interior stains</li>
          <li>Size: 24 x 18 cm</li>
          <li>Material: Leather</li>
        </ul>
      </div>
    </div>
  );
};

export default  AdminAuctionItemDetails;
