import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import './sellerAuctionDetails.css'; 
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';
import ConfirmDialogBox from '../../components/DialogBoxes/ConfirmDialogBox';

const SellerAuctionDetails = () => {
  const location = useLocation();
  const { auctionId } = location.state;
  const [auction, setAuction] = useState([]);
  const navigate = useNavigate();

  const checkAuctionStatus = (startingDate, endDate, isDispatched) => {
    const currentDate = new Date();
    const start = new Date(startingDate);
    const end = new Date(endDate);

    if (isDispatched === 1) {
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

  const navigateTo = () => {
    navigate('/sellerauctionlist'); 
  };

  useEffect(() => {
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

  const handleUpdate = () => {
    navigate('/sellerauctionlist/sellerauctiondetails/updateauction', { state: { auctionId: auction.auctionId } });
  };

  const handleViewBids = () => {
    navigate('/sellerauctionlist/sellerauctiondetails/sellerauctionallbids', { state: { auctionId: auction.auctionId } });
  };

  const deleteAuctionDialog = () => {
    ConfirmDialogBox({
      title: 'Are You Sure?',
      text: 'This action cannot be reverted!',
      onConfirm: deleteAuction,
    })
  };

  const dispatchProductDialog = () => {
    ConfirmDialogBox({
      title: 'Are You Sure?',
      text: 'This action cannot be reverted!',
      onConfirm: dispatchProduct,
    })
  }

  const dispatchProduct = async () => {
    const jwtToken = await getJwtToken();

    try{
      const response = await axios.get(`http://localhost:5101/api/Auction/DispatchProduct/${auctionId}`, 
        {
          headers: {
              Authorization: `Bearer ${jwtToken}`,
          },
        }
      );

      if(response.status === 200){
        SuccessDialogBox({
          title: 'Product Has Been Dispatched',
          text: 'The auction status is changed to dispatched',
          onConfirm: navigateTo,
        })
      }
    }
    catch (error) {
      if(error.response && error.response.status === 401){
        navigate('/signin');
      }
      else{
        ErrorDialogBox({
          title: 'Product Dispatch Failed',
          text: error.response.data
        })
      }
      console.error(error);
    }};

  const deleteAuction = async () => {
    const jwtToken = await getJwtToken();

    try{
      const response = await axios.delete(`http://localhost:5101/api/Auction/${auctionId}`, 
        {
          headers: {
              Authorization: `Bearer ${jwtToken}`,
          },
        }
      );

      if(response.status === 200){
        SuccessDialogBox({
          title: 'Auction Deleted Successfully',
          text: 'The Auction Has Been Deleted',
          onConfirm: navigateTo,
        })
      }
    }
    catch (error) {
      if(error.response && error.response.status === 401){
        navigate('/signin');
      }
      else{
        ErrorDialogBox({
          title: 'Auction Deletion Failed',
          text: error.response.data
        })
      }
      console.error(error);
    }};

  const auctionStatus = checkAuctionStatus(auction.startingDate, auction.endDate, auction.isDispatched);

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
          <p><strong>Starting Date:</strong> <span style={{ marginLeft: '10px' }}>{new Date(auction.startingDate).toLocaleString()}</span></p>
          <p><strong>Ending Date:</strong> <span style={{ marginLeft: '10px' }}>{new Date(auction.endDate).toLocaleString()}</span></p>
          <p><strong>Highest Bid Price:</strong> <span style={{ marginLeft: '10px' }}>
                                                    {auction.highestBidPrice ? `Rs. ${auction.highestBidPrice}` : "None"}
                                                  </span></p>
          <p><strong>Auction Status:</strong> <span style={{ marginLeft: '10px' }}  className={getStatusClass(auctionStatus)}>{auctionStatus}</span></p>
          
          <div className="auction-buttons">
            <button className='bids-button' onClick={handleViewBids}>
              View All Bids
            </button>
            {auction.isDispatched === 1 ? (
              <button className="dispatched-button" style={{ backgroundColor: 'purple', color: 'white' }} disabled>Dispatched</button>
            ) : (
              <>
                <button className="update-button" onClick={handleUpdate}>
                  Update
                </button>
                <button className="remove-button" onClick={deleteAuctionDialog}>
                  Remove
                </button>
                {auctionStatus === 'Ended' && auction.highestBidderEmail && (
                  <button className="dispatch-button" onClick={dispatchProductDialog}>
                    Dispatch
                  </button>
                )}
              </>
            )}
          </div>
        </div>
      </div>

      <div className="highest-bidder-details">
        <h3>Highest Bidder Details</h3>
        <p>
          <strong>Highest Bidder:</strong>
          <span style={{ marginLeft: '15px' }}>{auction.highestBidderEmail || "None"}</span>
        </p>

        <p>
          <strong>Shipping Name:</strong>
          <span style={{ marginLeft: '15px' }}>{auction.highestBidShippingName || "None"}</span>
        </p>
        
        <p>
          <strong>Contact Number:</strong>
          <span style={{ marginLeft: '15px' }}>{auction.highestBidShippingPhoneNumber || "None"}</span>
        </p>
        
        <p>
          <strong>Shipping Address:</strong>
          <span style={{ marginLeft: '15px' }}>{auction.highestBidShippingAddress || "None"}</span>
        </p>
      </div>

      <div className="product-description">
        <h3>Product Description</h3>
        <p>{auction.productDescription}</p>
      </div>
    </div>
  );
};

export default SellerAuctionDetails;



