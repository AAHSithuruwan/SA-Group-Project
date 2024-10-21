import React from 'react'
import './addproduct.css';

const AddProduct = () => {
  return (
    <div className="create-auction-container">
      
      
      <div className="auction-form">
        <center><h2>Create Auction</h2></center>

        <form>
          <label>Give Product Name:</label>
          <input type="text" name="productName" placeholder="Enter Product Name" />
          <label>Add Image:</label>
          <div className="textare">
          <input type="File"/></div>
          
          
          <label>Category:</label>
          <select name="category">
            <option value="fashion">Fashion</option>
            <option value="electronics">Electronics</option>
            <option value="home">Home</option>
            <option value="sports">Sports</option>
          </select>

          <label>Starting Bid Price:</label>
          <input type="number" name="startingBidPrice" placeholder="Enter Starting Bid Price" />

          <label>Bidding Date:</label>
          <input type="date" name="biddingDate" />

          <label>Bidding Time:</label>
          <input type="time" name="biddingTime" />

          <label>Enter Description:</label>
          <textarea name="description" rows="5" placeholder="Enter Product Description"></textarea>
          
          <button type="submit">Submit</button>
         
        </form>
      </div>
    </div>
  )
}

export default AddProduct;
