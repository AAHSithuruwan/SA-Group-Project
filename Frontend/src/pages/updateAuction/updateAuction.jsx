import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import './updateAuction.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const UpdateAuction = () => {

  const navigate = useNavigate();
  const location = useLocation();
  const { auctionId } = location.state;
  const [productName, setProductName] = useState([]);
  const [productDescription, setProductDescription] = useState([]);
  const [startingPrice, setStartingPrice] = useState([]);
  const [bidIncrement, setBidIncrement] = useState([]);
  const [startingDate, setStartingDate] = useState([]);
  const [endDate, setEndDate] = useState([]);
  const [productImage, setProductImage] = useState([]);
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState('');

  const navigateTo = () => {
    navigate('/sellerauctionlist'); 
  };

  useEffect(() => {
    const fetchCategories = async () => {
      const jwtToken = await getJwtToken();
      try {
        const response = await axios.get('http://localhost:5101/api/Category/All',{
          headers: {
              Authorization: `Bearer ${jwtToken}`,
          },
        });
        setCategories(response.data);
      } catch (error) {
        if(error.response && error.response.status === 401){
          navigate('/signin');
        }
        console.error(error.response);
      }
    };

    const fetchAuctionDetails = async () => {
        try {
          const response = await axios.get(`http://localhost:5101/api/Auction/${auctionId}`);
          setProductName(response.data.productName);
          setProductDescription(response.data.productDescription);
          setStartingDate(response.data.startingDate);
          setEndDate(response.data.endDate);
          setBidIncrement(response.data.bidIncrement);
          setStartingPrice(response.data.startingPrice);
          setSelectedCategory(response.data.categoryId);
        } catch (error) {
          console.error(error);
        }
      };

    fetchCategories();
    fetchAuctionDetails(); // Call the async function
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const jwtToken = await getJwtToken();

    const formData = new FormData();
    
      formData.append('ProductName', productName);
      formData.append('ProductDescription', productDescription);
      formData.append('CategoryId', selectedCategory);
      formData.append('StartingPrice', startingPrice);
      formData.append('BidIncrement', bidIncrement);
      formData.append('StartingDate', startingDate);
      formData.append('EndDate', endDate);
      formData.append('ProductImage', productImage);
      formData.append('auctionId', auctionId);

    try{
      const response = await axios.put('http://localhost:5101/api/Auction', 
        formData,
        {
          headers: {
              'Content-Type': 'multipart/form-data',
              Authorization: `Bearer ${jwtToken}`,
          },
        }
    );

      if(response.status === 200){
        SuccessDialogBox({
          title: 'Auction Updated Successfully',
          text: 'The Auction Has Been Updated',
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
          title: 'Auction Update Failed',
          text: error.response.data
        })
      }
      console.error(error);
    }};
    return (
        <div className="update-auction-container">
          <div className="auction-form">
            <h2 className="form-title">UPDATE AUCTION</h2>
      
            <form onSubmit={handleSubmit}>
              <div className="input-row">
                <label>Product Name:</label>
                <input
                  type="text"
                  value={productName}
                  placeholder="Enter Product Name"
                  onChange={(e) => setProductName(e.target.value)}
                  required
                />
              </div>
      
              <div className="input-row">
                <label>Category:</label>
                <select
                  name="category"
                  value={selectedCategory}
                  onChange={(e) => setSelectedCategory(e.target.value)}
                  required
                >
                  <option value="">Select Category</option>
                  {categories.map((category) => (
                    <option key={category.categoryId} value={category.categoryId}>
                      {category.name}
                    </option>
                  ))}
                </select>
              </div>
      
              <div className="input-row">
                <label>Starting Bid Price:</label>
                <input
                  type="number"
                  value={startingPrice}
                  placeholder="Enter Starting Bid Price"
                  onChange={(e) => setStartingPrice(e.target.value)}
                  required
                />
              </div>
      
              <div className="input-row">
                <label>Bid Increment:</label>
                <input
                  type="number"
                  value={bidIncrement}
                  placeholder="Enter Bid Increment"
                  onChange={(e) => setBidIncrement(e.target.value)}
                  required
                />
              </div>
      
              <div className="input-row">
                <label>Auction Starting Date:</label>
                <input
                  type="datetime-local"
                  value={startingDate}
                  placeholder="Enter Auction Starting Date"
                  onChange={(e) => setStartingDate(e.target.value)}
                  required
                />
              </div>
      
              <div className="input-row">
                <label>Auction End Date:</label>
                <input
                  type="datetime-local"
                  value={endDate}
                  placeholder="Enter Auction End Date"
                  onChange={(e) => setEndDate(e.target.value)}
                  required
                />
              </div>
      
              <div className="input-row">
                <label>Product Image:</label>
                <input
                  type="file"
                  onChange={(e) => setProductImage(e.target.files[0])}
                />
              </div>
      
              <div className="input-row">
                <label>Description:</label>
                <textarea
                  value={productDescription}
                  rows="5"
                  placeholder="Enter Product Description"
                  onChange={(e) => setProductDescription(e.target.value)}
                  required
                ></textarea>
              </div>
      
              <div className="submit-row">
                <button type="submit">Update</button>
              </div>
            </form>
          </div>
        </div>
      );
      
};

export default UpdateAuction;

