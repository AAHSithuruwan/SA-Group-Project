import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import './userAuctionUserBids.css';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const userAuctionUserBids = () => {
  const location = useLocation();
  const [auctionBids, setAuctionBids] = useState([]);
  const navigate = useNavigate();
  const { auctionId } = location.state;

  useEffect(() => {
    const fetchAuctionBids = async () => {
      const jwtToken = await getJwtToken();

      try {
        const response = await axios.get(`http://localhost:5101/api/Bid/User/${auctionId}`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
          },
        });
        setAuctionBids(response.data);
      } catch (error) {
        if (error.response && error.response.status === 401) {
          navigate('/signin');
        }
        console.error('There was an error fetching the bids!', error);
      }
    };

    fetchAuctionBids();
  }, []);

  return (
    <div className="bids-container">
      <div className="bids-details">
        <h2>YOUR BIDS</h2>
        <table className="bids-table">
          <thead>
            <tr>
              <th>Bidded Date</th>
              <th>Bidded Price</th>
              <th>Shipping Name</th>
              <th>Contact No.</th>
              <th>Shipping Address</th>
            </tr>
          </thead>
          <tbody>
            {auctionBids.length > 0 ? (
              auctionBids.map((bid, index) => (
                <tr key={bid.bidId}>
                  <td>{new Date(bid.bidDate).toLocaleString()}</td>
                  <td>Rs. {bid.price}</td>
                  <td>{bid.shippingName}</td>
                  <td>{bid.shippingPhoneNumber}</td>
                  <td>{bid.shippingAddress}</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="3" className="no-bids-message"><br></br>No bids available at the moment</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default userAuctionUserBids;
