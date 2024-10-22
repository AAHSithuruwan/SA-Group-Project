// src/pages/categoryItems/categoryItems.jsx
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useLocation, useNavigate } from 'react-router-dom';
import './categoryItems.css';
import { ThreeDots } from 'react-loader-spinner';

const CategoryItems = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { categoryId, categoryName } = location.state;
  const [categoryAuctions, setCategoryAuctions] = useState([]);
  const [timer, setTimer] = useState([]);
  const [loading, setLoading] = useState(true);

  const handleAuctionClick = (auction) => {
    navigate('/item-details', { state: { auctionId: auction.auctionId } });
  };

  useEffect(() => {
    const fetchCategoryAuctions = async () => {
      try {
        const response = await axios.get(`http://localhost:5101/api/Auction/Category/${categoryId}`);
        setCategoryAuctions(response.data);
        initializeTimer(response.data);
      } catch (error) {
        console.error('There was an error fetching the category auctions!', error);
      }
    };

    fetchCategoryAuctions();
  }, [categoryId]);

  const initializeTimer = (categoryAuctions) => {
    const interval = setInterval(() => {
      const updatedTimer = categoryAuctions.map(auction => {
        const currentTime = new Date();
        const startingTime = new Date(auction.startingDate);
        const endTime = new Date(auction.endDate);

        if (currentTime < startingTime) {
          return { status: 'Not Started', remainingTime: startingTime - currentTime };
        } else if (currentTime > endTime) {
          return { status: 'Ended', remainingTime: 0 };
        } else {
          return { status: 'Ongoing', remainingTime: endTime - currentTime };
        }
      });
      setTimer(updatedTimer);
      setLoading(false);
    }, 1000); // Update every second

    return () => clearInterval(interval); // Cleanup on unmount
  };

  const formatRemainingTime = (milliseconds) => {
    const totalSeconds = Math.floor(milliseconds / 1000);
    const days = Math.floor(totalSeconds / 86400); // 86400 seconds in a day
    const hours = Math.floor((totalSeconds % 86400) / 3600); // 3600 seconds in an hour
    const minutes = Math.floor((totalSeconds % 3600) / 60); // 60 seconds in a minute
    return `${days}d ${hours}h ${minutes}m`;
  };

  return (
    <section className="category-items-section">
      <h2>{categoryName}</h2>
      <div className="items-grid">
        {categoryAuctions.map((auction, index) => {
          const { status, remainingTime } = timer[index] || {};
          const displayTime = status === 'Ongoing' ? formatRemainingTime(remainingTime) : '';

          return (
            <div key={index} className="item-card" onClick={() => handleAuctionClick(auction)}>
              <img src={`http://localhost:5101/Images/ProductImages/${auction.productId}.png`} alt={auction.productName} />
              <h3>{auction.productName}</h3>
              <h3>Rs. {auction.nextBidPrice}</h3>
              {loading && <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
                              <ThreeDots
                                visible={true}
                                height="40"
                                width="50"
                                color="#4fa94d"
                                radius="9"
                                ariaLabel="three-dots-loading"
                              />
                          </div>                
              }
              {!loading && (
                  <>
                    {status === 'Ongoing' && <h4 style={{ color: 'blue' }}>Time Left: {displayTime}</h4>}
                    {status === 'Not Started' && <h4 style={{ color: 'brown' }}>Starts At: {new Date(auction.startingDate).toLocaleString()}</h4>}
                    {status === 'Ended' && <h4 style={{ color: 'red' }}>Ended</h4>}
                  </>
              )}
            </div>
          );
        })}
      </div>
    </section>
  );
};

export default CategoryItems;
