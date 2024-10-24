import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './userAuctionList.css';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const UserAuctionList = () => {
  const [userAuctions, setUserAuctions] = useState([]);
  const navigate = useNavigate();

  const checkAuctionStatus = (startingDate, endDate, isDispatched) => {
    const currentDate = new Date();
    const start = new Date(startingDate);
    const end = new Date(endDate);

    if (isDispatched == 1) {
      return 'Dispatched';
    } else if (currentDate < start) {
      return 'Not Started';
    } else if (currentDate >= start && currentDate <= end) {
      return 'Ongoing';
    } else if (currentDate > end) {
      return 'Ended';
    }
  };

  const getStatusClass = (status) => {
    switch (status) {
      case 'Dispatched':
        return 'status-dispatched';
      case 'Ended':
        return 'status-ended';
      case 'Not Started':
        return 'status-not-started';
      case 'Ongoing':
        return 'status-ongoing';
      default:
        return '';
    }
  };  

  const handleDetailsClick = (auction) => {
    navigate('/userauctionlist/userauctiondetails', { state: { auctionId: auction.auctionId } }); 
  };

  useEffect(() => {
    const fetchUserAuctions = async () => {
      const jwtToken = await getJwtToken();

      try {
        const response = await axios.get(`http://localhost:5101/api/Auction/User`,{
          headers: {
              Authorization: `Bearer ${jwtToken}`,
          },
        });
        setUserAuctions(response.data);
      } catch (error) {
        if(error.response && error.response.status === 401){
          navigate('/signin');
        }
        console.error('There was an error fetching the user auctions!', error);
      }
    };

    fetchUserAuctions();
  }, []);

  return (
    <div className="auction-container">
      <div className="auction-details">
        <h2>AUCTION DETAILS</h2>
        <table className="auction-table">
          <thead>
            <tr>
              <th>Auction</th>
              <th className="status-column">Status</th>
              <th className='details-column'></th>
            </tr>
          </thead>
          <tbody>
            {userAuctions.length > 0 ? (
              userAuctions.map((auction) => (
                <tr key={auction.auctionId}>
                  <td>
                    <div className="image-text-container">
                      <img src={`http://localhost:5101/Images/ProductImages/${auction.productId}.png`} alt={auction.productName} className="auction-image" />
                      <span className="product-name">{auction.productName}</span>
                    </div>
                  </td>
                  <td className={getStatusClass(checkAuctionStatus(auction.startingDate, auction.endDate, auction.isDispatched))}>
                    {checkAuctionStatus(auction.startingDate, auction.endDate, auction.isDispatched)}
                  </td>
                  <td>
                    
                      <button className="details-button" onClick={() => handleDetailsClick(auction)} >Details</button>  
                  
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="3" className="no-auctions-message"><br></br>No auctions available at the moment</td>
              </tr>
            )}
          </tbody>

        </table>
      </div>
    </div>
  );
};

export default UserAuctionList;