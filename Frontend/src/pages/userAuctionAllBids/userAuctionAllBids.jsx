import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import './userAuctionAllBids.css';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const userAuctionAllBids = () => {
  const location = useLocation();
  const [auctionBids, setAuctionBids] = useState([]);
  const navigate = useNavigate();
  const [userId, setUserId] = useState([]);
  const { auctionId } = location.state;

  useEffect(() => {
    const fetchAuctionBids = async () => {
      const jwtToken = await getJwtToken();

      try {
        const response = await axios.get(`http://localhost:5101/api/Bid/All/${auctionId}`, {
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

    const fetchUserData = async () => {
        const jwtToken = await getJwtToken();
  
        try {
          const response = await axios.get('http://localhost:5101/api/User', {
            headers: {
              Authorization: `Bearer ${jwtToken}`,
            },
          });
          setUserId(response.data.userId);
        } catch (error) {
          console.error(error);
        }
      };

    fetchAuctionBids();
    fetchUserData();
  }, []);

  return (
    <div className="bids-container">
      <div className="bids-details">
        <h2>AUCTION BIDS</h2>
        <table className="bids-table">
          <thead>
            <tr>
              <th>Bidder</th>
              <th>Bidded Date</th>
              <th>Bidded Price</th>
            </tr>
          </thead>
          <tbody>
            {auctionBids.length > 0 ? (
              auctionBids.map((bid, index) => (
                <tr key={bid.bidId} className={index === 0 ? 'highest-bid' : ''}>
                  <td>{bid.userId === userId ? bid.userEmail : `Bidder ${auctionBids.length - index}`}</td>
                  <td>{new Date(bid.bidDate).toLocaleString()}</td>
                  <td>Rs. {bid.price}</td>
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

export default userAuctionAllBids;
